using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Kinampage.ParticleSimulation
{
    public class EffectUtil
    {

        

        public static void createExplosion(ParticleSystem ps)
        {
            ps.emitterList.Add(new ExplosionEmitter(new Vector2(300, 200), 1, 20, 1, 0, GraphicsUtil.explosion, ps));
        }

        public static void createExplosionWithSmoke(ParticleSystem ps, Vector2 position, float direction)
        {
            ps.emitterList.Add(new ExplosionEmitter(position, 8, 7, 0.7f, direction, GraphicsUtil.smoke, ps, 600, new Vector2(-1f, -0.1f)));
        }

        public static void createShockwave(ParticleSystem ps, Vector2 position)
        {
            ps.emitterList.Add(new ShockwaveEmitter(position, 60, GraphicsUtil.smoke, ps));
        }

        public static void createDeadPerson(ParticleSystem ps, Vector2 pos, Vector2 dir, int i, float scale)
        {
            ps.particleList.Add(new Particle(pos, dir, 1f, 400, scale, GraphicsUtil.getPeopleDead(i), 0.03f));
        }

        /// <summary>
        /// ParticleSystem ps
        /// Creates an Explosion on the position "position"
        /// in the direction "direction"
        /// with the scatter "scatter"
        /// with the particlegraphics "graphics"
        /// create number of particles "number"
        /// </summary>
        /// <param name="?"></param>
        public static void createExplosion(ParticleSystem ps, Vector2 position, Vector2 direction, Vector2 scatter, int number)
        {
            Random rand = new Random();

            for (int i = 0; i < number; i++)
            {
                direction.X += rand.Next((int)-scatter.X, (int)scatter.X);
                direction.Y += rand.Next((int)-scatter.Y, (int)scatter.Y);
                ps.particleList.Add(new Particle(position, direction, 1f, 300, 0.8f, GraphicsUtil.getWreckage()));
            }
        }
    }
}
