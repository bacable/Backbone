using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public class ModelHelper
    {
        public static void DrawHexTile(Model model, Matrix world, Matrix view, Matrix projection, float alpha, ColorType colorFront, ColorType colorBack, ColorType colorHex, Dictionary<string, MeshProperty> Settings)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                    if(Settings.ContainsKey(mesh.Name))
                    {
                        var meshSetting = Settings[mesh.Name];
                        if(meshSetting.Texture != null)
                        {
                            effect.TextureEnabled = true;
                            effect.Texture = meshSetting.Texture;
                        }
                        else
                        {
                            effect.DiffuseColor = meshSetting.Color;
                        }
                    }

                    if(mesh.Name == "InnerFront")
                    {
                        //effect.TextureEnabled = true;
                        //effect.Texture = ContentStore.Textures[ProximityND.Enums.TextureType.Marble];
                        //
                        //effect.DiffuseColor = ColorType3D.Get(ColorType.White);
                        effect.DiffuseColor = ColorType3D.Get(colorFront);
                    }

                    if (mesh.Name == "InnerBack")
                    {
                        //effect.TextureEnabled = true;
                        //effect.Texture = ContentStore.Textures[ProximityND.Enums.TextureType.Grass];
                        //effect.DiffuseColor = ColorType3D.Get(ColorType.White);
                        effect.DiffuseColor = ColorType3D.Get(colorBack);
                    }
                    
                    if (mesh.Name == "BorderMesh")
                    {
                        //effect.TextureEnabled = true;
                        //effect.Texture = ContentStore.Textures[ProximityND.Enums.TextureType.RustedGround];
                        //effect.DiffuseColor = ColorType3D.Get(ColorType.White);
                        effect.DiffuseColor = ColorType3D.Get(colorHex);
                    }

                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
                    effect.DirectionalLight0.Direction = new Vector3(0, 0, -1);
                    effect.DirectionalLight0.SpecularColor = new Vector3(0f, 0f, 0f);

                    effect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.DirectionalLight1.Direction = new Vector3(0, 0, 1);
                    effect.DirectionalLight1.SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);

                    //effect.DirectionalLight1.Enabled = false;
                    effect.DirectionalLight2.Enabled = false;

                    effect.Alpha = alpha;
                }

                //mesh.Draw();

            }

            model.Draw(world, view, projection);
        }
    }
}
