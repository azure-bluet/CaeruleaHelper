using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Effects;

public class NoiseCircleArea : Backdrop
{
    public const int DIV = 4000, MSIZE = 8;
    public const double EPSILON = 2 * Math.PI / DIV;
    private VirtualRenderTarget Buffer = null;
    private readonly Vector2 Center;
    private readonly float Radius;
    private readonly Color color;
    public struct TFP
    {
        public float mult;
        public float cons;
    }
    private readonly double[] fs = new double[DIV];
    public NoiseCircleArea(Vector2 center, float radius, Color color)
    {
        Center = center;
        Radius = radius;
        this.color = color;
        TFP[] sm = new TFP[MSIZE], cm = new TFP[MSIZE];
        for (int i = 0; i < MSIZE; i++)
        {
            sm[i].mult = Random.Shared.Range(-1 / MSIZE, 1 / MSIZE);
            cm[i].mult = Random.Shared.Range(-1 / MSIZE, 1 / MSIZE);
            sm[i].cons = Random.Shared.Range(0f, (float)(2 * Math.PI));
            cm[i].cons = Random.Shared.Range(0f, (float)(2 * Math.PI));
        }
        for (int i = 0; i < DIV; i++)
        {
            double theta = EPSILON * i;
            fs[i] = 0;
            for (int j = 0; j < MSIZE; j++)
                fs[i] += Math.Sin((j + 1) * theta + sm[j].cons) * sm[j].mult
                       + Math.Cos((j + 1) * theta + cm[j].cons) * cm[j].mult;
        }
    }
    public override void BeforeRender(Scene scene)
    {
        Viewport vp = (scene as Level).Camera.Viewport;
        if (Buffer == null || Buffer.IsDisposed) Buffer = VirtualContent.CreateRenderTarget("Noise Circle Area", vp.Width, vp.Height);
        Engine.Graphics.GraphicsDevice.SetRenderTarget(Buffer);
        Engine.Graphics.GraphicsDevice.Clear(Color.Transparent);
        Draw.SpriteBatch.Begin();
        for (int i = 0; i < DIV; i++)
        {
            double theta = EPSILON * i;
            float nradius = (float)(Radius + Radius / 8 * fs[i]);
            Vector2 nv = new((float)Math.Cos(theta), (float)Math.Sin(theta)); nv *= nradius;
            Draw.LineAngle(Center, (float)theta, nradius, color, 2);
        }
        Draw.SpriteBatch.End();
    }
    public override void Ended(Scene scene)
    {
        if (Buffer != null)
        {
            Buffer.Dispose();
            Buffer = null;
        }
    }
    public override void Render(Scene scene)
    {
        if (Buffer != null && !Buffer.IsDisposed)
        {
            Vector2 vector = new(Buffer.Width, Buffer.Height);
            Draw.SpriteBatch.Draw((RenderTarget2D)Buffer, vector, Color.White);
        }
    }
}
