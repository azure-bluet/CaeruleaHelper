using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/AlwaysFailWallbounceTrigger")]
public class AlwaysFailWallbounceTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    private bool Orig;
    private readonly bool Enable = data.Bool("enable", true), RevertOnLeave = data.Bool("revertOnLeave", true);

    public override void OnEnter(Player player)
    {
        Orig = CaeruleaHelperModule.Session.AlwaysFailWallbounce;
        CaeruleaHelperModule.Session.AlwaysFailWallbounce = Enable;
    }
    public override void OnLeave(Player player)
    {
        if (RevertOnLeave) CaeruleaHelperModule.Session.AlwaysFailWallbounce = Orig;
    }
}
