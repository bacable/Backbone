using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.Graphics
{
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


    public class Graph3D
    {
        public Vector3 Origin { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }
        public int XAxisSegments { get; private set; }
        public int YAxisSegments { get; private set; }
        public Vector2[] XAxisValues { get; private set; }
        public Vector2[] YAxisValues { get; private set; }
        public GraphLineWidths LineWidths { get; private set; }
        public GraphicsDevice GraphicsDevice { get; set; }

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
        }

        public void Draw(BasicEffect effect, Color[] teamColors, float[][] teamData)
        {
            // Draw grid background
            DrawGrid(effect);

            // Draw team data
            DrawTeamData(effect, teamColors, teamData);
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

        private void DrawTeamData(BasicEffect effect, Color[] teamColors, float[][] teamData)
        {
            int numTeams = teamColors.Length;
            float teamDepth = Depth / (numTeams * 2); // Change depth difference between team lines

            for (int i = 0; i < numTeams; i++)
            {
                float z = Origin.Z + i * 2;// (i * 2 + 1) * teamDepth; // Shift team lines forward in depth and ensure no overlap
                Vector3 previousPoint = new Vector3(Origin.X, Origin.Y + Height * teamData[i][0], z);

                int segmentsPerXAxis = Math.Max(teamData[i].Length / (XAxisSegments - 1), 1);

                for (int j = 1; j < teamData[i].Length; j++)
                {
                    float x = Origin.X + Width * j / (teamData[i].Length - 1);
                    //float y = Origin.Y + Height * teamData[i][j] + (i * 5);
                    float y = Origin.Y + Height * teamData[i][j];
                    Vector3 currentPoint = new Vector3(x, y, z);

                    if (j % segmentsPerXAxis == 0)
                    {
                        VertexPositionColor[] vertices = new VertexPositionColor[]
                        {
                    new VertexPositionColor(previousPoint, teamColors[i]),
                    new VertexPositionColor(currentPoint, teamColors[i])
                        };

                        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            DrawThinRectangle(GraphicsDevice, effect, previousPoint, currentPoint, LineWidths.Values, teamColors[i]);
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
    }
}