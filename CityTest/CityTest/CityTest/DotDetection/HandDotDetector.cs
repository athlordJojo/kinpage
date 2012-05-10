using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KinectProject
{
    class HandDotDetector
    {
        DotDetector_ResultObject[] results { get; set; }
        Vector2[] handPositions = null;
        List<Color> colorList = null;
        Rectangle redDotDetectionArea = new Rectangle(0, 0, 0, 0);
        AbstractDotDetector dotDetector = null;
        private String detectionColor = AbstractDotDetector.detectGreenDot;
        int neededPixels = -1;


        // thresholddetection
        int detectedDotsInRow = 0;
        int detectedDotsInRow_Threshold = 50;// in 50 frames muss der punkt erkannt werden, damit gesagt wird: Threshold detected
        public Boolean ready { get; set; }
        public Boolean startThresholdSearch {get; set;}


        // varibalen für aussreiser (bolter) behandlung 
        int leftHandbolterCounter = 0;
        int rightHandbolterCounter = 0;
        int bolterThreshold = 25;
        Dictionary<int, int> hand_bolterValue = new Dictionary<int, int>();
        KinectSensor kinectSensor = null;

        public HandDotDetector(Rectangle redDotDetectionArea, KinectSensor k)
        {
            this.kinectSensor = k;
            this.redDotDetectionArea = redDotDetectionArea;
            results = new DotDetector_ResultObject[2];
            results[0] = new DotDetector_ResultObject();
            results[1] = new DotDetector_ResultObject();
            handPositions = new Vector2[2];
            colorList = new List<Color>();
            dotDetector = new DotDetectorAlgorithm_Counter(detectionColor);
            neededPixels = redDotDetectionArea.Width * redDotDetectionArea.Height;
            this.ready = false;

            hand_bolterValue.Add(0, 0);
            hand_bolterValue.Add(1, 0);
           

        }

        public DotDetector_ResultObject[] dotDetected(Vector2[] handPositions, AllFramesReadyEventArgs e)
        {
            this.handPositions = handPositions;
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                byte[] pixelsFromFrame = null;
                if (colorImageFrame == null) return null;

                pixelsFromFrame = new byte[colorImageFrame.PixelDataLength];
                colorImageFrame.CopyPixelDataTo(pixelsFromFrame);

                //Tiefenbildauswertung
                using (DepthImageFrame depthImageFrame = e.OpenDepthImageFrame())
                {
                    if (depthImageFrame == null) return null;

                    short[] depthPixelsFromFrame = new short[depthImageFrame.PixelDataLength];
                    depthImageFrame.CopyPixelDataTo(depthPixelsFromFrame);

                    algoChangeCheck();//anderen algo verwenden ?

                    for (int i = 0; i < handPositions.Length; i++)
                    {
                        colorList.Clear();
                        Vector2 handPosition = handPositions[i];
                        if (handPosition != null)
                        {
                            ColorImagePoint cip = this.kinectSensor.MapDepthToColorImagePoint(DepthImageFormat.Resolution640x480Fps30, (int)handPosition.X, (int)handPosition.Y, 0, ColorImageFormat.RgbResolution640x480Fps30);
                            int startXPosition = (int)cip.X - (redDotDetectionArea.Width / 2);
                            int startYPosition = (int)cip.Y - (redDotDetectionArea.Height / 2);
                            Color c = new Color();
                            this.dotDetector.reset();

                            for (int y = startYPosition; y < startYPosition + redDotDetectionArea.Height; y++)
                            {
                                Boolean inYBoundaries = y >= 0 && y < colorImageFrame.Height;//gucken ob y wert in den grenzen liegt
                                for (int x = startXPosition; x < startXPosition + redDotDetectionArea.Width; x++)
                                {
                                    int y_X_kinectWidth = y * colorImageFrame.Width;
                                    Boolean inXBoundaries = x >= 0 && x < colorImageFrame.Width;
                                    if (inXBoundaries && inYBoundaries)
                                    {
                                        //ColorImagePoint cip = depthImageFrame.MapToColorImagePoint(x, y, ColorImageFormat.RgbResolution640x480Fps30);
                                        //cip = this.kinectSensor.MapDepthToColorImagePoint(DepthImageFormat.Resolution640x480Fps30, x, y, 0, ColorImageFormat.RgbResolution640x480Fps30);
                                        //int tx = cip.X;
                                        //int ty = cip.Y;
                                        //int y_X_kinectWidth = ty * colorImageFrame.Width;
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
                                                int pos = positionInArray * 4;
                                                c = new Color(pixelsFromFrame[pos + 2], pixelsFromFrame[pos + 1], pixelsFromFrame[pos]);
                                                this.dotDetector.addColor(c);
                                            }
                                            else
                                            {
                                                c = new Color(0, 0, 0);
                                            }
                                            colorList.Add(c);
                                        }
                                    }
                                }
                            }

                            //In dem ergebnis-Objekt die verwendeten Daten setzten 
                            DotDetector_ResultObject r = results[i];
                            r.usedHandPosition = handPosition;
                            Boolean dotDetected = dotDetector.isDot();

                            //int bolterCounter = i == 0 ? rightHandbolterCounter: leftHandbolterCounter;
                            //int bolterCounter = -1;
                            //hand_bolterValue.TryGetValue(i, out bolterCounter);

                            if (dotDetected)
                            {
                                if (i == 0 && rightHandbolterCounter < bolterThreshold)
                                {
                                    rightHandbolterCounter++;
                                }
                                else if (i == 1 && leftHandbolterCounter < bolterThreshold)
                                {
                                    leftHandbolterCounter++;
                                }
                            }
                            else
                            {
                                if (i == 0 && rightHandbolterCounter > 0)
                                {
                                    rightHandbolterCounter--;
                                }
                                else if (i == 1 && leftHandbolterCounter > 0)
                                {
                                    leftHandbolterCounter--;
                                }
                            }
                           // hand_bolterValue.Remove(i);
                            //hand_bolterValue.Add(i, bolterCounter);


                            //if (i == 0)
                            //{
                            //    Console.WriteLine(rightHandbolterCounter);
                            //}
                            //else if (i == 1)
                            //{
                            //    Console.WriteLine(leftHandbolterCounter);
                            //}

                            if (i == 0)
                            {
                                r.dotDetected = dotDetected || rightHandbolterCounter > 0;
                            }
                            else
                            {
                                r.dotDetected = dotDetected || leftHandbolterCounter > 0;
                            }
                            
                            r.redMode = dotDetector.redMode;
                            r.greenMode = dotDetector.greenMode;
                            r.blueMode = dotDetector.blueMode;
                            if (neededPixels == colorList.Count)
                            {
                                r.usedColorObjects = colorList.ToList();
                            }
                            else
                            {
                                r.usedColorObjects = null;// wenn teile  des um die hand gezecihenten rechtecks ausserhalb der grenzen liegen, sind weniger pixel in der liste
                            }

                        }
                    }
                }
            }

            return results;
        }


        public DotDetector_ResultObject[] findThreshold(Vector2[] handPositions, AllFramesReadyEventArgs e)
        {

            if (!startThresholdSearch && !Keyboard.GetState().IsKeyDown(Keys.Space))// wurde noch nicht gesucht und es wurde NICHT space gedrückt
            {
                return null;
            }
            else if (!startThresholdSearch && Keyboard.GetState().IsKeyDown(Keys.Space))// wurde noch nicht gesucht und es wurde space gedrückt
            {
                startThresholdSearch = !startThresholdSearch;
            }

            DotDetector_ResultObject[] r =  this.dotDetected(handPositions, e);
            if (r == null) return null;

            if (!r[0].dotDetected)// ist in rechter hand etwas entdeckt worden ? nein
            {
                if (dotDetector.colorDiffrenceThreshold > 0)
                {
                    dotDetector.colorDiffrenceThreshold -= 0.2;
                    //Console.WriteLine("New Threshold: " + dotDetector.colorDiffrenceThreshold);
                }
                else
                {
                    dotDetector.colorDiffrenceThreshold = 120;
                }

                detectedDotsInRow = 0;
            }
            else // ist in rechter hand etas entdeckt worden ? Ja
            {
                this.detectedDotsInRow++;
                
            }

            ready = detectedDotsInRow > detectedDotsInRow_Threshold;
            if (ready) Console.WriteLine("Threshold found: " + dotDetector.colorDiffrenceThreshold);
            return r;
        }

        private void algoChangeCheck()
        {
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Q))
            {
                double oldThreshold = this.dotDetector.colorDiffrenceThreshold;
                this.dotDetector = new DotDetectorAlgorithm_Counter(detectionColor);
                this.dotDetector.colorDiffrenceThreshold = oldThreshold;
                Console.WriteLine("Dotdetectroalgorithm: Counter");

            }
            else if (k.IsKeyDown(Keys.W))
            {
                this.dotDetector = new DotDetectorAlgorithm_Average(detectionColor);
                Console.WriteLine("Dotdetectroalgorithm: Average");
            }
            else if (k.IsKeyDown(Keys.Y))
            {
                detectionColor = AbstractDotDetector.detectRedDot;
                this.dotDetector.changeDetectionColor(detectionColor);
            }
            else if (k.IsKeyDown(Keys.X))
            {
                detectionColor = AbstractDotDetector.detectGreenDot;
                this.dotDetector.changeDetectionColor(detectionColor);
            }
            else if (k.IsKeyDown(Keys.C))
            {
                detectionColor = AbstractDotDetector.detectBlueDot;
                this.dotDetector.changeDetectionColor(detectionColor);
            }

        }

    }
}
