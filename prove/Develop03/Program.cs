using System;
using System.Collections.Generic;

public class Word
{
    private string text;
    private bool isHidden;

    public Word(string text)
    {
        this.text = text;
        isHidden = false;
    }

    public void Hide()
    {
        isHidden = true;
    }

    public bool IsHidden()
    {
        return isHidden;
    }

    public override string ToString()
    {
        return isHidden ? "_____" : text;
    }
}

public class Reference
{
    private string book;
    private int chapter;
    private int verseStart;
    private int? verseEnd;

    public Reference(string book, int chapter, int verseStart, int? verseEnd = null)
    {
        this.book = book;
        this.chapter = chapter;
        this.verseStart = verseStart;
        this.verseEnd = verseEnd;
    }

    public override string ToString()
    {
        return verseEnd.HasValue ? $"{book} {chapter}:{verseStart}-{verseEnd}" : $"{book} {chapter}:{verseStart}";
    }
}

public class Scripture
{
    private Reference reference;
    private List<Word> words;

    public Scripture(Reference reference, string text)
    {
        this.reference = reference;
        words = new List<Word>();
        foreach (var word in text.Split(' '))
        {
            words.Add(new Word(word));
        }
    }

    public void HideRandomWords(int numberOfWords)
    {
        Random random = new Random();
        for (int i = 0; i < numberOfWords; i++)
        {
            int index = random.Next(words.Count);
            words[index].Hide();
        }
    }

    public bool AllWordsHidden()
    {
        foreach (var word in words)
        {
            if (!word.IsHidden())
                return false;
        }
        return true;
    }

    public override string ToString()
    {
        string result = reference.ToString() + "\n";
        foreach (var word in words)
        {
            result += word.ToString() + " ";
        }
        return result.Trim();
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Reference reference = new Reference("Proverbs", 3, 5, 6);
        Scripture scripture = new Scripture(reference, "Trust in the Lord with all thine heart and lean not unto thine own understanding");

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine(scripture);
            Console.WriteLine("\nPress Enter to hide words, or type 'quit' to exit.");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
            {
                running = false;
            }
            else
            {
                scripture.HideRandomWords(3); 
                if (scripture.AllWordsHidden())
                {
                    Console.Clear();
                    Console.WriteLine("All words are hidden. Memorization complete!");
                    running = false;
                }
            }
        }
    }
}
