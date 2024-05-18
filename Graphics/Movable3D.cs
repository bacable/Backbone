using Backbone.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Backbone.Graphics
{
    // just adding a comment so i can check this in, since there seems to be a synching problem

    public class Movable3D : IInteractive
    {
        public Model Model { get; set; }
        public Vector3 Position { get; set; }

        public int Id { get; set; } = 0;

        public float RotationX = 0f;
        public float RotationY = 0f;
        public float RotationZ = 0f;
        public Matrix ScaleMatrix { get; private set; } = Matrix.Identity;
        public Matrix RotationMatrix { get; private set; } = Matrix.Identity;
        public Matrix TranslationMatrix { get; private set; } = Matrix.Identity;

        public Vector3 Scale = Vector3.Zero;

        private Movable3D _parent;
        public Movable3D Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                UpdateMatrix();
            }
        }

        public bool IsVisible { get; set; } = true;

        public Matrix World { get; private set; } = Matrix.Identity;

        public Dictionary<string, MeshProperty> MeshProperties { get; set; } = new Dictionary<string, MeshProperty>();

        public float? collisionRadius = null;

        public Boolean IsAnimating {  
            get
            {
                return (queuedActions != null && queuedActions.Count > 0);
            }
        }

        /// 0f is fully transparent, 1f is fully opaque
        public float Alpha { get; set; } = 1f;
        public bool IsInteractive { get { return !IsAnimating && IsVisible; } }

        private List<IAction3D> queuedActions = new List<IAction3D>();

        public string Color1 { get; set; } = ColorHex.DefaultColorHexCodes[ColorType.White];
        public string Color2 { get; set; } = ColorHex.DefaultColorHexCodes[ColorType.White];
        //public string ColorBkg = ColorHex.DefaultColorHexCodes[ColorType.Gray];
        public string ColorText { get; set; } = ColorHex.DefaultColorHexCodes[ColorType.DefaultText];

        public Movable3D(Model model, Vector3 startPosition, float scale)
        {
            Model = model;
            Position = startPosition;
            SetScale(scale);
            UpdateMatrix();
        }

        public Movable3D()
        {
        }

        public void SetScale(float scale)
        {
            SetScale(new Vector3(scale, scale, scale));
        }
        public void SetScale(Vector3 newScale)
        {
            Scale = newScale;

            var matrixScale = newScale;
            var parent = Parent;
            while(parent != null)
            {
                matrixScale = matrixScale * parent.Scale;
                parent = parent.Parent;
            }

            ScaleMatrix = Matrix.CreateScale(matrixScale);
        }

        public void Update(GameTime gameTime)
        {
            if(queuedActions != null && queuedActions.Count > 0)
            {
                var currentAction = queuedActions[0];
                // if true, it's finished with the animation
                if(currentAction.Update(this, gameTime))
                {
                    queuedActions.Remove(currentAction);
                }
            }

            UpdateMatrix();

        }

        private void UpdateMatrix()
        {
            float rotXRadians = (Parent != null) ? DegToRad(RotationX + Parent.RotationX) : DegToRad(RotationX);
            float rotYRadians = (Parent != null) ? DegToRad(RotationY + Parent.RotationY) : DegToRad(RotationY);
            float rotZRadians = (Parent != null) ? DegToRad(RotationZ + Parent.RotationZ) : DegToRad(RotationZ);

            RotationMatrix = Matrix.CreateRotationY(rotYRadians) * Matrix.CreateRotationX(rotXRadians) * Matrix.CreateRotationZ(rotZRadians);

            TranslationMatrix = Matrix.CreateTranslation(PositionWithParents);

            World = ScaleMatrix * RotationMatrix * TranslationMatrix;
        }

        // takes into account the parent, and any parents of those parents
        private Vector3 PositionWithParents
        {
            get
            {
                var position = Position;
                var parent = Parent;
                while (parent != null)
                {
                    position = position + parent.Position;
                    parent = parent.Parent;
                }
                return position;
            }
        }

        public string Name { get; set; }

        public float DegToRad(float rotationDegrees)
        {
            return (float)(rotationDegrees * System.Math.PI) / 180.0f;
        }

        /// <summary>
        /// Keep the currently running action, but wrap it and the passed in action into a new group action
        /// and make that the current action instead. Allows for things like letting something keep it's current
        /// movement animation, but adding a fade out animation to it in the middle of it for a transition.
        /// </summary>
        /// <param name="action">The animation to group with the current running animation.</param>
        public void RunBlendWithExisting(IAction3D action)
        {
            if(queuedActions.Count > 0)
            {
                var currentAction = queuedActions[0];
                var groupAction = ActionBuilder.Group(action, currentAction);
                queuedActions[0] = groupAction;
            }
        }

        public void Run(IAction3D action, bool replaceExisting = false)
        {
            if(replaceExisting)
            {
                queuedActions.Clear();
            }

            queuedActions.Add(action);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            if(IsVisible && Alpha > 0f)
            {
                //TODO: this shouldn't be getting created every draw call, that's not good
                MeshProperties["InnerFront"] = new MeshProperty() { Color = ColorHex.Get(Color1) };
                MeshProperties["InnerBack"] = new MeshProperty() { Color = ColorHex.Get(Color2) };
                //MeshProperties["BorderMesh"] = new MeshProperty() { Color = ColorHex.Get(ColorBkg) };
                MeshProperties["NumberFront"] = new MeshProperty() { Color = ColorHex.Get(ColorText) };
                MeshProperties["NumberBack"] = new MeshProperty() { Color = ColorHex.Get(ColorText) };

                var settings = new ModelDrawSettings()
                {
                    Alpha = Alpha,
                    MeshProperties = MeshProperties,
                    Model = Model,
                    Projection = projection,
                    View = view,
                    World = World
                };
                ModelHelper.Draw(settings);
            }
        }

        public static Movable3D Empty()
        {
            return new Movable3D(null, Vector3.Zero, 0f);
        }

        public bool Intersects(Viewport viewport, Vector2 position, Vector2 zero, Vector2 ratio, float? overrideRadius)
        {
            var xpos = ((this.Parent != null) ? Position.X + this.Parent.Position.X : Position.X) * ratio.X;
            var ypos = ((this.Parent != null) ? Position.Y + this.Parent.Position.Y : Position.Y) * ratio.Y;

            return Collision2D.IntersectCircle(position, new Vector2(xpos, ypos), overrideRadius ?? collisionRadius ?? 0f);
        }

        public void Run(IAction3D action)
        {
            this.Run(action, true);
        }

        public void UpdatePosition(Vector3 newPosition)
        {
            Position = newPosition;
            UpdateMatrix();
        }

        public void UpdateColor(string meshName, string newColor)
        {
            var vectorColor = ColorHex.Get(newColor);
            MeshProperties[meshName] = new MeshProperty { Color = vectorColor };
        }

        internal void AddMeshProperties(Dictionary<string, MeshProperty> newMeshProperties)
        {
            foreach(var kvp in newMeshProperties)
            {
                MeshProperties[kvp.Key] = kvp.Value;
            }
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public void SetAlpha(float alpha)
        {
            Alpha = alpha;
        }
    }
}
