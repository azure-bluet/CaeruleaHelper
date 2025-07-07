using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

public class DashSpeedHook
{
    private static ILHook DashCoroutineHook;
    public static void Load()
    {
        DashCoroutineHook = new ILHook(
            typeof(Player).GetMethod("DashCoroutine", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetStateMachineTarget(), ModifyDashCoroutineIL
        );
    }
    public static void Unload()
    {
        DashCoroutineHook.Dispose();
    }
    private static void ModifyDashCoroutineIL(ILContext ctx)
    {
        ILCursor cursor = new(ctx);

        cursor.GotoNext(MoveType.After, instr => instr.MatchStfld<Player>("AutoJumpTimer"));
        cursor.GotoNext(MoveType.After, instr => instr.MatchLdfld<Vector2>("Y"));
        cursor.EmitDelegate(PositiveINF);
        cursor.Emit(OpCodes.Add);

        cursor.GotoNext(MoveType.After, instr => instr.MatchLdcR4(0f));
        cursor.GotoNext(MoveType.After, instr => instr.MatchLdcR4(0f));
        cursor.EmitDelegate(PositiveINF);
        cursor.Emit(OpCodes.Sub);
    }
    private static float PositiveINF()
    {
        return CaeruleaHelperModule.Session.NoDashSpeedReset
            ? float.PositiveInfinity
            : 0;
    }
}
