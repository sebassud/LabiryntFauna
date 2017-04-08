using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabiryntFauna.Models
{
    public interface ILabyrinth
    {
        void LoadContent(ContentManager Content);
        void DrawModel(Effect effect, Matrix view, Vector3 cameraPosition, Vector3 lightPosition, Color lightColor, bool flat);
        bool IsCollision(Vector3 position);
        Vector3 GetStartPosition();
        void Update(float amount);
        bool IsEnd(Vector3 p);
    }
}
