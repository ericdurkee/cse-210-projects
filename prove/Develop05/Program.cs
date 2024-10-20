using System;
using System.Threading;

namespace MindfulnessApp
{
    public abstract class MindfulnessActivity
    {
        protected int Duration;

        public void StartActivity()
        {
            Console.WriteLine(GetStartingMessage());
            Console.Write("Enter the duration in seconds: ");
            Duration = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Prepare to begin...");
            Pause(3);
        }

        protected abstract string GetStartingMessage();

        protected void FinishActivity()
        {
            Console.WriteLine("Congratulations! You have completed the activity.");
            Console.WriteLine($"Activity Duration: {Duration} seconds.");
            Pause(3);
        }

        protected void Pause(int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            Console.WriteLine();
        }
    }

    public class BreathingActivity : MindfulnessActivity
    {
        protected override string GetStartingMessage()
        {
            return "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.";
        }

        public void Run()
        {
            StartActivity();
            int elapsed = 0;

            while (elapsed < Duration)
            {
                Console.WriteLine("Breathe in...");
                Pause(4);
                Console.WriteLine("Breathe out...");
                Pause(4); 
                elapsed += 8;
            }

            FinishActivity();
        }
    }

    public class ReflectionActivity : MindfulnessActivity
    {
        private readonly string[] prompts = {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        private readonly string[] questions = {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        protected override string GetStartingMessage()
        {
            return "This activity will help you reflect on times in your life when you have shown strength and resilience.This will help you recognize the power you have and how you can use it in other aspects of your life.";
        }

        public void Run()
        {
            StartActivity();
            Random rand = new Random();
            int elapsed = 0;

            while (elapsed < Duration)
            {
                string prompt = prompts[rand.Next(prompts.Length)];
                Console.WriteLine(prompt);
                Pause(3); 

                foreach (var question in questions)
                {
                    Console.WriteLine(question);
                    Pause(4); 
                    elapsed += 7; 
                    if (elapsed >= Duration) break; 
                }
            }

            FinishActivity();
        }
    }

    public class ListingActivity : MindfulnessActivity
    {
        private readonly string[] prompts = {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        protected override string GetStartingMessage()
        {
            return "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.";
        }

        public void Run()
        {
            StartActivity();
            Random rand = new Random();
            string prompt = prompts[rand.Next(prompts.Length)];
            Console.WriteLine(prompt);
            Pause(3); 

            Console.WriteLine("Begin listing items (you have " + Duration + " seconds)...");
            int count = 0;
            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalSeconds < Duration)
            {
                Console.Write("Enter an item (or type 'exit' to finish): ");
                string item = Console.ReadLine();
                if (item.ToLower() == "exit") break;
                count++;
            }

            Console.WriteLine($"You listed {count} items.");
            FinishActivity();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to my Mindfulness App");
            while (true)
            {
                Console.WriteLine("Choose an activity:");
                Console.WriteLine("1. Breathing Activity");
                Console.WriteLine("2. Reflection Activity");
                Console.WriteLine("3. Listing Activity");
                Console.WriteLine("4. Exit");
                Console.Write("Your choice: ");
                string choice = Console.ReadLine();

                MindfulnessActivity activity = null;

                switch (choice)
                {
                    case "1":
                        activity = new BreathingActivity();
                        break;
                    case "2":
                        activity = new ReflectionActivity();
                        break;
                    case "3":
                        activity = new ListingActivity();
                        break;
                    case "4":
                        Console.WriteLine("Thanks for playing!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;
                }

                if (activity != null)
                {
                    if (activity is BreathingActivity breathingActivity)
                    {
                        breathingActivity.Run();
                    }
                    else if (activity is ReflectionActivity reflectionActivity)
                    {
                        reflectionActivity.Run();
                    }
                    else if (activity is ListingActivity listingActivity)
                    {
                        listingActivity.Run();
                    }
                }
            }
        }
    }
}
