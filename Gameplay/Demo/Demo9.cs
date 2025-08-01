using Rubedo.Lib;
using Rubedo.Lib.Coroutines;
using System.Collections;

namespace Test.Gameplay.Demo;

/// <summary>
/// Coroutine Test
/// </summary>
internal class Demo9 : DemoBase
{
    int victory = 0;

    public Demo9()
    {
        description = "Coroutine Test";
    }
    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();
        state.AddDebugLabel(state.debugRoot, () => "Victory: "+victory);
        victory = 0;

        for (int i = 0; i < 100; i++)
        {
            Coroutine.Start(Waiter(Random.Range(1f, 5f)));
        }
    }

    public IEnumerator Waiter(float time)
    {
        yield return Coroutine.WaitForSeconds(time);
        victory++;
    }

    public override void HandleInput(DemoState state)
    {
    }

    public override void Update(DemoState state)
    {
    }
}