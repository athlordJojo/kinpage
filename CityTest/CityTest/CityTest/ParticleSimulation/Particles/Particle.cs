using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Kinampage.ParticleSimulation
{
    public class Particle : IParticlable
    {
        public Vector2 position;
        protected Vector2 direction;
        protected int lifetime;
        protected AnimatedTexture texture;
        protected float scale = 0.5f;
        protected float mass = 1f;

        public Particle(Vector2 position, Vector2 direction, float mass, int lifetime, float scale, AnimatedTexture at)
        {
            this.position = position;
            this.direction = direction;
            this.lifetime = lifetime;
            this.texture = at;
            this.scale = scale;
            this.mass = mass;
        }

        public virtual void update(Vector2 gravity)
        {
            this.scale += 0.001f;
            if (this.scale < 0) this.scale = 0;
            this.direction = Vector2.Add(this.direction, gravity);
            this.direction = Vector2.Multiply(this.direction, 0.99f * this.mass);
            this.position = Vector2.Add(this.position, this.direction);
            this.lifetime--;
        }

        public virtual void draw(SpriteBatch sp)
        {
            this.texture.Scale = this.scale;
            Vector2 pos = this.position;
            pos.X -= (this.texture.myTexture.Width * this.scale) / 2;
            pos.Y -= (this.texture.myTexture.Height * this.scale) / 2;
           // Console.WriteLine(this.position +" " + this.scale);

            this.texture.DrawFrame(sp, pos, false);
        }

        //*************************** GETTER SETTER ***************************
        public void setLifeTime(int lifeTime)
        {
            this.lifetime = lifeTime;
        }

        public int getLifeTime()
        {
            return this.lifetime;
        }

        public float getScale()
        {
            return this.scale;
        }

        public void changeScale(float c)
        {
            this.scale += c;
        }
    }
}
