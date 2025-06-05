using Rubedo.Object;
using Rubedo.Physics2D.Dynamics;
using Rubedo.Physics2D;
using Rubedo;
using Microsoft.Xna.Framework;
using Rubedo.Physics2D.Dynamics.Shapes;
using Rubedo.Lib;
using Rubedo.Input;

namespace Test.Gameplay.Demo;

/// <summary>
/// Many Bodies Demo
/// </summary>
internal class Demo6 : DemoBase
{
    public Demo6()
    {
        description = "Many Bodies";
    }

    public override void Initialize(DemoState state)
    {
        state.CreateFPSDebugGUI();
        state.CreateDemoDebugGUI();
        state.CreatePhysicsDebugGUI();

        const float width = 33.3f;
        const float height = 20f;

        PhysicsMaterial material = new PhysicsMaterial(1, 0.5f, 0.5f, 0, 0.5f);
        Entity entity;
        Collider comp;

        //left wall
        entity = new Entity(new Vector2(-width / 2 + 0.5f, 0.6f));
        comp = Collider.CreateBox(1, (height - 0.5f) * 10f);
        state.MakeBody(entity, material, comp, true);

        //right wall
        entity = new Entity(new Vector2(width / 2 - 0.5f, 0.6f));
        comp = Collider.CreateBox(1, (height - 0.5f) * 10f);
        state.MakeBody(entity, material, comp, true);

        //floor
        entity = new Entity(new Vector2(0, -height / 2 + 0.5f));
        comp = Collider.CreateBox(width, 1);
        state.MakeBody(entity, material, comp, true);

        Vector2 xy = new Vector2(-width / 2 + Collider.UNIT_BOX_SIDE, -height / 2 + 0.5f);

        const int W = 30;
        const int H = 70;

        for (int x = 0; x < W; x++)
        {
            xy.X += Collider.UNIT_BOX_SIDE;
            for (int y = 0; y < H; y++)
            {
                xy.Y += Collider.UNIT_BOX_SIDE;
                entity = new Entity(xy);
                comp = Collider.CreateUnitShape(ShapeType.Box);
                state.MakeBody(entity, material, comp, false);
            }
            xy.Y = -height / 2 + 0.5f;
        }
    }

    public override void Update(DemoState state) { }

    private bool shapeSet = true;
    public override void HandleInput(DemoState state)
    {
        if (InputManager.MousePressed(InputManager.MouseButtons.Left) ||
            (DemoState.fastPlace && InputManager.MouseDown(InputManager.MouseButtons.Left)))
        {
            PhysicsMaterial material = new PhysicsMaterial(1, 0.5f, 0.5f, 0, 0.5f);
            float x = Random.Range(0.5f, 2f);
            float y = Random.Range(0.5f, 2f);

            Entity entity = new Entity(InputManager.MouseWorldPosition(), 0, new Vector2(x, y));
            Collider comp = Collider.CreateUnitShape(shapeSet ? ShapeType.Circle : ShapeType.Capsule);
            state.MakeBody(entity, material, comp, false);
        }
        if (InputManager.MousePressed(InputManager.MouseButtons.Right) ||
            (DemoState.fastPlace && InputManager.MouseDown(InputManager.MouseButtons.Right)))
        {
            PhysicsMaterial material = new PhysicsMaterial(1, 0.5f, 0.5f);
            float x = Random.Range(0.5f, 2f);
            float y = Random.Range(0.5f, 2f);

            Entity entity = new Entity(InputManager.MouseWorldPosition(), 0, new Vector2(x, y));
            Collider comp = Collider.CreateUnitShape(shapeSet ? ShapeType.Box : ShapeType.Polygon, 3);
            state.MakeBody(entity, material, comp, false);
        }
        if (InputManager.MousePressed(InputManager.MouseButtons.Middle))
        {
            shapeSet = !shapeSet;
        }
    }
}