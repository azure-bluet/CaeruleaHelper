using System;
using System.Collections.Generic;
using Celeste.Mod.CaeruleaHelper.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Hooks;

public class ActorHook
{
    private static bool DeadPlayer(Actor actor)
    {
        Scene scene = Engine.Scene;
        if (scene != null && scene is Level level)
        {
            if (level.Tracker.GetEntities<JellyFreezingFixController>().Count == 0) return false;
            if (actor == null) return true;
            if (actor is Player player)
            {
                if (player.Dead)
                    return true;
            }
        }
        return false;
    }
    private static bool DeadPlayerOnGroundInt(On.Celeste.Actor.orig_OnGround_int orig, Actor actor, int downCheck)
    {
        if (DeadPlayer(actor)) return false;
        return orig(actor, downCheck);
    }
    private static bool DeadPlayerOnGroundVec(On.Celeste.Actor.orig_OnGround_Vector2_int orig, Actor actor, Vector2 at, int downCheck)
    {
        if (DeadPlayer(actor)) return false;
        return orig(actor, at, downCheck);
    }
    public static void Load()
    {
        On.Celeste.Actor.OnGround_int += DeadPlayerOnGroundInt;
        On.Celeste.Actor.OnGround_Vector2_int += DeadPlayerOnGroundVec;
    }
    public static void Unload()
    {
        On.Celeste.Actor.OnGround_int -= DeadPlayerOnGroundInt;
        On.Celeste.Actor.OnGround_Vector2_int -= DeadPlayerOnGroundVec;
    }
}
