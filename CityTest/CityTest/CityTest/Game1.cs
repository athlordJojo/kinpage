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
using Kinampage.GameSimulation;
using Kinampage.ParticleSimulation;
using Microsoft.Kinect;
using KinectProject;
using Kinampage.shader;
namespace Kinampage
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //RESOLUTION
        //1280 * 720
        //tmp
        int count = 0;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameSimulator gameSim = new GameSimulator(player);
        //Particle Engine
        public static ParticleSimulator particleSimulator = new ParticleSimulator();
        public static KinectDraw kinectDrawer = new KinectDraw(1280, 720);
        public static Player player = new Player();
        
        //mouse
        public Vector2 mousePos;
        //Hand myHand;

        // dotDetection
        Vector2[] handPositions = new Vector2[2];
        HandDotDetector handDotDetector = null;
        Rectangle redDotDetectionArea = new Rectangle(0, 0, 180, 180);
        DotDetector_ResultObject[] handDot_Results = null;

        //Shader
        Effect effect;
        ShaderParameterController shaderController = null;

        //Kinect
        KinectSensor kinect;
        Skeleton[] skeletonData;
        Skeleton playerSkeleton;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            shaderController = new ShaderParameterController();

            Content.RootDirectory = "Content";
            GraphicsUtil.init();

            //Add ps to Simulator

            //myHand = new Hand(Vector2.Zero);



            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //init Kinect
            try
            {

                //JOAN
                kinect = KinectSensor.KinectSensors[0];
                kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                kinect.SkeletonStream.Enable(new TransformSmoothParameters()
                {
                    Smoothing = 0.9f,
                    Correction = 0.9f,//0.5
                    Prediction = 0.5f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                    /*Smoothing = 0.5f,
                    Correction = 0.2f,//0.5
                    Prediction = 0.5f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f*/
                });
                kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(allFramesReadyEventhandler);
                kinect.Start();

                handDotDetector = new HandDotDetector(kinect, false);
                /*
                //BUNNI
                kinect = KinectSensor.KinectSensors[0];
                kinect.Start();
               // kinect.SkeletonStream.Enable();
                kinect.SkeletonStream.Enable(new TransformSmoothParameters()
                {
                    Smoothing = 0.5f,
                    Correction = 0.2f,//0.5
                    Prediction = 0.5f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                });

                kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);
                 * */
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

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

            //load Graphics
            //spritePerson.Load(Content, "personH", 2, 2);

            GraphicsUtil.load(Content);
            effect = Content.Load<Effect>("shader"); 
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
            KeyboardState k = Keyboard.GetState();
            shaderController.updateBloodParameter(k.IsKeyDown(Keys.Y));
            effect.Parameters["bloodpower"].SetValue(shaderController.bloodPower);

            MouseState mouseState = Mouse.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            //Update Player
            if (playerSkeleton != null)
            {
                player.updatePlayer(kinectDrawer.calculatePos(playerSkeleton.Joints[JointType.HandLeft].Position), kinectDrawer.calculatePos(playerSkeleton.Joints[JointType.HandRight].Position), kinectDrawer.calculatePos(playerSkeleton.Joints[JointType.KneeLeft].Position), kinectDrawer.calculatePos(playerSkeleton.Joints[JointType.KneeRight].Position));
            }

            //update Player Hand state
            if (handDot_Results != null)
            {
                kinectDrawer.isRightHandOpen = handDot_Results[0].dotDetected;// rechte hand
                kinectDrawer.isLeftHandOpen = handDot_Results[1].dotDetected;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            count--;
            if (count < 0)
            {
                //Controls
                KeyboardState keyState = Keyboard.GetState();
                if (keyState.IsKeyDown(Keys.A))
                {
                    count = 70;
                    kinectDrawer.changePosW(10f);
                   // EffectUtil.createExplosionWithSmoke(particleSimulator.psGrav, -2);
                }
                if (keyState.IsKeyDown(Keys.D))
                {
                   // EffectUtil.createExplosionWithSmoke(particleSimulator.psGrav, 2);
                    kinectDrawer.changePosW(-10f);
                    count = 70;
                }
                if (keyState.IsKeyDown(Keys.W))
                {
                   // EffectUtil.createExplosionWithSmoke(particleSimulator.psGrav, 0);
                    kinectDrawer.changePosH(10f);
                    count = 70;
                }
                if (keyState.IsKeyDown(Keys.S))
                {
                    //EffectUtil.createShockwave(particleSimulator.psNoGrav, new Vector2(300, 400));
                    //kinectDrawer.changePosH(-10f);
                    EffectUtil.createFire(new Vector2(500, 400), 600, 0);
                    count = 70;
                }
            }
            


            //Graphics update
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);

            //Update GameSimulation
            gameSim.populate();
            gameSim.update();

            //Physics update
            particleSimulator.update();

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            RenderTarget2D t = new RenderTarget2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            GraphicsDevice.SetRenderTarget(t);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);//!!!! INTERMEDIATE. sonst keine shader

      

            //Draw Background
            GraphicsUtil.street.DrawFrame(spriteBatch, new Vector2(0, 580), false);
            //draw Gameparts
            gameSim.draw(spriteBatch);
            //draw Particles
            particleSimulator.draw(spriteBatch);

            spriteBatch.Draw(GraphicsUtil.getWreckage().myTexture, mousePos, null, Color.White);


            //Draw Player
            if (playerSkeleton != null)
            {
                 kinectDrawer.drawBody(playerSkeleton, spriteBatch);
            }


            //red updaten
            //effect.CurrentTechnique.Passes[2].Apply();
            //spriteBatch.Draw(t, Vector2.Zero, Color.White);
            // auf die gesammte erstellte textur den graufilter anwenden oder bloodfilter
            
            
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);//!!!! INTERMEDIATE. sonst keine shader

            GraphicsDevice.SetRenderTarget(null);
            effect.CurrentTechnique.Passes[2].Apply();
            spriteBatch.Draw(t, Vector2.Zero, Color.White);
            t.Dispose();
            spriteBatch.End();


            t.Dispose();// wenn nichtz gerufen--> out of memory

            base.Draw(gameTime);
        }




        //************************* CUSTOM METHODS *********************************************************
        /*void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    playerSkeleton = (from s in skeletonData where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();

                    if (playerSkeleton != null)
                    {
                        Console.WriteLine(playerSkeleton.Joints[JointType.Head].Position.Y.ToString() + " DSD");
                    }
                }
            }
        }*/

        private void allFramesReadyEventhandler(object sender, AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    playerSkeleton = (from s in skeletonData where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();

                    if (playerSkeleton != null)
                    {
                        //Console.WriteLine(playerSkeleton.Joints[JointType.Head].Position.Y.ToString() + " DSD");
                        Joint rightHand = playerSkeleton.Joints[JointType.HandRight];
                        Vector2 rightHandPosition = new Vector2((((0.5f * rightHand.Position.X) + 0.5f) * (640)), (((-0.5f * rightHand.Position.Y) + 0.5f) * (480)));
                        handPositions[0] = rightHandPosition;

                        Joint leftHand = playerSkeleton.Joints[JointType.HandLeft];
                        Vector2 leftHandPosition = new Vector2((((0.5f * leftHand.Position.X) + 0.5f) * (640)), (((-0.5f * leftHand.Position.Y) + 0.5f) * (480)));
                        handPositions[1] = leftHandPosition;

                        //handDot_Results = handDotDetector.dotDetected(handPositions, e);
                    }
                }

                if (!handDotDetector.ready)
                {
                    handDot_Results = handDotDetector.findThreshold(handPositions, e);
                }
                else
                {
                    handDot_Results = handDotDetector.dotDetected(handPositions, e);
                }
            }
        }

        //**** KINECT DRAW ******************
        

    }



    
}
