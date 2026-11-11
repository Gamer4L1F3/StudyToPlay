using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System.Collections.Generic;

namespace StudyToPlay.Common.Players;

public class StudyPlayer : ModPlayer
{
    public int Energy;
    public int Insight;
    public int CorrectAnswers;
    public int StudyStreak;
    public bool DoubleEnergy;
    public bool ExtraInsight;

    public int EnergyCap => 9999;

    public override void Initialize()
    {
        Energy = 0;
        Insight = 0;
        CorrectAnswers = 0;
        StudyStreak = 0;
        DoubleEnergy = false;
        ExtraInsight = false;
    }

    public bool TrySpendEnergy(int amount)
    {
        if (amount < 0 || Energy < amount) return false;
        Energy -= amount;
        return true;
    }

    public void RewardCorrectAnswer()
    {
        StudyStreak++;
        CorrectAnswers++;
        int reward = 10 + (StudyStreak >= 5 ? 5 : 0) + (StudyStreak >= 10 ? 5 : 0);
        if (DoubleEnergy) reward *= 2;
        Energy = System.Math.Min(EnergyCap, Energy + reward);
        if (CorrectAnswers % 5 == 0)
            Insight += ExtraInsight ? 2 : 1;
    }

    public void RecordWrongAnswer() => StudyStreak = 0;

    public override void SaveData(TagCompound tag)
    {
        tag["Energy"] = Energy;
        tag["Insight"] = Insight;
        tag["CorrectAnswers"] = CorrectAnswers;
        tag["StudyStreak"] = StudyStreak;
        tag["DoubleEnergy"] = DoubleEnergy;
        tag["ExtraInsight"] = ExtraInsight;
    }

    public override void LoadData(TagCompound tag)
    {
        Energy = tag.GetInt("Energy");
        Insight = tag.GetInt("Insight");
        CorrectAnswers = tag.GetInt("CorrectAnswers");
        StudyStreak = tag.GetInt("StudyStreak");
        DoubleEnergy = tag.GetBool("DoubleEnergy");
        ExtraInsight = tag.GetBool("ExtraInsight");
    }
}
