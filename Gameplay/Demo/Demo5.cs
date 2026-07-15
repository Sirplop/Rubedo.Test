using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rubedo;
using Rubedo.Graphics.Sprites;
using Rubedo.Input;
using Rubedo.Lib;
using Rubedo.Resources;
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

    Horizontal horz;

    public Demo5()
    {
        description = "Button Test";
    }

    public override void Initialize(DemoState state)
    {
        Assets.CreateNewFontSystem("fs-default", "fonts/DroidSans.ttf", "fonts/DroidSansJapanese.ttf", "fonts/Symbola-Emoji.ttf");

        vert = GUI.Root.AddVertical(new Padding(5, 5, 0, 0), 5);
        vert.Offset = new Vector2(0, 30);

        Horizontal hor1 = vert.AddHorizontal(5);
        Horizontal hor2 = vert.AddHorizontal(5);
        Horizontal hor3 = vert.AddHorizontal(5);
        hor3.Anchor = Anchor.Top;

        GetButton(hor1, 1, false);
        GetButton(hor1, 2, false);
        GetButton(hor1, 3, false);
        GetButton(hor2, 4, false);
        GetButton(hor2, 5, false);
        GetButton(hor2, 6, false);
        GetButton(hor3, 7, true);
        GetButton(hor3, 8, true);
        GetButton(hor3, 9, true);

        horz = GUI.Root.AddHorizontal(0);
        horz.Anchor = Anchor.BottomLeft;

        Image spaceTest = horz.AddImage("ball", Color.White);
        spaceTest.Anchor = Anchor.Center;

        Button textButton = vert.AddButton(TextButtonCallback);
        textButton.Height = 100;
        textButton.Anchor = Anchor.Left;
        
        text = textButton.AddLabel("The quick いろは brown\nfox にほへ jumps over\nt🙌h📦e l👏a👏zy dog adfasoqiw yraldh ald halwdha ldjahw dlawe havbx get872rq", Color.White, 18);
        text.MaxSize = new Vector2(64, -1);
        textButton.Anchor = Anchor.Left;
        textButton.AddChild(new SelectableTintSet(text, 1f));

        image = GUI.Root.AddTiledImage("ball", 320, 320, Color.White);
        image.Anchor = Anchor.BottomRight;
        image.uvOffset = new Vector2(0.5f, 0.5f);

        state.CreateFPSDebugGUI();
    }

    private void GetButton(UIComponent component, int x, bool tileImage)
    {
        Label text = null;
        Button button = component.AddButton((b) =>
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
        });
        if (tileImage)
        {
            Image image = button.AddTiledImage("ball", 96, 96, Color.Blue);
            image.uvOffset = new Vector2(0.25f, 0.25f);
            tileImages.Add(image);
        }
        else
        {
            NineSliceImage image = button.AddNineSlice("button_sliced", Random.Range(96, 256), 100, Color.White, true, 0.25f);
            images.Add(image);

            button.AddChild(new SelectableTintSet(image, 1f));
        }
        text = button.AddLabel(x.ToString() + ": This is short text, but it could also be longer.", Color.Red);
        text.MaxSize = new Vector2(64, -1);
        text.Anchor = Anchor.Center;
        text.horizontalAlignment = Label.HorizontalAlignment.Center;
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
        if (pauseTextScale || text == null)
            return;
        float t = Wave.Sine((float)Time.RunningTime, 4, 0.5f, 0) + 0.5f;
        float val = Math.Mix(64, 512, t);
        text.MaxSize = new Vector2(val, -1);

        Vector2 mouse = InputManager.MouseScreenPosition();

        image.uvOffset = new Vector2(-mouse.X / 64f, -mouse.Y / 64f);
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
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.V))
        {
            switch (horz.Anchor)
            {
                case Anchor.TopLeft:
                    horz.Anchor = Anchor.Top;
                    break;
                case Anchor.Top:
                    horz.Anchor = Anchor.TopRight;
                    break;
                case Anchor.TopRight:
                    horz.Anchor = Anchor.Left;
                    break;
                case Anchor.Left:
                    horz.Anchor = Anchor.Center;
                    break;
                case Anchor.Center:
                    horz.Anchor = Anchor.Right;
                    break;
                case Anchor.Right:
                    horz.Anchor = Anchor.BottomLeft;
                    break;
                case Anchor.BottomLeft:
                    horz.Anchor = Anchor.Bottom;
                    break;
                case Anchor.Bottom:
                    horz.Anchor = Anchor.BottomRight;
                    break;
                case Anchor.BottomRight:
                    horz.Anchor = Anchor.TopLeft;
                    break;
            }
        }

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
}