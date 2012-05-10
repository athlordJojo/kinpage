using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Kinampage.GameSimulation
{
    public class Hand
    {
        public Vector2 pos;
        private Vector2 oldPos = Vector2.Zero;
        public Vector2 moveVector = Vector2.Zero;

        public Hand(Vector2 position)
        {
            this.pos = position;
        }

        public void update()
        {
            this.moveVector = Vector2.Subtract(pos, oldPos);
            oldPos = this.pos;
            //Console.WriteLine(moveVector);
        }

        public float getHorizontalSpeed()
        {
            return Math.Abs(this.moveVector.X);
        }

        public float getSpeed()
        {
            return this.moveVector.Length();
        }
    }
}
