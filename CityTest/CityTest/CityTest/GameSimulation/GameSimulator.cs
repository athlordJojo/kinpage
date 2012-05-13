using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kinampage.ParticleSimulation;

namespace Kinampage.GameSimulation
{
    public class GameSimulator
    {
        public List<People> peopleList = new List<People>();
        private List<Building> buildingList = new List<Building>();

        private bool isFirstUpdate=true;

        Random rand = new Random();
        private Player player;

        public GameSimulator(Player player)
        {
            //Initiate lanes
            this.player = player;
        }

        private void firstUpdate()
        {
            createBuildings();
        }

        private void createBuildings()
        {
            buildingList.Add(new Building(new Vector2(250, 520), 6));
            buildingList.Add(new Building(new Vector2(900, 520), 5));
        }

        public void populate()
        {
            //Create People
            if (rand.Next(20) != 10) return;
            int rnd = rand.Next(2);
            //Console.WriteLine(rnd);
            int i = GraphicsUtil.getPeople();

            int tmp = rand.Next(1, 20);
            //float speed = ((float)tmp) / 10;
            float speed = 1f;
            if (rnd == 0)
            {
                //peopleList[lane].Add(new People(new Vector2(500, (lane * 20) + 200), new Vector2(-1, 0)));
                this.peopleList.Add(new People(new Vector2(1300, 690), new Vector2(-1,0), GraphicsUtil.getPeople(i), i, speed));

            }
            else if (rnd == 1)
            {
                //peopleList[lane].Add(new People(new Vector2(0, (lane * 20) + 200), new Vector2(1, 0)));
                this.peopleList.Add(new People(new Vector2(0, 690), new Vector2(1, 0), GraphicsUtil.getPeople(i), i, speed));

            }
        }

        public void update()
        {
            if (isFirstUpdate)//erster Update aufruf init game
            {
                isFirstUpdate = false;
                firstUpdate();
            }
            playerEffects();
            //update People
            for (int i = 0; i < this.peopleList.Count; i++)
            {
                People p = this.peopleList[i];
                //Remove People if it is dead (life = 0 or below)
                if (p.getHealth() <= 0 || p.position.X < -20 || p.position.X > 1300)
                {
                    this.peopleList.RemoveAt(i);
                    i--;
                    continue;
                }

                p.update();
            }

            //buildings
            for (int i = 0; i < this.buildingList.Count; i++)
            {
                buildingList[i].update();
            }

        }

        public void draw(SpriteBatch sp)
        {
            //Draw all People
            foreach (People p in this.peopleList)
            {
                p.draw(sp);
            }

            //draw all Buildings
            foreach (Building b in this.buildingList)
            {
                b.draw(sp);
            }
        }

        public void killPeople(Vector2 v)
        {
            for (int i = 0; i < this.peopleList.Count; i++)
            {
                People p = this.peopleList[i];
                if (Vector2.Distance(p.position, v) < 90)
                {
                    p.setHealth(0);
                    EffectUtil.createDeadPerson(Game1.particleSimulator.psGrav, p.position, new Vector2(rand.Next(-6, 6), -rand.Next(5,13)), p.getType(), p.getScale());
                }
            }
        }


        /// <summary>
        /// check for playereffects like kick or bash for the hands/feet
        /// </summary>
        public void playerEffects()
        {
            //HandBash player ground
            bash(player.getLeftHand());
            bash(player.getRightHand());

            kick(player.getLeftFoot());
            kick(player.getRightFoot());
        }

        //******************* Player Effects Like Kicking and Punching ***************************
        /// <summary>
        /// Bash with the hand on the ground
        /// </summary>
        /// <param name="h"></param>
        public void bash(Hand h)
        {
            float hSpeed = h.getHorizontalSpeed();
            float speed = h.moveVector.Length();
            //hit ground
            if (h.pos.Y > 620 && speed > 5)//.Y > 10 && h.moveVector.Y > 0
            {
                EffectUtil.createShockwave(Game1.particleSimulator.psNoGrav, h.pos);
                EffectUtil.createExplosion(Game1.particleSimulator.psGrav, h.pos, new Vector2(0, -4), new Vector2(6, 2), 2);
                killPeople(h.pos);
                return;
            }

            //hit house
            if (hSpeed > 20)
            {
                for (int i = 0; i < buildingList.Count; i++)
                {
                    Floor f = buildingList[i].ishit(h.pos, 30, 200);
                    if (f != null)
                    {
                        f.damage(10 + rand.Next(5));
                        EffectUtil.createExplosionWithSmoke(Game1.particleSimulator.psGrav, h.pos, h.moveVector.X);
                        EffectUtil.createExplosion(Game1.particleSimulator.psGrav, h.pos, new Vector2(0, -4), new Vector2(6, 2), 2);
                    }
                }
            }
        }

        /// <summary>
        /// Kick with the foot
        /// </summary>
        /// <param name="h"></param>
        public void kick(Hand h)
        {
            float speed = h.moveVector.Length();
            if (speed < 15) return;
            //stomp on ground
            if (h.pos.Y > 610 && h.moveVector.Y > 20 && h.moveVector.Y > 0)
            {
                EffectUtil.createShockwave(Game1.particleSimulator.psNoGrav, h.pos);
                EffectUtil.createExplosion(Game1.particleSimulator.psGrav, h.pos, new Vector2(0, -4), new Vector2(6, 2), 2);
                killPeople(h.pos);
                return;
            }
            //kick people
            for (int i = 0; i < this.peopleList.Count; i++)
            {
                People p = this.peopleList[i];
                if (Vector2.Distance(p.position, h.pos) < 50)
                {
                    p.setHealth(0);
                    Vector2 kickV = h.moveVector;
                    if(kickV.Y > 0) kickV.Y *= -1;
                    if (kickV.Y < -10) kickV.Y = -10;
                    EffectUtil.createDeadPerson(Game1.particleSimulator.psGrav, p.position, kickV , p.getType(), p.getScale());
                }
            }
        }



        //*************************** GETTER SETTER ***************************
    }
}
