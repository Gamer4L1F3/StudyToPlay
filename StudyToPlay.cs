using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;

namespace StudyToPlay;

public class StudyToPlay : Mod
{
    internal static ModKeybind OpenMenuKeybind;

    public override void Load()
    {
        OpenMenuKeybind = KeybindLoader.RegisterKeybind(this, "Open Study Menu", Keys.P);
    }

    public override void Unload()
    {
        OpenMenuKeybind = null;
    }
}
