using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;

namespace KinectProject
{
    class PixelGrabber_Rect
    {
        public ColorImageFrame colorImageFrame { get; set; }
        private List<Color> l = new List<Color>();
        public byte[] pixelsFromFrame { get; set; }
        Color c = Color.Wheat;


        public List<Color> grabPixels(Vector2 handPosition, int scannedSizeOfArea)
        {
            l.Clear();
            int startXPosition = (int)handPosition.X - (scannedSizeOfArea / 2);
            int startYPosition = (int)handPosition.Y - (scannedSizeOfArea / 2);

            for (int y = startYPosition; y < startYPosition + scannedSizeOfArea; y++)
            {
                if (y < 0 || y > colorImageFrame.Height) continue;
                int y_X_kinectWidth = y * colorImageFrame.Width;

                for (int x = startXPosition; x < startXPosition + scannedSizeOfArea; x++)
                {
                    if (x < 0 || x > colorImageFrame.Width) continue;
                    int positionInArray = (y_X_kinectWidth + x) * 4;
                    if ( positionInArray + 2 < pixelsFromFrame.Count())
                    {
                        c = new Color(pixelsFromFrame[positionInArray + 2], pixelsFromFrame[positionInArray + 1], pixelsFromFrame[positionInArray]);
                        l.Add(c);
                    }
                }
            }

            return l;
        }

    }
}
