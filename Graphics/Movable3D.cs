﻿using Backbone.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public class Movable3D
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
        public Movable3D Parent { get; set; }

        public bool IsVisible { get; set; } = true;

        public Matrix World { get; private set; } = Matrix.Identity;

        public Dictionary<string, MeshProperty> MeshProperties { get; private set; } = new Dictionary<string, MeshProperty>();



        public Boolean IsAnimating {  
            get
            {
                return (queuedActions != null && queuedActions.Count > 0);
            }
        }

        /// 0f is fully transparent, 1f is fully opaque
        public float Alpha { get; set; } = 1f;

        private List<IAction3D> queuedActions = new List<IAction3D>();

        public ColorType Color1 = ColorType.White;
        public ColorType Color2 = ColorType.White;
        public ColorType ColorBkg = ColorType.Gray;

        public Movable3D(Model model, Vector3 startPosition, float scale)
        {
            Model = model;
            Position = startPosition;
            SetScale(scale);
            UpdateMatrix();
        }
        public void SetScale(float scale)
        {
            SetScale(new Vector3(scale, scale, scale));
        }
        public void SetScale(Vector3 newScale)
        {
            Scale = newScale;
            ScaleMatrix = (Parent != null) ? Matrix.CreateScale(Scale * Parent.Scale) : Matrix.CreateScale(Scale);
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

            TranslationMatrix = (Parent != null) ? Matrix.CreateTranslation(Position + Parent.Position) : Matrix.CreateTranslation(Position);

            World = ScaleMatrix * RotationMatrix * TranslationMatrix;
        }

        public float DegToRad(float rotationDegrees)
        {
            return (float)(rotationDegrees * System.Math.PI) / 180.0f;
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
                ModelHelper.DrawHexTile(Model, World, view, projection, Alpha, Color1, Color2, ColorBkg, MeshProperties);
            }
        }

        public static Movable3D Empty()
        {
            return new Movable3D(null, Vector3.Zero, 0f);
        }
    }
}
