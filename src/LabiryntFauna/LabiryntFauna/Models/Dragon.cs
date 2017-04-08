using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabiryntFauna.Models
{
    public class Dragon
    {
        private Model dragonModel;
        private Model dragonModelFlat;
        private Texture2D dragonTexture;
        private Model wingModel;
        private Model wingModelFlat;
        private Texture2D wingTexture;

        private Matrix projection;
        private Matrix world;
        private float scale=0.03f;

        public Dragon()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            world = Matrix.CreateTranslation(0, 0, 0);
        }

        public void LoadContent(ContentManager Content)
        {
            wingModel = Content.Load<Model>("Models/dragon_wing");
            wingModelFlat = Content.Load<Model>("ModelsFlat/dragon_wing");
            wingTexture = Content.Load<Texture2D>("Textures/dragon_wing");
            dragonModel = Content.Load<Model>("Models/Dragon");
            dragonModelFlat = Content.Load<Model>("ModelsFlat/Dragon");
            dragonTexture = Content.Load<Texture2D>("Textures/dragon");
        }

        public void Draw(Effect effect, Matrix view, Vector3 cameraPosition, Vector3 lightPosition, Color lightColor, Vector3 position, bool flat)
        {
            Vector4 colorVector = new Vector4(lightColor.R, lightColor.G, lightColor.B, 1);

            Model currentModel = flat ? dragonModelFlat : dragonModel;

            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * Matrix.CreateRotationZ(MathHelper.PiOver2) * mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(7, 0.2f, -20)));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(dragonTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }
            currentModel = flat ? wingModelFlat : wingModel;
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * Matrix.CreateRotationZ(MathHelper.PiOver2) * mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(7, 0.2f, -20)));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(wingTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }
        }

    }
}
