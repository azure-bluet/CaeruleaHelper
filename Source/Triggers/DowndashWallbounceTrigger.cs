using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/DowndashWallbounceTrigger")]
public class DowndashWallbounceTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    private bool Orig;
    private readonly bool Enable = data.Bool("enable", true), RevertOnLeave = data.Bool("revertOnLeave", true);

    public override void OnEnter(Player player)
    {
        Orig = CaeruleaHelperModule.Session.DowndashWallbounce;
        CaeruleaHelperModule.Session.DowndashWallbounce = Enable;
    }
    public override void OnLeave(Player player)
    {
        if (RevertOnLeave) CaeruleaHelperModule.Session.DowndashWallbounce = Orig;
    }
}
