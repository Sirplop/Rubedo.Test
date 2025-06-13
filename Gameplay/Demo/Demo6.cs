using Microsoft.Xna.Framework;
using Rubedo;
using Rubedo.Components;
using Rubedo.Input;
using Rubedo.Internal.Assets;
using Rubedo.Object;

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
        sprite = new Sprite("ball", 0f);
        mouseSprite.Transform.LocalScale = new Vector2(state.MainCamera.Z);
        mouseSprite.Add(sprite);
        state.Add(mouseSprite);

        Entity overlap = new Entity();
        Sprite lowerSprite = new Sprite("ball", -0.5f);
        overlap.Transform.LocalScale = new Vector2(state.MainCamera.Z);
        overlap.Add(lowerSprite);
        state.Add(overlap);
    }

    public override void HandleInput(DemoState state)
    {
        if (InputManager.MousePressed(InputManager.MouseButtons.Left))
        {
            sprite.LayerDepth = sprite.LayerDepth == -1 ? 0 : -1;
        }
    }

    public override void Update(DemoState state)
    {
        mouseSprite.Transform.Position = InputManager.MouseWorldPosition();
    }
}