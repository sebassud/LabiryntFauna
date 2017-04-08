using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabiryntFauna.Models
{
    public class Camera
    {
        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        public Vector3 CameraPosition { get; set; } = new Vector3(2, 2f, -2);
        float leftrightRot = 0;
        float updownRot = -MathHelper.Pi / 10.0f;
        const float rotationSpeed = 0.03f;
        const float moveSpeed = 1.0f;
        MouseState originalMouseState;
        int widthScreen;
        int heightScreen;
        Vector3 pointPlayer = new Vector3(2, 2f, 0);

        public Camera(int widthScreen, int heightScreen)
        {
            this.widthScreen = widthScreen;
            this.heightScreen = heightScreen;
            Mouse.SetPosition(widthScreen / 2, heightScreen / 2);
            originalMouseState = Mouse.GetState();
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = CameraPosition + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            view = Matrix.CreateLookAt(CameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        private void AddToCameraPosition(Vector3 pointPlayer)
        {
            Vector3 OrbitOffset = new Vector3(2, 0, 0);
            Matrix Rotation = Matrix.CreateFromYawPitchRoll(0, 0, 0);

            Vector3.Transform(ref OrbitOffset, ref Rotation, out OrbitOffset);

        }

        private void ProcessInput(float amount)
        {
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                leftrightRot -= rotationSpeed * xDifference * amount;
                updownRot -= rotationSpeed * yDifference * amount;
            }
        }

        public Matrix GetView(float timeDifference, Vector3 pointPlayer, bool move)
        {
            ProcessInput(timeDifference);
            if(move) AddToCameraPosition(pointPlayer);
            Mouse.SetPosition(widthScreen / 2, heightScreen / 2);
            CameraPosition = Vector3.Transform(pointPlayer + new Vector3(0, 1.5f, -2f), Matrix.CreateTranslation(-pointPlayer) * Matrix.CreateRotationY((leftrightRot - MathHelper.Pi)) * Matrix.CreateTranslation(pointPlayer));
            UpdateViewMatrix();
            return view;
        }
    }
}
