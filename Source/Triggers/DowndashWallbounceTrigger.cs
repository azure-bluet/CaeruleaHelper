using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/DowndashWallbounceTrigger")]
public class DowndashWallbounceTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    private bool Orig;
    private readonly bool Enable = data.Bool("enable", true), RevertOnLeave = data.Bool("revertOnLeave", true);
    private readonly float horizontalSpeedMultiplier = data.Float("horizontalSpeedMultiplier", 1f),
                           verticalSpeedMultiplier = data.Float("verticalSpeedMultiplier", 1f),
                           jumpTimerMultiplier = data.Float("jumpTimerMultiplier", 1f);
    public override void OnEnter(Player player)
    {
        Orig = CaeruleaHelperModule.Session.DowndashWallbounce;
        CaeruleaHelperModule.Session.DowndashWallbounce = Enable;
        CaeruleaHelperModule.Session.DowndashWallbounceHorizontalSpeedMultiplier = horizontalSpeedMultiplier;
        CaeruleaHelperModule.Session.DowndashWallbounceVerticalSpeedMultiplier = verticalSpeedMultiplier;
        CaeruleaHelperModule.Session.DowndashWallbounceJumpTimerMultiplier = jumpTimerMultiplier;
    }
    public override void OnLeave(Player player)
    {
        if (RevertOnLeave) CaeruleaHelperModule.Session.DowndashWallbounce = Orig;
    }
}
