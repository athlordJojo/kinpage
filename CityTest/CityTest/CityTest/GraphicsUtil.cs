using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage
{
    public static class GraphicsUtil
    {
        static Random rand = new Random();

        public static AnimatedTexture explosion;
        public static AnimatedTexture smoke;
        public static AnimatedTexture explosion1;
        public static AnimatedTexture dust;

        public static List<AnimatedTexture> wreckages = new List<AnimatedTexture>();
        public static List<AnimatedTexture> garbages = new List<AnimatedTexture>();
        public static List<AnimatedTexture> people = new List<AnimatedTexture>();
        public static List<AnimatedTexture> buildings = new List<AnimatedTexture>();
        public static List<AnimatedTexture> peopleDead = new List<AnimatedTexture>();

        public static Texture2D head, body, fist, hand, arm, foot, leg; 

        //backgrounnd
        public static AnimatedTexture street;

        public static void init()
        {
            //Textures PArticles
            GraphicsUtil.explosion = new AnimatedTexture(Vector2.Zero, 0, 1f, 0.1f);
            GraphicsUtil.smoke = new AnimatedTexture(Vector2.Zero, 0, 1f, 0.4f);
            
            GraphicsUtil.explosion1 = new AnimatedTexture(Vector2.Zero, 0, 1f, 0.1f);
            GraphicsUtil.dust = new AnimatedTexture(Vector2.Zero, 0, 1f, 1f);
            GraphicsUtil.street = new AnimatedTexture(Vector2.Zero, 0, 1f, 1f);

            //Wreckages
            for (int i = 0; i < 4; i++)
            {
                GraphicsUtil.wreckages.Add(new AnimatedTexture(Vector2.Zero, 0, 1f, 0.1f));
            }

            //garbages
            for (int i = 0; i < 3; i++)
            {
                GraphicsUtil.garbages.Add(new AnimatedTexture(Vector2.Zero, 0, 1f, 0.1f));
            }

            //people
            for (int i = 0; i < 3; i++)
            {
                GraphicsUtil.people.Add(new AnimatedTexture(Vector2.Zero, 0, 1f, 0.6f));
                GraphicsUtil.peopleDead.Add(new AnimatedTexture(Vector2.Zero, 0, 1f, 0.6f));
            }
            //buildings
            for (int i = 0; i < 2; i++)
            {
                GraphicsUtil.buildings.Add(new AnimatedTexture(Vector2.Zero, 0, 1f, 0.9f));
            }
            
        }

        public static void load(ContentManager Content)
        {
            GraphicsUtil.explosion.Load(Content, "fire", 1, 1);
            GraphicsUtil.smoke.Load(Content, "smoke", 1, 1);
            GraphicsUtil.explosion1.Load(Content, "explosion1", 1, 1);
            GraphicsUtil.dust.Load(Content, "dust", 1, 1);

            //Player Parts
            head = Content.Load<Texture2D>("player/head");
            body = Content.Load<Texture2D>("player/body");
            fist = Content.Load<Texture2D>("player/fist");
            hand = Content.Load<Texture2D>("player/hand");
            arm = Content.Load<Texture2D>("player/upperarm");
            foot = Content.Load<Texture2D>("player/foot");
            leg = Content.Load<Texture2D>("player/leg");

            //Backgrounds
            GraphicsUtil.street.Load(Content, "background/street", 1, 1);

            for (int i = 0; i < wreckages.Count; i++)
            {
                GraphicsUtil.wreckages[i].Load(Content, "wreckage/wreckage"+(i+1)+"", 1, 1);
            }

            for (int i = 0; i < garbages.Count; i++)
            {
                GraphicsUtil.garbages[i].Load(Content, "garbage/garbage" + (i + 1) + "", 1, 1);
            }

            for (int i = 0; i < people.Count; i++)
            {
                //Console.WriteLine("load people/person" + (i+1));
                GraphicsUtil.people[i].Load(Content, "people/person" + (i + 1) + "", 2, 1);
                GraphicsUtil.peopleDead[i].Load(Content, "people/dead/persondead" + (i + 1) + "", 1, 1);
            }

            for (int i = 0; i < buildings.Count; i++)
            {
                GraphicsUtil.buildings[i].Load(Content, "building/building" + (i + 1) + "", 1, 1);
            }
        }


        //*********************** GETTER ***************************+
        public static AnimatedTexture getWreckage()
        {
            return wreckages[rand.Next(0, wreckages.Count)];
        }

        public static AnimatedTexture getGarbage()
        {
            return garbages[rand.Next(0, garbages.Count)];
        }

        public static int getPeople()
        {
            return rand.Next(0, people.Count);
        }

        public static AnimatedTexture getPeople(int i)
        {
            return (AnimatedTexture)people[i].Clone();
        }

        public static AnimatedTexture getPeopleDead(int i)
        {
            return (AnimatedTexture)peopleDead[i].Clone();
        }

        public static AnimatedTexture getBuilding()
        {
            return buildings[rand.Next(0, buildings.Count)];
        }
    }
}
