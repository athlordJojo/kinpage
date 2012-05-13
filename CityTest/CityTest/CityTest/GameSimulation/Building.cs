using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.GameSimulation
{
    public class Building
    {
        public Vector2 position;
        private List<Floor> floorList = new List<Floor>();


        public Building(Vector2 position, int height)
        {
            this.position = position;

            //left Building
            for (int i = 0; i < height; i++)
            {
                this.floorList.Add(new Floor(new Vector2(this.position.X, this.position.Y - (i * 80)), GraphicsUtil.getBuildingFront(0), false));
                this.floorList.Add(new Floor(new Vector2(this.position.X+ 80, this.position.Y - (i * 80)), GraphicsUtil.getBuildingFront(0), true));
            }
        }

        public void draw(SpriteBatch sb)
        {
            foreach(Floor f in this.floorList)
            {
                f.draw(sb);
            }
        }


        public void update()
        {
            for (int i = 0; i < this.floorList.Count; i++)
            {
                /*if (this.floorList[i].getHealth() <= 0)
                {
                    this.floorList.RemoveAt(i);
                    i--;
                }*/
            }
        }

        public Floor ishit(Vector2 v, float xRange, float yRange)
        {
            foreach (Floor f in this.floorList)
            {
                if (f.ishit(v, xRange, yRange))
                {
                    return f;
                }
            }

            return null;
        }

      
    }
}
