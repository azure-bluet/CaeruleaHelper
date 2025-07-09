using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Effects;

public class CustomAscendingStars : Backdrop
{
    public static void Load()
    {
    }
    public static void Unload()
    { }
    public struct Star
    {
        public MTexture Texture;
        public Vector2 position;
    }
    private static float Mod(float n, float p)
    {
        return (n % p + p) % p;
    }
    private static Vector2 Mod(Vector2 orig, Viewport viewport)
    {
        int W = viewport.Width + 10, H = viewport.Height + 10;
        return new Vector2(
            Mod(orig.X, W) - 5,
            Mod(orig.Y, H) - 5
        );
    }
    public Star[] Stars { get; private set; }
    public Color StarColor { get; private set; }
    public CustomAscendingStars(string texture, Color clr, int count, float speedX, float speedY)
    {
        Speed = new Vector2(speedX, speedY);
        StarColor = clr;
        Stars = new Star[count];
        List<MTexture> textures = GFX.Game.GetAtlasSubtextures(texture);
        for (int i = 0; i < Stars.Length; i++)
        {
            MTexture mt = Random.Shared.Choose(textures);
            Vector2 vec = new(Random.Shared.NextInt64() % 16384, Random.Shared.NextInt64() % 16384);
            Stars[i].Texture = mt;
            Stars[i].position = vec;
        }
    }
    public override void Update(Scene scene)
    {
        base.Update(scene);
        // Update stars
        for (int i = 0; i < Stars.Length; i++)
            Stars[i].position += Speed / 60;
    }
    public override void Render(Scene scene)
    {
        Vector2 vcc = (scene as Level).Camera.Position.Floor();
        Vector2 scr = (Position - vcc * Scroll).Floor();
        // Render stars
        for (int i = 0; i < Stars.Length; i++)
        {
            Vector2 pos = Mod(Stars[i].position + scr, (scene as Level).Camera.Viewport);
            Stars[i].Texture.DrawCentered(pos, StarColor);
        }
    }
}
