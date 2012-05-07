using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.ParticleSimulation
{
    public class ParticleSystem
    {
        public List<IParticlable> particleList = new List<IParticlable>();
        public List<IEmitterable> emitterList = new List<IEmitterable>();
        private Vector2 gravity;

        public ParticleSystem(Vector2 gravity)
        {
            this.gravity = gravity;
        }

        public void update()
        {
            //Console.WriteLine("ParticleSystem update, particle count: " + this.particleList.Count);
            for(int i=0; i < this.emitterList.Count; i++)
            {
                IEmitterable e = this.emitterList[i];

                if(e.getLifetime() <= 0)
                {
                    this.emitterList.RemoveAt(i);
                    i--;
                    continue;
                }

                e.update();
            }

            for (int i = 0; i < this.particleList.Count; i++)
            {
                IParticlable p = this.particleList[i];
                //Particle dead? remove!
                if (p.getLifeTime() <= 0)
                {
                    this.particleList.RemoveAt(i);
                    i--;
                    continue;
                }

                //Update Particle
                p.update(this.gravity);
            }
        }

        public void draw(SpriteBatch sp)
        {
            foreach (IParticlable p in this.particleList)
            {
                p.draw(sp);
            }
        }
    }
}
