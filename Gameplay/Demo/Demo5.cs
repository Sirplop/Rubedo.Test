using Microsoft.Xna.Framework;
using Rubedo;
using Rubedo.Components;
using Rubedo.UI;
using Rubedo.UI.Graphics;
using Rubedo.UI.Input;
using Rubedo.UI.Layout;
using System.Diagnostics;

namespace Test.Gameplay.Demo;

/// <summary>
/// TODO: I am Demo5, and I don't have a summary yet.
/// </summary>
internal class Demo5 : DemoBase
{
    Vertical vert;
    public Demo5()
    {
        description = "Button Test";
    }

    public override void Initialize(DemoState state)
    {
        state.drawDebug = false;
        
        vert = new Vertical();
        Horizontal hor1 = new Horizontal();
        Horizontal hor2 = new Horizontal();
        Horizontal hor3 = new Horizontal();

        hor1.AddChild(GetButton(1));
        hor1.AddChild(GetButton(2));
        hor1.AddChild(GetButton(3));

        hor2.AddChild(GetButton(4));
        hor2.AddChild(GetButton(5));
        hor2.AddChild(GetButton(6));

        hor3.AddChild(GetButton(7));
        hor3.AddChild(GetButton(8));
        hor3.AddChild(GetButton(9));

        vert.AddChild(hor1);
        vert.AddChild(hor2);
        vert.AddChild(hor3);

        GUI.Root.AddChild(vert);
    }
    private Button GetButton(int x)
    {
        Button button = new Button();
        Image image = new Image(new Rubedo.Graphics.Texture2DRegion(AssetManager.LoadTexture("button_circle")));
        button.AddChild(image);

        button.MinSize = new Vector2(100, -1);
        image.Anchor = Anchor.Center;

        button.OnPressed += (b) => TestPress(x, image);
        button.OnHeld += (b) => TestHeld(x, b);
        button.OnReleased += (b) => TestReleased(x);
        //button.OnReleasedNoHover += (b) => { image.Color = Color.White; };
        //button.OnHover += (s) => { if (s is Button b && !b.Clicked) image.Color = Color.LightGray; };
        //button.OnHoverLost += (s) => { if (s is Button b && !b.Clicked) image.Color = Color.White; };
        button.OnFocused += (s) => { image.Color = Color.Yellow; };
        button.OnFocusLost += (s) => { image.Color = Color.White; };
        return button;
    }

    private void TestPress(int x, Image img)
    {
        //img.Color = Color.Gray;
        Debug.WriteLine($"You pressed button {x}! Yippee!");
    }
    private void TestHeld(int x, Button button)
    {
        Debug.WriteLine($"Holding button {x}... for {button.HeldDuration.ToString("0.00")} seconds.");
    }
    private void TestReleased(int x)
    {
        Debug.WriteLine($"You released button {x}! So sad!");
        switch (vert.Anchor)
        {
            case Anchor.TopLeft:
                vert.Anchor = Anchor.Top;
                break;
            case Anchor.Top:
                vert.Anchor = Anchor.TopRight;
                break;
            case Anchor.TopRight:
                vert.Anchor = Anchor.Left;
                break;
            case Anchor.Left:
                vert.Anchor = Anchor.Center;
                break;
            case Anchor.Center:
                vert.Anchor = Anchor.Right;
                break;
            case Anchor.Right:
                vert.Anchor = Anchor.BottomLeft;
                break;
            case Anchor.BottomLeft:
                vert.Anchor = Anchor.Bottom;
                break;
            case Anchor.Bottom:
                vert.Anchor = Anchor.BottomRight;
                break;
            case Anchor.BottomRight:
                vert.Anchor = Anchor.TopLeft;
                break;
        }
    }

    public override void Update(DemoState state)
    {
    }
    public override void HandleInput(DemoState state) { }
}