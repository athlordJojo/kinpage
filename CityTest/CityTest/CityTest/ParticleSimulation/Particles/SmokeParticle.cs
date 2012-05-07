using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.ParticleSimulation
{
    public class SmokeParticle : Particle
    {
        Random rand = new Random();

        public SmokeParticle(Vector2 position, Vector2 direction, float mass, int lifetime, float scale, AnimatedTexture at)
            :base(position, direction, mass, lifetime, scale, at)
        {
            float s = rand.Next(-10, 10);
            s /= 500;
            this.scale += s;
            int scatter = 10;
            this.position.X += (rand.Next(-scatter, scatter));
            this.position.Y += (rand.Next(-scatter, scatter));
        }

        public override void draw(SpriteBatch sp)
        {
            this.scale += 0.0004f;
            this.texture.Scale = this.scale;
            Vector2 pos = this.position;
            pos.X -= (this.texture.myTexture.Width * this.scale) / 2;
            pos.Y -= (this.texture.myTexture.Height * this.scale) / 2;

            //
            byte a = 155;
            
            if (this.lifetime < 155)
            {
                a = (byte)this.lifetime;
            }

            //draw sprite with alpha
            this.texture.DrawFrameAlpha(sp, pos, false, a);
        }
    }
}
