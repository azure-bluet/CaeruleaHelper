using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/SetSpinnerInvisibleTrigger")]
public class SetSpinnerInvisibleTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    private readonly bool Value = data.Bool("value", true);
    public override void OnEnter(Player player)
    {
        CaeruleaHelperModule.Session.SetSpinnerInvisible = Value;
    }
}
