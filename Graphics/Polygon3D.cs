using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProximityND.Backbone.Graphics
{
    public class Polygon3D : IGUI3D
    {
        GraphicsDevice graphics;
        VertexBuffer vertexBuffer;
        BasicEffect effect;

        public static VertexPositionColor[] DemoVertices = new VertexPositionColor[3]
        {
            new VertexPositionColor(new Vector3(0, 100, 10), Color.Red),
            new VertexPositionColor(new Vector3(50f, 0, 10), Color.Green),
            new VertexPositionColor(new Vector3(-50f, 0, 10), Color.Blue)
        };

        public Polygon3D(GraphicsDevice graphics, VertexPositionColor[] vertices)
        {
            this.graphics = graphics;
            effect = new BasicEffect(graphics);
            vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);
        }
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(Matrix view, Matrix projection)
        {
            effect.World = Matrix.Identity; //??
            effect.View = view;
            effect.Projection = projection;
            effect.VertexColorEnabled = true;

            graphics.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.RasterizerState = rasterizerState;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

        }

        public void HandleMouse(HandleMouseCommand command)
        {
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }
    }
}
