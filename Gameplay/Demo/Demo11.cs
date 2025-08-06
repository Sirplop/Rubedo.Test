using Microsoft.Xna.Framework;
using Rubedo.Object;
using Rubedo.Physics2D.Dynamics;
using Rubedo.Physics2D.Dynamics.Shapes;
using System;
using Rubedo.Physics2D.Collision;
using Microsoft.Xna.Framework.Graphics;
using Rubedo;
using Rubedo.Graphics.Particles;
using Rubedo.Physics2D.Common;
using Rubedo.Graphics.Particles.Modifiers;
using Rubedo.Graphics.Particles.Origins;
using Rubedo.Lib.Tweening;
using Rubedo.Components;
using System.Linq;

namespace Test.Gameplay.Demo;

/// <summary>
/// TODO: I am Demo11, and I don't have a summary yet.
/// </summary>
internal class Demo11 : DemoBase
{
    private Texture2D pixel;
    private Texture2D smallPixel;
    private Texture2D fadedcircle;
    private Circle circle = new Circle(8);
    private Circle circle2 = new Circle(3);

    PhysicsMaterial material = new PhysicsMaterial(1, 0.5f, 0, 0, 0.5f);

    public Demo11()
    {
        description = "Physics Particles Test";
    }
    public override void Initialize(DemoState state)
    {
        PhysicsLayer.SetCollisionWithLayer(0, 2, true);
        PhysicsLayer.SetCollisionWithLayer(2, 2, false);
        DemoState.drawBodies = false;
        DemoState.deleteBodyIfTooFar = false;

        state.MainCamera.Zoom = 1;
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();
        state.CreatePhysicsDebugGUI();

        fadedcircle = Assets.LoadTexture("circle");

        pixel = new Texture2D(RubedoEngine.Graphics.GraphicsDevice, 16, 16);
        pixel.SetData<Color>([.. Enumerable.Repeat(Color.White, 256)]);
        smallPixel = new Texture2D(RubedoEngine.Graphics.GraphicsDevice, 4, 4);
        smallPixel.SetData<Color>([.. Enumerable.Repeat(Color.White, 16)]);

        Texture2D veryWide = new Texture2D(RubedoEngine.Graphics.GraphicsDevice, 1000, 30);
        veryWide.SetData<Color>([.. Enumerable.Repeat(Color.White, 1000 * 30)]);

        Entity entity = new Entity(new Vector2(-150, 30), 30);
        Collider comp = Collider.CreateBox(16, 16);
        state.MakeBody(entity, material, comp, true);
        Sprite sprite = new Sprite(pixel, -1, Color.Green);
        entity.Add(sprite);

        entity = new Entity(new Vector2(-100, 180), 0);
        comp = Collider.CreateBox(16, 16);
        state.MakeBody(entity, material, comp, true);
        sprite = new Sprite(pixel, -1, Color.Green);
        entity.Add(sprite);
        entity = new Entity(new Vector2(-84, 180), 0);
        comp = Collider.CreateBox(16, 16);
        state.MakeBody(entity, material, comp, true);
        sprite = new Sprite(pixel, -1, Color.Green);
        entity.Add(sprite);

        entity = new Entity(new Vector2(100, 50), 10);
        comp = Collider.CreateBox(16, 16);
        state.MakeBody(entity, material, comp, true);
        sprite = new Sprite(pixel, -1, Color.Green);
        entity.Add(sprite);

        entity = new Entity(new Vector2(300, 0), 70);
        comp = Collider.CreateBox(16, 16);
        state.MakeBody(entity, material, comp, true);
        sprite = new Sprite(pixel, -1, Color.Green);
        entity.Add(sprite);

        entity = new Entity(new Vector2(0, -100), 0);
        comp = Collider.CreateBox(1000, 30);
        state.MakeBody(entity, material, comp, true);
        sprite = new Sprite(veryWide, -1, Color.Green);
        entity.Add(sprite);

        Shape boxShape = new Box(20, 20);

        entity = new Entity(new Vector2(0, 500));
        PhysicsParticleEmitter emitter2 = new PhysicsParticleEmitter("Boxes", boxShape, material, new Interval(50, 150), new Interval(-Math.PI, 0), 5.0f, new Interval(4000, 4000), true, 2);
        emitter2.AddModifier(new ColorRangeModifier(Color.LightBlue, Color.Purple));
        emitter2.Origin = new PointOrigin();
        emitter2.OnCollision += Emitter2_OnCollision;
        emitter2.GravityScale = 50;
        emitter2.Texture = pixel;
        emitter2.Start();

        entity.Add(emitter2);
        state.Add(entity);
    }

    private ContactAction Emitter2_OnCollision(PhysicsParticle sender, PhysicsBody other, Manifold m)
    {
        Explode(sender.Transform.Position);
        //delete the particle
        return ContactAction.DESTROY;
    }
    private void Explode(Vector2 pos2)
    {
        Entity entity = new Entity(pos2);

        PhysicsParticleEmitter emitter = new PhysicsParticleEmitter("Explosion", circle, material, new Interval(0, 150), new Interval(-Math.PI, Math.PI), 10, new Interval(500, 1000), false, 2);

        emitter.AddModifier(new ColorRangeModifier(Color.Orange, Color.Transparent));
        emitter.Origin = new PointOrigin();
        emitter.GravityScale = 0;
        emitter.LinearDamping = 0.04f;
        emitter.Texture = fadedcircle;

        entity.Add(emitter);

        PhysicsParticleEmitter emitter2 = new PhysicsParticleEmitter("Debris", circle2, material, new Interval(0, 200), new Interval(-Math.PI, Math.PI), 15.0f, new Interval(1000, 2000), false, 2);
        emitter2.AddModifier(new AlphaFadeModifier());
        emitter2.AddModifier(new ColorRangeModifier(Color.LightBlue, Color.Purple));
        emitter2.Origin = new PointOrigin();
        emitter2.GravityScale = 25;
        emitter2.LinearDamping = 0.02f;
        emitter2.Texture = smallPixel;

        entity.Add(emitter2);

        DestroyWhenNoParticles destroyer = new DestroyWhenNoParticles(emitter, emitter2);
        entity.Add(destroyer);
        RubedoEngine.CurrentState.Add(entity);

        emitter2.PlayBurst(10);
        emitter.PlayBurst(40);
    }

    public override void Update(DemoState state)
    {

    }

    public override void HandleInput(DemoState state)
    {

    }


    private class DestroyWhenNoParticles : Component
    {
        public Emitter emitter1;
        public Emitter emitter2;

        public DestroyWhenNoParticles(Emitter emitter1, Emitter emitter2)
        {
            this.emitter1 = emitter1;
            this.emitter2 = emitter2;
        }

        public override void Update()
        {
            if (emitter1.IsDestroyed &&  emitter2.IsDestroyed)
                Entity.Destroy();
        }
    }
}