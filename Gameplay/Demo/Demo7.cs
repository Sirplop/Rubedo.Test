using Microsoft.Xna.Framework;
using Rubedo.Input;
using Rubedo.UI;
using Rubedo.UI.Graphics;
using Rubedo.UI.Layout;

namespace Test.Gameplay.Demo;

/// <summary>
/// TODO: I am Demo7, and I don't have a summary yet.
/// </summary>
internal class Demo7 : DemoBase
{
    private Vertical testVert;

    public Demo7()
    {
        description = "UI Alignment";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        Vertical vertical = GUI.Root.AddVertical(0);
        vertical.AddImage("ball", 64, 64, Color.White);

        vertical = GUI.Root.AddVertical(0);
        vertical.Anchor = Anchor.TopRight;
        vertical.AddImage("ball", 64, 64, Color.White);

        vertical = GUI.Root.AddVertical(0);
        vertical.Anchor = Anchor.BottomLeft;
        vertical.AddImage("ball", 64, 64, Color.White);

        testVert = GUI.Root.AddVertical(0);
        testVert.Anchor = Anchor.BottomRight;
        testVert.AddImage("ball", 64, 64, Color.White);

        Image image = GUI.Root.AddImage("ball", 64, 64, Color.White);
        image.Anchor = Anchor.Top;
        image.Rotation = 45;
    }

    public override void HandleInput(DemoState state)
    {
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.B))
        {
            GUI.DebugDraw = !GUI.DebugDraw;
        }
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageUp))
        {
            GUI.DebugDrawDepthMin++;
        }
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown))
        {
            int depth = GUI.DebugDrawDepthMin - 1;
            GUI.DebugDrawDepthMin = depth < 0 ? 0 : depth;
        }
    }
    public override void Update(DemoState state)
    {
        return;
    }
}