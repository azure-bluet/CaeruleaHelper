using System;
using Microsoft.Xna.Framework;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

public class DashCorrectionProtection
{
    private static int SpikeInvincibilityCooldown;
    public static void Load()
    {
        Everest.Events.Player.OnBeforeUpdate += PlayerUpdate;
        IL.Celeste.Spikes.OnCollide += ModifySpikesCollideIL;
    }
    public static void Unload()
    {
        Everest.Events.Player.OnBeforeUpdate -= PlayerUpdate;
        IL.Celeste.Spikes.OnCollide -= ModifySpikesCollideIL;
    }
    private static void PlayerUpdate(Player player)
    {
        if (SpikeInvincibilityCooldown > 0)
        {
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
    private static int CheckDeath(Player player, Vector2 direction, bool LongName1, bool LongName2, Spikes spike)
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
                return 0;
            }
        }

        // We return an int because the original CIL contains a `pop`
        // and im lazy to remove it lol
        player.Die(direction, LongName1, LongName2);
        return 313;
    }
}
