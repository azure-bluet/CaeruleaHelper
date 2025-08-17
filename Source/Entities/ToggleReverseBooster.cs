using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[Tracked]
[CustomEntity("CaeruleaHelper/ToggleReverseBooster")]
public class ToggleReverseBooster : Booster
{
    public readonly static string ToggleReverseFlag = "caeruleaHelperToggleReverseBoosterFlag";
    private readonly Sprite TwistSprite;
    private readonly string directory;
    private readonly bool global;
    private bool current;

    public ToggleReverseBooster(EntityData data, Vector2 offset) : base(data.Position + offset, data.Bool("red"))
    {
        directory = data.Attr("twistSprite", "objects/caerulea/revbooster/twist/twist").TrimEnd(['/']);
        Add(TwistSprite = new Sprite(GFX.Game, directory));
        TwistSprite.AddLoop("twist", "", .1f);
        TwistSprite.Play("twist");
        TwistSprite.CenterOrigin();
        global = data.Bool("global", false);
        current = data.Bool("initial", false);
    }
    public bool CurrentState(Level level)
    {
        return global ? level.Session.GetFlag(ToggleReverseFlag) : current;
    }
    private void Use(Player player)
    {
        bool cur = CurrentState(player.SceneAs<Level>());
        if (cur)
        {
            player.Speed *= -1f;
            player.DashDir *= -1f;
        }
        if (global) player.SceneAs<Level>().Session.SetFlag(ToggleReverseFlag, !cur);
        else current = !cur;
    }
    public override void Update()
    {
        if (CurrentState(SceneAs<Level>())) TwistSprite.SetColor(Color.White);
        else TwistSprite.SetColor(Color.Transparent);
        base.Update();
    }
    public static void HookUse(On.Celeste.Booster.orig_PlayerBoosted orig, Booster self, Player player, Vector2 direction)
    {
        orig(self, player, direction);
        if (self is ToggleReverseBooster reverse) reverse.Use(player);
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
