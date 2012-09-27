using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VoxEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState prevMouseState;
        KeyboardState prevKeyboardState;

        Camera m_camera;

        Matrix view = Matrix.Identity;
        Matrix projection = Matrix.Identity; 

        List<Vox> voxels = new List<Vox>();
        Effect fx;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferMultiSampling = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);//perspective
            //projection = Matrix.CreateOrthographic(10 * GraphicsDevice.Viewport.AspectRatio, 10, 1.0f, 300.0f);//orthographic

            //voxels.Add(new Vox(Vector3.Zero, 1, Color.Black, GraphicsDevice, view, projection));



            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fx = Content.Load<Effect>("fx");

            m_camera = new Camera(new Vector3(0, 0, 10), new Vector3(0), 1f, 1000, this,view,projection);

            //voxels.Add(new Vox(Vector3.Zero, 1, Color.CornflowerBlue, GraphicsDevice, view, projection, fx));


            voxels.Add(new Vox(new Vector3(1, 0, -1), 1, Color.Aquamarine, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 0, -1), 1, Color.Red, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(1, 1, -1), 1, Color.Indigo, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 1, -1), 1, Color.LavenderBlush, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(1, 2, -1), 1, Color.LawnGreen, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 2, -1), 1, Color.Moccasin, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(0, 2, -1), 1, Color.Magenta, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(0, 0, -1), 1, Color.Maroon, GraphicsDevice, view, projection, fx));

            voxels.Add(new Vox(new Vector3(1, 0, 0), 1, Color.MediumSlateBlue, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 0, 0), 1, Color.MistyRose, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(1, 2, 0), 1, Color.Navy, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 2, 0), 1, Color.Olive, GraphicsDevice, view, projection, fx));

            voxels.Add(new Vox(new Vector3(1, 0, 1), 1, Color.PaleGreen, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 0, 1), 1, Color.Plum, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(1, 1, 1), 1, Color.PeachPuff, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 1, 1), 1, Color.RoyalBlue, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(1, 2, 1), 1, Color.Silver, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(-1, 2, 1), 1, Color.SteelBlue, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(0, 2, 1), 1, Color.Violet, GraphicsDevice, view, projection, fx));
            voxels.Add(new Vox(new Vector3(0, 0, 1), 1, Color.Turquoise, GraphicsDevice, view, projection, fx));
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

            // Move forward/backward
            if (keyboardState.IsKeyDown(Keys.Up))
                m_camera.MoveForward(-0.1f);
            else if (keyboardState.IsKeyDown(Keys.Down))
                m_camera.MoveForward(0.1f);

            // Strafe left/right
            if (keyboardState.IsKeyDown(Keys.A))
                m_camera.Starfe(0.1f, true);
            else if (keyboardState.IsKeyDown(Keys.D))
                m_camera.Starfe(-0.1f, true);

            // Strafe left/right
            if (keyboardState.IsKeyDown(Keys.Z))
                m_camera.Starfe(-5f, false);
            else if (keyboardState.IsKeyDown(Keys.C))
                m_camera.Starfe(5f, false);

            // Levitate up/down
            if (keyboardState.IsKeyDown(Keys.W))
                m_camera.Levitate(-0.1f);
            else if (keyboardState.IsKeyDown(Keys.S))
                m_camera.Levitate(0.1f);


            m_camera.UpdateMouseRay(mousePos, graphics.GraphicsDevice.Viewport);


            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                List<Vox> toCheckDistanceOn = new List<Vox>();
                float distance;
                Nullable<float> tempDistance = null;
                Vox tempVox = new Vox();



                for (int i = 0; i <= voxels.Count - 1; i++)
                {

                    if (voxels[i].CheckRayIntersection(m_camera.MouseRay) != null)
                        toCheckDistanceOn.Add(voxels[i]);

                }

                for (int i = 0; i <= toCheckDistanceOn.Count - 1; i++)
                {
                    float tempX = toCheckDistanceOn[i].Position.X - m_camera.position.X;
                    float tempY = toCheckDistanceOn[i].Position.Y - m_camera.position.Y;
                    float tempZ = toCheckDistanceOn[i].Position.Z - m_camera.position.Z;
                    distance = (float)Math.Sqrt(Math.Abs(tempX * tempX + tempY * tempY + tempZ * tempZ));
                    if (distance < tempDistance)
                    {
                        tempDistance = distance;
                        tempVox = toCheckDistanceOn[i];

                    }
                    if (tempDistance == null)
                    {
                        tempDistance = distance;
                        tempVox = toCheckDistanceOn[i];
                    }
                }

                if (toCheckDistanceOn.Count > 0)
                voxels.Remove(tempVox);

            }

            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                float distance;
                Nullable<float> tempDistance = null;
                Nullable<Vector3> tempDist = new Nullable<Vector3>();

                Nullable<Vector3> rayDistances = new Nullable<Vector3>();
                List<Nullable<Vector3>> toCheckDistanceOn = new List<Nullable<Vector3>>();

                for (int i = 0; i <= voxels.Count - 1; i++)
                {
                    rayDistances = voxels[i].CheckRayIntersection(m_camera.MouseRay);
                    if (rayDistances != null)
                        toCheckDistanceOn.Add(rayDistances);

                }

                for (int i = 0; i <= toCheckDistanceOn.Count - 1; i++)
                {
                    float tempX = toCheckDistanceOn[i].Value.X - m_camera.position.X;
                    float tempY = toCheckDistanceOn[i].Value.Y - m_camera.position.Y;
                    float tempZ = toCheckDistanceOn[i].Value.Z - m_camera.position.Z;
                    distance = (float)Math.Sqrt(Math.Abs(tempX * tempX + tempY * tempY + tempZ * tempZ));
                    if (distance < tempDistance)
                    {
                        tempDistance = distance;
                        tempDist = toCheckDistanceOn[i];

                    }
                    if (tempDistance == null)
                    {
                        tempDistance = distance;
                        tempDist = toCheckDistanceOn[i];
                    }
                }

                List<Vox> VOXtoCheckDistanceOn = new List<Vox>();
                float VOXdistance;
                Nullable<float> VOXtempDistance = null;
                Vox tempVox = new Vox();



                for (int i = 0; i <= voxels.Count - 1; i++)
                {

                    if (voxels[i].CheckRayIntersection(m_camera.MouseRay) != null)
                        VOXtoCheckDistanceOn.Add(voxels[i]);

                }

                for (int i = 0; i <= VOXtoCheckDistanceOn.Count - 1; i++)
                {
                    float tempX = VOXtoCheckDistanceOn[i].Position.X - m_camera.position.X;
                    float tempY = VOXtoCheckDistanceOn[i].Position.Y - m_camera.position.Y;
                    float tempZ = VOXtoCheckDistanceOn[i].Position.Z - m_camera.position.Z;
                    VOXdistance = (float)Math.Sqrt(Math.Abs(tempX * tempX + tempY * tempY + tempZ * tempZ));
                    if (VOXdistance < VOXtempDistance)
                    {
                        VOXtempDistance = VOXdistance;
                        tempVox = VOXtoCheckDistanceOn[i];

                    }
                    if (VOXtempDistance == null)
                    {
                        VOXtempDistance = VOXdistance;
                        tempVox = VOXtoCheckDistanceOn[i];
                    }
                }

                Vector3 valueOfCube = new Vector3();
                if (tempDist != null)
                {
                    if (tempDist.Value.X - tempVox.Position.X == 0.5)
                        valueOfCube = new Vector3(1, 0, 0);

                    if (tempDist.Value.Y - tempVox.Position.Y == 0.5)
                        valueOfCube = new Vector3(0, 1, 0);

                    if (tempDist.Value.Z - tempVox.Position.Z == 0.5)
                        valueOfCube = new Vector3(0, 0, 1);

                    if (tempDist.Value.X - tempVox.Position.X == -0.5)
                        valueOfCube = new Vector3(-1, 0, 0);

                    if (tempDist.Value.Y - tempVox.Position.Y == -0.5)
                        valueOfCube = new Vector3(0, -1, 0);

                    if (tempDist.Value.Z - tempVox.Position.Z == -0.5)
                        valueOfCube = new Vector3(0, 0, -1);


                    if (toCheckDistanceOn.Count > 0)
                        voxels.Add(new Vox(new Vector3(tempVox.Position.X + valueOfCube.X, tempVox.Position.Y + valueOfCube.Y, tempVox.Position.Z + valueOfCube.Z), 1, Color.Black, GraphicsDevice, view, projection, fx));
                }
            }

            // TODO: Add your update logic here

            for (int i = 0; i < voxels.Count; i++)
            {
                voxels[i].Update(gameTime);
            }

            m_camera.Update();

            prevMouseState = mouseState;
            prevKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            for (int i = 0; i < voxels.Count; i++)
            {
                voxels[i].Draw(m_camera,gameTime);
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
