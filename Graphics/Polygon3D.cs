using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Backbone.Graphics
{
    public class Polygon3D : IGUI3D
    {
        GraphicsDevice graphics;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect effect;

        public static VertexPositionColor[] DemoVertices = new VertexPositionColor[3]
        {
            new VertexPositionColor(new Vector3(0, 1, 0.1f), Color.Red),
            new VertexPositionColor(new Vector3(0.5f, 0, 0.1f), Color.Green),
            new VertexPositionColor(new Vector3(-0.5f, 0, 0.1f), Color.Blue)
        };

        public static VertexPositionColor[] DemoFlatHexagon = new VertexPositionColor[7]
        {
            new VertexPositionColor(new Vector3(-1,0,0.1f), Color.White), //left center - 0
            new VertexPositionColor(new Vector3(-0.5f,1,0.1f), Color.White), //upper left - 1
            new VertexPositionColor(new Vector3(0.5f,1,0.1f), Color.White), // upper right - 2
            new VertexPositionColor(new Vector3(1,0,0.1f), Color.White), // right center - 3
            new VertexPositionColor(new Vector3(0.5f,-1,0.1f), Color.White), // bottom right - 4
            new VertexPositionColor(new Vector3(-0.5f,-1,0.1f), Color.White), // bottom left - 5
            new VertexPositionColor(new Vector3(0,0,0.1f), Color.Black), // center point - 6
        };


        public static VertexPositionColor[] SquishedPointyHexagon = new VertexPositionColor[7]
{
            new VertexPositionColor(new Vector3(0,1,0.1f), Color.White), //top - 0
            new VertexPositionColor(new Vector3(1,0.5f,0.1f), Color.White), //top right - 1
            new VertexPositionColor(new Vector3(1,-0.5f,0.1f), Color.White), // bottom right - 2
            new VertexPositionColor(new Vector3(0,-1,0.1f), Color.White), // bottom - 3
            new VertexPositionColor(new Vector3(-1,-0.5f,0.1f), Color.White), // bottom left - 4
            new VertexPositionColor(new Vector3(-1,0.5f,0.1f), Color.White), // top left - 5
            new VertexPositionColor(new Vector3(0,0,0.1f), Color.White), // center point - 6
};

        const float sqr3div2 = 0.86602540378f;
        const float negsqr3div2 = -1 * sqr3div2;
        public static VertexPositionColor[] RegularPointyHexagon = new VertexPositionColor[7]
{
            new VertexPositionColor(new Vector3(0,sqr3div2,0.1f), Color.White), //top - 0
            new VertexPositionColor(new Vector3(sqr3div2,0.5f,0.1f), Color.White), //top right - 1
            new VertexPositionColor(new Vector3(sqr3div2,-0.5f,0.1f), Color.White), // bottom right - 2
            new VertexPositionColor(new Vector3(0,negsqr3div2,0.1f), Color.White), // bottom - 3
            new VertexPositionColor(new Vector3(negsqr3div2,-0.5f,0.1f), Color.White), // bottom left - 4
            new VertexPositionColor(new Vector3(negsqr3div2,0.5f,0.1f), Color.White), // top left - 5
            new VertexPositionColor(new Vector3(0,0,0.1f), Color.White), // center point - 6
};


        public static VertexPositionColor[] Icosahedron = new VertexPositionColor[12]
        {
            new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, 0.42532500f), Color.Red),
            new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, 0.42532500f), Color.Orange),
            new VertexPositionColor(new Vector3(-0.26286500f, 0.0000000f, -0.42532500f), Color.Yellow),
            new VertexPositionColor(new Vector3(0.26286500f, 0.0000000f, -0.42532500f), Color.Green),
            new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, 0.26286500f), Color.Blue),
            new VertexPositionColor(new Vector3(0.0000000f, 0.42532500f, -0.26286500f), Color.Indigo),
            new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, 0.26286500f), Color.Purple),
            new VertexPositionColor(new Vector3(0.0000000f, -0.42532500f, -0.26286500f), Color.White),
            new VertexPositionColor(new Vector3(0.42532500f, 0.26286500f, 0.0000000f), Color.Cyan),
            new VertexPositionColor(new Vector3(-0.42532500f, 0.26286500f, 0.0000000f), Color.Black),
            new VertexPositionColor(new Vector3(0.42532500f, -0.26286500f, 0.0000000f), Color.DodgerBlue),
            new VertexPositionColor(new Vector3(-0.42532500f, -0.26286500f, 0.0000000f), Color.Crimson)
        };

        public static short[] HexagonIndices = new short[18]
        {
            0, 6, 1,
            1, 6, 2,
            2, 6, 3,
            3, 6, 4,
            4, 6, 5,
            5, 6, 0
        };

        public static short[] IcosahedronIndices = new short[60]
        {
            0,6,1,
            0,11,6,
            1,4,0,
            1,8,4,
            1,10,8,
            2,5,3,
            2,9,5,
            2,11,9,
            3,7,2,
            3,10,7,
            4,8,5,
            4,9,0,
            5,8,3,
            5,9,4,
            6,10,1,
            6,11,7,
            7,10,6,
            7,11,2,
            8,10,3,
            9,11,0
        };

        public Polygon3D(GraphicsDevice graphics, VertexPositionColor[] vertices, short[] indices)
        {
            this.graphics = graphics;
            effect = new BasicEffect(graphics);
            vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), 12, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            indexBuffer = new IndexBuffer(graphics, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(Matrix view, Matrix projection)
        {
            effect.World = Matrix.CreateScale(100f) * Matrix.CreateTranslation(0, 0, 0);
            effect.View = view;
            effect.Projection = projection;
            effect.VertexColorEnabled = true;

            graphics.SetVertexBuffer(vertexBuffer);
            graphics.Indices = indexBuffer;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.RasterizerState = rasterizerState;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 6, 0, 6);
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
