using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/AlwaysFailSuperTrigger")]
public class AlwaysFailSuperTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    private bool Orig;
    private readonly bool Enable = data.Bool("enable", true), RevertOnLeave = data.Bool("revertOnLeave", true), AllowReverse = data.Bool("allowReverse", true);

    public override void OnEnter(Player player)
    {
        Orig = CaeruleaHelperModule.Session.AlwaysFailSuper;
        CaeruleaHelperModule.Session.AlwaysFailSuper = Enable;
        CaeruleaHelperModule.Session.AllowReverseFailSuper = AllowReverse;
    }
    public override void OnLeave(Player player)
    {
        if (RevertOnLeave) CaeruleaHelperModule.Session.AlwaysFailSuper = Orig;
    }
}