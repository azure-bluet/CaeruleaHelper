using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

public class SuperJumpHook
{
    private static Hook hook_wallsuperjump;
    // hook
    public static void Load()
    {
        On.Celeste.Player.SuperJump += SuperJump;
        IL.Celeste.Player.DashUpdate += ModifyPlayerDashIL;
        IL.Celeste.Player.RedDashUpdate += ModifyPlayerDashIL;
        IL.Celeste.Player.WallJumpCheck += ModifyWallJumpCheckIL;
        hook_wallsuperjump = new Hook(
            typeof(Player).GetProperty("SuperWallJumpAngleCheck", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true),
            typeof(SuperJumpHook).GetMethod("SuperWallJump", BindingFlags.NonPublic | BindingFlags.Static)
        );
    }
    public static void Unload()
    {
        On.Celeste.Player.SuperJump -= SuperJump;
        IL.Celeste.Player.DashUpdate -= ModifyPlayerDashIL;
        IL.Celeste.Player.RedDashUpdate -= ModifyPlayerDashIL;
        IL.Celeste.Player.WallJumpCheck -= ModifyWallJumpCheckIL;
        hook_wallsuperjump.Dispose();
    }
    private static void ModifyPlayerDashIL(ILContext ctx)
    {
        ILCursor cursor = new(ctx);
        // In dash update, jumpGraceTimer is only used for super jump condition
        while (cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdfld<Player>("jumpGraceTimer")))
        {
            cursor.EmitDelegate(JumpGraceTimerFactor);
            cursor.Emit(OpCodes.Mul);
        }
    }
    private static float JumpGraceTimerFactor()
    {
        if (CaeruleaHelperModule.Session.DisableSuper) return 0f;
        else return 1f;
    }
    private static void SuperJump(On.Celeste.Player.orig_SuperJump orig, Player self)
    {
        if (CaeruleaHelperModule.Session.AlwaysFailSuper)
        {
            float fac;
            if (CaeruleaHelperModule.Session.AllowReverseFailSuper) fac = (float)self.Facing;
            else fac = Vector2.Normalize(self.DashDir).X;
            self.Speed = new(160f * fac, 0f);
            self.Jump();
        }
        else orig(self);
    }
    // for wall super jumps, aka wallbounces
    private delegate bool orig_Player_SuperWallJumpAngleCheck(Player self);
    private static bool SuperWallJump(orig_Player_SuperWallJumpAngleCheck orig, Player self)
    {
        if (CaeruleaHelperModule.Session.DowndashWallbounce)
        {
            Vector2 dir = self.DashDir;
            if (Math.Abs(dir.X) <= 0.2f && dir.Y >= 0.75f) return true;
        }
        if (CaeruleaHelperModule.Session.AlwaysFailWallbounce) return false;
        else return orig(self);
    }
    public static float CheckWallBouncable(float Y, Player player)
    {
        if (CaeruleaHelperModule.Session.DowndashWallbounce && player.DashDir.Y == 1f) return -1f;
        return Y;
    }
    private static void ModifyWallJumpCheckIL(ILContext ctx)
    {
        // This is a hacky implementation:
        // Originally the function checks whether the player's Y dash dir is -1: `DashDir.Y==-1f`
        // So instead we replace `DashDir.Y` with `CheckWallBouncable(DashDir.Y, player)`
        ILCursor cursor = new(ctx);
        cursor.GotoNext(MoveType.After, instr => instr.MatchLdfld<Vector2>("Y"));
        cursor.Emit(OpCodes.Ldarg_0);
        cursor.EmitDelegate(CheckWallBouncable);
    }
}
