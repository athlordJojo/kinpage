using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Kinampage.ParticleSimulation
{
    public class Emitter : IEmitterable
    {
        public Vector2 position;
        protected int lifeTime = 1;
        protected AnimatedTexture texture;
        protected ParticleSystem ps;


        public Emitter(Vector2 position, int lifetime, AnimatedTexture texture, ParticleSystem particleSystem)
        {
            this.position = position;
            this.lifeTime = lifetime;
            this.texture = texture;
            this.ps = particleSystem;
        }

        public virtual void update()
        {
            this.lifeTime--;
        }

        // **************** GETTER SETTER ***********************
        public int getLifetime()
        {
            return this.lifeTime;
        }

    }

}
