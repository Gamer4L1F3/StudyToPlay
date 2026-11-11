using Terraria;
using Terraria.ModLoader;
using StudyToPlay.Common.Systems;

namespace StudyToPlay.Common.Commands;

public class StudyCommand : ModCommand
{
    public override string Command => "study";
    public override CommandType Type => CommandType.Chat;
    public override string Description => "Open the Study to Play menu.";
    public override string Usage => "/study";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (Main.netMode == Terraria.ID.NetmodeID.Server)
        {
            caller.Reply("Open the menu from your client with the P key.");
            return;
        }
        StudyToPlay.Common.UI.StudyUISystem.ToggleMenu();
    }
}

public class StudyBossCommand : ModCommand
{
    public override string Command => "studyboss";
    public override CommandType Type => CommandType.Chat;
    public override string Description => "Use your purchased Boss Permit to authorize a boss summon.";
    public override string Usage => "/studyboss";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (!StudySystem.BossPermitAvailable)
        {
            caller.Reply("You need to buy a Boss Permit in the Study menu first.");
            return;
        }
        StudySystem.BossPermitAvailable = false;
        caller.Reply("Boss Permit used! Your next boss summon is authorized.");
    }
}
