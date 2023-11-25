using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Graphics.Particles
{
    public class Particle
    {
        public Model Model;
        public Matrix World;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public float Life;
        public float Gravity;
        public float Alpha; // Transparency component
        private float Alive = 0;

        public Particle(Model model, Vector3 position, Vector3 velocity, Vector3 angularVelocity, float life, float gravity)
        {
            Model = model;
            Position = position;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            Life = life;
            Gravity = gravity;
            Alpha = 1.0f; // Fully opaque initially
            UpdateWorldMatrix();
        }

        private void UpdateWorldMatrix()
        {
            World = Matrix.CreateTranslation(Position);
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Alive += elapsed;

            // Update particle position based on velocity and gravity
            Position += Velocity * elapsed;
            Velocity += Vector3.Down * Gravity * elapsed;

            // Update the world matrix
            UpdateWorldMatrix();

            // Update particle rotation based on angular velocity
            float rotationAmount = AngularVelocity.Length() * elapsed;
            if (rotationAmount > 0.0f)
            {
                AngularVelocity.Normalize();
                float rotationSpeed = rotationAmount / elapsed;
                Quaternion rotation = Quaternion.CreateFromAxisAngle(AngularVelocity, rotationSpeed * Alive);
                World = Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(Position);
            }

            // Update particle life
            Life -= elapsed;

            // Update particle transparency
            Alpha = MathHelper.Clamp(Life / 0.4f, 0.0f, 1.0f); // Adjust the divisor for the desired fade-out duration

        }
    }

}
