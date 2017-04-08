using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabiryntFauna.Models
{
    public class Player
    {
        private Model model;
        private Model modelFlat;
        private Texture2D texture;

        private Matrix projection;
        private Matrix world;
        private float scale;
        private Vector3 position;
        private Matrix rotation = Matrix.Identity;

        float leftrightRot = MathHelper.Pi;
        MouseState originalMouseState;
        const float rotationSpeed = 0.03f;

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        public Player(Vector3 positionStart)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            world = Matrix.CreateTranslation(0, 0, 0);
            scale = 0.01f;
            position = positionStart;
            originalMouseState = Mouse.GetState();
        }

        public void SetPosition(Vector3 p)
        {
            position = p;
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/droid");
            model = Content.Load<Model>("Models/bb8");
            modelFlat = Content.Load<Model>("ModelsFlat/bb8");
        }

        public void Move(Vector3 vector, float amount, bool move)
        {
            ProcessInput(amount);
            if (!move) return;
            Matrix cameraRotation = Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vector, cameraRotation);
            rotatedVector *= amount;
            rotation *= Matrix.CreateRotationX(vector.Z * amount) * Matrix.CreateRotationY(vector.X * amount);
            position += rotatedVector;
        }

        public Vector3 GetPosition(Vector3 move, float amount)
        {
            MouseState currentMouseState = Mouse.GetState();
            float xDifference = currentMouseState.X - originalMouseState.X;
            float leftrightRot2 = leftrightRot - rotationSpeed * xDifference * amount;
            Matrix cameraRotation = Matrix.CreateRotationY(leftrightRot2);
            Vector3 rotatedVector = Vector3.Transform(move, cameraRotation);
            rotatedVector *= amount;

            return position + rotatedVector;
        }

        public void DrawModel(Effect effect, Matrix view, Vector3 cameraPosition, Vector3 lightPosition, bool flat)
        {
            Model currentModel = flat ? modelFlat : model;
            Matrix[] transforms = new Matrix[currentModel.Bones.Count];
            currentModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    if(mesh.ParentBone.Index == 6 && !flat || mesh.ParentBone.Index == 1 && flat)
                        effect.Parameters["World"].SetValue(world * rotation * transforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale) * Matrix.CreateRotationY(leftrightRot) * Matrix.CreateTranslation(position));
                    else
                        effect.Parameters["World"].SetValue(world * transforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale) * Matrix.CreateRotationY(leftrightRot) * Matrix.CreateTranslation(position));
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    effect.Parameters["ModelTexture"].SetValue(texture);
                    effect.Parameters["LightPosition1"].SetValue(lightPosition);
                    effect.Parameters["AmbientIntensity"].SetValue(0.01f);
                }
                mesh.Draw();
            }
        }

        private void ProcessInput(float amount)
        {
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                leftrightRot -= rotationSpeed * xDifference * amount;
            }
        }
    }
}
