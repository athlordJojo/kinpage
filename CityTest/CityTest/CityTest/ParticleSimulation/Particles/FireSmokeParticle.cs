using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.ParticleSimulation
{
    class FireSmokeParticle : Particle
    {
        private int firetime = 0;
        private Random rand = new Random();

        public FireSmokeParticle(Vector2 position, Vector2 direction, float mass, int lifetime, float scale, AnimatedTexture at, int firetime)
            : base(position, direction, mass, lifetime, scale, at)
        {
            this.firetime = firetime;
        }

        public override void update(Vector2 gravity)
        {
            base.update(gravity);

            if (firetime == 0)
            {
                if (rand.Next(7) == 1)
                {
                    this.lifetime += 250;
                    this.texture = GraphicsUtil.smoke;
                    this.scale += 0.2f;
                    this.mass = 0.9f;
                    this.position.X += rand.Next(-5, 5);
                }
                else
                {
                    this.lifetime = 0;
                }
            }

            if (firetime > -1)
            {
                this.firetime--;
            }

            
        }

        public override void draw(SpriteBatch sp)
        {
            base.draw(sp);
        }
    }
}
