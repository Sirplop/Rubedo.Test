using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rubedo;
using System;
using Rubedo.Object;
using Rubedo.Graphics.Particles;
using Rubedo.Graphics.Particles.Modifiers;
using Rubedo.Graphics.Particles.Origins;

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

    public Demo10()
    {
        description = "Particle Test";
    }

    public override void Initialize(DemoState state)
    {
        state.MainCamera.Zoom = 1;
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        star = Assets.LoadTexture("star");
        circle = Assets.LoadTexture("circle");
        fadedcircle = Assets.LoadTexture("fadedcircle");

        pixel = new Texture2D(RubedoEngine.Graphics.GraphicsDevice, 1, 1);
        pixel.SetData<Color>(new Color[1] { Color.White });

        Entity entity = new Entity(new Vector2(-350, -200));
        ParticleEmitter emitter = new ParticleEmitter("Small Star Burst", new Interval(25, 50), new Interval(-Math.PI, Math.PI), 20.0f, new Interval(2000, 3000));
        emitter.AddModifier(new ColorRangeModifier(Color.Transparent, Color.Red, Color.Yellow, new Color(0, 255, 0), Color.Blue, new Color(255, 0, 255), Color.Transparent));
        emitter.Origin = new PointOrigin();
        emitter.Texture = star;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

        entity = new Entity(new Vector2(0, -200));
        emitter = new ParticleEmitter("Star Ring Implode", new Interval(15, 80), new Interval(-Math.PI, Math.PI), 300.0f, new Interval(500, 1000));
        emitter.AddModifier(new ColorRangeModifier(Color.Transparent, Color.Red, new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 255), Color.Transparent));
        emitter.AddBirthModifier(new InwardBirthModifier());
        emitter.Origin = new CircleOrigin(100, true);
        emitter.Texture = star;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

        entity = new Entity(new Vector2(350, -200));
        emitter = new ParticleEmitter("Star Box Implode", new Interval(5, 25), new Interval(-Math.PI, Math.PI), 200.0f, new Interval(2000, 2000));
        emitter.AddModifier(new ColorRangeModifier(Color.Transparent, Color.Red, new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 255), Color.Transparent));
        emitter.Origin = new RectangleOrigin(300, 100);
        emitter.AddBirthModifier(new InwardBirthModifier());
        emitter.Texture = star;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

        entity = new Entity(new Vector2(-350, 0));
        emitter = new ParticleEmitter("Star Burst", new Interval(55, 100), new Interval(-Math.PI, Math.PI), 100.0f, new Interval(2000, 2000));
        emitter.AddModifier(new ScaleModifier(1, 4));
        emitter.AddModifier(new AlphaFadeModifier());
        emitter.AddBirthModifier(new ColorBirthModifier(Color.Red, new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 255)));
        emitter.Origin = new PointOrigin();
        emitter.Texture = star;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

        entity = new Entity(new Vector2(0, 0));
        emitter = new ParticleEmitter("Star Circle Explode", new Interval(5, 25), new Interval(-Math.PI, Math.PI), 100.0f, new Interval(2000, 2000));
        emitter.AddModifier(new ColorRangeModifier(Color.Transparent, Color.Red, new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 255), Color.Transparent));
        emitter.Origin = new CircleOrigin(100, true);
        emitter.AddBirthModifier(new OutwardBirthModifier());
        emitter.Texture = star;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

        entity = new Entity(new Vector2(350, 0));
        emitter = new ParticleEmitter("Star Box Explode", new Interval(5, 25), new Interval(-Math.PI, Math.PI), 200.0f, new Interval(2000, 2000));
        emitter.AddModifier(new ColorRangeModifier(Color.Transparent, Color.Red, new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 255), Color.Transparent));
        emitter.Origin = new RectangleOrigin(300, 100, true);
        emitter.AddBirthModifier(new OutwardBirthModifier());
        emitter.Texture = star;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);


        entity = new Entity(new Vector2(-350, 200));
        emitter = new ParticleEmitter("Fire", new Interval(5, 25), new Interval(-Math.PI, Math.PI), 50.0f, new Interval(2000, 2500));
        emitter.Origin = new CircleOrigin(10, false);
        emitter.AddModifier(new ScaleModifier(3, 4));
        emitter.AddModifier(new ColorRangeModifier(Color.White, Color.Orange, Color.DarkRed, Color.Black, Color.Transparent));
        emitter.AddModifier(new GravityModifier(new Vector2(0, -100)));
        emitter.Texture = circle;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

        entity = new Entity(new Vector2(0, 200));
        emitter = new ParticleEmitter("Multi-shape burst", new Interval(100, 150), new Interval(-Math.PI, Math.PI), 50.0f, new Interval(1000, 1500));
        emitter.AddBirthModifier(new ScaleBirthModifier(new Interval(1, 3)));
        emitter.AddBirthModifier(new TextureBirthModifier(star, circle));
        emitter.AddBirthModifier(new ColorBirthModifier(Color.LightBlue));
        emitter.AddModifier(new AlphaFadeModifier());
        emitter.Origin = new PointOrigin();
        emitter.Texture = star;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

        entity = new Entity(new Vector2(350, 200));
        emitter = new ParticleEmitter("Portal", new Interval(0, 0), new Interval(-Math.PI, Math.PI), 50.0f, new Interval(2000, 6000));
        emitter.AddModifier(new ColorRangeModifier(Color.Transparent, Color.LightBlue, Color.Orange, Color.Transparent));
        emitter.AddBirthModifier(new ScaleBirthModifier(new Interval(2, 15)));
        emitter.AddModifier(new ActionModifier(
            (e, p) => p.Transform.Position = e.Transform.Position + new Vector2((float)Math.Sin(p.Age / p.MaxAge * 6.28) * 150, 
            p.Transform.Position.Y - e.Transform.Position.Y)));
        emitter.Origin = new RectangleOrigin(300, 200);
        emitter.Texture = pixel;
        emitter.Start();
        entity.Add(emitter);
        state.Add(entity);

    }
    public override void Update(DemoState state)
    {

    }

    public override void HandleInput(DemoState state)
    {

    }
}