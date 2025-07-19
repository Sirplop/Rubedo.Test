using Microsoft.Xna.Framework;
using Rubedo;
using Rubedo.Audio;
using Rubedo.Components;
using Rubedo.Graphics.Sprites;
using Rubedo.Input;
using Rubedo.Object;
using System;

namespace Test.Gameplay.Demo;

/// <summary>
/// Sprite Test
/// </summary>
internal class Demo6 : DemoBase
{
    Entity mouseSprite;
    AnimatedSprite sprite;

    SoundPlayer player1;
    SoundPlayer player2;
    SoundPlayer player3;

    public Demo6()
    {
        description = "Sprite Test";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();

        mouseSprite = new Entity();
        sprite = new AnimatedSprite("jotaro\\jotaro", 5, Color.White);
        //sprite.Controller.IsPingPong = true;
        sprite.Pivot = new Vector2(0.5f, 0f);
        sprite.Controller.Speed = 0.5f;
        //sprite.Controller.Pause();
        mouseSprite.Transform.LocalScale = new Vector2(state.MainCamera.Z * 4);
        mouseSprite.Add(sprite);
        state.Add(mouseSprite);
        /*
        Entity overlap = new Entity();
        Sprite lowerSprite = new Sprite("ball", 1);
        lowerSprite.Pivot = new Vector2(0.25f, 0.25f);
        overlap.Transform.LocalScale = new Vector2(state.MainCamera.Z);
        overlap.Add(lowerSprite);
        state.Add(overlap);

        Entity parallaxEnt = new Entity(new Vector2(10, 0), 0, new Vector2(state.MainCamera.Z * 10));
        parallaxEnt.Add(new Sprite("ball", -1))
                   .Add(new Parallax(state.MainCamera, 0.9f, 0.75f, Parallax.ParallaxType.Both));
        state.Add(parallaxEnt);

        parallaxEnt = new Entity(new Vector2(-10, 0), 0, new Vector2(state.MainCamera.Z * 0.75f));
        parallaxEnt.Add(new Sprite("ball", 4, Color.Gray))
                   .Add(new Parallax(state.MainCamera, -0.5f, Parallax.ParallaxType.Horizontal));
        state.Add(parallaxEnt);
        */
        state.AddDebugLabel(state.debugRoot, () => $"Frame Index: {sprite.Controller.CurrentFrame}");

        player1 = new SoundPlayer("test1", 1, RubedoEngine.Audio);
        player1.loop = true;
        player1.volume = 1f;
        player1.Play();
        player2 = new SoundPlayer("test2", 1, RubedoEngine.Audio);
        player2.loop = true;
        player2.volume = 0.5f;
        player2.Play();

        player3 = new SoundPlayer("test3", 1, RubedoEngine.Audio);
        player3.loop = true;
        player3.volume = 1f;
        AudioInstance instance = player3.Play();
        instance.SetPause(true);
    }

    public override void HandleInput(DemoState state)
    {
        if (InputManager.MousePressed(InputManager.MouseButtons.Left))
        {
            sprite.LayerDepth = sprite.LayerDepth == 2 ? 0 : 2;
        }
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
        {
            RubedoEngine.Audio.audioGroups["effects"].FlipVolume();
        }
    }

    public override void Update(DemoState state)
    {
        mouseSprite.Transform.Position = InputManager.MouseWorldPosition();
    }
}