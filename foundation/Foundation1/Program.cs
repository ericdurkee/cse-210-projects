using System;
using System.Collections.Generic;

public class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; } 
    private List<Comment> comments;

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return comments.Count;
    }

    public List<Comment> GetComments()
    {
        return comments;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        List<Video> videos = new List<Video>();

        Video video1 = new Video("Daily gym vlog #3", "John Smith", 600);
        video1.AddComment(new Comment("Alice", "Great explanation of squatting!"));
        video1.AddComment(new Comment("Bob", "I learned a lot from this video."));
        video1.AddComment(new Comment("Charlie", "Thanks for the lessons."));
        videos.Add(video1);

        Video video2 = new Video("Why C# is a great class", "Jane Smith", 750);
        video2.AddComment(new Comment("Dave", "Very helpful!"));
        video2.AddComment(new Comment("Eve", "I love the way you explained everything!."));
        video2.AddComment(new Comment("Frank", "Can't wait to apply this in my project."));
        videos.Add(video2);

        Video video3 = new Video("Why Mcdonalds is bad for you", "Alice Chains", 900);
        video3.AddComment(new Comment("Grace", "The visuals helped a lot!"));
        video3.AddComment(new Comment("Hank", "Could you do a video on Wendy's?"));
        videos.Add(video3);

        foreach (var video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.Length} seconds");
            Console.WriteLine($"Number of Comments: {video.GetCommentCount()}");
            Console.WriteLine("Comments:");

            foreach (var comment in video.GetComments())
            {
                Console.WriteLine($" - {comment.CommenterName}: {comment.Text}");
            }

            Console.WriteLine(); 
        }
    }
}
