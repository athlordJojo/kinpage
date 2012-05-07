using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Kinampage.ParticleSimulation
{
    public class ShockwaveEmitter : Emitter
    {
        private Vector2 left, right;
        private float stepX = 20;
        private float smokeSize = 0.8f;
        int count = 0;

        public ShockwaveEmitter(Vector2 position, int lifetime, AnimatedTexture texture, ParticleSystem particleSystem)
            :base(position, lifetime, texture, particleSystem)
        {
            this.left = position;
            this.right = position;
        }

        public override void update()
        {
            base.update();

            if (this.lifeTime % 10 == 0)
            {
                this.left.X -= this.stepX;
                this.right.X += this.stepX;
                this.smokeSize -= 0.1f;
                this.count++;
            }


            if (this.lifeTime % 3 == 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    Game1.particleSimulator.psNoGrav.particleList.Add(new SmokeParticle(this.left, new Vector2(-0.4f, (0.05f * this.count)), 1f, 100, this.smokeSize, GraphicsUtil.smoke));
                    Game1.particleSimulator.psNoGrav.particleList.Add(new SmokeParticle(this.right, new Vector2(0.4f, (0.05f * this.count)), 1f, 200, this.smokeSize, GraphicsUtil.smoke));
                }
            }
        }
    }
}
