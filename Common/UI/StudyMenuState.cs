using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using System;
using StudyToPlay.Common.Players;
using StudyToPlay.Common.Systems;

namespace StudyToPlay.Common.UI;

public class StudyMenuState : UIState
{
    private UIPanel panel;
    private UIText status;
    private UIText prompt;
    private int questionIndex;
    private bool editorMode;
    private UITextBox promptInput;
    private UITextBox answer1Input;
    private UITextBox answer2Input;
    private UITextBox answer3Input;
    private UITextBox answer4Input;

    public override void OnInitialize()
    {
        panel = new UIPanel();
        panel.Width.Set(620, 0);
        panel.Height.Set(500, 0);
        panel.HAlign = 0.5f;
        panel.VAlign = 0.5f;
        Append(panel);

        var title = new UIText("STUDY TO PLAY", 1.2f);
        title.HAlign = 0.5f;
        title.Top.Set(12, 0);
        panel.Append(title);

        status = new UIText("");
        status.Top.Set(48, 0);
        status.Left.Set(20, 0);
        panel.Append(status);

        prompt = new UIText("");
        prompt.Top.Set(100, 0);
        prompt.Left.Set(20, 0);
        prompt.Width.Set(570, 0);
        panel.Append(prompt);

        AddButton("Answer A", 0, 155, () => Answer(0));
        AddButton("Answer B", 1, 205, () => Answer(1));
        AddButton("Answer C", 2, 255, () => Answer(2));
        AddButton("Answer D", 3, 305, () => Answer(3));
        AddButton("Next question", 0, 365, NextQuestion);
        AddButton("Upgrades", 1, 365, BuyUpgrade);
        AddButton("Boss Permit (100 Energy)", 2, 365, BuyBossPermit);
        AddButton("Question editor", 3, 365, ToggleEditor);
        AddButton("Close", 3, 425, () => StudyUISystem.HideMenu());

        BuildEditor();
        Refresh();
    }

    private void AddButton(string label, int column, int top, Action action)
    {
        var button = new UITextPanel<string>(label);
        button.Width.Set(135, 0);
        button.Height.Set(38, 0);
        button.Left.Set(20 + column * 145, 0);
        button.Top.Set(top, 0);
        button.OnLeftClick += (_, _) => action();
        panel.Append(button);
    }

    private void BuildEditor()
    {
        promptInput = MakeInput("Question text", 20, 100);
        answer1Input = MakeInput("Answer A", 20, 150);
        answer2Input = MakeInput("Answer B", 20, 200);
        answer3Input = MakeInput("Answer C", 20, 250);
        answer4Input = MakeInput("Answer D", 20, 300);
        AddButton("Save (A correct)", 0, 355, SaveQuestion);
        AddButton("Save (B correct)", 1, 355, () => SaveQuestion(1));
        AddButton("Save (C correct)", 2, 355, () => SaveQuestion(2));
        AddButton("Save (D correct)", 3, 355, () => SaveQuestion(3));
    }

    private UITextBox MakeInput(string hint, int left, int top)
    {
        var input = new UITextBox(hint);
        input.Width.Set(560, 0);
        input.Height.Set(36, 0);
        input.Left.Set(left, 0);
        input.Top.Set(top, 0);
        input.SetTextMaxLength(180);
        panel.Append(input);
        return input;
    }

    private void ToggleEditor()
    {
        editorMode = !editorMode;
        promptInput?.SetText("");
        answer1Input?.SetText("");
        answer2Input?.SetText("");
        answer3Input?.SetText("");
        answer4Input?.SetText("");
        Refresh();
    }

    private void SaveQuestion(int correct = 0)
    {
        var p = promptInput.CurrentString.Trim();
        var answers = new[] { answer1Input.CurrentString.Trim(), answer2Input.CurrentString.Trim(),
            answer3Input.CurrentString.Trim(), answer4Input.CurrentString.Trim() };
        if (string.IsNullOrWhiteSpace(p) || Array.Exists(answers, string.IsNullOrWhiteSpace))
        {
            Main.NewText("Fill in the question and all four answers first.");
            return;
        }
        StudySystem.AddQuestion(new StudyQuestion(p, answers, correct, "Custom"));
        Main.NewText("Question added to this world's question bank!");
        promptInput.SetText("");
        answer1Input.SetText("");
        answer2Input.SetText("");
        answer3Input.SetText("");
        answer4Input.SetText("");
    }

    private void Answer(int selected)
    {
        if (editorMode || StudySystem.Questions.Count == 0) return;
        var player = Main.LocalPlayer.GetModPlayer<StudyPlayer>();
        var q = StudySystem.Questions[questionIndex % StudySystem.Questions.Count];
        if (selected == q.CorrectIndex)
        {
            player.RewardCorrectAnswer();
            Main.NewText("Correct! Energy earned. Current Energy: " + player.Energy);
        }
        else
        {
            player.RecordWrongAnswer();
            Main.NewText("Not quite! The correct answer was " + q.Answers[q.CorrectIndex] + ".");
        }
        NextQuestion();
    }

    private void NextQuestion()
    {
        if (StudySystem.Questions.Count > 0)
            questionIndex = (questionIndex + 1) % StudySystem.Questions.Count;
        Refresh();
    }

    private void BuyUpgrade()
    {
        var player = Main.LocalPlayer.GetModPlayer<StudyPlayer>();
        if (!player.DoubleEnergy)
        {
            if (player.TrySpendEnergy(150)) { player.DoubleEnergy = true; Main.NewText("Upgrade unlocked: Double Energy!"); }
            else Main.NewText("You need 150 Energy.");
        }
        else if (!player.ExtraInsight)
        {
            if (player.TrySpendEnergy(200)) { player.ExtraInsight = true; Main.NewText("Upgrade unlocked: Extra Insight!"); }
            else Main.NewText("You need 200 Energy.");
        }
        else Main.NewText("All current upgrades unlocked!");
        Refresh();
    }

    private void BuyBossPermit()
    {
        var player = Main.LocalPlayer.GetModPlayer<StudyPlayer>();
        if (StudySystem.BossPermitAvailable) { Main.NewText("A Boss Permit is already ready."); return; }
        if (player.TrySpendEnergy(100))
        {
            StudySystem.BossPermitAvailable = true;
            Main.NewText("Boss Permit purchased! Use /studyboss before a boss fight.");
        }
        else Main.NewText("You need 100 Energy.");
        Refresh();
    }

    private void Refresh()
    {
        var p = Main.LocalPlayer.GetModPlayer<StudyPlayer>();
        status.SetText($"Energy: {p.Energy}   Insight: {p.Insight}   Streak: {p.StudyStreak}   Questions: {StudySystem.Questions.Count}");
        if (StudySystem.Questions.Count > 0)
        {
            var q = StudySystem.Questions[questionIndex % StudySystem.Questions.Count];
            prompt.SetText(editorMode ? "EDITOR MODE — enter a question and four answers below." :
                $"[{q.Category}] {q.Prompt}\nA: {q.Answers[0]}     B: {q.Answers[1]}\nC: {q.Answers[2]}     D: {q.Answers[3]}");
        }
        promptInput?.SetVisibility(editorMode ? 1f : 0f, editorMode ? 1f : 0f);
        answer1Input?.SetVisibility(editorMode ? 1f : 0f, editorMode ? 1f : 0f);
        answer2Input?.SetVisibility(editorMode ? 1f : 0f, editorMode ? 1f : 0f);
        answer3Input?.SetVisibility(editorMode ? 1f : 0f, editorMode ? 1f : 0f);
        answer4Input?.SetVisibility(editorMode ? 1f : 0f, editorMode ? 1f : 0f);
    }
}
