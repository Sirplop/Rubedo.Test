using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rubedo.Components;
using Rubedo.Graphics.Animation;
using Rubedo.Input;
using Rubedo.Object;
using Rubedo.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Gameplay.Demo;
internal class Demo12 : DemoBase
{
    public Entity[] sprites;
    public Effect pixelArtShader;
    public bool enabled = false;

    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();
        DemoGame.ClearColor = Color.White;

        pixelArtShader = Assets.GetResource<Effect>("pixelart");
        pixelArtShader.Parameters["PixelPerfectEnabled"]?.SetValue(0);

        sprites = new Entity[4];

        Sprite stationarySprite = new Sprite("megaman", 1, Color.White);
        pixelArtShader.Parameters["TexelSize"]?.SetValue(stationarySprite.Size);
        stationarySprite.LayerDepth = 1;
        stationarySprite.Shader = pixelArtShader;
        sprites[0] = new Entity(new Vector2(-10, 0));
        sprites[0].Add(stationarySprite);
        state.Add(sprites[0]);

        stationarySprite = new Sprite("megaman", 1, Color.White);
        stationarySprite.LayerDepth = 1;
        stationarySprite.Shader = pixelArtShader;
        sprites[1] = new Entity(new Vector2(-10, 7));
        sprites[1].Add(stationarySprite);
        state.Add(sprites[1]);

        stationarySprite = new Sprite("megaman", 1, Color.White);
        stationarySprite.LayerDepth = 1;
        stationarySprite.Shader = pixelArtShader;
        sprites[2] = new Entity(new Vector2(-10, -7), 0, Vector2.One * 0.25f);
        sprites[2].Add(stationarySprite);
        state.Add(sprites[2]);

        stationarySprite = new Sprite("megaman", 1, Color.White);
        stationarySprite.LayerDepth = 1;
        stationarySprite.Shader = pixelArtShader;
        sprites[3] = new Entity(new Vector2(0, 0), 0, Vector2.One * 0.25f);
        sprites[3].Add(stationarySprite);
        state.Add(sprites[3]);


        state.AddDebugLabel(state.debugRoot, () => $"Pixel Art Filter: {(enabled ? "Enabled" : "Disabled")}");
    }

    public override void HandleInput(DemoState state)
    {
        if (InputManager.MousePressed(InputManager.MouseButtons.Right))
        {
            enabled = !enabled;
            pixelArtShader.Parameters["PixelPerfectEnabled"]?.SetValue(enabled ? 1 : 0);
        }
    }

    public override void Update(DemoState state)
    {
        const float MAX_SCALE = 0.25f;

        float sin = (float)Math.Sin(Rubedo.Time.RunningTime / 2f);

        sprites[0].Transform.RotationDegrees = sin * 180f;
        sprites[0].Transform.Scale = new Vector2(sin * MAX_SCALE, sin * MAX_SCALE);

        sprites[1].Transform.Scale = new Vector2(sin * MAX_SCALE, sin * MAX_SCALE);

        sprites[2].Transform.Position = new Vector2((sin * 3) + -5, -7);

        sprites[3].Transform.RotationDegrees = sin * 90f;
    }
}
