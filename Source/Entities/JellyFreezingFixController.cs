using System;
using Celeste.Mod.Entities;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/JellyFreezingFixController")]
public class JellyFreezingFixController : Entity
{
    // Actually does nothing. The real logic is in ActorHook.
}
