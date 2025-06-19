using FontStashSharp;
using Microsoft.Xna.Framework;
using Rubedo;
using Rubedo.Graphics;
using Rubedo.Input;
using Rubedo.Internal.Assets;
using Rubedo.Lib;
using Rubedo.UI;
using Rubedo.UI.Graphics;
using Rubedo.UI.Input;
using Rubedo.UI.Layout;
using Rubedo.UI.Text;
using System.Collections.Generic;

namespace Test.Gameplay.Demo;

/// <summary>
/// TODO: I am Demo5, and I don't have a summary yet.
/// </summary>
internal class Demo5 : DemoBase
{
    Vertical vert;
    Label text;
    List<NineSliceImage> images = new List<NineSliceImage>();
    List<Image> tileImages = new List<Image>();
    Image image;

    public Demo5()
    {
        description = "Button Test";
    }

    public override void Initialize(DemoState state)
    {
        AssetManager.CreateNewFontSystem("fs-default", "fonts/DroidSans.ttf", "fonts/DroidSansJapanese.ttf", "fonts/Symbola-Emoji.ttf");
        
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
        
        hor3.AddChild(GetButtonTile(7));
        hor3.AddChild(GetButtonTile(8));
        hor3.AddChild(GetButtonTile(9));
        hor3.Anchor = Anchor.Top;

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
        FontSystem font = AssetManager.GetFontSystem("fs-default");
        text = new Label(font, "The quick いろは brown\nfox にほへ jumps over\nt🙌h📦e l👏a👏zy dog adfasoqiw yraldh ald halwdha ldjahw dlawe havbx get872rq", Color.White, 18);
        text.MaxSize = new Vector2(64, -1);
        Button textButton = new Button();
        //textButton.Anchor = Anchor.Center;
        //textButton.Offset = new Vector2(0, 100);
        textButton.AddChild(new SelectableTintSet(text, 1f));
        textButton.AddChild(text);
        textButton.OnReleased += TextButtonCallback;
        vert.AddChild(textButton);

        image = new Image(AssetManager.LoadTexture("ball"), 320, 320);
        image.drawMode = Image.DrawMode.Tiled;
        image.Anchor = Anchor.BottomRight;
        image.uvOffset = new Vector2(0.5f, 0.5f);
        GUI.Root.AddChild(image);

        UIComponent comp = GUI.Root.Children[0];
        GUI.Root.RemoveChild(comp);
        GUI.Root.AddChild(comp);

        state.CreateFPSDebugGUI();
    }
    private Button GetButton(int x)
    {
        Button button = new Button();
        NineSliceImage image = new NineSliceImage(new Texture2DRegion(AssetManager.LoadTexture("button_sliced")).CreateNineSliceFromUVs(0.25f), Random.Range(96, 256), 100);
        image.Image.filled = true;
        images.Add(image);
        FontSystem font = AssetManager.GetFontSystem("fs-default");
        Label text = new Label(font, x.ToString() + ": This is short text, but it could also be longer.", Color.Red, 12);
        text.MaxSize = new Vector2(64, -1);
        text.Anchor = Anchor.Center;
        text.horizontalAlignment = Label.HorizontalAlignment.Center;
        image.AddChild(text);
        button.AddChild(image);
        button.AddChild(new SelectableTintSet(image, 1f));

        button.OnReleased += (b) =>
        {
            switch (text.horizontalAlignment)
            {
                case Label.HorizontalAlignment.Left:
                    text.horizontalAlignment = Label.HorizontalAlignment.Center;
                    break;
                case Label.HorizontalAlignment.Center:
                    text.horizontalAlignment = Label.HorizontalAlignment.Right;
                    break;
                case Label.HorizontalAlignment.Right:
                    text.horizontalAlignment = Label.HorizontalAlignment.Left;
                    break;
            }
        };

        return button;
    }
    private Button GetButtonTile(int x)
    {
        Button button = new Button();
        Image image = new Image(AssetManager.LoadTexture("ball"), 96, 96);
        image.drawMode = Image.DrawMode.Tiled;
        image.uvOffset = new Vector2(0.5f, 0.5f);
        tileImages.Add(image);
        FontSystem font = AssetManager.GetFontSystem("fs-default");
        Label text = new Label(font, x.ToString() + ": This is short text, but it could also be longer.", Color.Red, 12);
        text.MaxSize = new Vector2(64, -1);
        text.Anchor = Anchor.Center;
        text.horizontalAlignment = Label.HorizontalAlignment.Center;
        image.AddChild(text);
        button.AddChild(image);
        button.AddChild(new SelectableTintSet(image, 1f));

        button.OnReleased += (b) =>
        {
            switch (text.horizontalAlignment)
            {
                case Label.HorizontalAlignment.Left:
                    text.horizontalAlignment = Label.HorizontalAlignment.Center;
                    break;
                case Label.HorizontalAlignment.Center:
                    text.horizontalAlignment = Label.HorizontalAlignment.Right;
                    break;
                case Label.HorizontalAlignment.Right:
                    text.horizontalAlignment = Label.HorizontalAlignment.Left;
                    break;
            }
        };

        return button;
    }

    private void TextButtonCallback(Button pusher)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].PrefWidth = Random.Range(96, 256);
            images[i].PrefHeight = Random.Range(90, 130);
            images[i].Image.pixelMultiplier = Random.Range(1f, 2f);
        }
    }

    private bool pauseTextScale = false;
    public override void Update(DemoState state)
    {
        if (pauseTextScale)
            return;
        float t = Wave.Sine((float)Time.RunningTime, 4, 0.5f, 0) + 0.5f;
        float val = Math.Mix(64, 512, t);
        text.MaxSize = new Vector2(val, -1);

        Vector2 mouse = InputManager.MouseScreenPosition();

        image.uvOffset = new Vector2(mouse.X / 64f, mouse.Y / 64f);
    }
    public override void HandleInput(DemoState state) 
    { 
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
        {
            Label.DrawBackground = !Label.DrawBackground;
        }
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
        {
            pauseTextScale = !pauseTextScale;
        }
    }
}