using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Hooks;

public class HookUtil
{
    public static void OutputIL(ILContext ctx)
    {
        // Banana watch hates this, but my discord account was banned so I don't care about banana-watch
        foreach (var instr in ctx.Instrs)
            Console.WriteLine($"{instr.Offset:X4}: {instr.OpCode} {instr.Operand}");
    }
}
