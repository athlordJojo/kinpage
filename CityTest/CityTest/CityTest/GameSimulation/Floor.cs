using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.GameSimulation
{
    public class Floor
    {
        public Vector2 position;

        private int height = 80;
        private int width = 160;

        private int health = 100;
        private List<AnimatedTexture> textures;
        private AnimatedTexture midTexture;
        private AnimatedTexture backTexture;
        private bool isMirrored = false;

        private int healthDiff =0;
        private bool isBackDamaged = false;

        //Random
        private Random rand = new Random();

        public Floor(Vector2 position, AnimatedTexture at, bool mirrored)
        {
            this.position = position;
            this.isMirrored = mirrored;

            //get building parts
            this.textures = GraphicsUtil.getBuildingFronts();
            this.midTexture = GraphicsUtil.getBuildingMid();
            this.backTexture = GraphicsUtil.getBuildingBack(0);

            Console.WriteLine("Floor build parts: " + textures.Count);
        }

        public bool ishit(Vector2 v, float xRange, float yRange)
        {
            if (this.health <= 0) return false;

            bool isXhit = ((this.position.X  < v.X) && ((this.position.X + this.width  ) > v.X));
            bool isYhit = ((this.position.Y  < v.Y) && ((this.position.Y + this.height ) > v.Y));

            return (isXhit && isYhit);
        }

        public virtual void draw(SpriteBatch sp)
        {
            //this.texture.DrawFrame(sp, position, false);//look right
            this.midTexture.DrawFrame(sp, position, isMirrored);
            this.backTexture.DrawFrame(sp, position, isMirrored);

            foreach (AnimatedTexture at in this.textures)
            {
                at.DrawFrame(sp, position, isMirrored);
            }
        }

        public void damage(int dmg)
        {
            this.healthDiff += dmg;
            this.health -= dmg;

            if (this.healthDiff > 10)
            {
                removePart();
                this.healthDiff = 0;

                if (rand.Next(7) == 1)
                {
                    Vector2 posTmp = this.position;
                    posTmp.X += rand.Next(10, 70);
                    posTmp.Y += rand.Next(10, 70);
                    int x = -1;
                    if(isMirrored) x *=-1;
                    EffectUtil.createFire(posTmp, 300 + rand.Next(400), x);
                }
                
            }

            if (health < 60 && !isBackDamaged)
            {
                this.isBackDamaged = true;
                this.backTexture = GraphicsUtil.getBuildingBack();

                //pos schieb
                //this.position.X -= rand.Next(1, 16);
                //this.midTexture.Origin.X -= rand.Next(1, 16);
                //this.backTexture.Origin.X -= rand.Next(1, 16);
                /*foreach(AnimatedTexture at in this.textures)
                {
                    at.Origin.X -= rand.Next(1, 7);
                }*/
            }  
        }



        private void removePart()
        {
            if (this.textures.Count < 1) return;

            int r = rand.Next(0, this.textures.Count);
            //this.textures[r].Scale = 0.0f;
            this.textures.RemoveAt(r);
        }

        private void removePart(int max)
        {
            if (this.textures.Count < 1) return;

            int r = rand.Next(0, max);
            //this.textures[r].Scale = 0.0f;
            this.textures.RemoveAt(r);
        }


        //************** GETTER SETTER *****************
        public int getHealth()
        {
            return this.health;
        }
    }
}
