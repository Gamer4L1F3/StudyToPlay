using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using StudyToPlay.Common.UI;

namespace StudyToPlay.Common.UI;

[Autoload(Side = ModSide.Client)]
public class StudyUISystem : ModSystem
{
    private UserInterface userInterface;
    private StudyMenuState menu;
    private static bool visible;

    public override void Load()
    {
        if (Main.dedServ) return;
        menu = new StudyMenuState();
        menu.Activate();
        userInterface = new UserInterface();
        userInterface.SetState(menu);
        visible = false;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.dedServ) return;
        if (StudyToPlay.OpenMenuKeybind.JustPressed) ToggleMenu();
        if (visible) userInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers)
    {
        if (Main.dedServ || !visible) return;
        int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (index != -1)
            layers.Insert(index, new LegacyGameInterfaceLayer("Study To Play: Menu", () =>
            {
                userInterface.Draw(Main.spriteBatch, new GameTime());
                return true;
            }, InterfaceScaleType.UI));
    }

    public static void ToggleMenu()
    {
        visible = !visible;
        if (visible) Main.playerInventory = true;
    }

    public static void HideMenu() => visible = false;

    public override void Unload()
    {
        userInterface = null;
        menu = null;
    }
}
