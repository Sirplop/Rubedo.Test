using Rubedo.Internal.Assets;
using Rubedo.UI;
using Rubedo.UI.Graphics;
using Rubedo.UI.Layout;

namespace Test.Gameplay.Demo;

/// <summary>
/// TODO: I am Demo7, and I don't have a summary yet.
/// </summary>
internal class Demo7 : DemoBase
{
    public Demo7()
    {
        description = "UI Alignment";
    }
    public override void Initialize(DemoState state)
    {
        Vertical vertical = new Vertical();
        Image image = new Image(AssetManager.LoadTexture("ball"), 64, 64);
        vertical.AddChild(image);
        GUI.Root.AddChild(vertical);
        vertical = new Vertical();
        vertical.Anchor = Anchor.TopRight;
        image = new Image(AssetManager.LoadTexture("ball"), 64, 64);
        vertical.AddChild(image);
        GUI.Root.AddChild(vertical);
        vertical = new Vertical();
        vertical.Anchor = Anchor.BottomLeft;
        image = new Image(AssetManager.LoadTexture("ball"), 64, 64);
        vertical.AddChild(image);
        GUI.Root.AddChild(vertical);
        vertical = new Vertical();
        vertical.Anchor = Anchor.BottomRight;
        image = new Image(AssetManager.LoadTexture("ball"), 64, 64);
        vertical.AddChild(image);
        GUI.Root.AddChild(vertical);
    }

    public override void HandleInput(DemoState state)
    {
        return;
    }
    public override void Update(DemoState state)
    {
        return;
    }
}