using Terraria.ModLoader.IO;
using System.Collections.Generic;

namespace StudyToPlay.Common.UI;

public class StudyQuestion
{
    public string Prompt;
    public string[] Answers;
    public int CorrectIndex;
    public string Category;

    public StudyQuestion(string prompt, string[] answers, int correctIndex, string category = "General")
    {
        Prompt = prompt;
        Answers = answers;
        CorrectIndex = correctIndex;
        Category = category;
    }

    public TagCompound ToTag() => new TagCompound {
        ["Prompt"] = Prompt, ["Answers"] = Answers, ["CorrectIndex"] = CorrectIndex, ["Category"] = Category
    };

    public static StudyQuestion FromTag(TagCompound tag) =>
        new StudyQuestion(tag.GetString("Prompt"), tag.GetString("Answers").Split('|'),
            tag.GetInt("CorrectIndex"), tag.GetString("Category"));

    public static List<StudyQuestion> CreateDefaults() => new() {
        new("What is 7 × 8?", new[] {"54", "56", "64", "48"}, 1, "Math"),
        new("Which planet is known as the Red Planet?", new[] {"Venus", "Jupiter", "Mars", "Mercury"}, 2, "Science"),
        new("What gas do plants absorb?", new[] {"Oxygen", "Carbon dioxide", "Nitrogen", "Hydrogen"}, 1, "Science"),
        new("How many sides does a hexagon have?", new[] {"5", "6", "7", "8"}, 1, "Math"),
        new("What is the largest ocean on Earth?", new[] {"Atlantic", "Indian", "Arctic", "Pacific"}, 3, "Geography")
    };
}
