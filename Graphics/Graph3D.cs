using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND.Enums;
using ProximityND.Managers;
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

    public struct AxisSettings
    {
        public string Label;
        public int MinValue;
        public int MaxValue;
        public int NumSegments;
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
        public string HorizontalAxisLabel { get; set; } = string.Empty;
        public string VerticalAxisLabel { get; set; } = string.Empty;
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
        private string HorizontalAxisLabel { get; set; }
        private string VerticalAxisLabel { get; set; }
        private Vector2[] XAxisValues { get; set; }
        private Vector2[] YAxisValues { get; set; }
        private GraphLineWidths LineWidths { get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        
        private BasicEffect basicEffect;

        // horizontal numbers and header label (should be turns)
        private List<TextGroup> horizontalAxisLabels = new List<TextGroup>();

        // vertical numbers and header label (should either be points or stars)
        private List<TextGroup> verticalAxisLabels = new List<TextGroup>();

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
            HorizontalAxisLabel = settings.HorizontalAxisLabel;
            VerticalAxisLabel = settings.VerticalAxisLabel;

            basicEffect = new BasicEffect(settings.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;

        }

        public void SetupLabels(AxisSettings horizontalAxisSettings, AxisSettings verticalAxisSettings)
        {
            var textColor = ColorProvider.Get(ThemeElementType.TextColor);

            var axisLabelScale = 50f;

            var horizAxisLabel = new TextGroup(new TextGroupSettings()
            {
                Alignment = UI.TextAlign.Right,
                Color = textColor,
                Id = 0,
                Parent = Movable3D.Empty(),
                Position = new Vector3(Origin.X - 20f, Origin.Y - 40f, -1),
                Scale = axisLabelScale,
                Text = horizontalAxisSettings.Label,
            });
            horizontalAxisLabels.Add(horizAxisLabel);

            var xNumberRange = horizontalAxisSettings.MaxValue - horizontalAxisSettings.MinValue;
            var xAxisPositionXGap = (int)(Width / (horizontalAxisSettings.NumSegments - 1));
            for (var i = 1; i < horizontalAxisSettings.NumSegments; i++)
            {
                var xAxisValue = (int)Math.Floor(horizontalAxisSettings.MinValue + (int)Math.Ceiling((float)(xNumberRange * (i + 1))) / (float)verticalAxisSettings.NumSegments);

                var horizNumSegment = new TextGroup(new TextGroupSettings()
                {
                    Alignment = UI.TextAlign.Right,
                    Color = textColor,
                    Id = i,
                    Parent = Movable3D.Empty(),
                    Position = new Vector3(Origin.X + xAxisPositionXGap * i + 10.0f, Origin.Y - 40f, -1),
                    Scale = axisLabelScale,
                    Text = xAxisValue.ToString(),
                });
                horizontalAxisLabels.Add(horizNumSegment);
            }

            var vertAxisLabel = new TextGroup(new TextGroupSettings()
            {
                Alignment = UI.TextAlign.Left,
                Color = textColor,
                Id = 0,
                Parent = Movable3D.Empty(),
                Position = new Vector3(Origin.X + Width + 150f, Origin.Y + Height, -1),
                Scale = axisLabelScale,
                Text = verticalAxisSettings.Label,
            });
            verticalAxisLabels.Add(vertAxisLabel);

            var yNumberRange = verticalAxisSettings.MaxValue - verticalAxisSettings.MinValue;
            var yAxisPositionXGap = (int)(Height / (verticalAxisSettings.NumSegments - 1));

            for (var i = 1; i < verticalAxisSettings.NumSegments; i++)
            {
                var yAxisValue = (int)Math.Floor(verticalAxisSettings.MinValue + (int)Math.Ceiling((float)(yNumberRange * (i + 1))) / (float)verticalAxisSettings.NumSegments);

                var vertNumSegment = new TextGroup(new TextGroupSettings()
                {
                    Alignment = UI.TextAlign.Left,
                    Color = textColor,
                    Id = i,
                    Parent = Movable3D.Empty(),
                    Position = new Vector3(Origin.X + Width + 25.0f, Origin.Y + yAxisPositionXGap * i, -1),
                    Scale = axisLabelScale,
                    Text = yAxisValue.ToString(),
                });
                verticalAxisLabels.Add(vertNumSegment);
            }
        }

        public void SetGraphData(List<GraphData> graphData)
        {
            GraphData = graphData;
            MaxValue = graphData.Max(data => data.Values.Length > 0 ? data.Values.Max() : 0);
            MinValue = graphData.Min(data => data.Values.Length > 0 ? data.Values.Min() : 0);
            MaxCount = graphData.Max(data => data.Values.Length > 0 ? data.Values.Count() : 0);

            var horizontalAxisSettings = new AxisSettings()
            {
                NumSegments = XAxisSegments,
                Label = HorizontalAxisLabel,
                MaxValue = MaxCount,
                MinValue = 0
            };

            var verticalAxisSettings = new AxisSettings()
            {
                NumSegments = YAxisSegments,
                Label = VerticalAxisLabel,
                MaxValue = (int)Math.Round(MaxValue),
                MinValue = 0,
            };

            SetupLabels(horizontalAxisSettings, verticalAxisSettings);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.World = Matrix.Identity;

            verticalAxisLabels.ForEach(label => label.Draw(view, projection));
            horizontalAxisLabels.ForEach(label =>  label.Draw(view, projection));

            // Draw grid background
            DrawGrid(basicEffect);

            // Draw team data
            DrawLineData(basicEffect);
        }

        private void DrawGrid(BasicEffect effect)
        {
            Color gridColor = Color.LightGray;
            float z = Origin.Z - 20;

            // Draw vertical lines
            for (int i = 0; i < XAxisSegments; i++)
            {
                float x = Origin.X + Width * i / (XAxisSegments - 1);
                Vector3 startPoint = new Vector3(x, Origin.Y, z);
                Vector3 endPoint = new Vector3(x, Origin.Y + Height, z);

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
                Vector3 startPoint = new Vector3(Origin.X, y, z);
                Vector3 endPoint = new Vector3(Origin.X + Width, y, z);

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

            float xDistance = Width / (MaxCount - 1);

            for (int teamIndex = 0; teamIndex < numLines; teamIndex++)
            {
                var data = GraphData[teamIndex];

                float z = Origin.Z - teamIndex * 2 * teamDepth;// (i * 2 + 1) * teamDepth; // Shift team lines forward in depth and ensure no overlap
                var height = (data.Values.Length == 0 || data.Values[0] == 0) ? 0: data.Values[0] / MaxValue; // prevent no data and divide by zero error
                Vector3 previousPoint = new Vector3(Origin.X, Origin.Y + Height * height, z);

                int segmentsPerXAxis = Math.Max(data.Values.Length / (XAxisSegments - 1), 1);


                for (int j = 1; j < data.Values.Length; j++)
                {
                    float x = Origin.X + xDistance * j;
                    var height2 = (data.Values[j] == 0) ? 0 : data.Values[j] / MaxValue;
                    float y = Origin.Y + Height * height2;
                    Vector3 currentPoint = new Vector3(x, y, z);

                    
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
            verticalAxisLabels.ForEach(label => label.Update(gameTime));
            horizontalAxisLabels.ForEach(label => label.Update(gameTime));
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