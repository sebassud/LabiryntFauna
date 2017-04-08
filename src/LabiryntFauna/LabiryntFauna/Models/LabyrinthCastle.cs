using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabiryntFauna.Models
{
    public class LabyrinthCastle : ILabyrinth
    {
        private Model model;
        private Texture2D texture;
        private Model modelFlat;

        private Model treasureModel;
        private Texture2D treasureTexture;
        private Model treasureModelFlat;

        private Model ghostModel;
        private Texture2D ghostTexture;
        private Model ghostModelFlat;

        private Dragon dragon;

        private Model tableModel;
        private Texture2D tableTexture;
        private Model tableModelFlat;

        private Model chimneyModel;
        private Texture2D chimneyTexture;

        private Model pictureModel;
        private Texture2D pictureTexture;

        private Matrix projection;
        private Matrix world;
        private float scale;
        private bool[,] board;
        private Vector3 ghostPosition;

        public LabyrinthCastle()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            world = Matrix.CreateTranslation(0, 0, 0);
            scale = 0.02f;
            dragon = new Dragon();
            InitializeBoard();
            ghostPosition = new Vector3(21, 0.2f, 7f);
        }

        private void InitializeBoard()
        {
            board = new bool[22, 24];

            for (int i = 0; i < 22; i++)
                for (int j = 0; j < 24; j++)
                {
                    if(i!=0 && j!=0 && i!=21 && j!=23) board[i, j] = true;
                }

            board[21, 2] = true;
            board[3, 20] = false;
            board[3, 19] = false;
            board[3, 18] = false;
            board[3, 17] = false;
            board[3, 16] = false;
            board[3, 15] = false;
            board[4, 15] = false;
            board[5, 15] = false;
            board[6, 15] = false;
            board[7, 15] = false;
            board[8, 15] = false;
            board[9, 15] = false;
            board[10, 15] = false;
            board[11, 15] = false;
            board[12, 15] = false;
            board[12, 16] = false;
            board[12, 17] = false;
            board[12, 18] = false;
            board[12, 19] = false;
            board[12, 20] = false;
            board[12, 21] = false;
            board[12, 22] = false;
            board[6, 22] = false;
            board[6, 21] = false;
            board[6, 20] = false;
            board[6, 19] = false;
            board[6, 18] = false;
            board[7, 18] = false;
            board[8, 18] = false;
            board[9, 18] = false;
            board[9, 19] = false;
            board[9, 20] = false;

            board[17, 20] = false;
            board[16, 20] = false;
            board[15, 20] = false;
            board[16, 19] = false;
            board[16, 18] = false;
            board[16, 17] = false;
            board[16, 16] = false;
            board[16, 15] = false;
            board[17, 15] = false;
            board[18, 15] = false;
            board[19, 18] = false;
            board[20, 18] = false;
            board[19, 12] = false;
            board[20, 12] = false;
            board[16, 12] = false;
            board[15, 12] = false;
            board[14, 12] = false;
            board[13, 12] = false;
            board[16, 11] = false;
            board[16, 10] = false;
            board[16, 9] = false;
            board[17, 9] = false;
            board[18, 9] = false;

            board[12, 12] = false;
            board[11, 12] = false;
            board[10, 12] = false;
            board[9, 12] = false;
            board[8, 12] = false;
            board[7, 12] = false;
            board[6, 12] = false;
            board[5, 12] = false;
            board[4, 12] = false;
            board[3, 12] = false;
            board[2, 12] = false;
            board[1, 12] = false;
            board[4, 11] = false;
            board[4, 10] = false;
            board[4, 9] = false;
            board[3, 9] = false;

            board[1, 6] = false;
            board[2, 6] = false;
            board[3, 6] = false;
            board[4, 6] = false;
            board[5, 6] = false;
            board[6, 6] = false;
            board[7, 6] = false;
            board[7, 7] = false;
            board[7, 8] = false;
            board[7, 9] = false;

            board[3, 3] = false;
            board[4, 3] = false;
            board[5, 3] = false;
            board[6, 3] = false;
            board[7, 3] = false;
            board[8, 3] = false;
            board[9, 3] = false;
            board[10, 3] = false;
            board[10, 4] = false;
            board[10, 5] = false;
            board[10, 6] = false;
            board[11, 6] = false;
            board[12, 6] = false;
            board[13, 6] = false;
            board[14, 6] = false;
            board[15, 6] = false;
            board[16, 6] = false;
            board[17, 6] = false;
            board[18, 6] = false;
            board[19, 6] = false;
            board[20, 6] = false;

            board[13, 3] = false;
            board[14, 3] = false;
            board[15, 3] = false;

            board[18, 1] = false;
            board[18, 2] = false;
            board[18, 3] = false;

            board[15, 1] = false;
            board[15, 2] = false;
            board[14, 1] = false;
            board[14, 2] = false;
            board[13, 1] = false;
            board[13, 2] = false;
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/wall");
            model = Content.Load<Model>("Models/labirynt7");
            modelFlat = Content.Load<Model>("ModelsFlat/labirynt7");
            treasureModel = Content.Load<Model>("Models/treasure_chest");
            treasureModelFlat = Content.Load<Model>("ModelsFlat/treasure_chest");
            treasureTexture = Content.Load<Texture2D>("Textures/treasure_chest");
            ghostModel = Content.Load<Model>("Models/knight");
            ghostModelFlat = Content.Load<Model>("ModelsFlat/knight");
            ghostTexture = Content.Load<Texture2D>("Textures/BB8B6D4F");
            tableModel = Content.Load<Model>("Models/table");
            tableModelFlat = Content.Load<Model>("ModelsFlat/table");
            tableTexture = Content.Load<Texture2D>("Textures/Steelplt");
            chimneyModel = Content.Load<Model>("Models/fireplace");
            chimneyTexture = Content.Load<Texture2D>("Textures/firelm");
            pictureModel = Content.Load<Model>("Models/picture");
            pictureTexture = Content.Load<Texture2D>("Textures/textpicture");
            dragon.LoadContent(Content);
        }

        public void DrawModel(Effect effect, Matrix view, Vector3 cameraPosition, Vector3 lightPosition, Color lightColor, bool flat)
        {
            Model currentModel = flat ? modelFlat : model;
            Vector4 colorVector = new Vector4(lightColor.R, lightColor.G, lightColor.B, 1);
            foreach (ModelMesh mesh in currentModel.Meshes)
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
                    effect.Parameters["SpecularIntensity"].SetValue(0.8f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }

            currentModel = flat ? treasureModelFlat : treasureModel;
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * Matrix.CreateRotationZ(MathHelper.PiOver2) * mesh.ParentBone.Transform * Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(new Vector3(21, 0.2f, -19)));
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

            currentModel = flat ? ghostModelFlat : ghostModel;
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (mesh.ParentBone.Index != 1 && !flat || mesh.ParentBone.Index != 2 && flat) continue;
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * Matrix.CreateRotationZ(-MathHelper.PiOver2) * mesh.ParentBone.Transform * Matrix.CreateScale(0.025f) * Matrix.CreateTranslation(ghostPosition));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(ghostTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(1);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }

            currentModel = flat ? tableModelFlat : tableModel;
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * Matrix.CreateRotationY(-MathHelper.PiOver2) * mesh.ParentBone.Transform * Matrix.CreateScale(1) * Matrix.CreateTranslation(new Vector3(2, 0, -5)));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(tableTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }

            foreach (ModelMesh mesh in chimneyModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform * Matrix.CreateRotationY(-MathHelper.Pi) * Matrix.CreateScale(0.0005f) * Matrix.CreateTranslation(new Vector3(2, 0, -10.2f)));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(chimneyTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }

            foreach (ModelMesh mesh in pictureModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    if(mesh.ParentBone.Index==5 || mesh.ParentBone.Index == 6 || mesh.ParentBone.Index == 7 || mesh.ParentBone.Index == 8 || mesh.ParentBone.Index == 4)
                        effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform * Matrix.CreateRotationY(-MathHelper.Pi) * Matrix.CreateScale(0.005f) * Matrix.CreateTranslation(new Vector3(11, 3.5f, 21.9f)));
                    else
                        effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform * Matrix.CreateRotationY(-MathHelper.Pi) * Matrix.CreateScale(0.005f) * Matrix.CreateTranslation(new Vector3(11, 1.5f, 21.9f)));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(pictureTexture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["SpecularIntensity"].SetValue(0.5f);
                    effect.Parameters["LightColor1"].SetValue(colorVector);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }

            dragon.Draw(effect, view, cameraPosition, lightPosition, lightColor, new Vector3(7, 0.2f, -20), flat);

        }

        public bool IsCollision(Vector3 position)
        {
            if (IsFreeField(position + Vector3.UnitX / 2) &&
                IsFreeField(position - Vector3.UnitX / 2) &&
                IsFreeField(position + Vector3.UnitZ / 2) &&
                IsFreeField(position - Vector3.UnitZ / 2)) return true;

            return false;
        }

        private bool IsFreeField(Vector3 position)
        {
            position /= 2;
            position.X += 11;
            position.Z += 12;

            return board[(int)position.X, (int)position.Z];
        }

        public Vector3 GetStartPosition()
        {
            return new Vector3(-15f, 0.2f, 21f);
        }

        public void Update(float amount)
        {
            ghostPosition -= Vector3.UnitX * amount / 1.5f;

            if (ghostPosition.X <= -29) ghostPosition.X = 21;
        }

        public bool IsEnd(Vector3 p)
        {
            if (p.X > 20.5 && p.X < 21.5 && p.Z > -19.5 && p.Z < -18.5) return true;

            return false;
        }
    }
}
