using System;
using Microsoft.Xna.Framework;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Hooks;

public class DashCorrectionProtection
{
    private static int SpikeInvincibilityCooldown;
    private static Vector2 death = Vector2.Zero;
    public static void Load()
    {
        Everest.Events.Player.OnAfterUpdate += PlayerUpdate;
        IL.Celeste.Spikes.OnCollide += ModifySpikesCollideIL;
    }
    public static void Unload()
    {
        Everest.Events.Player.OnAfterUpdate -= PlayerUpdate;
        IL.Celeste.Spikes.OnCollide -= ModifySpikesCollideIL;
    }
    private static void PlayerUpdate(Player player)
    {
        if (SpikeInvincibilityCooldown > 0)
        {
            if (SpikeInvincibilityCooldown == 1
                && player.StateMachine.State != Player.StDash
                && player.StateMachine.State != Player.StRedDash)
            {
                player.Die(death);
            }
            SpikeInvincibilityCooldown--;
        }
    }
    private static void ModifySpikesCollideIL(ILContext ctx)
    {
        ILCursor cursor = new(ctx);
        // For every player.Die(), we replace it with our CheckDeath()
        while (cursor.TryGotoNext(MoveType.Before,
            instr => instr.MatchLdcI4(0),
            instr => instr.MatchLdcI4(1),
            instr => instr.MatchCallvirt<Player>("Die")
        ))
        {
            // call ShouldProtect
            cursor.EmitDup();
            cursor.EmitLdarg0();
            cursor.EmitLdarg1();
            cursor.EmitDelegate(ShouldProtect);

            // original
            ILLabel orig = ctx.DefineLabel();
            cursor.EmitBrtrue(orig);

            // if original was skipped, we have to pop the Vector2 then Player
            // however we can use the `pop` after orig to pop the Player
            cursor.EmitPop();

            // after the pop, goto after orig
            ILLabel after = ctx.DefineLabel();
            cursor.EmitBr(after);

            // now the orig
            cursor.MarkLabel(orig);

            // after orig
            cursor.TryGotoNext(MoveType.Before, instr => instr.MatchPop());
            cursor.MarkLabel(after);
        }
    }
    private static bool ShouldProtect(Vector2 direction, Spikes spike, Player player)
    {
        if (CaeruleaHelperModule.Session.SpikeCorrectionLeniency && SpikeInvincibilityCooldown % 2 == 0)
        {
            Vector2 dashdir = new(0f, 0f);
            switch (spike.Direction)
            {
                case Spikes.Directions.Up:
                    dashdir = new Vector2(0f, 1f);
                    break;
                case Spikes.Directions.Down:
                    dashdir = new Vector2(0f, -1f);
                    break;
                case Spikes.Directions.Left:
                    dashdir = new Vector2(1f, 0f);
                    break;
                case Spikes.Directions.Right:
                    dashdir = new Vector2(-1f, 0f);
                    break;
            }

            if (player.StateMachine.State == Player.StDash && player.DashDir.Equals(dashdir))
            {
                SpikeInvincibilityCooldown = 2;
                death = direction;
                return false;
            }
        }

        return true;
    }
}
