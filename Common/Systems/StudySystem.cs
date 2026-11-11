using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System.Linq;
using StudyToPlay.Common.UI;

namespace StudyToPlay.Common.Systems;

public class StudySystem : ModSystem
{
    public static List<StudyQuestion> Questions = new();
    public static bool BossPermitAvailable;

    public override void OnWorldLoad()
    {
        Questions = StudyQuestion.CreateDefaults();
        BossPermitAvailable = false;
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag["StudyQuestions"] = Questions.Select(q => q.ToTag()).ToList();
        tag["BossPermitAvailable"] = BossPermitAvailable;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        Questions = tag.ContainsKey("StudyQuestions")
            ? tag.GetList<TagCompound>("StudyQuestions").Select(StudyQuestion.FromTag).ToList()
            : StudyQuestion.CreateDefaults();
        BossPermitAvailable = tag.GetBool("BossPermitAvailable");
    }

    public static void AddQuestion(StudyQuestion question)
    {
        if (Questions.Count < 500 && !string.IsNullOrWhiteSpace(question.Prompt))
            Questions.Add(question);
    }
}
