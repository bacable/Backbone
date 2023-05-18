using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Graphics
{
    public class GraphData
    {
        public Color Color { get; set; } = Color.White;
        public float[] Values { get; set; }
    }

    public class GraphLineWidths
    {
        public float Outer { get; set; }
        public float Inner { get; set; }
        public float Values { get; set; }
    }

    public class Graph3DSettings
    {
        public Vector3 Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }
        public int XAxisSegments { get; set; }
        public int YAxisSegments { get; set; }
        public GraphLineWidths LineWidths { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
    }


    public class Graph3D : IGUI3D
    {
        /// <summary>
        /// Dictionary of the Color of the line, and the values for that line
        /// </summary>        
        private List<GraphData> GraphData { get; set; } = new List<GraphData>();

        public float MinValue { get; set; } = 0f;
        public float MaxValue { get; set; } = 0f;

        public int MaxCount { get; set; } = 0;

        private Vector3 Origin { get; set; }
        private float Width { get; set; }
        private float Height { get; set; }
        private float Depth { get; set; }
        private int XAxisSegments { get; set; }
        private int YAxisSegments { get; set; }
        private Vector2[] XAxisValues { get; set; }
        private Vector2[] YAxisValues { get; set; }
        private GraphLineWidths LineWidths { get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        
        private BasicEffect basicEffect;

        public Graph3D(Graph3DSettings settings)
        {
            Origin = settings.Origin;
            Width = settings.Width;
            Height = settings.Height;
            Depth = settings.Depth;
            XAxisSegments = settings.XAxisSegments;
            YAxisSegments = settings.YAxisSegments;
            XAxisValues = new Vector2[settings.XAxisSegments];
            YAxisValues = new Vector2[settings.YAxisSegments];
            LineWidths = settings.LineWidths;
            GraphicsDevice = settings.GraphicsDevice;

            basicEffect = new BasicEffect(settings.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;

        }

        public void SetGraphData(List<GraphData> graphData)
        {
            GraphData = graphData;
            MaxValue = graphData.Max(data => data.Values.Length > 0 ? data.Values.Max() : 0);
            MinValue = graphData.Min(data => data.Values.Length > 0 ? data.Values.Min() : 0);
            MaxCount = graphData.Max(data => data.Values.Length > 0 ? data.Values.Count() : 0);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.World = Matrix.Identity;

            // Draw grid background
            DrawGrid(basicEffect);

            // Draw team data
            DrawLineData(basicEffect);
        }

        private void DrawGrid(BasicEffect effect)
        {
            Color gridColor = Color.LightGray;

            // Draw vertical lines
            for (int i = 0; i < XAxisSegments; i++)
            {
                float x = Origin.X + Width * i / (XAxisSegments - 1);
                Vector3 startPoint = new Vector3(x, Origin.Y, Origin.Z);
                Vector3 endPoint = new Vector3(x, Origin.Y + Height, Origin.Z);

                XAxisValues[i] = new Vector2(x, Origin.Y); // Store grid values

                VertexPositionColor[] vertices = new VertexPositionColor[]
                {
                new VertexPositionColor(startPoint, gridColor),
                new VertexPositionColor(endPoint, gridColor)
                };

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    DrawThinRectangle(GraphicsDevice, effect, startPoint, endPoint, (i == 0 || i == XAxisSegments - 1) ? LineWidths.Outer : LineWidths.Inner, gridColor);
                }
            }

            // Draw horizontal lines
            for (int i = 0; i < YAxisSegments; i++)
            {
                float y = Origin.Y + Height * i / (YAxisSegments - 1);
                Vector3 startPoint = new Vector3(Origin.X, y, Origin.Z);
                Vector3 endPoint = new Vector3(Origin.X + Width, y, Origin.Z);

                YAxisValues[i] = new Vector2(Origin.X, y); // Store grid values

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    DrawThinRectangle(GraphicsDevice, effect, startPoint, endPoint, (i == 0 || i == YAxisSegments - 1) ? LineWidths.Outer : LineWidths.Inner, gridColor);
                }
            }
        }

        private void DrawLineData(BasicEffect effect)
        {
            int numLines = GraphData.Count;
            float teamDepth = Depth / (numLines * 2); // Change depth difference between team lines

            float xDistance = Width / MaxCount;

            for (int i = 0; i < numLines; i++)
            {
                var data = GraphData[i];

                float z = Origin.Z - i * 2 * teamDepth;// (i * 2 + 1) * teamDepth; // Shift team lines forward in depth and ensure no overlap
                var height = (data.Values.Length == 0 || data.Values[0] == 0) ? 0: data.Values[0] / MaxValue; // prevent no data and divide by zero error
                Vector3 previousPoint = new Vector3(Origin.X, Origin.Y + Height * height, z);

                int segmentsPerXAxis = Math.Max(data.Values.Length / (XAxisSegments - 1), 1);


                for (int j = 1; j < data.Values.Length; j++)
                {
                    float x = Origin.X + xDistance * j;
                    //float y = Origin.Y + Height * teamData[i][j] + (i * 5);
                    var height2 = (data.Values[j] == 0) ? 0 : data.Values[j] / MaxValue;
                    float y = Origin.Y + Height * height2;
                    Vector3 currentPoint = new Vector3(x, y, z);

                    if (j % segmentsPerXAxis == 0)
                    {
                        VertexPositionColor[] vertices = new VertexPositionColor[]
                        {
                    new VertexPositionColor(previousPoint, data.Color),
                    new VertexPositionColor(currentPoint, data.Color)
                        };

                        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            DrawThinRectangle(GraphicsDevice, effect, previousPoint, currentPoint, LineWidths.Values, data.Color);
                        }
                    }

                    previousPoint = currentPoint;
                }
            }
        }

        private void DrawThinRectangle(GraphicsDevice graphicsDevice, BasicEffect effect, Vector3 startPoint, Vector3 endPoint, float lineWidth, Color color)
        {
            Vector3 delta = endPoint - startPoint;
            Vector3 sideOffset = Vector3.Normalize(new Vector3(-delta.Y, delta.X, 0)) * lineWidth / 2;

            VertexPositionColor[] vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(startPoint - sideOffset, color),
                new VertexPositionColor(startPoint + sideOffset, color),
                new VertexPositionColor(endPoint - sideOffset, color),

                new VertexPositionColor(endPoint - sideOffset, color),
                new VertexPositionColor(startPoint + sideOffset, color),
                new VertexPositionColor(endPoint + sideOffset, color)
            };

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, vertices, 0, 2);
            }
        }

        public void Update(GameTime gameTime)
        {
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