using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectProject
{
    class DotDetectorAlgorithm_Average: AbstractDotDetector 
    {
        Boolean debug = false;

        private int pixelCounter = 0, dotDiffrenceThreshold = 75;
        Double r = 0, g = 0, b = 0;

        public DotDetectorAlgorithm_Average(String colorString) : base(colorString)
        {

        }
        
        public override void addColor(Microsoft.Xna.Framework.Color c)
        {
            r += c.R;
            g += c.G;
            b += c.B;

            pixelCounter++;
        }

        public override Boolean isDot()
        {

            //gucken ob rot-wert ueberwiegt (im durchschnitt)
            r /= pixelCounter;
            g /= pixelCounter;
            b /= pixelCounter;

            double rToGDist = r - g;
            double rToBDist = r - b;
            Boolean dotDetected = rToBDist > dotDiffrenceThreshold && rToGDist > dotDiffrenceThreshold;

            if(debug){
                //Console.WriteLine("R: " + r + ", G: " + g + ", B: " + b + ", PixelCounter = " + pixelCounter);
                Console.WriteLine("rtog: " + rToGDist + ", rToB: " + rToBDist);
            }

            return dotDetected;
        }

        public override void reset()
        {
            this.pixelCounter = 0;
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }
    }
}
