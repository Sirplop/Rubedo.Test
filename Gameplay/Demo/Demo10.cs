using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rubedo;
using System;
using Rubedo.Object;
using Rubedo.Graphics.Particles;
using Rubedo.Graphics.Particles.Modifiers;
using Rubedo.Graphics.Particles.Origins;
using Rubedo.Input;
using Rubedo.Components;
using Rubedo.Resources;

namespace Test.Gameplay.Demo;

/// <summary>
/// TODO: I am Demo10, and I don't have a summary yet.
/// </summary>
internal class Demo10 : DemoBase
{
    private Texture2D star;
    private Texture2D circle;
    private Texture2D fadedcircle;
    private Texture2D pixel;

    private ParticleEmitter e1;
    private ParticleEmitter e2;
    private ParticleEmitter e3;
    private ParticleEmitter e4;
    private ParticleEmitter e5;
    private ParticleEmitter e6;
    private ParticleEmitter e7;
    private ParticleEmitter e8;
    private ParticleEmitter e9;

    public Demo10()
    {
        description = "Particle Test";
    }

    public override void Initialize(DemoState state)
    {
        state.MainCamera.SetZoomToUnitHeight(25);
        DemoState.allowCameraMove = false;
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        star = Assets.GetResource<Texture2D>("star");
        circle = Assets.GetResource<Texture2D>("circle");
        fadedcircle = Assets.GetResource<Texture2D>("fadedcircle");

        pixel = new Texture2D(RubedoEngine.Graphics.GraphicsDevice, 1, 1);
        pixel.SetData<Color>(new Color[1] { Color.White });

        ParticleBuilder builder = new ParticleBuilder("Small Star Burst", 20f, false)
            .SetSpeed(1, 2)
            .SetDirection(-Math.PI, Math.PI)
            .SetMaxAge(2000, 3000)
            .AddModifier(new ColorRangeModifier(Color.Transparent, Color.Red, Color.Yellow, new Color(0, 255, 0), Color.Blue, new Color(255, 0, 255), Color.Transparent))
            .SetOrigin(new PointOrigin())
            .SetTexture(star, 0);
        Entity entity = new Entity(new Vector2(-12.5f, -7.5f));
        e1 = builder.BuildAndStart();
        entity.Add(e1);
        state.Add(entity);

        //NOTE: you probably shouldn't be reusing builders like this - you can, but it's messy and probably bad practice.
        //      Should probably make new builders like above per system type. (you can build multiple copies from one builder, though!)
        builder.SetName("Star Ring Implode")
            .AddBirthModifier(new InwardBirthModifier())
            .SetOrigin(new CircleOrigin(3, true))
            .SetParticleRate(300f)
            .SetSpeed(0.4f, 1.5f)
            .SetMaxAge(1000, 1500);
        entity = new Entity(new Vector2(0, -7.5f));
        e2 = builder.BuildAndStart();
        entity.Add(e2);
        state.Add(entity);

        builder.SetName("Star Box Implode")
            .SetSpeed(0.2f, 0.8f)
            .SetMaxAge(2000, 2000)
            .SetOrigin(new RectangleOrigin(11, 4f));
        entity = new Entity(new Vector2(12.5f, -7.5f));
        e3 = builder.BuildAndStart();
        entity.Add(e3);
        state.Add(entity);

        builder.SetName("Star Burst")
            .SetSpeed(1f, 3f)
            .SetParticleRate(100)
            .ClearBirthModifiers().ClearModifiers()
            .AddModifier(new ScaleModifier(1, 4))
            .AddModifier(new AlphaFadeModifier())
            .AddBirthModifier(new ColorBirthModifier(Color.Red, new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 255)))
            .SetOrigin(new PointOrigin());
        entity = new Entity(new Vector2(-12.5f, 0));
        e4 = builder.BuildAndStart();
        entity.Add(e4);
        state.Add(entity);

        builder.SetName("Star Circle Explode")
            .SetSpeed(0.2f, 0.8f)
            .ClearBirthModifiers().ClearModifiers()
            .AddModifier(new ColorRangeModifier(Color.Transparent, Color.Red, new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 255), Color.Transparent))
            .AddBirthModifier(new OutwardBirthModifier())
            .SetOrigin(new CircleOrigin(3, true));
        entity = new Entity(new Vector2(0, 0));
        e5 = builder.BuildAndStart();
        entity.Add(e5);
        state.Add(entity);

        builder.SetName("Star Box Explode")
            .SetSpeed(0.2f, 0.8f)
            .SetOrigin(new RectangleOrigin(11, 4, true));
        entity = new Entity(new Vector2(12.5f, 0));
        e6 = builder.BuildAndStart();
        entity.Add(e6);
        state.Add(entity);

        builder = new ParticleBuilder("Fire", 50f, false)
            .SetDirection(-Math.PI, Math.PI)
            .SetSpeed(0.2f, 1f)
            .SetMaxAge(2000, 2500)
            .SetTexture(circle, 0)
            .SetOrigin(new CircleOrigin(0.2f, false))
            .AddModifier(new ScaleModifier(3, 4))
            .AddModifier(new ColorRangeModifier(Color.White, Color.Orange, Color.DarkRed, Color.Transparent))
            .AddModifier(new GravityModifier(new Vector2(0, -4)));
        entity = new Entity(new Vector2(-12.5f, 7.5f));
        e7 = builder.BuildAndStart();
        entity.Add(e7);
        state.Add(entity);

        builder = new ParticleBuilder("Multi-shape Burst", 50f, false)
            .SetSpeed(1f, 1.5f)
            .SetDirection(-Math.PI, Math.PI)
            .SetMaxAge(1000, 1500)
            .SetTexture(star, 0)
            .SetOrigin(new CircleAnimatedOrigin(2.5f, 4))
            .AddBirthModifier(new ScaleBirthModifier(new Interval(1, 3)))
            .AddBirthModifier(new TextureBirthModifier(star, circle))
            //.AddBirthModifier(new ColorBirthModifier(Color.GhostWhite, Color.LightBlue))
            .AddModifier(new ColorRangeModifier(Color.GhostWhite, Color.LightBlue))
            .AddModifier(new AlphaFadeModifier());
        entity = new Entity(new Vector2(0, 7.5f));
        e8 = builder.BuildAndStart();
        entity.Add(e8);
        state.Add(entity);

        builder = new ParticleBuilder("Portal", 50, false)
            .SetSpeed(0, 0)
            .SetDirection(-Math.PI, Math.PI)
            .SetMaxAge(2000, 6000)
            .SetTexture(pixel, 0)
            .SetOrigin(new RectangleOrigin(8, 8))
            .AddModifier(new ColorRangeModifier(Color.Transparent, Color.LightBlue, Color.Orange, Color.Transparent))
            .AddBirthModifier(new ScaleBirthModifier(new Interval(2, 15)))
            .AddModifier(new ActionModifier(
                (e, p) => p.Transform.Position = e.Transform.Position + new Vector2((float)Math.Sin(p.Age / p.MaxAge * 6.28) * 5,
                p.Transform.Position.Y - e.Transform.Position.Y)));
        entity = new Entity(new Vector2(12.5f, 7.5f));
        e9 = builder.BuildAndStart();
        entity.Add(e9);
        state.Add(entity);

    }
    public override void Update(DemoState state)
    {

    }

    public override void HandleInput(DemoState state)
    {

    }
}