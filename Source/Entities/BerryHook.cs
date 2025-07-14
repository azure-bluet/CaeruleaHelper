using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Monocle;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Entities;

public class BerryHook
{
    public static void Load()
    {
        IL.Celeste.Strawberry.Added += ModSprite;
    }
    public static void Unload()
    {
        IL.Celeste.Strawberry.Added -= ModSprite;
    }
    public static FieldReference FindReferenceToThisInCoroutine(ILCursor cursor)
    {
        // coroutines are cursed and references to "this" are actually references to this.<>4__this
        cursor.GotoNext(instr => instr.OpCode == OpCodes.Ldfld && (instr.Operand as FieldReference).Name == "<>4__this");
        FieldReference refToThis = cursor.Next.Operand as FieldReference;
        cursor.Index = 0;
        return refToThis;
    }
    private static void ModSprite(ILContext il)
    {
        ILCursor cursor = new(il);

        // catch the moment where the sprite is added to the entity
        if (cursor.TryGotoNext(
            instr => instr.MatchLdarg(0),
            instr => instr.MatchLdfld<Strawberry>("sprite"),
            instr => instr.MatchCall<Entity>("Add")))
        {
            cursor.Index++;
            FieldReference strawberrySprite = cursor.Next.Operand as FieldReference;
            cursor.Emit(OpCodes.Ldarg_0); // for stfld
            cursor.Index++;
            cursor.Emit(OpCodes.Ldarg_0); // for the delegate call
            cursor.EmitDelegate(HookSprite);
            cursor.Emit(OpCodes.Stfld, strawberrySprite);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, strawberrySprite);
        }
    }
    private static Sprite HookSprite(Sprite orig, Strawberry berry)
    {
        if (berry is BronzeBerry bronze) return SaveData.Instance.CheckStrawberry(bronze.ID) ? bronze.GhostSprite : bronze.DefaultSprite;
        else return orig;
    }
}
