using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/InvisibleSpinnerController")]
public class InvisibleSpinnerController : Entity
{
    private static void HookCreateSprite
        (On.Celeste.CrystalStaticSpinner.orig_CreateSprites orig, CrystalStaticSpinner self)
    {
        if (self.Scene.Tracker.GetEntity<InvisibleSpinnerController>() == null
                && CaeruleaHelperModule.Session.SetSpinnerInvisible == false)
            orig(self);
    }
    public static void Load()
    {
        On.Celeste.CrystalStaticSpinner.CreateSprites += HookCreateSprite;
    }
    public static void Unload()
    {
        On.Celeste.CrystalStaticSpinner.CreateSprites -= HookCreateSprite;
    }
}
