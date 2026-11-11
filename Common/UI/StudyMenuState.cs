using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using System;
using StudyToPlay.Common.Players;
using StudyToPlay.Common.Systems;

namespace StudyToPlay.Common.UI;

public class StudyMenuState : UIState
{
    private UIPanel panel;
    private int questionIndex;
    private bool editorMode;
    private UITextBox promptInput;
    private UITextBox answer1Input;
    private UITextBox answer2Input;
    private UITextBox answer3Input;
    private UITextBox answer4Input;
    private UITextBox categoryInput;
    private int selectedCorrectAnswer;
    private bool editingExisting;

    public override void OnInitialize() => BuildMainMenu();

    private void PreparePanel(string heading)
    {
        RemoveAllChildren();
        panel = new UIPanel();
        panel.Width.Set(660, 0);
        panel.Height.Set(540, 0);
        panel.HAlign = 0.5f;
        panel.VAlign = 0.5f;
        Append(panel);

        var title = new UIText(heading, 1.15f);
        title.HAlign = 0.5f;
        title.Top.Set(12, 0);
        panel.Append(title);
    }

    private void AddButton(string label, int column, int top, Action action, int width = 145)
    {
        var button = new UITextPanel<string>(label);
        button.Width.Set(width, 0);
        button.Height.Set(38, 0);
        button.Left.Set(18 + column * 155, 0);
        button.Top.Set(top, 0);
        button.OnLeftClick += (_, _) => action();
        panel.Append(button);
    }

    private void AddText(string value, float left, float top, float width = 600)
    {
        var text = new UIText(value);
        text.Left.Set(left, 0);
        text.Top.Set(top, 0);
        text.Width.Set(width, 0);
        panel.Append(text);
    }

    private void BuildMainMenu()
    {
        editorMode = false;
        PreparePanel("STUDY TO PLAY");
        var player = Main.LocalPlayer.GetModPlayer<StudyPlayer>();
        AddText($"Energy: {player.Energy}     Insight: {player.Insight}     Streak: {player.StudyStreak}", 22, 52);
        AddText($"Global question bank: {StudySystem.Questions.Count}/500 questions", 22, 78);

        if (StudySystem.Questions.Count > 0)
        {
            questionIndex = Math.Clamp(questionIndex, 0, StudySystem.Questions.Count - 1);
            var q = StudySystem.Questions[questionIndex];
            AddText($"Question {questionIndex + 1}: [{q.Category}] {q.Prompt}", 22, 125, 610);
            for (int i = 0; i < 4; i++)
            {
                int answer = i;
                var label = $"{(char)('A' + i)}: {q.Answers[i]}";
                AddButton(label, i % 2, 180 + (i / 2) * 55, () => Answer(answer), 295);
            }
            AddButton("Previous", 0, 310, () => { questionIndex = (questionIndex - 1 + StudySystem.Questions.Count) % StudySystem.Questions.Count; BuildMainMenu(); });
            AddButton("Next", 1, 310, () => { questionIndex = (questionIndex + 1) % StudySystem.Questions.Count; BuildMainMenu(); });
        }
        else
        {
            AddText("Your question bank is empty. Open the editor to create your first question.", 22, 140, 610);
        }

        AddButton("Question Manager", 0, 380, BuildQuestionManager, 145);
        AddButton("Upgrades", 1, 380, BuyUpgrade, 145);
        AddButton("Boss Permit: 100 Energy", 2, 380, BuyBossPermit, 145);
        AddButton("Close", 3, 445, () => StudyUISystem.HideMenu(), 145);
    }

    private void BuildQuestionManager()
    {
        editorMode = true;
        PreparePanel("GLOBAL QUESTION MANAGER");
        AddText("Changes here are shared across every world on this computer.", 22, 50);
        AddText($"Saved questions: {StudySystem.Questions.Count}/500", 22, 78);

        var listTop = 112;
        int start = Math.Max(0, Math.Min(questionIndex - 3, Math.Max(0, StudySystem.Questions.Count - 7)));
        for (int i = start; i < Math.Min(StudySystem.Questions.Count, start + 7); i++)
        {
            int index = i;
            var q = StudySystem.Questions[i];
            AddButton($"{i + 1}. {Shorten(q.Prompt, 34)}", 0, listTop + (i - start) * 43,
                () => { questionIndex = index; BuildQuestionManager(); }, 450);
            AddButton("Edit", 3, listTop + (i - start) * 43,
                () => OpenEditor(index), 100);
        }

        AddButton("New Question", 0, 435, () => OpenEditor(-1), 145);
        AddButton("Back", 3, 435, BuildMainMenu, 145);
    }

    private static string Shorten(string value, int max) =>
        value.Length <= max ? value : value.Substring(0, max - 3) + "...";

    private void OpenEditor(int index)
    {
        editingExisting = index >= 0;
        questionIndex = index;
        selectedCorrectAnswer = 0;
        PreparePanel(editingExisting ? "EDIT QUESTION" : "CREATE QUESTION");

        promptInput = MakeInput("Question prompt", 22, 65, 610);
        categoryInput = MakeInput("Category (e.g. Science, Math, History)", 22, 112, 610);
        answer1Input = MakeInput("Answer A", 22, 175, 295);
        answer2Input = MakeInput("Answer B", 337, 175, 295);
        answer3Input = MakeInput("Answer C", 22, 230, 295);
        answer4Input = MakeInput("Answer D", 337, 230, 295);

        if (editingExisting)
        {
            var q = StudySystem.Questions[index];
            promptInput.SetText(q.Prompt);
            categoryInput.SetText(q.Category);
            answer1Input.SetText(q.Answers[0]);
            answer2Input.SetText(q.Answers[1]);
            answer3Input.SetText(q.Answers[2]);
            answer4Input.SetText(q.Answers[3]);
            selectedCorrectAnswer = q.CorrectIndex;
        }

        AddText("Choose the correct answer:", 22, 290);
        for (int i = 0; i < 4; i++)
        {
            int answer = i;
            AddButton($"Correct: {(char)('A' + i)}", i, 325,
                () => { selectedCorrectAnswer = answer; RefreshCorrectAnswerButtons(); }, 145);
        }
        AddText("Selected correct answer: " + (char)('A' + selectedCorrectAnswer), 22, 372);
        AddButton("Save Question", 0, 425, SaveQuestion, 145);
        if (editingExisting)
            AddButton("Delete", 1, 425, DeleteQuestion, 145);
        AddButton("Cancel", 3, 425, BuildQuestionManager, 145);
    }

    private UITextBox MakeInput(string hint, int left, int top, int width)
    {
        var input = new UITextBox(hint);
        input.Width.Set(width, 0);
        input.Height.Set(38, 0);
        input.Left.Set(left, 0);
        input.Top.Set(top, 0);
        input.SetTextMaxLength(180);
        panel.Append(input);
        return input;
    }

    private void RefreshCorrectAnswerButtons()
    {
        Main.NewText("Correct answer set to " + (char)('A' + selectedCorrectAnswer) + ".");
    }

    private void SaveQuestion()
    {
        var prompt = promptInput.CurrentString.Trim();
        var answers = new[] { answer1Input.CurrentString.Trim(), answer2Input.CurrentString.Trim(),
            answer3Input.CurrentString.Trim(), answer4Input.CurrentString.Trim() };
        if (string.IsNullOrWhiteSpace(prompt) || Array.Exists(answers, string.IsNullOrWhiteSpace))
        {
            Main.NewText("Fill in the question and all four answers first.");
            return;
        }

        var category = string.IsNullOrWhiteSpace(categoryInput.CurrentString) ? "General" : categoryInput.CurrentString.Trim();
        var question = new StudyQuestion(prompt, answers, selectedCorrectAnswer, category);
        if (editingExisting)
            StudyQuestionBank.Replace(questionIndex, question);
        else
        {
            if (StudySystem.Questions.Count >= 500)
            {
                Main.NewText("The global question bank is full (500 questions).");
                return;
            }
            StudyQuestionBank.Add(question);
            questionIndex = StudySystem.Questions.Count - 1;
        }
        Main.NewText("Question saved to your global question bank!");
        BuildQuestionManager();
    }

    private void DeleteQuestion()
    {
        StudyQuestionBank.Delete(questionIndex);
        questionIndex = Math.Max(0, questionIndex - 1);
        Main.NewText("Question deleted from the global bank.");
        BuildQuestionManager();
    }

    private void Answer(int selected)
    {
        if (StudySystem.Questions.Count == 0) return;
        var player = Main.LocalPlayer.GetModPlayer<StudyPlayer>();
        var q = StudySystem.Questions[questionIndex];
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
        BuildMainMenu();
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
        BuildMainMenu();
    }

    private void BuyBossPermit()
    {
        var player = Main.LocalPlayer.GetModPlayer<StudyPlayer>();
        if (StudySystem.BossPermitAvailable) { Main.NewText("A Boss Permit is already ready."); return; }
        if (player.TrySpendEnergy(100))
        {
            StudySystem.BossPermitAvailable = true;
            Main.NewText("Boss Permit purchased! Use /studyboss to consume it.");
        }
        else Main.NewText("You need 100 Energy.");
        BuildMainMenu();
    }
}
