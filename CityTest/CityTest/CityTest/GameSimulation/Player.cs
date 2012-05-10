using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Kinampage.GameSimulation
{
    public class Player
    {
        private Hand rightHand, leftHand;
        private Hand rightFoot, leftFoot;

        public Player()
        {
            this.rightHand = new Hand(Vector2.Zero);
            this.leftHand = new Hand(Vector2.Zero);

            this.rightFoot = new Hand(Vector2.Zero);
            this.leftFoot = new Hand(Vector2.Zero);
        }

        public void updatePlayer(Vector2 handLeft, Vector2 handRight, Vector2 footLeft, Vector2 footRight)
        {
            //hands
            this.leftHand.pos = handLeft;
            this.rightHand.pos = handRight;
            //feet
            this.leftFoot.pos = footLeft;
            this.rightFoot.pos = footRight;

            this.leftHand.update();
            this.rightHand.update();

            this.leftFoot.update();
            this.rightFoot.update();
        }



        public Hand getLeftHand()
        {
            return this.leftHand;
        }

        public Hand getRightHand()
        {
            return this.rightHand;
        }

        public Hand getLeftFoot()
        {
            return this.leftFoot;
        }

        public Hand getRightFoot()
        {
            return this.rightFoot;
        }

    }
}
