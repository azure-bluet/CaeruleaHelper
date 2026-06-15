using System;
using System.Linq;
using System.Reflection;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Monocle;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/CustomStarJumpBlock")]
public class CustomStarJumpBlock(EntityData data, Vector2 offset) : StarJumpBlock(data, offset)
{
    private readonly bool edges = data.Bool("edges", true), railing = data.Bool("railing", true);
    private readonly string edgeTexture = data.String("edgeTexture", "objects/starjumpBlock/");
    private readonly string railingTexture = data.String("railingTexture", "objects/starjumpBlock/");
    private static Hook hook_dashless_open = null;
    private static bool normal_hook = false;
    public static void Load()
    {
        IL.Celeste.StarJumpBlock.Awake += ModifyAwake;
        // Dashless Helper used a very bad method: just throw away orig
        // so we have to hook their hook
        var dashlessHelper = Everest.Modules.FirstOrDefault(m => m.Metadata?.Name == "DashlessHelper");
        if (dashlessHelper != null)
        {
            hook_dashless_open = new Hook(
                dashlessHelper.GetType().GetMethod("railLessMerge", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(CustomStarJumpBlock).GetMethod("IsOpenDashless", BindingFlags.NonPublic | BindingFlags.Static)
            );
            normal_hook = false;
        }
        else
        {
            hook_dashless_open = null;
            normal_hook = true;
            On.Celeste.StarJumpBlock.Open += IsOpen;
        }
    }
    public static void Unload()
    {
        IL.Celeste.StarJumpBlock.Awake -= ModifyAwake;
        if (normal_hook) On.Celeste.StarJumpBlock.Open -= IsOpen;
        hook_dashless_open?.Dispose();
        hook_dashless_open = null;
    }
    private static readonly int[] Edges = [6, 8, 10, 12, 13];
    private static readonly int[] Railing = [9, 14, 15, 16, 17];
    private static int ShouldCareRendering(Instruction instr)
    {
        foreach (int i in Edges)
            if (instr.MatchLdloc(i))
                return 1;
        foreach (int i in Railing)
            if (instr.MatchLdloc(i))
                return 2;
        return 0;
    }
    private static bool ShouldRender(StarJumpBlock entity, int type)
    {
        if (entity is CustomStarJumpBlock block)
        {
            if (type == 1) return block.edges;
            if (type == 2) return block.railing;
        }
        return true;
    }
    private static readonly string[] EdgeNames = [
        "objects/starjumpBlock/edgeH",
        "objects/starjumpBlock/edgeV",
        "objects/starjumpBlock/corner"
    ];
    private static readonly string[] RailingNames = [
        "objects/starjumpBlock/leftrailing",
        "objects/starjumpBlock/railing",
        "objects/starjumpBlock/rightrailing"
    ];
    private static int ShouldCareImage(Instruction instruction)
    {
        foreach (var str in EdgeNames)
            if (instruction.MatchLdstr(str))
                return 1;
        foreach (var str in RailingNames)
            if (instruction.MatchLdstr(str))
                return 2;
        return 0;
    }
    private static string WrapImage(string orig, int type, StarJumpBlock entity)
    {
        if (entity is CustomStarJumpBlock block)
        {
            if (type == 1) return orig.Replace("objects/starjumpBlock/", block.edgeTexture);
            if (type == 2) return orig.Replace("objects/starjumpBlock/", block.railingTexture);
        }
        return orig;
    }
    private static void ModifyAwake(ILContext ctx)
    {
        ILCursor cursor = new(ctx);
        while (cursor.TryGotoNext(MoveType.After, instr => ShouldCareImage(instr) > 0))
        {
            cursor.EmitLdcI4(ShouldCareImage(cursor.Prev));
            cursor.EmitLdarg0();
            cursor.EmitDelegate(WrapImage);
        }
        cursor = new(ctx);
        while (cursor.TryGotoNext(MoveType.Before,
            instr => ShouldCareRendering(instr) > 0,
            instr => instr.MatchCall<Entity>("Add")
        ))
        {
            cursor.EmitLdarg0();
            cursor.EmitLdcI4(ShouldCareRendering(cursor.Next));
            cursor.EmitDelegate(ShouldRender);
            ILLabel orig = ctx.DefineLabel();
            cursor.EmitBrtrue(orig);
            cursor.EmitPop();
            ILLabel after = ctx.DefineLabel();
            cursor.EmitBr(after);
            cursor.MarkLabel(orig);
            cursor.GotoNext(MoveType.After, instr => instr.MatchCall<Entity>("Add"));
            cursor.MarkLabel(after);
        }
    }
    private static bool BasicIsOpen(Entity self, float x, float y)
    {
        return !self.Scene.CollideCheck<CustomStarJumpBlock>(new Vector2(self.X + x + 4f, self.Y + y + 4f));
    }
    private static bool IsOpen(On.Celeste.StarJumpBlock.orig_Open orig, StarJumpBlock self, float x, float y)
    {
        return orig(self, x, y) && BasicIsOpen(self, x, y);
    }
    private delegate bool DashlessHelperHookOpen(
        object instance, On.Celeste.StarJumpBlock.orig_Open orig,
        StarJumpBlock self, float x, float y
    );
    private static bool IsOpenDashless(
        DashlessHelperHookOpen orig, object module,
        On.Celeste.StarJumpBlock.orig_Open obsolete, StarJumpBlock self, float x, float y
    )
    {
        return orig(module, obsolete, self, x, y) && BasicIsOpen(self, x, y);
    }
}
