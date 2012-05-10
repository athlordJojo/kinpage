using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.GameSimulation
{
    public class People
    {
        public Vector2 position;
        public Vector2 movement;
        protected int health = 1;
        private int type;
        private float speed = 1;
        //Graphics
        public AnimatedTexture texture;
        protected float scale = 1f;

        private Random rand = new Random();

        public People(Vector2 position, Vector2 movement, AnimatedTexture at, int type, float speed)
        {
            this.position = position;
            this.movement = movement;
            this.texture = at;

            /*
             * int rnd = rand.Next(0, 100);
            this.position.Y -= rnd;
            float rndf = ((float)rnd) / 50;
            this.scale -= 0.01f * rnd;
            this.texture.Depth = (0.8f + (float)lane / 100);
            this.type = type;
            this.speed = speed;
            float tmp = ((float)lane) / 5;
            this.speed -= tmp;
            this.movement = Vector2.Multiply(this.movement, this.speed);
             */

            int rnd = rand.Next(0, 100);
            //float rndf = ((float)rnd) / 100;
            this.position.Y -= rnd;
            this.scale -= 0.004f * rnd;
            this.texture.Depth = (0.8f + rnd*0.001f);
            this.type = type;
            this.speed = speed;
            this.speed -= rnd * 0.005f;
            this.movement = Vector2.Multiply(this.movement, this.speed);
        }

        public void update()
        {
            this.position = Vector2.Add(this.position, this.movement);
        }

        public virtual void draw(SpriteBatch sp)
        {
            this.texture.UpdateFrame(0.06f);//0.1f
            this.texture.Scale = this.scale;
            Vector2 pos = this.position;
            pos.X -= (this.texture.myTexture.Width * this.scale) / 2;
            pos.Y -= (this.texture.myTexture.Height * this.scale) / 2;
            //Console.WriteLine("draw " + pos + "texture " + this.texture.Scale.ToString());
            //where is the person looking
            if (this.movement.X > 0)
            {
                this.texture.DrawFrame(sp, pos, true);//look right
            }
            else
            {
                this.texture.DrawFrame(sp, pos, false);//look left
            }

            
        }


        //*************************** GETTER SETTER ***************************
        public int getHealth()
        {
            return this.health;
        }

        public void setHealth(int health)
        {
            this.health = health;
        }

        public int getType()
        {
            return this.type;
        }

        public float getScale()
        {
            return this.scale;
        }
    }
}
