using Terraria.ModLoader;
using StudyToPlay.Common.UI;

namespace StudyToPlay.Common.Systems;

public class StudySystem : ModSystem
{
    public static System.Collections.Generic.List<StudyQuestion> Questions => StudyQuestionBank.Questions;
    public static bool BossPermitAvailable;

    public override void Load()
    {
        StudyQuestionBank.Load();
    }

    public override void OnWorldLoad()
    {
        BossPermitAvailable = false;
    }
}
