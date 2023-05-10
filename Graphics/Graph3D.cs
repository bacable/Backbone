using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximityND.Backbone.Graphics
{
    public class Graph3D
    {
        public Vector3 Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }

        public Graph3D(Vector3 origin, float width, float height, float depth)
        {
            Origin = origin;
            Width = width;
            Height = height;
            Depth = depth;
        }

        public void Draw(BasicEffect effect, GraphicsDevice graphicsDevice, Color[] teamColors, float[][] teamData)
        {
            int numTeams = teamColors.Length;
            float teamDepth = Depth / numTeams;

            for (int i = 0; i < numTeams; i++)
            {
                float z = Origin.Z + i * teamDepth;
                Vector3 previousPoint = new Vector3(Origin.X, Origin.Y + Height * teamData[i][0], z);

                for (int j = 1; j < teamData[i].Length; j++)
                {
                    float x = Origin.X + Width * j / (teamData[i].Length - 1);
                    float y = Origin.Y + Height * teamData[i][j];
                    Vector3 currentPoint = new Vector3(x, y, z);

                    VertexPositionColor[] vertices = new VertexPositionColor[]
                    {
                new VertexPositionColor(previousPoint, teamColors[i]),
                new VertexPositionColor(currentPoint, teamColors[i])
                    };

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
                    }

                    previousPoint = currentPoint;
                }
            }
        }

        /*
        public void Draw(BasicEffect effect, GraphicsDevice graphicsDevice, Color[] teamColors, float[][] teamData)
        {
            int numTeams = teamColors.Length;
            float teamWidth = Width / numTeams;

            for (int i = 0; i < numTeams; i++)
            {
                float x = Origin.X + i * teamWidth;
                Vector3 previousPoint = new Vector3(x, Origin.Y, Origin.Z);

                for (int j = 0; j < teamData[i].Length; j++)
                {
                    float z = Origin.Z + Depth * j / (teamData[i].Length - 1);
                    float y = Origin.Y + Height * teamData[i][j];
                    Vector3 currentPoint = new Vector3(x, y, z);

                    VertexPositionColor[] vertices = new VertexPositionColor[]
                    {
                new VertexPositionColor(previousPoint, teamColors[i]),
                new VertexPositionColor(currentPoint, teamColors[i])
                    };

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
                    }

                    previousPoint = currentPoint;
                }
            }
        }*/
    }
}
