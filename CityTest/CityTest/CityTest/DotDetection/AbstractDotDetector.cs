using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace KinectProject
{
    abstract class AbstractDotDetector
    {
        public  const String detectRedDot = "Detectionmode_RED";
        public  const String detectGreenDot = "Detectionmode_GREEN";
        public  const String detectBlueDot = "Detectionmode_BLUE";

        public Boolean redMode{get;set;}
        public Boolean greenMode { get; set; }
        public Boolean blueMode { get; set; }

        public double colorDiffrenceThreshold = 120;
        public AbstractDotDetector(String colorString)
        {
            changeDetectionColor(colorString);
        }

        public void changeDetectionColor(String colorString){
            if (colorString == null) throw new NullReferenceException("Dot-Detection-Color-String can not be null");

            if (colorString.Equals(AbstractDotDetector.detectRedDot))
            {
                redMode = true;
                greenMode = false;
                blueMode = false;
                colorDiffrenceThreshold = 120;
            }
            else if (colorString.Equals(AbstractDotDetector.detectGreenDot))
            {
                redMode = false;
                greenMode = true;
                blueMode = false;
                colorDiffrenceThreshold = 50;
            }
            else if (colorString.Equals(AbstractDotDetector.detectBlueDot))
            {
                redMode = false;
                greenMode = false;
                blueMode = true;
                colorDiffrenceThreshold = 120;
            }
            else
            {
                throw new Exception("Unknown Dot-Detection-Color-String: " + colorString);
            }

            Console.WriteLine(colorString);
        }


        public abstract void addColor(Color c);
        public abstract void addAllColors(List<Color> list);
        public abstract Boolean isDot();
        public abstract void reset();
    }
}
