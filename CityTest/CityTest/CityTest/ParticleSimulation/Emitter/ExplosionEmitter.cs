using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Kinampage.ParticleSimulation
{
    public class ExplosionEmitter : IEmitterable
    {
        public Vector2 position;
        public int area;
        private int lifeTime = 1;
        private float perFrame;
        private float perFrameCount = 0;
        private AnimatedTexture texture;
        private ParticleSystem ps;
        private float xDirection;

        //Particle infos
        private int particleLifeTime = 60;
        private Vector2 particleDirection = Vector2.Zero;

        private Random rand = new Random();

        public ExplosionEmitter(Vector2 position, int area, int lifeTime, float perFrame, float xdirection, AnimatedTexture texture ,ParticleSystem ps)
        {
            this.position = position;
            this.area = area;
            this.lifeTime = lifeTime;
            this.perFrame = perFrame;
            this.ps = ps;
            this.texture = texture;
            this.xDirection = xdirection;


            init();
        }

        public ExplosionEmitter(Vector2 position, int area, int lifeTime, float perFrame, float xdirection, AnimatedTexture texture, ParticleSystem ps, int particleLifetime, Vector2 particleDirection)
        {
            this.position = position;
            this.area = area;
            this.lifeTime = lifeTime;
            this.perFrame = perFrame;
            this.ps = ps;
            this.texture = texture;
            this.particleLifeTime = particleLifetime;
            this.particleDirection = particleDirection;
            this.xDirection = xdirection;


            init();
        }


        /// <summary>
        /// Creates the wreckage parts in the beginning of the explosion
        /// </summary>
        public void init()
        {
            Vector2 dir = Vector2.Zero;
            Vector2 pos = new Vector2(this.position.X, this.position.Y);
            int c = rand.Next(2, 6);

            //Add wrecks with smokeline
            for (int i = 0; i < c; i++)
            {
                pos.X += rand.Next(-this.area, this.area);
                pos.Y += rand.Next(-this.area, this.area);
                /*
                if (this.xDirection > 0)//Explode to right
                {
                    dir.X = rand.Next(0, this.area* 5);
                    dir.Y = rand.Next(-this.area * 2, this.area *2);
                }
                else if (this.xDirection < 0)//explode to left
                {
                    dir.X = rand.Next(-this.area * 5, 0);
                    dir.Y = rand.Next(-this.area *2, this.area *2);
                }
                else//Explode random
                {
                    dir.X = rand.Next(-this.area * 2, this.area * 2);
                    dir.Y = rand.Next(-this.area * 3, -this.area);
                }*/

                dir.X = xDirection * -0.2f;
                dir.Y = rand.Next(-this.area * 3, this.area);
                ps.particleList.Add(new SmokeLineParticle(pos, dir, particleLifeTime, 0.6f, GraphicsUtil.getWreckage(), ps));
               // ps.particleList.Add(new SmokeLineParticle(pos, dir, particleLifeTime, 0.6f, GraphicsUtil.getGarbage(), ps));
            }

            //add wrecks
            for (int i = 0; i < 8; i++)
            {
                dir = Vector2.Zero;

                if (this.xDirection > 0)//Explode to right
                {
                    dir.X = rand.Next(0, this.area);
                    dir.Y = rand.Next(-this.area, this.area);
                }
                else if (this.xDirection < 0)//explode to left
                {
                    dir.X = rand.Next(-this.area, 0);
                    dir.Y = rand.Next(-this.area, this.area);
                }
                else//Explode random
                {
                    dir.X = rand.Next(-this.area, this.area);
                    dir.Y = rand.Next(-this.area, this.area);
                }

                ps.particleList.Add(new Particle(pos, dir, 1f, 80, 0.6f, GraphicsUtil.getWreckage()));
            }
        }
        public void update()
        {
            //Console.WriteLine("Emitter update");
            this.lifeTime--;
            if (this.lifeTime <= 0) return;

            this.perFrameCount += perFrame;

            while(this.perFrameCount >=1)
            {
                this.perFrameCount -= 1;
                Vector2 pos = new Vector2(this.position.X, this.position.Y);

                //Game1.particleSimulator.psNoGrav.particleList.Add(new ExplosionParticle(pos, Vector2.Zero, 70, Graphics.smoke));
                //Game1.particleSimulator.psNoGrav.particleList.Add(new SmokeParticle(this.position, Vector2.Zero, 0f, 300, 0.7f, GraphicsUtil.smoke));
                //Game1.particleSimulator.psGrav.particleList.Add(new Particle(pos, Vector2.Zero, 1f,  70, 4f, Graphics.dust));
            }

            
        }

        // **************** GETTER SETTER ***********************
        public int getLifetime()
        {
            return this.lifeTime;
        }
    }
}
