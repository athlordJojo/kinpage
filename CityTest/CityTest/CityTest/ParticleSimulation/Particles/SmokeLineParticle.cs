using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.ParticleSimulation
{
    public class SmokeLineParticle : IParticlable
    {
        public Vector2 position;
        private Vector2 direction;
        private int lifetime;
        private AnimatedTexture texture;
        private float scale = 1f;
        private ParticleSystem ps;

        private float smokeSum = 0;
        private float smokeAdd = 0.1f;

        private Vector2 drawPos = Vector2.Zero;

        private float smokeScale = 0.5f;

        public SmokeLineParticle(Vector2 position, Vector2 direction, int lifetime, float scale, AnimatedTexture at, ParticleSystem ps)
        {
            this.position = position;
            this.direction = direction;
            this.lifetime = lifetime;
            this.texture = at;
            this.ps = ps;
            this.scale = scale;
        }

        public void update(Vector2 gravity)
        {
            this.smokeScale -= 0.003f;
            if (this.scale < 0) this.scale = 0;

            this.direction = Vector2.Add(this.direction, gravity);
            this.direction = Vector2.Multiply(this.direction, 0.99f);
            Vector2 tmp = this.direction;
            tmp = Vector2.Multiply(tmp, 0.2f);
            this.position = Vector2.Add(this.position, tmp);
            this.lifetime--;

            smokeSum += smokeAdd;

            if (smokeSum >= 1)
            {
                smokeSum -= 1;


                this.ps.particleList.Add(new SmokeParticle(this.position, Vector2.Zero, 0f, 280, smokeScale, GraphicsUtil.smoke));
            }

           

        }

        public void draw(SpriteBatch sp)
        {
            // Console.WriteLine(this.position +" " + this.scale);
            this.drawPos.X = (this.position.X - this.texture.myTexture.Width / 2);
            this.drawPos.Y = (this.position.Y - this.texture.myTexture.Height / 2);

            this.texture.Scale = this.scale;
            this.texture.DrawFrame(sp, this.position, false);
            //this.texture.DrawFrameAlpha(sp, this.position, false, 0f);
        }

        public int getLifeTime()
        {
            return this.lifetime;
        }

    }
}
