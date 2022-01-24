using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProximityND;
using System.Collections.Generic;

namespace Backbone.Graphics
{
    public class ModelHelper
    {
        public static void DrawHexTile(Model model, Matrix world, Matrix view, Matrix projection, ColorType colorFront, ColorType colorBack, ColorType colorHex, Dictionary<string, ColorType> MeshColors)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                    if(MeshColors.ContainsKey(mesh.Name))
                    {
                        effect.DiffuseColor = ColorType3D.Get(MeshColors[mesh.Name]);
                    }
                    else
                    {
                        if(mesh.Name == "char")
                        {
                            effect.DiffuseColor = ColorType3D.Get(ColorType.DefaultText);
                        }
                    }



                    if(mesh.Name == "InnerFront")
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = ContentStore.Textures[ProximityND.Enums.TextureType.ColoredDots];
                        effect.DiffuseColor = ColorType3D.Get(ColorType.White);
                        //effect.DiffuseColor = ColorType3D.Get(colorFront);
                    }

                    if (mesh.Name == "InnerBack")
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = ContentStore.Textures[ProximityND.Enums.TextureType.XmasLeaves];
                        effect.DiffuseColor = ColorType3D.Get(ColorType.White);
//                        effect.DiffuseColor = ColorType3D.Get(colorBack);
                    }
                    
                    if (mesh.Name == "BorderMesh")
                    {
                        
                        effect.TextureEnabled = true;
                        effect.Texture = ContentStore.Textures[ProximityND.Enums.TextureType.Rust];
                        effect.DiffuseColor = ColorType3D.Get(ColorType.White);

                        //                        effect.DiffuseColor = ColorType3D.Get(colorHex);
                    }

                    if (mesh.Name == "NumberFront" || mesh.Name == "NumberBack")
                    {
//                        effect.TextureEnabled = true;
//                        effect.Texture = ContentStore.Textures[ProximityND.Enums.TextureType.Circles];
//                        effect.DiffuseColor = ColorType3D.Get(ColorType.White);
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

                    /*
                    if (mesh.Name == "NumberFront" || mesh.Name == "NumberBack")
                    {
                        effect.DiffuseColor = ColorType3D.Get(ColorType.White);
                        effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                        effect.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f);
                    }*/

                    effect.Alpha = 1.0f;
                }

                //mesh.Draw();

            }

            model.Draw(world, view, projection);
        }
    }
}
