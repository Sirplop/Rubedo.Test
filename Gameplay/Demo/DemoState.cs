﻿using Microsoft.Xna.Framework;
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
using Rubedo.Rendering;
using Rubedo.UI;
using System;
using System.Collections.Generic;
using Rubedo.Physics2D.Common;
using Rubedo.UI.Text;
using FontStashSharp;
using Rubedo.Internal.Assets;
using Rubedo.UI.Layout;

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
        new Demo6()
    };

    private readonly AllCondition prevDemo = new AllCondition(new KeyCondition(Keys.Left), new KeyCondition(Keys.LeftShift, true));
    private readonly AllCondition nextDemo = new AllCondition(new KeyCondition(Keys.Right), new KeyCondition(Keys.LeftShift, true));

    private readonly KeyCondition cameraLeft = new KeyCondition(Keys.H);
    private readonly KeyCondition cameraRight = new KeyCondition(Keys.K);
    private readonly KeyCondition cameraUp = new KeyCondition(Keys.U);
    private readonly KeyCondition cameraDown = new KeyCondition(Keys.J);

    private int selectedDemo = 0;
    public List<DebugTextEntry> debugText = new List<DebugTextEntry>();

    private float deltaTime = 0.0f;

    public DemoState(StateManager sm) : base(sm)
    {
        shapes = new Shapes(RubedoEngine.Instance);
        _name = "DemoState";
    }

    public override void LoadContent()
    {
        AssetManager.CreateNewFontSystem("fs-default", "fonts/DroidSans.ttf", "fonts/DroidSansJapanese.ttf", "fonts/Symbola-Emoji.ttf");
        base.LoadContent();
    }

    public override void Enter()
    {
        base.Enter();
        debugRoot = new Vertical();
        debugRoot.Offset = new Vector2(30, 0);
        GUI.Root.AddChild(debugRoot);

        RubedoEngine.SizeOfMeter = 1;
        PhysicsWorld.ResetGravity();
        RubedoEngine.Instance.Camera.SetZoom(24);

        //construct debug GUI

        demos[selectedDemo].Initialize(this);
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
        AddDebugLabel(debugRoot, () => $"(C)olor velocity: {(colorVelocity ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(S)how velocity: {(showVelocity ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(A)ABBs visible: {(showAABB ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"C(o)ntacts visible: {(PhysicsWorld.showContacts ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(D)raw Broadphase: {(PhysicsWorld.drawBroadphase ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(B)rute force: {(PhysicsWorld.bruteForce ? "Yes" : "No")}");
        AddDebugLabel(debugRoot, () => $"(F)ast Place: {(fastPlace ? "On" : "Off")}");
    }

    private void AddDebugLabel(Vertical vert, Func<string> valueFunc)
    {
        FontSystem font = AssetManager.GetFontSystem("fs-default");
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

        if (cameraLeft.Pressed() || cameraLeft.Held())
            RubedoEngine.Instance.Camera.Move(new Vector2(-1, 0));
        if (cameraRight.Pressed() || cameraRight.Held())
            RubedoEngine.Instance.Camera.Move(new Vector2(1, 0));
        if (cameraUp.Pressed() || cameraUp.Held())
            RubedoEngine.Instance.Camera.Move(new Vector2(0, 1));
        if (cameraDown.Pressed() || cameraDown.Held())
            RubedoEngine.Instance.Camera.Move(new Vector2(0, -1));

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
        PhysicsWorld.ResetGravity();
        RubedoEngine.Instance.Camera.SetZoom(24);
        RubedoEngine.Instance.World.Clear();
        foreach (Entity ent in Entities)
            Entities.Remove(ent);

        //remove all GUI elements except the debug vertical.    
        GUI.Root.RemoveChild(debugRoot);
        GUI.Root.DestroyChildren();
        debugRoot.DestroyChildren();
        GUI.Root.AddChild(debugRoot);
    }

    public PhysicsBody MakeBody(Entity entity, PhysicsMaterial material, Collider collider, bool isStatic)
    {
        PhysicsBody body = new PhysicsBody(collider, material, true, true);
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
        deltaTime += (RubedoEngine.RawDeltaTime - deltaTime) * 0.1f;
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
        /*Vector2 mouse = InputManager.MouseWorldPosition();
        Vector2 mouseScreen = InputManager.MouseScreenPosition();
        DebugText.Instance.DrawText(mouseScreen, 1f, mouse.ToNiceString(), 16, Renderer.Space.Screen);
        DebugText.Instance.DrawText(mouseScreen + new Vector2(0, 30), 1f, mouseScreen.ToNiceString(), 16, Renderer.Space.Screen);*/

        base.Draw(sb);

        shapes.Begin(RubedoEngine.Instance.Camera);

        for (int i = 0; i < RubedoEngine.Instance.World.bodies.Count; i++)
        {
            //RubedoEngine.Instance.World.GetBody(i, out PhysicsBody body);
            PhysicsBody body = RubedoEngine.Instance.World.bodies[i];
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
                    Vector2 vA = shape.transform.Position;
                    Vector2 vB = shape.transform.LocalToWorldPosition(Vector2.UnitY * shape.radius);

                    shapes.DrawCircleFill(shape.transform, shape.radius, 32, speedColor);
                    shapes.DrawLine(vA, vB, Color.White);
                    shapes.DrawCircle(shape.transform, shape.radius, 32, Color.White);
                    break;
                case ShapeType.Box:
                    Box box = (Box)body.collider.shape;
                    shapes.DrawBoxFill(box.transform, box.width, box.height, speedColor);
                    shapes.DrawBox(box.transform, box.width, box.height, Color.White);
                    break;
                case ShapeType.Capsule:
                    Capsule capsule = (Capsule)body.collider.shape;
                    capsule.TransformPoints();
                    shapes.DrawCapsuleFill(capsule.transform, capsule.transRadius, capsule.transStart, capsule.transEnd, 20, speedColor);
                    shapes.DrawCapsule(capsule.transform, capsule.transRadius, capsule.transStart, capsule.transEnd, 20, Color.White);
                    break;
                case ShapeType.Polygon:
                    Polygon polygon = (Polygon)body.collider.shape;
                    shapes.DrawPolygonFill(polygon.vertices, ShapeUtility.ComputeTriangles(polygon.VertexCount), polygon.transform, speedColor);
                    shapes.DrawPolygon(polygon.vertices, polygon.transform, Color.White);
                    break;
            }
            if (showVelocity)
                shapes.DrawLine(body.compTransform.Position, body.compTransform.Position + body.LinearVelocity, Color.Aquamarine);
        }

        //foreach (Entity ent in Entities)
        //    shapes.DrawBox(ent.transform, 0.25f, 0.25f, Color.Yellow);

        RubedoEngine.Instance.World.DebugDraw(shapes);

        /*RubedoEngine.Instance.Camera.GetExtents(out Vector2 min, out _);
        shapes.DrawLine(mouse, new Vector2(mouse.X, min.Y), Color.DarkRed);
        shapes.DrawLine(mouse, new Vector2(min.X, mouse.Y), Color.DarkRed);*/

        shapes.End();
        //draw text
        DebugText.Instance.Draw(sb);
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