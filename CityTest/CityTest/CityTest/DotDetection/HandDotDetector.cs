using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace KinectProject
{
    class HandDotDetector
    {
        DotDetector_ResultObject[] results { get; set; }
        List<Color> colorList = null;
        Rectangle redDotDetectionArea = new Rectangle(0, 0, 0, 0);

        int neededPixels = -1;// 0= rectangle mode, 1= depthmode

        // thresholddetection
        int detectedDotsInRow = 0;
        int detectedDotsInRow_Threshold = 50;// in 50 frames muss der punkt erkannt werden, damit gesagt wird: Threshold detected
        public Boolean ready { get; set; }
        public Boolean startThresholdSearch {get; set;}

        KinectSensor kinectSensor = null;

        Handetector_Threading detector_1 = null;
        Handetector_Threading detector_2 = null;
        int mode = -1;
        public Boolean isDebugMode {get; set;}
        public HandDotDetector(KinectSensor k, Boolean _isDebug)
        {
            this.kinectSensor = k;
            results = new DotDetector_ResultObject[2];
            results[0] = new DotDetector_ResultObject();
            results[1] = new DotDetector_ResultObject();
            colorList = new List<Color>();
            neededPixels = redDotDetectionArea.Width * redDotDetectionArea.Height;
            this.ready = false;
            mode = 0;
            isDebugMode = _isDebug;
            detector_1 = new Handetector_Threading(kinectSensor, mode, isDebugMode);
            detector_2 = new Handetector_Threading(kinectSensor, mode, isDebugMode);
        }

        public DotDetector_ResultObject[] dotDetected(Vector2[] handPositions, AllFramesReadyEventArgs e)
        {
            DateTime start = DateTime.Now;

            //this.handPositions = handPositions;
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

                    short[] depthPixelsFromFrame;
                    if (mode == 1)
                    {// modeus wo auch die teifeninformationen ausgewertet werden

                        depthPixelsFromFrame = new short[depthImageFrame.PixelDataLength];
                        depthImageFrame.CopyPixelDataTo(depthPixelsFromFrame);
                        detector_1.depthPixelsFromFrame = depthPixelsFromFrame;
                        detector_2.depthPixelsFromFrame = depthPixelsFromFrame;
                        detector_1.depthImageFrame = depthImageFrame;
                        detector_2.depthImageFrame = depthImageFrame;
                    }

                    Vector2 rightHand = handPositions[0];
                    ManualResetEvent[] doneEvents = new ManualResetEvent[2];
                    detector_1.colorImageFrame = colorImageFrame;
                    detector_1.depthImageFrame = depthImageFrame;
                    detector_1.pixelsFromFrame = pixelsFromFrame;
                    detector_1._doneEvent = new ManualResetEvent(false);
                    doneEvents[0] = detector_1._doneEvent;
                    ThreadPool.QueueUserWorkItem(detector_1.findDot, rightHand);

                    Vector2 leftHand = handPositions[1];
                    detector_2.colorImageFrame = colorImageFrame;
                    detector_2.pixelsFromFrame = pixelsFromFrame;
                    detector_2._doneEvent = new ManualResetEvent(false);
                    doneEvents[1] = detector_2._doneEvent;

                    ThreadPool.QueueUserWorkItem(detector_2.findDot, leftHand);
                    WaitHandle.WaitAll(doneEvents);

                    results[0] = detector_1.r;
                    results[1] = detector_2.r;
                }
            }

            //DateTime end = DateTime.Now;
            //Console.WriteLine(end - start);

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
                if (detector_1.dotDetector.colorDiffrenceThreshold > 0)
                {
                    detector_1.dotDetector.colorDiffrenceThreshold -= 0.2;
                }
                else
                {
                    detector_1.dotDetector.colorDiffrenceThreshold = 120;
                }

                detectedDotsInRow = 0;
            }
            else // ist in rechter hand etwas entdeckt worden ? Ja
            {
                this.detectedDotsInRow++;
            }

            ready = detectedDotsInRow > detectedDotsInRow_Threshold;
            if (ready)
            {
                Console.WriteLine("Threshold found: " + detector_1.dotDetector.colorDiffrenceThreshold);
                detector_2.dotDetector.colorDiffrenceThreshold = detector_1.dotDetector.colorDiffrenceThreshold;
            }
            return r;
        }
    }
}




