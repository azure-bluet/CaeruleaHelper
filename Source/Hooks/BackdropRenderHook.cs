using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using MonoMod.Cil;

namespace Celeste.Mod.CaeruleaHelper.Hooks;

public class BackdropRenderHook
{
    public static void Load()
    {
        IL.Celeste.BackdropRenderer.Render += ModifyBackdropRenderer;
        if (Engine.Scene is Level)
        {
            // Ensure the buffers are correct
            EnsureBuffer();
        }
    }
    private static void EnsureBuffer()
    {
        mybuf = EnsureBuffer(mybuf, "caerulea_blur_buffer");
        temp = EnsureBuffer(temp, "caerulea_blur_temp");
    }
    private static int GetWidth()
    {
        return GameplayBuffers.Level.Width;
    }
    private static int GetHeight()
    {
        return GameplayBuffers.Level.Height;
    }
    private static VirtualRenderTarget EnsureBuffer(VirtualRenderTarget buf, string name)
    {
        if (buf == null || buf.Width != GetWidth() || buf.Height != GetHeight())
            return VirtualContent.CreateRenderTarget(name, GetWidth(), GetHeight());
        else return buf;
    }
    public static void Unload()
    {
        IL.Celeste.BackdropRenderer.Render -= ModifyBackdropRenderer;
        mybuf?.Dispose(); mybuf = null;
        temp?.Dispose(); temp = null;
    }
    private static void ModifyBackdropRenderer(ILContext il)
    {
        ILCursor cursor = new(il);
        while (cursor.TryGotoNext(MoveType.Before,
            instr => instr.MatchCallvirt<Backdrop>("Render")))
        {
            // check if it should be taken
            cursor.EmitPop();
            cursor.EmitDup();
            cursor.EmitDelegate(ShouldBeTakenByCaerulea);
            ILLabel orig = il.DefineLabel();
            cursor.EmitBrfalse(orig);

            // call our renderer, then go after orig
            cursor.EmitLdarg0();
            cursor.EmitLdarg1();
            cursor.EmitLdloc0();
            cursor.EmitDelegate(RenderBackdrop);
            ILLabel after = il.DefineLabel();
            cursor.EmitBr(after);

            // this is orig
            cursor.MarkLabel(orig);
            cursor.EmitLdarg1();
            cursor.TryGotoNext(MoveType.After, instr => instr.MatchCallvirt<Backdrop>("Render"));
            cursor.MarkLabel(after);
        }
    }
    private static VirtualRenderTarget mybuf = null, temp = null;
    public static void AddBlur(string tag, float value)
    {
        CaeruleaHelperModule.Session.BlurEffectValues.Remove(tag);
        if (value > 1e-5) CaeruleaHelperModule.Session.BlurEffectValues.Add(tag, value);
    }
    private static bool ShouldBeTakenByCaerulea(Backdrop backdrop)
    {
        // For convenience, we take over the rendering even if no trigger specifies it
        // in that case, the styleground is just removed
        if (backdrop.Tags.Contains("caeruleaBlurEffect")) return true;
        return false;
    }
    private static void RenderBackdrop(Backdrop backdrop, BackdropRenderer renderer, Scene scene, BlendState blend)
    {
        // We first check if there's a tag for the blur effect
        // and get the corresponding infos
        string tag = null;
        foreach (var t in backdrop.Tags)
        {
            if (CaeruleaHelperModule.Session.BlurEffectValues.ContainsKey(t))
            {
                tag = t;
                // We assume there's only one tag, otherwise idk what to do anyways
                break;
            }
        }
        if (tag == null)
        {
            backdrop.Render(scene);
            return;
        }
        CaeruleaHelperModule.Session.BlurEffectValues.TryGetValue(tag, out float strength);
        // We set the RT to our buffer
        renderer.EndSpritebatch();
        GraphicsDevice device = Engine.Graphics.GraphicsDevice;
        var old = device.GetRenderTargets();
        EnsureBuffer();
        device.SetRenderTarget(mybuf);
        if (backdrop.UseSpritebatch)
        {
            if (backdrop is Parallax) renderer.StartSpritebatchLooping(blend);
            else renderer.StartSpritebatch(blend);
        }
        // Do the original render, then blur it
        device.Clear(Color.Transparent);
        backdrop.Render(scene);
        renderer.EndSpritebatch();
        GaussianBlur.Blur(mybuf, temp, mybuf, 0, true, GaussianBlur.Samples.Nine, strength);
        device.SetRenderTargets(old);
        renderer.StartSpritebatch(blend);
        Draw.SpriteBatch.Draw(mybuf, Vector2.Zero, Color.White);
    }
}
