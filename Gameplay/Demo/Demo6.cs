using Microsoft.Xna.Framework;
using Rubedo;
using Rubedo.Components;
using Rubedo.Input;
using Rubedo.Internal.Assets;
using Rubedo.Object;
using System;

namespace Test.Gameplay.Demo;

/// <summary>
/// Sprite Test
/// </summary>
internal class Demo6 : DemoBase
{
    Entity mouseSprite;
    Sprite sprite;

    public Demo6()
    {
        description = "Sprite Test";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        mouseSprite = new Entity();
        sprite = new Sprite("ball", 0);
        mouseSprite.Transform.LocalScale = new Vector2(state.MainCamera.Z);
        sprite.Color = Color.Yellow;
        mouseSprite.Add(sprite);
        state.Add(mouseSprite);

        Entity overlap = new Entity();
        Sprite lowerSprite = new Sprite("ball", 1);
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