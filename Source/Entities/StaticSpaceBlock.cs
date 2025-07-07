using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[CustomEntity("CaeruleaHelper/StaticSpaceBlock")]
public class StaticSpaceBlock(EntityData data, Vector2 offset) : FloatySpaceBlock(data.Position + offset, data.Width, data.Height, data.Char("tiletype", '3'), true)
{
    public override void Update()
    {
        base.Update();
        sineWave = 0f;
    }
}
