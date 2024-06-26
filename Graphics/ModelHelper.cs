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

            foreach (ModelMesh mesh in settings.Model.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = settings.World;
                    effect.View = settings.View;
                    effect.Projection = settings.Projection;

                    if (settings.MeshProperties.ContainsKey(mesh.Name))
                    {
                        var meshSetting = settings.MeshProperties[mesh.Name];
                        if (meshSetting.Texture != null)
                        {
                            effect.TextureEnabled = true;
                            effect.Texture = meshSetting.Texture;
                        }
                        else
                        {
                            effect.DiffuseColor = meshSetting.Color;
                        }
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

                    effect.Alpha = settings.Alpha;
                }

                //mesh.Draw();

            }

            settings.Model.Draw(settings.World, settings.View, settings.Projection);

        }
    }
}
