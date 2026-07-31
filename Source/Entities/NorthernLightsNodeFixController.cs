using System;
using Celeste.Mod.Entities;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/NorthernLightsNodeFixController")]
public class NorthernLightsNodeFixController : Entity
{
    // Actually does nothing. The real logic is in NorthernLightsHook.
}
