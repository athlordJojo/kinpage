using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Kinampage.ParticleSimulation
{
    class FireEmitter : Emitter
    {
        private Random rand = new Random();
        private float xDirection = 0;
        private float fireScale = 0.25f;

        public FireEmitter(Vector2 position, int lifetime, AnimatedTexture texture, ParticleSystem particleSystem, float xDirection)
            :base(position, lifetime, texture, particleSystem)
        {
            this.xDirection = xDirection;
        }


        public override void update()
        {
            this.lifeTime--;

            if (this.lifeTime < 200) this.fireScale -= 0.005f;
            {
                Vector2 pos = this.position;
                pos.X += (float)rand.Next(-15, 15);
                this.particleSystem.particleList.Add(new FireSmokeParticle(pos, new Vector2(this.xDirection + rand.Next(-10, 10) * 0.1f, 0), 1f, rand.Next(70, 120), this.fireScale, this.texture, rand.Next(20, 50)));
            }
        }
    }
}
