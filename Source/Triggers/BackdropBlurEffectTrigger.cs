using System;
using Celeste.Mod.CaeruleaHelper.Hooks;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/BackdropBlurEffectTrigger")]
public class BackdropBlurEffectTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    private readonly string BlurTag = data.String("blurTag");
    private readonly float strength = data.Float("strength");
    public override void OnEnter(Player player)
    {
        if (BlurTag != null) BackdropRenderHook.AddBlur(BlurTag, strength);
    }
}
