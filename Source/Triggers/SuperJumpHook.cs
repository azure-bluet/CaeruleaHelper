using System;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Monocle;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

public class SuperJumpHook
{
    // hook
    public static void Load()
    {
        On.Celeste.Player.SuperJump += SuperJump;
        IL.Celeste.Player.DashUpdate += ModifyPlayerDashIL;
        IL.Celeste.Player.RedDashUpdate += ModifyPlayerDashIL;
    }
    public static void Unload()
    {
        On.Celeste.Player.SuperJump -= SuperJump;
        IL.Celeste.Player.DashUpdate -= ModifyPlayerDashIL;
        IL.Celeste.Player.RedDashUpdate -= ModifyPlayerDashIL;
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
}
