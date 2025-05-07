using Microsoft.Xna.Framework;
using Rubedo.Components;
using Rubedo.Internal.Assets;
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

        hor1.childPadding = 5;
        hor2.childPadding = 5;
        hor3.childPadding = 5;
        vert.childPadding = 5;
        vert.paddingLeft = 5;
        vert.paddingTop = 5;

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
        button.AddChild(new SelectableTintSet(image, 1f));
        image.Anchor = Anchor.Center;

        button.OnReleased += (b) => b.SetActive(false);

        return button;
    }

    public override void Update(DemoState state)
    {

    }
    public override void HandleInput(DemoState state) { }
}