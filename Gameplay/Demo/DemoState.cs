using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rubedo;
using Rubedo.EngineDebug;
using Rubedo.Input;
using Rubedo.Input.Conditions;
using Rubedo.Lib;
using Rubedo.Lib.Extensions;
using Rubedo.Object;
using Rubedo.Physics2D;
using Rubedo.Physics2D.Dynamics.Shapes;
using Rubedo.Physics2D.Dynamics;
using Rubedo.Physics2D.Math;
using Rubedo.UI;
using System;
using System.Collections.Generic;
using Rubedo.Physics2D.Common;
using Rubedo.UI.Text;
using FontStashSharp;
using Rubedo.UI.Layout;
using Microsoft.Xna.Framework.Graphics;
using Rubedo.Graphics.Viewports;
using Rubedo.Graphics;
using Rubedo.Graphics.Sprites;
using Rubedo.Audio;
using Rubedo.Lib.Coroutines;

namespace Test.Gameplay.Demo;

/// <summary>
/// I am Demo, and this is my summary.
/// </summary>
internal class DemoState : GameState
{
    public static bool colorVelocity = false;
    public static bool showVelocity = false;
    public static bool showAABB = false;
    public static bool fastPlace = false;

    public Vertical debugRoot;

    private Shapes shapes;

    private DemoBase[] demos = new DemoBase[]
    {
        new Demo1(),
        new Demo2(),
        new Demo3(),
        new Demo4(),
        new Demo5(),
        new Demo6(),
        new Demo7(),
        new Demo8(),
        new Demo9(),
    };

    private readonly AllCondition prevDemo = new AllCondition(new KeyCondition(Keys.Left), new KeyCondition(Keys.LeftShift, true));
    private readonly AllCondition nextDemo = new AllCondition(new KeyCondition(Keys.Right), new KeyCondition(Keys.LeftShift, true));

    private readonly KeyCondition cameraLeft = new KeyCondition(Keys.H);
    private readonly KeyCondition cameraRight = new KeyCondition(Keys.K);
    private readonly KeyCondition cameraUp = new KeyCondition(Keys.U);
    private readonly KeyCondition cameraDown = new KeyCondition(Keys.J);
    private readonly KeyCondition cameraScaleUp = new KeyCondition(Keys.OemPlus);
    private readonly KeyCondition cameraScaleDown = new KeyCondition(Keys.OemMinus);
    private readonly KeyCondition cameraRotateCW = new KeyCondition(Keys.I);
    private readonly KeyCondition cameraRotateCCW = new KeyCondition(Keys.Y);
    private readonly KeyCondition cameraReset = new KeyCondition(Keys.R);

    private int selectedDemo = 5;
    public List<DebugTextEntry> debugText = new List<DebugTextEntry>();
    private Vertical mouseVertical;

    private double deltaTime = 0.0f;

    private Camera _camera;
    private float _cameraLeft = 0.0f;

    public DemoState(StateManager sm) : base(sm)
    {
        shapes = new Shapes(RubedoEngine.Instance);
        _name = "DemoState";
    }

    public override void LoadContent()
    {
        Assets.CreateNewFontSystem("fs-default", "fonts/DroidSans.ttf", "fonts/DroidSansJapanese.ttf", "fonts/Symbola-Emoji.ttf");
        base.LoadContent();
    }

    public override void Enter()
    {
        GUI.Root = new GUIRoot(new Point(800, 480), false);
        Renderables.Add(GUI.Root);
        base.Enter();
        CreateCamera();
        debugRoot = new Vertical();
        debugRoot.Offset = new Vector2(30, 0);
        GUI.Root.AddChild(debugRoot);

        RubedoEngine.SizeOfMeter = 1;
        PhysicsWorld.ResetGravity();

        //construct debug GUI

        demos[selectedDemo].Initialize(this);
    }

    public void CreateMouseDebugGUI()
    {
        mouseVertical = new Vertical();
        FontSystem font = Assets.GetFontSystem("fs-default");
        Label world = new Label(font, string.Empty, Color.AntiqueWhite, 18);
        Label screen = new Label(font, string.Empty, Color.AntiqueWhite, 18);
        mouseVertical.AddChild(world);
        mouseVertical.AddChild(screen);
        GUI.Root.AddChild(mouseVertical);
    }

    private void CreateCamera()
    {
        if (_camera != null)
            _camera.Dispose();
        _camera = new Camera(this, new PixelViewport(RubedoEngine.Instance.GraphicsDevice, RubedoEngine.Instance.Window, 640, 640), 0);
        _camera.RenderLayers.Add((int)RenderLayer.Default);
        _camera.RenderLayers.Add((int)RenderLayer.UI);
        _camera.Zoom = 24f;
    }

    public void CreateFPSDebugGUI()
    {
        AddDebugLabel(debugRoot, () => string.Format("{0:0.0} ms ({1:0.} fps)", deltaTime * 1000.0f, 1.0f / deltaTime));
    }

    public void CreateDemoDebugGUI()
    {
        AddDebugLabel(debugRoot, () => $"Selected Demo: {demos[selectedDemo].description}");
        AddDebugLabel(debugRoot, () => $"Demo {selectedDemo + 1} of {demos.Length}");
    }

    public void CreatePhysicsDebugGUI()
    {
        AddDebugLabel(debugRoot, () => RubedoEngine.Instance.World.timer.GetAsString(", "));
        AddDebugLabel(debugRoot, () => $"Bodies: {RubedoEngine.Instance.World.bodies.Count} " +
            $"| Physics time: {RubedoEngine.Instance._physicsTimer.GetAsString("")}");
        AddDebugLabel(debugRoot, () => $"(C) Color velocity: {(colorVelocity ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(S) Show velocity: {(showVelocity ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(A) AABBs visible: {(showAABB ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(O) Contacts visible: {(PhysicsWorld.showContacts ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(D) Draw Broadphase: {(PhysicsWorld.drawBroadphase ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(B) Brute force: {(PhysicsWorld.bruteForce ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(F) Fast Place: {(fastPlace ? "On" : "Off")}");
    }

    public void AddDebugLabel(Vertical vert, Func<string> valueFunc)
    {
        FontSystem font = Assets.GetFontSystem("fs-default");
        Label label = new Label(font, string.Empty, Color.AntiqueWhite, 18);
        label.TightLineHeight = true;
        DebugTextEntry entry = new DebugTextEntry(label, valueFunc);
        debugText.Add(entry);
        vert.AddChild(label);
    }

    public override void HandleInput()
    {
        base.HandleInput();
        demos[selectedDemo].HandleInput(this);

        if (InputManager.KeyPressed(Keys.Z))
            RubedoEngine.Instance.physicsOn = !RubedoEngine.Instance.physicsOn;
        if (InputManager.KeyPressed(Keys.X))
            RubedoEngine.Instance.stepPhysics = true;
        if (InputManager.KeyPressed(Keys.S))
            showVelocity = !showVelocity;
        if (InputManager.KeyPressed(Keys.C))
            colorVelocity = !colorVelocity;
        if (InputManager.KeyPressed(Keys.A))
            showAABB = !showAABB;
        if (InputManager.KeyPressed(Keys.O))
            PhysicsWorld.showContacts = !PhysicsWorld.showContacts;
        if (InputManager.KeyPressed(Keys.B))
            PhysicsWorld.bruteForce = !PhysicsWorld.bruteForce;
        if (InputManager.KeyPressed(Keys.D))
            PhysicsWorld.drawBroadphase = !PhysicsWorld.drawBroadphase;
        if (InputManager.KeyPressed(Keys.F))
            fastPlace = !fastPlace;
        if (InputManager.KeyPressed(Keys.OemCloseBrackets))
        {
            _cameraLeft += 0.1f;
            if (_cameraLeft >= 1f)
                _cameraLeft = 0f;
            CreateCamera();
        }

        if (cameraLeft.Pressed() || cameraLeft.Held())
            MainCamera.XY += new Vector2(-1, 0);
        if (cameraRight.Pressed() || cameraRight.Held())
            MainCamera.XY += new Vector2(1, 0);
        if (cameraUp.Pressed() || cameraUp.Held())
            MainCamera.XY += new Vector2(0, 1);
        if (cameraDown.Pressed() || cameraDown.Held())
            MainCamera.XY += new Vector2(0, -1);
        if (cameraRotateCW.Pressed() || cameraRotateCW.Held())
            MainCamera.Rotation += 0.01f;
        if (cameraRotateCCW.Pressed() || cameraRotateCCW.Held())
            MainCamera.Rotation -= 0.01f;
        if (cameraScaleDown.Pressed() || cameraScaleDown.Held())
            MainCamera.Scale -= new Vector2(0.01f, 0.01f);
        if (cameraScaleUp.Pressed() || cameraScaleUp.Held())
            MainCamera.Scale += new Vector2(0.01f, 0.01f);
        if (cameraReset.Pressed())
        {
            MainCamera.XY = Vector2.Zero;
            MainCamera.Rotation = 0;
            MainCamera.Scale = Vector2.One;
        }

        if (prevDemo.Released())
        {
            Reset();
            if (selectedDemo == 0)
                selectedDemo = demos.Length - 1;
            else
                selectedDemo--;
            demos[selectedDemo].Initialize(this);
        }
        if (nextDemo.Released())
        {
            Reset();
            selectedDemo = (selectedDemo + 1) % demos.Length;
            demos[selectedDemo].Initialize(this);
        }
    }

    private void Reset()
    {
        RubedoEngine.SizeOfMeter = 1;
        _camera.Zoom = 24;
        PhysicsWorld.ResetGravity();
        RubedoEngine.Instance.World.Clear();
        foreach (Entity ent in Entities)
            Entities.Remove(ent);

        //remove all GUI elements except the debug vertical.    
        GUI.Root.RemoveChild(debugRoot);
        GUI.Root.DestroyChildren();
        debugRoot.DestroyChildren();
        GUI.Root.AddChild(debugRoot);

        Coroutine.StopAllCoroutines();
        RubedoEngine.Audio.StopAll();
    }

    public PhysicsBody MakeBody(Entity entity, PhysicsMaterial material, Collider collider, bool isStatic)
    {
        PhysicsBody body = new PhysicsBody(collider, material);
        if (isStatic)
            body.SetStatic();
        entity.Add(body);
        entity.Add(collider);
        RubedoEngine.Instance.World.AddBody(body);
        this.Add(entity);
        return body;
    }

    public override void Update()
    {
        deltaTime += (Time.RawDeltaTime - deltaTime) * 0.1f;
        base.Update();
        demos[selectedDemo].Update(this);
        DeleteIfTooFar();
        for (int i = 0; i < debugText.Count; i++)
        {
            debugText[i].Update();
        }
    }
    private void DeleteIfTooFar()
    {
        if (RubedoEngine.Instance.World.bodies.Count == 0)
            return;

        for (int i = 0; i < RubedoEngine.Instance.World.bodies.Count; i++)
        {
            PhysicsBody body = RubedoEngine.Instance.World.bodies[i];
            if (body.isStatic)
                continue;

            AABB bounds = body.bounds;

            if (bounds.max.Y < -50)
            {
                RubedoEngine.Instance.World.RemoveBody(body);
                //if (!RubedoEngine.Instance.World.RemoveBody(body))
                //    throw new System.Exception("FUQ");
                //body.Entity.State.Remove(body.Entity);
            }
        }
    }

    public override void Draw(Renderer sb)
    {
        if (mouseVertical != null && !mouseVertical.IsDestroyed)
        {
            Vector2 mouse = InputManager.MouseWorldPosition(MainCamera);
            Vector2 mouseScreen = InputManager.MouseScreenPosition(MainCamera);
            mouseVertical.Offset = GUI.Root.ScreenToUI(mouseScreen + new Vector2(15, 0));

            Label world = mouseVertical.Children[0] as Label;
            Label screen = mouseVertical.Children[1] as Label;
            world.Text = mouse.ToNiceString();
            screen.Text = mouseScreen.ToNiceString();
        }

        /*Vector2 mouse = InputManager.MouseWorldPosition();
        Vector2 mouseScreen = InputManager.MouseScreenPosition();
        DebugText.Instance.DrawText(mouseScreen, 1f, mouse.ToNiceString(), 16, Renderer.Space.Screen);
        DebugText.Instance.DrawText(mouseScreen + new Vector2(0, 30), 1f, mouseScreen.ToNiceString(), 16, Renderer.Space.Screen);*/


        shapes.Begin(MainCamera);

        for (int i = 0; i < RubedoEngine.Instance.World.bodies.Count; i++)
        {
            PhysicsBody body = RubedoEngine.Instance.World.bodies[i];
            if (!mainCamera.Intersects(in body.bounds))
                continue; //shape not visible, don't draw!

            Color speedColor = Color.Black;
            if (colorVelocity)
            {
                if (body.isStatic)
                    speedColor = new Color(50, 50, 50);
                else
                {
                    float val = body.LinearVelocity.Length() * 20f;
                    float vel = 220 - System.MathF.Min(val, 220) % 360;
                    ColorExtensions.HsvToRgb(vel, 1, 1, out int r, out int g, out int b);
                    speedColor = new Color(r, g, b);
                }
            }
            if (showAABB)
            {
                AABB bounds = body.bounds;
                shapes.DrawBox(bounds.min, bounds.max, Color.Green);
            }

            switch (body.collider.shape.type)
            {
                case ShapeType.Circle:
                    Circle shape = (Circle)body.collider.shape;
                    Vector2 vA = shape.Transform.Position;
                    Vector2 vB = shape.Transform.LocalToWorldPosition(Vector2.UnitY * shape.radius);

                    shapes.DrawCircleFill(shape.Transform, shape.radius, speedColor);
                    shapes.DrawLine(vA, vB, Color.White);
                    shapes.DrawCircle(shape.Transform, shape.radius, Color.White);
                    break;
                case ShapeType.Box:
                    Box box = (Box)body.collider.shape;
                    shapes.DrawBoxFill(box.Transform, box.width, box.height, speedColor);
                    shapes.DrawBox(box.Transform, box.width, box.height, Color.White);
                    break;
                case ShapeType.Capsule:
                    Capsule capsule = (Capsule)body.collider.shape;
                    capsule.TransformPoints();
                    shapes.DrawCapsuleFill(capsule.Transform, capsule.transRadius, capsule.transStart, capsule.transEnd, speedColor);
                    shapes.DrawCapsule(capsule.Transform, capsule.transRadius, capsule.transStart, capsule.transEnd, Color.White);
                    break;
                case ShapeType.Polygon:
                    Polygon polygon = (Polygon)body.collider.shape;
                    shapes.DrawPolygonFill(polygon.vertices, ShapeUtility.ComputeTriangles(polygon.VertexCount), polygon.Transform, speedColor);
                    shapes.DrawPolygon(polygon.vertices, polygon.Transform, Color.White);
                    break;
            }
            if (showVelocity)
                shapes.DrawLine(body.Entity.Transform.Position, body.Entity.Transform.Position + body.LinearVelocity, Color.Aquamarine);
        }


        shapes.End();
        base.Draw(sb);

        /*shapes.Begin(mainCamera);
        foreach (IRenderable renderable in Renderables.ComponentsWithLayer((int)RenderLayer.Default))
        {
            RectF bounds = renderable.Bounds;
            shapes.DrawBox(bounds.TopLeft, bounds.BottomRight, Color.White);
        }

        RubedoEngine.Instance.World.DebugDraw(shapes);
        shapes.End();*/

        //GUI.Root?.DebugRender(shapes, mainCamera);

        /*MainCamera.GetExtents(out Vector2 min, out _);
        shapes.DrawLine(mouse, new Vector2(mouse.X, min.Y), Color.DarkRed);
        shapes.DrawLine(mouse, new Vector2(min.X, mouse.Y), Color.DarkRed);*/
        //draw text
        MainCamera.SetViewport();
        sb.Begin(MainCamera, SamplerState.PointClamp);
        DebugText.Instance.Draw(sb);
        sb.End();
        MainCamera.ResetViewport();
    }

    public class DebugTextEntry
    {
        Func<string> valueFunc;
        public Label label;

        public DebugTextEntry(Label label, Func<string> valueFunc)
        {
            this.label = label;
            this.valueFunc = valueFunc;
        }

        public void Update()
        {
            label.Text = valueFunc();
        }
    }
}