using System;
using Celeste.Mod.CaeruleaHelper.Entities;
using Iced.Intel;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Hooks;

public class NorthernLightsHook
{
    public static void Load()
    {
        IL.Celeste.NorthernLights.Strand.Reset += ModifyStrandReset;
        On.Celeste.NorthernLights.Update += HookUpdate;
    }
    public static void Unload()
    {
        IL.Celeste.NorthernLights.Strand.Reset -= ModifyStrandReset;
        On.Celeste.NorthernLights.Update -= HookUpdate;
    }
    private static void ModifyStrandReset(ILContext ctx)
    {
        ILCursor cursor = new(ctx);
        cursor.GotoNext(MoveType.After, instr => instr.MatchCall<Vector2>(".ctor"));
        cursor.EmitLdloc0();
        cursor.EmitLdarg0();
        cursor.EmitLdfld(typeof(NorthernLights.Strand).GetField("Duration"));
        cursor.EmitDelegate(OverridePositionRNG);
        cursor.EmitStloc0();
        cursor.GotoNext(
            MoveType.After,
            instr => instr.MatchLdcI4(4),
            instr => instr.MatchLdcI4(20),
            instr => true
        );
        cursor.EmitPop();
        cursor.EmitLdcI4(4);
        cursor.EmitLdcI4(20);
        cursor.EmitLdarg0();
        cursor.EmitLdfld(typeof(NorthernLights.Strand).GetField("Duration"));
        cursor.EmitLdloc3();
        cursor.EmitDelegate(GetSeededRandomNumber);
        cursor.GotoNext(
            MoveType.After,
            instr => instr.MatchLdcI4(-15),
            instr => instr.MatchLdcI4(15),
            instr => true
        );
        cursor.EmitPop();
        cursor.EmitLdcI4(-15);
        cursor.EmitLdcI4(15);
        cursor.EmitLdarg0();
        cursor.EmitLdfld(typeof(NorthernLights.Strand).GetField("Duration"));
        cursor.EmitLdloc3();
        cursor.EmitDelegate(GetSeededRandomNumber);
    }
    private static Vector2 OverridePositionRNG(Vector2 orig, float duration)
    {
        if (Engine.Scene is Level level)
        {
            Tracker tracker = level.Tracker;
            NorthernLightsHeightController controller = tracker.GetEntity<NorthernLightsHeightController>();
            if (controller != null) orig.Y = Calc.Random.Range(controller.min, controller.max);
            if (tracker.GetEntity<NorthernLightsNodeFixController>() != null)
            {
                orig.X = Calc.Random.Range(110, 210);
                // This is still not perfectly balanced unless you make the number of nodes 41, but it's so much better
                for (int i = 0; i < 20; i++)
                {
                    orig.X -= GetSeededRandomNumber(4, 20, duration, i);
                    orig.Y -= GetSeededRandomNumber(-15, 15, duration, i);
                }
            }
        }
        return orig;
    }
    private static int GetSeededRandomNumber(int min, int max, float seed, int i)
    {
        int nseed = (int)(seed * i * 65536) & 65535;
        return min + nseed % (max - min + 1);
    }
    // Hopefully this method works (it probably does)
    private static void HookUpdate(On.Celeste.NorthernLights.orig_Update orig, NorthernLights self, Scene scene)
    {
        if (self.timer == 0f && self.Visible && scene is Level level)
        {
            Tracker tracker = level.Tracker;
            if (tracker.GetEntity<NorthernLightsHeightController>() != null)
            {
                int count = self.strands.Count;
                self.strands.Clear();
                while (count-- > 0) self.strands.Add(new NorthernLights.Strand());
            }
        }
        orig(self, scene);
    }
}
