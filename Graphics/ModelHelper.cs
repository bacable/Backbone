using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public class ModelHelper
    {
        public static void Draw(ModelDrawSettings settings)
        {
            if (settings.Model == null) return;

            if (settings.CustomEffect == null && settings.BasicEffect == null)
            {
                // Create a new BasicEffect if it doesn't exist
                settings.BasicEffect = new BasicEffect(ScreenSettings.Graphics.GraphicsDevice)
                {
                    World = settings.World,
                    View = settings.View,
                    Projection = settings.Projection,
                    VertexColorEnabled = false, // Assuming no vertex colors are needed
                    LightingEnabled = true,
                    DiffuseColor = new Vector3(1, 1, 1), // Default color
                    Alpha = settings.Alpha
                };

                // Set up lighting (you can customize this)
                settings.BasicEffect.DirectionalLight0.Enabled = true;
                settings.BasicEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
                settings.BasicEffect.DirectionalLight0.Direction = new Vector3(0, 0, -1);
                settings.BasicEffect.DirectionalLight0.SpecularColor = new Vector3(0f, 0f, 0f);

                settings.BasicEffect.DirectionalLight1.Enabled = true;
                settings.BasicEffect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                settings.BasicEffect.DirectionalLight1.Direction = new Vector3(0, 0, 1);
                settings.BasicEffect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);

                settings.BasicEffect.DirectionalLight2.Enabled = false;
            }

            foreach (ModelMesh mesh in settings.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect effect = settings.CustomEffect ?? settings.BasicEffect;

                    if (effect is BasicEffect basicEffect)
                    {
                        // Set up the BasicEffect if no custom effect is provided
                        basicEffect.World = settings.World;
                        basicEffect.View = settings.View;
                        basicEffect.Projection = settings.Projection;

                        if (settings.MeshProperties.ContainsKey(mesh.Name))
                        {
                            var meshSetting = settings.MeshProperties[mesh.Name];
                            if (meshSetting.Texture != null)
                            {
                                basicEffect.TextureEnabled = true;
                                basicEffect.Texture = meshSetting.Texture;
                            }
                            else
                            {
                                basicEffect.DiffuseColor = meshSetting.Color;
                            }
                        }

                        basicEffect.LightingEnabled = true;

                        basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
                        basicEffect.DirectionalLight0.Direction = new Vector3(0, 0, -1);
                        basicEffect.DirectionalLight0.SpecularColor = new Vector3(0f, 0f, 0f);

                        basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                        basicEffect.DirectionalLight1.Direction = new Vector3(0, 0, 1);
                        basicEffect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);

                        basicEffect.DirectionalLight2.Enabled = false;

                        basicEffect.Alpha = settings.Alpha;
                    }
                    else
                    {
                        // Apply the custom effect (shader)
                        effect.Parameters["World"].SetValue(settings.World);
                        effect.Parameters["View"].SetValue(settings.View);
                        effect.Parameters["Projection"].SetValue(settings.Projection);

                        // Add any custom parameters you need to set for the shader
                        if (effect.Parameters["OutlineColor"] != null)
                            effect.Parameters["OutlineColor"].SetValue(new Vector4(1, 1, 1, 1)); // Example parameter

                        if (effect.Parameters["OutlineThickness"] != null)
                            effect.Parameters["OutlineThickness"].SetValue(115.0f); // Example parameter
                    }

                    // Draw the mesh part with the applied effect
                    part.Effect = effect;
                    mesh.Draw();
                }
            }
        }
    }
}
