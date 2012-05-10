using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.ParticleSimulation
{
    public class ExplosionParticle : Particle
    {
        Random rand = new Random();

        public ExplosionParticle(Vector2 position, Vector2 direction, int lifetime, AnimatedTexture at)
            :base(position, direction, 0f, lifetime, 1f, at)
        {
            int scatter = 60;
            this.position.X += rand.Next(-scatter, scatter);
            this.position.Y += rand.Next(-scatter, scatter);
        }

        public override void update(Vector2 gravity)
        {
            this.scale += 0.005f;
            this.lifetime--;
        }

        public override void draw(SpriteBatch sp)
        {
            this.texture.Scale = this.scale;
            Vector2 pos = this.position;
            pos.X -= (this.texture.myTexture.Width * this.scale) / 2;
            pos.Y -= (this.texture.myTexture.Height * this.scale) / 2;
            // Console.WriteLine(this.position +" " + this.scale);

            this.texture.DrawFrame(sp, pos, false);
        }
    }
}
