using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.ParticleSimulation
{
    public class ParticleSimulator
    {
        public ParticleSystem psGrav = new ParticleSystem(new Vector2(0, 0.2f));
        public ParticleSystem psNoGrav = new ParticleSystem(new Vector2(0, 0f));
        public ParticleSystem psNegGrav = new ParticleSystem(new Vector2(0, -0.1f));

        public List<ParticleSystem> particleSystemList = new List<ParticleSystem>();

        public ParticleSimulator()
        {
            this.particleSystemList.Add(psNoGrav);
            this.particleSystemList.Add(psGrav);
            this.particleSystemList.Add(psNegGrav);           
        }

        public void update()
        {
            for (int i = 0; i < this.particleSystemList.Count; i++)
            {
                ParticleSystem ps = particleSystemList[i];

                ps.update();
            }
        }

        public void draw(SpriteBatch sp)
        {
            for (int i = 0; i < this.particleSystemList.Count; i++)
            {
                ParticleSystem ps = particleSystemList[i];

                ps.draw(sp);
            }
        }
    }
}
