using Rubedo;
using Rubedo.Audio;
using Rubedo.Input;

namespace Test.Gameplay.Demo;

/// <summary>
/// Sprite Test
/// </summary>
internal class Demo8 : DemoBase
{
    //SoundPlayer player1;
    //SoundPlayer player2;
    //SoundPlayer player3;
    MusicPlayer music;

    bool playingBuggin = true;

    public Demo8()
    {
        description = "Audio Test";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();
       
        
        /*player1 = new SoundPlayer("test1", (int)DefaultMixers.Type.Effect, 1, RubedoEngine.Audio);
        player1.loop = true;
        player1.volume = 1f;
        player1.Play();
        player2 = new SoundPlayer("test2", (int)DefaultMixers.Type.Effect, 1, RubedoEngine.Audio);
        player2.loop = true;
        player2.volume = 0.5f;
        player2.Play();

        player3 = new SoundPlayer("test3", (int)DefaultMixers.Type.Effect, 1, RubedoEngine.Audio);
        player3.loop = true;
        player3.volume = 1f;
        AudioInstance instance = player3.Play();
        instance.SetPause(true);*/

        music = new MusicPlayer(RubedoEngine.Audio);
        music.PlayMusic("Buggin", 1f, 0);
    }

    public override void HandleInput(DemoState state)
    {
        if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
        {
            if (playingBuggin)
                music.FullSwapMusic("Legendary Foe", 1f, 0, 1f, 1f);
            else
                music.SwapMusic("Buggin", 1f, 0, 2f);
            playingBuggin = !playingBuggin;
        }
    }

    public override void Update(DemoState state)
    {
    }
}