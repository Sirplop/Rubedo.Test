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
internal class Demo7 : DemoBase
{
    Entity mouseSprite;
    Sprite sprite;
    Text text;

    public Demo7()
    {
        description = "Sprite Test";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        mouseSprite = new Entity();
        sprite = new Sprite("ball", 0f);
        sprite.compTransform.LocalScale = new Vector2(RubedoEngine.Instance.Camera.Z);
        text = new Text(AssetManager.GetFontSystem("fs-default"), "1", Color.White);
        text.fontSize = 24;
        text.layerDepth = 2;
        text.compTransform.LocalPosition = new Vector2(-0.5f, 0.5f);
        mouseSprite.Add(sprite).Add(text);
        state.Add(mouseSprite);

        Entity overlap = new Entity();
        Sprite lowerSprite = new Sprite("ball", -0.5f);
        lowerSprite.compTransform.LocalScale = new Vector2(RubedoEngine.Instance.Camera.Z);
        overlap.Add(lowerSprite);
        state.Add(overlap);
    }

    public override void HandleInput(DemoState state)
    {
        if (InputManager.MousePressed(InputManager.MouseButtons.Left))
        {
            sprite.layer = sprite.layer == -1 ? 0 : -1;
            text.SetText(sprite.layer.ToString());
        }
    }

    public override void Update(DemoState state)
    {
        mouseSprite.transform.Position = InputManager.MouseWorldPosition();
    }
}