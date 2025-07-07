using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CaeruleaHelper.Effects;

public class BackdropLoader
{
    public static void Load()
    {
        Everest.Events.Level.OnLoadBackdrop += LoadBackdrops;
    }
    public static void Unload()
    {
        Everest.Events.Level.OnLoadBackdrop -= LoadBackdrops;
    }
    public static Backdrop LoadBackdrops(MapData map, BinaryPacker.Element child, BinaryPacker.Element above)
    {
        if (child.Name.Equals("CaeruleaHelper/CustomAscendingStars", StringComparison.OrdinalIgnoreCase))
        {
            return new CustomAscendingStars(
                child.Attr("path", "particles/caerulea/stars/"),
                Calc.HexToColor(child.Attr("color", "ffffff")),
                int.Parse(child.Attr("count", "96")),
                child.AttrFloat("speedx"),
                child.AttrFloat("speedy")
            );
        }
        else if (child.Name.Equals("CaeruleaHelper/NoiseCircleArea", StringComparison.OrdinalIgnoreCase))
        {
            return new NoiseCircleArea(
                new Vector2(
                    child.AttrFloat("centerx", 160),
                    child.AttrFloat("centery", 90)
                ),
                child.AttrFloat("radius", 20),
                Calc.HexToColor(child.Attr("color", "ffffff"))
            );
        }
        return null;
    }
}
