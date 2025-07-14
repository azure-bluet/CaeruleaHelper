using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Entities;

[CustomEntity("CaeruleaHelper/BronzeBerry")]
[RegisterStrawberry(tracked: false, blocksCollection: true)]
public class BronzeBerry : Strawberry
{
    private bool natural = true;
    public Sprite DefaultSprite { get; private set; }
    public Sprite GhostSprite { get; private set; }
    public BronzeBerry(EntityData data, Vector2 offset, EntityID gid) : base(data, offset, gid)
    {
        Golden = true;
        Color c1, c2, c3, c4;
        c1 = Calc.HexToColor(data.String("bronze", "ffa332"));
        c2 = Calc.HexToColor(data.String("altbronze", "de6a1d"));
        c3 = Calc.HexToColor(data.String("ghost", "ffa332"));
        c4 = Calc.HexToColor(data.String("altghost", "de6a1d"));
        P_GoldGlow = new ParticleType(P_Glow) { Color = c1, Color2 = c2 };
        P_GhostGlow = new ParticleType(P_Glow) { Color = c3, Color2 = c4 };
        string def = data.String("sprite", "Caerulea_BronzeBerry");
        string gho = data.String("ghostsprite", "Caerulea_BronzeBerryGhost");
        DefaultSprite = GFX.SpriteBank.Create(def);
        GhostSprite = GFX.SpriteBank.Create(gho);
    }
    public override void Added(Scene scene)
    {
        base.Added(scene);

        Session session = (scene as Level).Session;
        if (natural &&
            ((session.FurthestSeenLevel != session.Level && session.Deaths != 0) ||
            (!SaveData.Instance.CheatMode && !SaveData.Instance.Areas_Safe[session.Area.ID].Modes[(int)session.Area.Mode].Completed))
        ) RemoveSelf();
    }

    [Command("give_bronze", "(Caerulea Helper) gives you a bronze berry")]
    private static void GiveBronze()
    {
        if (Engine.Scene is Level level)
        {
            Player player = level.Tracker.GetEntity<Player>();
            if (player != null)
            {
                EntityData data = new()
                {
                    Position = player.Position + new Vector2(0f, -16f),
                    ID = Calc.Random.Next(),
                    Name = "CaeruleaHelper/BronzeBerry"
                };
                BronzeBerry berry = new(data, Vector2.Zero, new EntityID(level.Session.Level, data.ID));
                berry.natural = false;
                level.Add(berry);
            }
        }
    }
}
