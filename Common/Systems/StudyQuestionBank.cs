using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Terraria;
using Terraria.ModLoader;
using StudyToPlay.Common.UI;

namespace StudyToPlay.Common.Systems;

/// <summary>
/// One question bank per local Terraria installation, shared by every world.
/// The file is managed by the mod; players edit it through the in-game menu.
/// </summary>
public static class StudyQuestionBank
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static string FilePath => Path.Combine(ModLoader.GetModConfigPath(), "StudyToPlay_Questions.json");
    public static List<StudyQuestion> Questions { get; private set; } = StudyQuestion.CreateDefaults();

    public static void Load()
    {
        try
        {
            Directory.CreateDirectory(ModLoader.GetModConfigPath());
            if (File.Exists(FilePath))
            {
                var loaded = JsonSerializer.Deserialize<List<StudyQuestion>>(File.ReadAllText(FilePath), JsonOptions);
                Questions = Sanitize(loaded);
            }
            else
            {
                Questions = StudyQuestion.CreateDefaults();
                Save();
            }
        }
        catch (Exception e)
        {
            Questions = StudyQuestion.CreateDefaults();
            ModContent.GetInstance<StudyToPlay>().Logger.Warn("Could not load the global question bank. Using defaults. " + e.Message);
        }
    }

    public static bool Save()
    {
        try
        {
            Directory.CreateDirectory(ModLoader.GetModConfigPath());
            File.WriteAllText(FilePath, JsonSerializer.Serialize(Questions, JsonOptions));
            return true;
        }
        catch (Exception e)
        {
            Main.NewText("Study to Play could not save your question bank: " + e.Message);
            return false;
        }
    }

    public static void Add(StudyQuestion question)
    {
        if (Questions.Count >= 500 || !IsValid(question)) return;
        Questions.Add(question);
        Save();
    }

    public static void Replace(int index, StudyQuestion question)
    {
        if (index < 0 || index >= Questions.Count || !IsValid(question)) return;
        Questions[index] = question;
        Save();
    }

    public static void Delete(int index)
    {
        if (index < 0 || index >= Questions.Count) return;
        Questions.RemoveAt(index);
        Save();
    }

    private static bool IsValid(StudyQuestion q) =>
        q != null && !string.IsNullOrWhiteSpace(q.Prompt) &&
        q.Answers != null && q.Answers.Count == 4 &&
        q.Answers.All(a => !string.IsNullOrWhiteSpace(a)) &&
        q.CorrectIndex >= 0 && q.CorrectIndex < 4;

    private static List<StudyQuestion> Sanitize(List<StudyQuestion> source)
    {
        if (source == null) return StudyQuestion.CreateDefaults();
        return source.Where(IsValid).Take(500).ToList();
    }
}
