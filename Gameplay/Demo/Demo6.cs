using Microsoft.Xna.Framework;
using Rubedo.Components;
using Rubedo.Input;
using Rubedo.Object;

namespace Test.Gameplay.Demo;

/// <summary>
/// Sprite Test
/// </summary>
internal class Demo6 : DemoBase
{
    Entity mouseSprite;
    AnimatedSprite sprite;

    public Demo6()
    {
        description = "Sprite Test";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        mouseSprite = new Entity();
        sprite = new AnimatedSprite("jotaro\\jotaro", 5, Color.White);
        //sprite.Controller.IsPingPong = true;
        sprite.Pivot = new Vector2(0.5f, 0f);
        sprite.Controller.Speed = 0.5f;
        //sprite.Controller.Pause();
        mouseSprite.Transform.LocalScale = new Vector2(state.MainCamera.Z * 4);
        mouseSprite.Add(sprite);
        state.Add(mouseSprite);
        /*
        Entity overlap = new Entity();
        Sprite lowerSprite = new Sprite("ball", 1);
        lowerSprite.Pivot = new Vector2(0.25f, 0.25f);
        overlap.Transform.LocalScale = new Vector2(state.MainCamera.Z);
        overlap.Add(lowerSprite);
        state.Add(overlap);

        Entity parallaxEnt = new Entity(new Vector2(10, 0), 0, new Vector2(state.MainCamera.Z * 10));
        parallaxEnt.Add(new Sprite("ball", -1))
                   .Add(new Parallax(state.MainCamera, 0.9f, 0.75f, Parallax.ParallaxType.Both));
        state.Add(parallaxEnt);

        parallaxEnt = new Entity(new Vector2(-10, 0), 0, new Vector2(state.MainCamera.Z * 0.75f));
        parallaxEnt.Add(new Sprite("ball", 4, Color.Gray))
                   .Add(new Parallax(state.MainCamera, -0.5f, Parallax.ParallaxType.Horizontal));
        state.Add(parallaxEnt);
        */
        state.AddDebugLabel(state.debugRoot, () => $"Frame Index: {sprite.Controller.CurrentFrame}");
    }

    public override void HandleInput(DemoState state)
    {
        if (InputManager.MousePressed(InputManager.MouseButtons.Left))
        {
            sprite.LayerDepth = sprite.LayerDepth == 2 ? 0 : 2;
        }
    }

    public override void Update(DemoState state)
    {
        mouseSprite.Transform.Position = InputManager.MouseWorldPosition();
    }
}