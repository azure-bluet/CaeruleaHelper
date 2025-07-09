using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Effects;

public class PureColor(Color color) : Backdrop
{
    public Color Clr { get; private set; } = color;
    public override void Render(Scene scene)
    {
        Engine.Graphics.GraphicsDevice.Clear(Clr);
    }
}
