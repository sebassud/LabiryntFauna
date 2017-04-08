using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabiryntFauna.Models
{
    public class LabyrinthForest : ILabyrinth
    {
        private Model model;
        private Texture2D texture;

        private Model treasureModel;
        private Texture2D treasureTexture;
        private Model treasureModelFlat;

        private Model centaurModel;
        private Texture2D centaurTexture;
        private Vector3 centaurPosition;
        private Model centaurModelFlat;

        private Matrix projection;
        private Matrix world;
        private float scale;

        public LabyrinthForest()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            world = Matrix.CreateTranslation(0, 0, 0);
            scale = 0.01f;
            centaurPosition = new Vector3(2, 0, -17);
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/textgrass");
            model = Content.Load<Model>("Models/labirynt5");
            treasureModel = Content.Load<Model>("Models/treasure_chest");
            treasureTexture = Content.Load<Texture2D>("Textures/treasure_chest");
            treasureModelFlat = Content.Load<Model>("ModelsFlat/treasure_chest");
            centaurModel = Content.Load<Model>("Models/cent");
            centaurModelFlat = Content.Load<Model>("ModelsFlat/cent");
            centaurTexture = Content.Load<Texture2D>("Textures/textcent");
        }

        public void DrawModel(Effect effect, Matrix view, Vector3 cameraPosition, Vector3 lightPosition , Color lightColor, bool flat)
        {
            Vector4 colorVector = new Vector4(lightColor.R, lightColor.G, lightColor.B, 1);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform * Matrix.CreateScale(scale));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(texture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }

            Model currentModel = flat ? treasureModelFlat : treasureModel;
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * Matrix.CreateRotationZ(MathHelper.Pi) * mesh.ParentBone.Transform * Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(new Vector3(-5,0,1)));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(treasureTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }

            currentModel = flat ? centaurModelFlat : centaurModel;
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform * Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(centaurPosition));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(centaurTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(1);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }
        }

        public bool IsCollision (Vector3 position)
        {
            if (Math.Abs(position.X) < 16.2 && Math.Abs(position.Z) < 16.2) return true;
            else return false;
        }

        public Vector3 GetStartPosition()
        {
            return new Vector3(15, 0.2f, 15);
        }

        public void Update(float amount)
        {
            centaurPosition += Vector3.UnitZ * amount/2;

            if (centaurPosition.Z >= 17) centaurPosition.Z = -17;
        }

        public bool IsEnd(Vector3 p)
        {
            if (p.X > -6 && p.X < -4 && p.Z > 0 && p.Z < 2) return true;

            return false;
        }
    }
}
