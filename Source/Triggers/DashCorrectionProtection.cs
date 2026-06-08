using System;
using Microsoft.Xna.Framework;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

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
        while (cursor.TryGotoNext(MoveType.Before, instr => instr.MatchCallvirt<Player>("Die")))
        {
            // Someone told me not to do this. I'm sorry, but I have to.
            // This might cause some issues, but it works fine with me (now)
            cursor.Remove();
            cursor.EmitLdarg0();
            cursor.EmitDelegate(CheckDeath);
        }
    }
    private static bool CheckDeath(Player player, Vector2 direction, bool LongName1, bool LongName2, Spikes spike)
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

        player.Die(direction, LongName1, LongName2);
        return true;
    }
}
