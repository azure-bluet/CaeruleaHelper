using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/DisableSuperTrigger")]
public class DisableSuperTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    private bool Orig;
    private readonly bool Enable = data.Bool("enable", true), RevertOnLeave = data.Bool("revertOnLeave", true);

    public override void OnEnter(Player player)
    {
        Orig = CaeruleaHelperModule.Session.DisableSuper;
        CaeruleaHelperModule.Session.DisableSuper = Enable;
    }
    public override void OnLeave(Player player)
    {
        if (RevertOnLeave) CaeruleaHelperModule.Session.DisableSuper = Orig;
    }
}