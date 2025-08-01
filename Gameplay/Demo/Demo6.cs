using Microsoft.Xna.Framework;
using Rubedo;
using Rubedo.Components;
using Rubedo.Graphics.Animation;
using Rubedo.Input;
using Rubedo.Lib.Extensions;
using Rubedo.Lib.StateMachine;
using Rubedo.Object;
using System.Collections.Generic;

namespace Test.Gameplay.Demo;

/// <summary>
/// Sprite Test
/// </summary>
internal class Demo6 : DemoBase
{
    Entity mouseSprite;
    Sprite sprite;
    Animator animator;

    public Demo6()
    {
        description = "Sprite Test";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        mouseSprite = new Entity();
        sprite = new Sprite("", 5, Color.White);
        sprite.Pivot = new Vector2(0.5f, 0f);

        animator = AnimatorExtensions.CreateSpriteAnimation("jotaro\\jotaro", 0.5f, sprite);

        mouseSprite.Transform.LocalScale = new Vector2(state.MainCamera.Z * 4);
        mouseSprite.Add(sprite);
        mouseSprite.Add(animator);
        state.Add(mouseSprite);

        state.AddDebugLabel(state.debugRoot, () => $"Frame Index: {animator.Current.CurrentFrame}");
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