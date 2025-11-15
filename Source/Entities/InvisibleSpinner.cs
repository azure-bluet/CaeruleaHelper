using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/InvisibleSpinnerController")]
public class InvisibleSpinnerController : Entity
{
    private static void HookCreateSprit
        (On.Celeste.CrystalStaticSpinner.orig_CreateSprites orig, CrystalStaticSpinner self)
    {
        if (self.Scene.Tracker.GetEntity<InvisibleSpinnerController>() == null)
            orig(self);
    }
    public static void Load()
    {
        On.Celeste.CrystalStaticSpinner.CreateSprites += HookCreateSprit;
    }
    public static void Unload()
    {
        On.Celeste.CrystalStaticSpinner.CreateSprites -= HookCreateSprit;
    }
}
