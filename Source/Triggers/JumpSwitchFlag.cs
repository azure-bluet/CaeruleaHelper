// I don't know where to put this

namespace Celeste.Mod.CaeruleaHelper.Triggers;

public class JumpSwitchFlag
{
    public readonly static string JumpSwitchFlagName = "caeruleaJumpSwitchFlag";
    private static void Invert(Level level)
    {
        Session session = level.Session;
        if (session.GetFlag(JumpSwitchFlagName)) session.SetFlag(JumpSwitchFlagName, false);
        else session.SetFlag(JumpSwitchFlagName);
    }
    private static void HookHiccupJump(On.Celeste.Player.orig_HiccupJump orig, Player self)
    {
        Invert(self.SceneAs<Level>());
        orig(self);
    }
    private static void HookJump(On.Celeste.Player.orig_Jump orig, Player self, bool particles, bool playSfx)
    {
        Invert(self.SceneAs<Level>());
        orig(self, particles, playSfx);
    }
    private static void HookSuperJump(On.Celeste.Player.orig_SuperJump orig, Player self)
    {
        Invert(self.SceneAs<Level>());
        orig(self);
    }
    private static void HookWallJump(On.Celeste.Player.orig_WallJump orig, Player self, int dir)
    {
        Invert(self.SceneAs<Level>());
        orig(self, dir);
    }
    private static void HookSuperWallJump(On.Celeste.Player.orig_SuperWallJump orig, Player self, int dir)
    {
        Invert(self.SceneAs<Level>());
        orig(self, dir);
    }
    public static void Load()
    {
        On.Celeste.Player.HiccupJump += HookHiccupJump;
        On.Celeste.Player.Jump += HookJump;
        On.Celeste.Player.SuperJump += HookSuperJump;
        On.Celeste.Player.WallJump += HookWallJump;
        On.Celeste.Player.SuperWallJump += HookSuperWallJump;
    }
    public static void Unload()
    {
        On.Celeste.Player.HiccupJump -= HookHiccupJump;
        On.Celeste.Player.Jump -= HookJump;
        On.Celeste.Player.SuperJump -= HookSuperJump;
        On.Celeste.Player.WallJump -= HookWallJump;
        On.Celeste.Player.SuperWallJump -= HookSuperWallJump;
    }
}
