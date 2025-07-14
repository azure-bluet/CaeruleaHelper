using System;
using System.Collections.Generic;
using Celeste.Mod.CaeruleaHelper.Entities;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Triggers;

[CustomEntity("CaeruleaHelper/BronzeCollectTrigger")]
public class BronzeCollectTrigger(EntityData data, Vector2 offset) : Trigger(data, offset)
{
    public override void OnEnter(Player player)
    {
        List<BronzeBerry> list = [];
        foreach (Follower follower in player.Leader.Followers)
        {
            if (follower.Entity is BronzeBerry berry)
                list.Add(berry);
        }
        foreach (BronzeBerry berry in list) berry.OnCollect();
    }
}