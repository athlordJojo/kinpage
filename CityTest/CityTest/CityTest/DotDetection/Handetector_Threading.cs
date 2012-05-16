using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace KinectProject
{
    class Handetector_Threading
    {
        public DepthImageFrame depthImageFrame { set; get; }
        public ColorImageFrame colorImageFrame { get; set; }
        public byte[] pixelsFromFrame { get; set; }
        public short[] depthPixelsFromFrame { get; set; }

        List<Color> colorList = null;
        List<Int32> maskPlayerPos = null;

        KinectSensor kinectSensor { get; set; }
        public AbstractDotDetector dotDetector = null;
        private String detectionColor = AbstractDotDetector.detectBlueDot;
        public ManualResetEvent _doneEvent { get; set; }

        public DotDetector_ResultObject r { get; set; }
        PixelGrabber_Rect grabber = new PixelGrabber_Rect();

        private int outlinerValue = 0;
        private int outlinerThreshold = 12;
        public Boolean isDebugMode { get; set; }
        private int mode = -1;
        public Handetector_Threading(KinectSensor kinectSensor, int mode, Boolean isDebug)
        {
            this.kinectSensor = kinectSensor;
            colorList = new List<Color>();
            maskPlayerPos = new List<Int32>();
            dotDetector = new DotDetectorAlgorithm_Counter(detectionColor);
            r = new DotDetector_ResultObject();
            this.mode = mode;
            isDebugMode = isDebug;
        }

        public void findDot(Object threadContext)
        {
            if(mode == 0){
                findDot_RectangleMode((Vector2)threadContext);
            }
            else if (mode == 1)
            {
                findDot_DepthMode((Vector2)threadContext, new Rectangle(0, 0, 120, 120));
            }
            else
            {
                throw new Exception("Unbekannter Punkt-Such-Modus");
            }
        }

        private void findDot_DepthMode(Vector2 handPosition, Rectangle redDotDetectionArea)
        {
            colorList.Clear();
            maskPlayerPos.Clear();
            if (handPosition != null)
            {
                //ColorImagePoint cip = this.kinectSensor.MapDepthToColorImagePoint(DepthImageFormat.Resolution640x480Fps30, (int)handPosition.X, (int)handPosition.Y, 0, ColorImageFormat.RgbResolution640x480Fps30);
                int startXPosition = (int)handPosition.X - (redDotDetectionArea.Width / 2);
                int startYPosition = (int)handPosition.Y - (redDotDetectionArea.Height / 2);
                Color c = new Color();
                //this.dotDetector.reset();
                for (int y = startYPosition; y < startYPosition + redDotDetectionArea.Height; y++)
                {
                    int y_X_kinectWidth = y * colorImageFrame.Width;
                    Boolean inYBoundaries = y >= 0 && y < colorImageFrame.Height;//gucken ob y wert in den grenzen liegt
                    for (int x = startXPosition; x < startXPosition + redDotDetectionArea.Width; x++)
                    {
                        if (y < 0 || y > colorImageFrame.Height) continue;
                        Boolean inXBoundaries = x >= 0 && x < colorImageFrame.Width;
                        if (inXBoundaries && inYBoundaries)
                        {
                            if (x < 0 || x > colorImageFrame.Width) continue;
                            //cip = depthImageFrame.MapToColorImagePoint(x, y, ColorImageFormat.RgbResolution640x480Fps30);
                            //cip = this.kinectSensor.MapDepthToColorImagePoint(DepthImageFormat.Resolution640x480Fps30, x, y, 0, ColorImageFormat.RgbResolution640x480Fps30);
                            //int tx = cip.X;
                            //int ty = cip.Y;
                            //y_X_kinectWidth = ty * colorImageFrame.Width;
                            //Console.WriteLine(x + "," + y);
                            int positionInArray = y_X_kinectWidth + x;
                            if (positionInArray < depthPixelsFromFrame.Length)
                            {
                                short depthInfoForColorPixel = depthPixelsFromFrame[positionInArray];
                                int playerIndex = depthInfoForColorPixel & 7;// bitweise verundung (letzten drei bits sind id. also kann hiermit gesagt wenrden ob der pixel zum spieler gehort)
                                //Pixel gehoert zu spieler
                                if (playerIndex != 0)
                                {
                                    //c = rgbColorObjects[positionInArray];
                                    //int pos = positionInArray * 4;
                                    //c = new Color(pixelsFromFrame[pos + 2], pixelsFromFrame[pos + 1], pixelsFromFrame[pos]);
                                    //this.dotDetector.addColor(c);
                                    maskPlayerPos.Add(x);
                                    maskPlayerPos.Add(y);
                                }
                                else
                                {
                                    //c = new Color(0, 0, 0);
                                    maskPlayerPos.Add(-1);
                                    maskPlayerPos.Add(-1);
                                }
                                //colorList.Add(c);
                            }
                        }
                    }
                }

                Boolean dotDetected = false;
                int maskX, maskY, calcPos, currentYOffset = 0;
                int passes = 0;
                int pixelAnzahl = pixelsFromFrame.Count();
                int colorImageFrameWidth = colorImageFrame.Width;
                for (int xOffset = -10; xOffset <= 10; xOffset += 10)
                {
                    for (int yOffset = -10; yOffset <= 10; yOffset += 10)
                    {
                        currentYOffset = yOffset;
                        this.dotDetector.reset();
                        colorList.Clear();
                        passes++;
                        for (int k = 0; k < maskPlayerPos.Count; k += 2)
                        {
                            maskX = maskPlayerPos.ElementAt(k);
                            maskY = maskPlayerPos.ElementAt(k + 1);
                            if (maskX != -1 && maskY != -1)
                            {
                                maskX += xOffset;
                                maskY += yOffset;
                                calcPos = ((maskY * colorImageFrameWidth) + maskX) * 4;// *4 da das im fabrpixel array ein pixel durch 4 werte dargestellt wird

                                if (calcPos>= 0 &&  calcPos + 2 < pixelAnzahl)
                                {
                                    c = new Color(pixelsFromFrame[calcPos + 2], pixelsFromFrame[calcPos + 1], pixelsFromFrame[calcPos]);
                                    dotDetector.addColor(c);
                                }
                                else
                                { // falls position ausserhalb des arrays liegt. zb durch offset
                                    c = Color.Black;
                                }
                            }
                            else
                            {
                                c = Color.Black;
                            }
                            colorList.Add(c);
                        }
                        dotDetected = dotDetector.isDot();

                    }
                    if (dotDetected)
                    {
                        //if (xOffset != 0 || currentYOffset != 0) Console.WriteLine("Verbessrung: " + DateTime.Now);
                        break;
                    }
                }
                //In dem ergebnis-Objekt die verwendeten Daten setzten 
                r.usedHandPosition = handPosition;
                //dotDetected = dotDetector.isDot();

                r.redMode = dotDetector.redMode;
                r.greenMode = dotDetector.greenMode;
                r.blueMode = dotDetector.blueMode;
                r.dotDetected = dotDetected;
                r.usedSize = redDotDetectionArea.Width;
                if(isDebugMode)r.usedColorObjects = colorList.ToList();
                r.usedSize = redDotDetectionArea.Width;
                _doneEvent.Set();
            }
        }

        private void findDot_RectangleMode(Vector2 handPosition)
        {
            DateTime start = DateTime.Now;

            //handPosition.X = 100; handPosition.Y = 100;
            if (handPosition != null)
            {
                Boolean dotDetected = false;
                List<Color> l = null;
                int startValue = 50;// startwert aus dem die suche nach dem punkt beginnt
                int endValue = 150;// maximaler wert des Rechtecks in dem nach dem Punkt gesucht wird
                int stepSize = 50;// groesse um die das rechteck erhoeht wird, wenn der punkt nicht gefunden wurde

                int actualValue = -1, lastValue = 0 ;
                grabber.pixelsFromFrame = pixelsFromFrame;
                grabber.colorImageFrame = colorImageFrame;
                for (actualValue = startValue; actualValue <= endValue; actualValue += stepSize)
                {
                    l = grabber.grabPixels(handPosition, actualValue);
                    dotDetector.addAllColors(l);
                    dotDetected = dotDetector.isDot();

                    lastValue = actualValue;
                    if (dotDetected)
                    {
                        break;
                    }
                }

                // aussreisser behandlung
                if (dotDetected && outlinerValue < outlinerThreshold)
                {
                    outlinerValue = outlinerThreshold;
                }
                else if (!dotDetected && outlinerValue > 0)
                {
                    outlinerValue--;
                }

                //In dem ergebnis-Objekt die verwendeten Daten setzten 
                r.usedHandPosition = handPosition;
                r.redMode = dotDetector.redMode;
                r.greenMode = dotDetector.greenMode;
                r.blueMode = dotDetector.blueMode;
                Boolean bolterTrue = outlinerValue > 0;
                if (!dotDetected && bolterTrue)
                {
                    Console.WriteLine("Ausreisser behandelt: " + DateTime.Now);
                }
                r.dotDetected = dotDetected || bolterTrue;

                if (isDebugMode)
                {
                    r.usedSize = lastValue;
                    Console.WriteLine(DateTime.Now - start);
                    r.usedColorObjects = l.ToList();
                }
                
                _doneEvent.Set();
            }
        }

    }

}
