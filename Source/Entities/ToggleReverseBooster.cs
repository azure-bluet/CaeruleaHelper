using System;
using System.Runtime.CompilerServices;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/ToggleReverseBooster")]
public class ToggleReverseBooster : Booster
{
    public readonly static string ToggleReverseFlag = "caeruleaHelperToggleReverseBoosterFlag";
    private readonly Sprite TwistSprite, RedSprite, GreenSprite;
    private readonly string directory;
    private readonly bool global, initial, toggleRedGreen, toggleReverse;
    private bool current;

    public ToggleReverseBooster(EntityData data, Vector2 offset) : base(data.Position + offset, data.Bool("red"))
    {
        directory = data.Attr("twistSprite", "objects/caerulea/revbooster/twist/twist").TrimEnd(['/']);
        Add(TwistSprite = new Sprite(GFX.Game, directory));
        TwistSprite.AddLoop("twist", "", .1f);
        TwistSprite.Play("twist");
        TwistSprite.CenterOrigin();
        global = data.Bool("global", false);
        initial = data.Bool("initial", false);
        toggleRedGreen = data.Bool("toggleType", false);
        toggleReverse = data.Bool("toggleReverse", true);
        current = false;
        Remove(sprite);
        Add(RedSprite = GFX.SpriteBank.Create("boosterRed"));
        Add(GreenSprite = GFX.SpriteBank.Create("booster"));
        if (this.red) sprite = RedSprite;
        else sprite = GreenSprite;
    }
    public bool CurrentState(Level level)
    {
        return global ? level.Session.GetFlag(ToggleReverseFlag) : current;
    }
    private void Use(Player player)
    {
        bool cur = CurrentState(player.SceneAs<Level>());
        if (cur ^ initial)
        {
            player.Speed *= -1f;
            player.DashDir *= -1f;
        }
        if (toggleReverse)
        {
            if (global) player.SceneAs<Level>().Session.SetFlag(ToggleReverseFlag, !cur);
            else current = !cur;
        }
    }
    public override void Update()
    {
        if (CurrentState(SceneAs<Level>()) ^ initial) TwistSprite.Visible = true;
        else TwistSprite.Visible = false;
        RedSprite.Visible = red;
        GreenSprite.Visible = !red;
        base.Update();
    }
    public static void HookUse(On.Celeste.Booster.orig_PlayerBoosted orig, Booster self, Player player, Vector2 direction)
    {
        orig(self, player, direction);
        if (self is ToggleReverseBooster reverse) reverse.Use(player);
    }
    public static void HookRespawn(On.Celeste.Booster.orig_Respawn orig, Booster self)
    {
        if (self is ToggleReverseBooster reverse)
        {
            if (reverse.toggleRedGreen)
            {
                reverse.red = !reverse.red;
                if (reverse.red)
                {
                    reverse.sprite = reverse.RedSprite;
                    reverse.particleType = P_BurstRed;
                }
                else
                {
                    reverse.sprite = reverse.GreenSprite;
                    reverse.particleType = P_Burst;
                }
            }
        }
        orig(self);
    }
    public static void Load()
    {
        On.Celeste.Booster.PlayerBoosted += HookUse;
        On.Celeste.Booster.Respawn += HookRespawn;
    }
    public static void Unload()
    {
        On.Celeste.Booster.PlayerBoosted -= HookUse;
        On.Celeste.Booster.Respawn -= HookRespawn;
    }
}
