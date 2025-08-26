using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/QuarterRotateBooster")]
public class QuarterRotateBooster : Booster
{
    private readonly Sprite TwistSprite;
    private readonly string directory;
    private readonly float radians;

    public QuarterRotateBooster(EntityData data, Vector2 offset) : base(data.Position + offset, data.Bool("red"))
    {
        directory = data.Attr("twistSprite", "objects/caerulea/revbooster/twist/twist").TrimEnd(['/']);
        Add(TwistSprite = new Sprite(GFX.Game, directory));
        TwistSprite.AddLoop("twist", "", .1f);
        TwistSprite.Play("twist");
        TwistSprite.CenterOrigin();
        radians = Calc.DegToRad * data.Float("degrees", 90);
    }
    private void Use(Player player)
    {
        player.Speed = player.Speed.Rotate(radians);
        player.DashDir = player.DashDir.Rotate(radians);
    }
    public static void HookUse(On.Celeste.Booster.orig_PlayerBoosted orig, Booster self, Player player, Vector2 direction)
    {
        orig(self, player, direction);
        if (self is QuarterRotateBooster qr) qr.Use(player);
    }
    public static void Load()
    {
        On.Celeste.Booster.PlayerBoosted += HookUse;
    }
    public static void Unload()
    {
        On.Celeste.Booster.PlayerBoosted -= HookUse;
    }
}
