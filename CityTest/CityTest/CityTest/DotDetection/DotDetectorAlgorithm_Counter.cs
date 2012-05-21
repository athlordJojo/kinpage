using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace KinectProject
{
    class DotDetectorAlgorithm_Counter:AbstractDotDetector
    {
        Boolean debug = false;
        List<Color> colorList = new List<Color>();
        private int pixelCounterThreshold = 40;//alt = 70;
        
        //variabeln für helligkeitsbestimmung
        Double r = 0, g = 0, b = 0, y=0;
        
        public DotDetectorAlgorithm_Counter(String colorString):base(colorString){
        
        }

        public override void addAllColors(List<Color> list)
        {
            colorList = list;
        }

        public override void addColor(Color c)
        {
            colorList.Add(c);

            r += c.R;
            g += c.G;
            b += c.B;
        }

        public override Boolean isDot()
        {
            if (colorList.Count > 0)
            {
                r /= colorList.Count;
                g /= colorList.Count;
                b /= colorList.Count;
                y = (r + g + b) / 3;
                //y = r * 0.3 + g * 0.59 + b * 0.11;// yuv
                //Console.WriteLine("R: " + r + ", G: " + g + ", B: " + b + ", y: " + y);
            }

            //double dynamicThreshold = colorList.Count * 0.02;
            int pixelCounter = 0;

            //KeyboardState k = Keyboard.GetState();
            //if (k.IsKeyDown(Keys.A))
            //{
            //    if (colorDiffrenceThreshold > 0) colorDiffrenceThreshold--;
            //    Console.WriteLine(colorDiffrenceThreshold);
            //}
            //else if (k.IsKeyDown(Keys.S))
            //{
            //    colorDiffrenceThreshold++;
            //    Console.WriteLine(colorDiffrenceThreshold);
            //}
            double delta_1, delta_2;
            Boolean isWhite = false;
            foreach (Color c in colorList)
            {
                isWhite = c.R > 240 && c.G > 240 && c.B > 240;
                if (isWhite)
                {
                    continue;
                }

                delta_1 = -1;
                delta_2 = -1;

                if (base.redMode)
                {
                    delta_1 = c.R - c.G;
                    delta_2 = c.R - c.B;
                }
                else if (base.greenMode)
                {
                    delta_1 = c.G - c.R;
                    delta_2 = c.G - c.B;
                }
                else if (base.blueMode)
                {
                    delta_1 = c.B - c.R;
                    delta_2 = c.B - c.G;
                }

                if (delta_1 > colorDiffrenceThreshold && delta_2 > colorDiffrenceThreshold)
                {
                    pixelCounter++;
                }

            }
            Boolean reddotDetected = pixelCounter > pixelCounterThreshold; 

            if (debug)
            {
                //Console.WriteLine("Pixelcounter: " + redPixelCounter + ", DynamicThreshold: " + dynamicThreshold);
                Console.WriteLine(pixelCounter);
            }

            return reddotDetected;
        }

        public override void reset()
        {
            this.colorList.Clear();
            
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }


    }
}
