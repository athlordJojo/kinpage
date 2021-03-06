#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kinampage
{
    public class AnimatedTexture : ICloneable
    {
        private int framecount;
        public Texture2D myTexture;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;
        public AnimatedTexture(Vector2 origin, float rotation, 
            float scale, float depth)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
        }
        public void Load(ContentManager content, string asset, 
            int frameCount, int framesPerSec)
        {
            framecount = frameCount;
            myTexture = content.Load<Texture2D>(asset);
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos, bool mirror)
        {
            DrawFrame(batch, Frame, screenPos, mirror);
        }

        public void DrawFrameAlpha(SpriteBatch batch, Vector2 screenPos, bool mirror, byte alpha)
        {
            int FrameWidth = myTexture.Width / framecount;
            Rectangle sourcerect = new Rectangle(FrameWidth * Frame, 0,
                FrameWidth, myTexture.Height);

            if(alpha < 0) alpha = 0;
            //Color alphaC = Color.White;
            Color alphaC = new Color(alpha, alpha, alpha, alpha);
            //alphaC.A = alpha;

            if (mirror)
            {
                batch.Draw(myTexture, screenPos, sourcerect, alphaC,
                Rotation, Origin, Scale, SpriteEffects.FlipHorizontally, Depth);
            }
            else
            {
                batch.Draw(myTexture, screenPos, sourcerect, alphaC,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
            }
        }


        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos, bool mirror)
        {
            int FrameWidth = myTexture.Width / framecount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, myTexture.Height);

            if (mirror)
            {
                batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.FlipHorizontally, Depth);
            }
            else
            {
                batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }


        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}
