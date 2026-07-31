using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/NorthernLightsHeightController")]
public class NorthernLightsHeightController(EntityData data, Vector2 offset) : Entity(data.Position + offset)
{
    public readonly int min = data.Int("minHeight", 40), max = data.Int("maxHeight", 90);
    // The real logic is in NorthernLightsHook.
    // If there are multiple controllers, only one will work.
}
