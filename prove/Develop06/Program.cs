using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    public abstract class Goal
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public bool IsCompleted { get; private set; } 

        protected Goal(string name, int points)
        {
            Name = name;
            Points = points;
            IsCompleted = false;
        }

        protected void MarkAsComplete()
        {
            IsCompleted = true;
        }

        public abstract int RecordProgress();
        public abstract bool CheckIfComplete();
        public abstract string GetStatus();
    }

    public class SimpleGoal : Goal
    {
        public SimpleGoal(string name, int points) : base(name, points) { }

        public override int RecordProgress()
        {
            if (!IsCompleted)
            {
                MarkAsComplete();
                return Points;
            }
            return 0;
        }

        public override bool CheckIfComplete() => IsCompleted;

        public override string GetStatus() => IsCompleted ? "[X] Completed" : "[ ] Not Completed";
    }

    public class EternalGoal : Goal
    {
        public EternalGoal(string name, int points) : base(name, points) { }

        public override int RecordProgress() => Points;

        public override bool CheckIfComplete() => false;

        public override string GetStatus() => "[ ] Always Active"; 
    }

    public class ChecklistGoal : Goal
    {
        public int TargetCount { get; private set; }
        public int CurrentCount { get; private set; }
        public int BonusPoints { get; private set; }

        public ChecklistGoal(string name, int points, int targetCount, int bonusPoints)
            : base(name, points)
        {
            TargetCount = targetCount;
            BonusPoints = bonusPoints;
            CurrentCount = 0;
        }

        public override int RecordProgress()
        {
            if (IsCompleted) return 0;

            CurrentCount++;
            if (CurrentCount >= TargetCount)
            {
                MarkAsComplete();
                return Points + BonusPoints;
            }
            return Points;
        }

        public override bool CheckIfComplete() => IsCompleted;

        public override string GetStatus() => 
            IsCompleted ? "[X] Completed" : $"[ ] Not Completed (Completed {CurrentCount}/{TargetCount})";
    }

    public class User
    {
        public int Score { get; private set; }
        private List<Goal> goals;

        public User()
        {
            Score = 0;
            goals = new List<Goal>();
        }

        public void CreateGoal(string goalType, string name, int points, int? targetCount = null, int? bonusPoints = null)
        {
            Goal goal = goalType switch
            {
                "Simple" => new SimpleGoal(name, points),
                "Eternal" => new EternalGoal(name, points),
                "Checklist" when targetCount.HasValue && bonusPoints.HasValue 
                    => new ChecklistGoal(name, points, targetCount.Value, bonusPoints.Value),
                _ => throw new ArgumentException("Invalid goal type."),
            };

            goals.Add(goal);
        }

        public void RecordGoalProgress(string goalName)
        {
            Goal goal = goals.Find(g => g.Name == goalName);
            if (goal != null)
            {
                Score += goal.RecordProgress();
            }
            else
            {
                Console.WriteLine("Goal not found.");
            }
        }

        public void DisplayGoals()
        {
            foreach (var goal in goals)
            {
                Console.WriteLine($"{goal.GetStatus()} - {goal.Name}");
            }
        }

        public void SaveProgress(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine($"Score:{Score}");
                foreach (var goal in goals)
                {
                    string goalType = goal.GetType().Name;
                    if (goal is ChecklistGoal checklistGoal)
                    {
                        writer.WriteLine($"{goalType},{goal.Name},{goal.Points},{checklistGoal.CurrentCount},{checklistGoal.TargetCount},{checklistGoal.BonusPoints},{goal.CheckIfComplete()}");
                    }
                    else
                    {
                        writer.WriteLine($"{goalType},{goal.Name},{goal.Points},{goal.CheckIfComplete()}");
                    }
                }
            }
        }

        public void LoadProgress(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Save file not found.");
                return;
            }

            goals.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                Score = int.Parse(reader.ReadLine().Split(':')[1]);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    string goalType = parts[0];
                    string name = parts[1];
                    int points = int.Parse(parts[2]);
                    bool isComplete = bool.Parse(parts[^1]);

                    switch (goalType)
                    {
                        case nameof(SimpleGoal):
                            var simpleGoal = new SimpleGoal(name, points);
                            if (isComplete) simpleGoal.RecordProgress(); 
                            goals.Add(simpleGoal);
                            break;

                        case nameof(EternalGoal):
                            var eternalGoal = new EternalGoal(name, points);
                            goals.Add(eternalGoal);
                            break;

                        case nameof(ChecklistGoal):
                            int currentCount = int.Parse(parts[3]);
                            int targetCount = int.Parse(parts[4]);
                            int bonusPoints = int.Parse(parts[5]);
                            var checklistGoal = new ChecklistGoal(name, points, targetCount, bonusPoints);
                            for (int i = 0; i < currentCount; i++) checklistGoal.RecordProgress(); 
                            if (isComplete) checklistGoal.RecordProgress();
                            goals.Add(checklistGoal);
                            break;

                        default:
                            Console.WriteLine("Unknown goal type.");
                            break;
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            User user = new User();

            // User Interface
            Console.WriteLine("Welcome to Eternal Quest!");
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Create a new goal");
                Console.WriteLine("2. Record progress");
                Console.WriteLine("3. Display goals");
                Console.WriteLine("4. Save");
                Console.WriteLine("5. Load");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");
                
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter goal type (Simple/Eternal/Checklist): ");
                        string goalType = Console.ReadLine();
                        Console.Write("Enter goal name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter points for the goal: ");
                        int points = int.Parse(Console.ReadLine());
                        if (goalType.Equals("Checklist", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Write("Enter target count: ");
                            int targetCount = int.Parse(Console.ReadLine());
                            Console.Write("Enter bonus points: ");
                            int bonusPoints = int.Parse(Console.ReadLine());
                            user.CreateGoal(goalType, name, points, targetCount, bonusPoints);
                        }
                        else
                        {
                            user.CreateGoal(goalType, name, points);
                        }
                        break;
                    case "2":
                        Console.Write("Enter goal name to record progress: ");
                        string goalName = Console.ReadLine();
                        user.RecordGoalProgress(goalName);
                        break;
                    case "3":
                        user.DisplayGoals();
                        Console.WriteLine($"Current Score: {user.Score}");
                        break;
                    case "4":
                        Console.Write("Enter filename to save progress: ");
                        string saveFilename = Console.ReadLine();
                        user.SaveProgress(saveFilename);
                        break;
                    case "5":
                        Console.Write("Enter filename to load progress: ");
                        string loadFilename = Console.ReadLine();
                        user.LoadProgress(loadFilename);
                        break;
                    case "6":
                        Console.WriteLine("Exiting the program.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
