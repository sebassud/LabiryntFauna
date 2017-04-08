using LabiryntFauna.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabiryntFauna.Presenters
{
    public class WorldPresenter
    {
        private ILabyrinth labyrinth;
        private Player player;
        private Camera camera1;
        private Effect[,] effects;
        private Matrix view;
        private int nrCamera;
        private Color lightColor;
        private bool isTurn;
        private LabyrinthCastle labyrinthCastle;
        private LabyrinthForest labyrinthForest;
        private int nrLabyrinth;
        private int nrEffect;
        private Song castleMusic;
        private bool flat;

        public WorldPresenter(GraphicsDevice GraphicsDevice)
        {
            labyrinthCastle = new LabyrinthCastle();
            labyrinthForest = new LabyrinthForest();
            labyrinth = labyrinthCastle;
            player = new Player(labyrinth.GetStartPosition());
            camera1 = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            nrCamera = 1;
            lightColor = new Color(1, 1, 1);
            isTurn = true;
            nrLabyrinth = 1;
            nrEffect = 0;
            effects = new Effect[2, 3];
            MediaPlayer.IsRepeating = true;
            flat = false;
        }

        public void LoadContent(ContentManager Content)
        {
            labyrinth.LoadContent(Content);
            labyrinthForest.LoadContent(Content);
            player.LoadContent(Content);
            effects[1, 0] = Content.Load<Effect>("Effects/ShaderPhongCastle");
            effects[0, 0] = Content.Load<Effect>("Effects/ShaderPhongForest");
            effects[1, 1] = Content.Load<Effect>("Effects/ShaderBlinnCastle");
            effects[0, 1] = Content.Load<Effect>("Effects/ShaderBlinnForest");
            effects[1, 2] = Content.Load<Effect>("Effects/ShaderGouraudCastle");
            effects[0, 2] = Content.Load<Effect>("Effects/ShaderGouraudForest");
            castleMusic = Content.Load<Song>("Soundtrack/castleMusic");
            MediaPlayer.Play(castleMusic);
        }

        public void Update(GameTime gameTime)
        {        
            Vector3 moveVector = new Vector3(0, 0, 0);
            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 400.0f;

            labyrinth.Update(timeDifference);

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.NumPad1)) nrCamera = 1;
            if (keyState.IsKeyDown(Keys.NumPad2)) nrCamera = 2;
            if (keyState.IsKeyDown(Keys.NumPad3)) nrCamera = 3;
            if (keyState.IsKeyDown(Keys.L)) isTurn = false;
            if (keyState.IsKeyDown(Keys.O)) isTurn = true;
            if(keyState.IsKeyDown(Keys.X))
            {
                labyrinth = labyrinthCastle;
                player.SetPosition(labyrinth.GetStartPosition());
                nrLabyrinth = 1;
            }
            if (keyState.IsKeyDown(Keys.Z))
            {
                labyrinth = labyrinthForest;
                player.SetPosition(labyrinth.GetStartPosition());
                nrLabyrinth = 0;
            }
            if (keyState.IsKeyDown(Keys.P))
            {
                nrEffect = 0;
                flat = false;
            }
            if (keyState.IsKeyDown(Keys.B))
            {
                nrEffect = 1;
                flat = false;
            }
            if (keyState.IsKeyDown(Keys.G))
            {
                nrEffect = 2;
                flat = false;
            }
            if (keyState.IsKeyDown(Keys.F))
            {
                nrEffect = 2;
                flat = true;
            }

            bool move = labyrinth.IsCollision(player.GetPosition(moveVector, timeDifference));
            player.Move(moveVector, timeDifference, move);
            view = camera1.GetView(timeDifference, player.Position,move);

            if (labyrinth.IsEnd(player.Position)) player.SetPosition(labyrinth.GetStartPosition());
            
        }

        public void Draw()
        {
            Vector3 lightPosition = player.Position + new Vector3(0, 2, 0);

            if (nrCamera == 1)
            {
                labyrinth.DrawModel(effects[nrLabyrinth, nrEffect], view, camera1.CameraPosition, lightPosition, isTurn == true ? lightColor : Color.Black, flat);
                player.DrawModel(effects[nrLabyrinth, nrEffect], view, camera1.CameraPosition, lightPosition, flat);
            }
            else if (nrCamera == 2)
            {
                Matrix View = Matrix.CreateLookAt(new Vector3(0, 50, 0), new Vector3(0, 0, 0), new Vector3(-1, 0, 0));

                labyrinth.DrawModel(effects[nrLabyrinth, nrEffect], View, new Vector3(0, 50, 0), lightPosition, isTurn == true ? lightColor : Color.Black, flat);
                player.DrawModel(effects[nrLabyrinth, nrEffect], View, new Vector3(0, 50, 0), lightPosition, flat);
            }
            else if (nrCamera == 3)
            {
                Matrix View = Matrix.CreateLookAt(new Vector3(10, 20, 0), player.Position, new Vector3(-1, 0, 0));

                labyrinth.DrawModel(effects[nrLabyrinth, nrEffect], View, new Vector3(0, 50, 0), lightPosition, isTurn == true ? lightColor : Color.Black, flat);
                player.DrawModel(effects[nrLabyrinth, nrEffect], View, new Vector3(0, 50, 0), lightPosition, flat);
            }
        }

    }
}
