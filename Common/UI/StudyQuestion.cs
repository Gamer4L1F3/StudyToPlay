using System.Collections.Generic;

namespace StudyToPlay.Common.UI;

// Public properties keep this class straightforward to save and reload as JSON.
public class StudyQuestion
{
    public string Prompt { get; set; } = "";
    public List<string> Answers { get; set; } = new();
    public int CorrectIndex { get; set; }
    public string Category { get; set; } = "General";

    public StudyQuestion() { }

    public StudyQuestion(string prompt, IEnumerable<string> answers, int correctIndex, string category = "General")
    {
        Prompt = prompt;
        Answers = new List<string>(answers);
        CorrectIndex = correctIndex;
        Category = category;
    }

    public static List<StudyQuestion> CreateDefaults() => new()
    {
        new("What is 7 × 8?", new[] {"54", "56", "64", "48"}, 1, "Math"),
        new("Which planet is known as the Red Planet?", new[] {"Venus", "Jupiter", "Mars", "Mercury"}, 2, "Science"),
        new("What gas do plants absorb?", new[] {"Oxygen", "Carbon dioxide", "Nitrogen", "Hydrogen"}, 1, "Science"),
        new("How many sides does a hexagon have?", new[] {"5", "6", "7", "8"}, 1, "Math"),
        new("What is the largest ocean on Earth?", new[] {"Atlantic", "Indian", "Arctic", "Pacific"}, 3, "Geography")
    };
}
