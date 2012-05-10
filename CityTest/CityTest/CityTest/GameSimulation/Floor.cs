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
        private AnimatedTexture texture;

        public Floor(Vector2 position, AnimatedTexture at)
        {
            this.position = position;
            this.texture = at;
        }

        public bool ishit(Vector2 v, float xRange, float yRange)
        {
            bool isXhit = ((this.position.X - xRange < v.X) && ((this.position.X + this.width + xRange) > v.X));
            bool isYhit = ((this.position.Y - yRange < v.Y) && ((this.position.Y + this.height + yRange) > v.Y));

            return (isXhit && isYhit);
        }

        public virtual void draw(SpriteBatch sp)
        {
            this.texture.DrawFrame(sp, position, false);//look right
        }
    }
}
