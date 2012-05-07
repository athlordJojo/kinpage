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
                this.floorList.Add(new Floor(new Vector2(this.position.X, this.position.Y - (i * 80)), GraphicsUtil.getBuilding()));
            }
        }

        public void draw(SpriteBatch sb)
        {
            foreach(Floor f in this.floorList)
            {
                f.draw(sb);
            }
        }

        public bool ishit(Vector2 v, float xRange, float yRange)
        {
            bool hit = false;
            foreach (Floor f in this.floorList)
            {
                hit = hit || f.ishit(v, xRange, yRange);
            }

            return hit;
        }

      
    }
}
