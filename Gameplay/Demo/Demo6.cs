using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rubedo;
using Rubedo.Components;
using Rubedo.Graphics.Animation;
using Rubedo.Input;
using Rubedo.Lib.Extensions;
using Rubedo.Lib.StateMachine;
using Rubedo.Object;
using Rubedo.Resources;
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
    Effect rainbowEffect;

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
        rainbowEffect = Assets.GetResource<Effect>("rainbow");
        sprite.Shader = rainbowEffect;

        animator = AnimatorExtensions.CreateSpriteAnimation("jotaro/jotaro", 0.5f, sprite);

        mouseSprite.Transform.LocalScale = new Vector2(4);
        mouseSprite.Add(sprite);
        mouseSprite.Add(animator);
        state.Add(mouseSprite);

        Sprite stationarySprite = new Sprite("ball", 1, Color.White);
        stationarySprite.LayerDepth = 1;
        Entity ent = new Entity();
        ent.Transform.Scale = new Vector2(3, 3);
        ent.Add(stationarySprite);
        state.Add(ent);

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