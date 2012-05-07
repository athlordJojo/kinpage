using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage
{
    public class KinectDraw
    {
        private float posW;
        private float posH;
        private float yOffset = 140;
        private float xOffset = 360;
        public bool isLeftHandOpen =false;
        public bool isRightHandOpen = false;

        public KinectDraw(float width, float height)
        {
            
            this.posW =  width / 2.2f;//1.8
            this.posH = height / 1.2f;//1.8 //changes monsterwidth
            Console.WriteLine("posH: " + posH + " posW: " + posW);
        }
        

        public void drawBody(Skeleton playerSkeleton, SpriteBatch sb)
        {
            //Draw Fist
            //drawBodyPartMiddle(playerSkeleton.Joints[JointType.HandRight], playerSkeleton.Joints[JointType.ElbowRight], spriteFist, 0.5f, false);
            if (isRightHandOpen)
            {
                drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.HandRight], playerSkeleton.Joints[JointType.ElbowRight], GraphicsUtil.hand, 0.5f, false, 0.41f);
            }
            else
            {
                drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.HandRight], playerSkeleton.Joints[JointType.ElbowRight], GraphicsUtil.fist, 0.5f, false, 0.41f);
            }

            if (isLeftHandOpen)
            {
                drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.HandLeft], playerSkeleton.Joints[JointType.ElbowLeft], GraphicsUtil.hand, 0.5f, true, 0.41f);
            }
            else
            {
                drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.HandLeft], playerSkeleton.Joints[JointType.ElbowLeft], GraphicsUtil.fist, 0.5f, true, 0.41f);
            }


            //head body
            drawBodyPartNoRotation(sb, playerSkeleton.Joints[JointType.ShoulderCenter], GraphicsUtil.body, 0.5f, false, new Vector2(5, 70), 0.48f);
            drawBodyPartNoRotation(sb, playerSkeleton.Joints[JointType.Head], GraphicsUtil.head, 0.7f, false, new Vector2(25, 20), 0.47f);
            //Draw Legs
            drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.HipRight], playerSkeleton.Joints[JointType.FootRight], GraphicsUtil.leg, 0.5f, false, 0.44f);
            drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.HipLeft], playerSkeleton.Joints[JointType.FootLeft], GraphicsUtil.leg, 0.5f, true,0.44f);
            //Draw Feet
            //drawBodyPartJointPos(sb, playerSkeleton.Joints[JointType.FootRight], playerSkeleton.Joints[JointType.HipRight], GraphicsUtil.foot, 0.5f, false, new Vector2(15,0) , 0.42f);
            //drawBodyPartJointPos(sb, playerSkeleton.Joints[JointType.KneeLeft], playerSkeleton.Joints[JointType.HipLeft], GraphicsUtil.foot, 0.5f, true, new Vector2(-15,0), 0.42f);
            drawBodyPartMiddleFoot(sb, playerSkeleton.Joints[JointType.FootRight], playerSkeleton.Joints[JointType.HipRight], GraphicsUtil.foot, 0.5f, false, 0.42f);
            drawBodyPartMiddleFoot(sb, playerSkeleton.Joints[JointType.FootLeft], playerSkeleton.Joints[JointType.HipLeft], GraphicsUtil.foot, 0.5f, true, 0.42f);
            //draw Arms
            drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.ElbowRight], playerSkeleton.Joints[JointType.ShoulderRight], GraphicsUtil.arm, 0.5f, false, 0.43f);
            drawBodyPartMiddle(sb, playerSkeleton.Joints[JointType.ElbowLeft], playerSkeleton.Joints[JointType.ShoulderLeft], GraphicsUtil.arm, 0.5f, true, 0.43f);
            


        }

        private void drawBodyPartJointPos(SpriteBatch spriteBatch, Joint a, Joint b, Texture2D part, float scale, bool mirror, Vector2 offset, float depth)
        {
            float angle = calculateAngle(a.Position, b.Position);
            Vector2 positionZ = calculatePos(a.Position);
            positionZ = Vector2.Add(positionZ, offset);
            if (mirror)
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, angle, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.FlipHorizontally, depth);
            }
            else
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, angle, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.None, depth);
            }
        }

        private void drawBodyPartNoRotation(SpriteBatch spriteBatch, Joint a, Texture2D part, float scale, bool mirror, Vector2 offset, float depth)
        {
            Vector2 positionZ = calculatePos(a.Position);
            positionZ = Vector2.Add(positionZ, offset);
            if (mirror)
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, 0f, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.FlipHorizontally, depth);
            }
            else
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, 0f, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.None, depth);
            }
        }

        private void drawBodyPartMiddle(SpriteBatch spriteBatch, Joint a, Joint b, Texture2D part, float scale, bool mirror, float depth)
        {
            float angle = calculateAngle(a.Position, b.Position);
            Vector2 positionZ = calculateMiddlePos(a.Position, b.Position, part, scale);
            if (mirror)
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, angle, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.FlipHorizontally, depth);
            }
            else
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, angle, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.None, depth);
            }

        }

        private void drawBodyPartMiddleFoot(SpriteBatch spriteBatch, Joint a, Joint b, Texture2D part, float scale, bool mirror, float depth)
        {
            float angle = calculateAngle(a.Position, b.Position);
            Vector2 positionZ = calculateMiddlePos(a.Position, b.Position, part, scale);
            if (positionZ.Y > 650) positionZ.Y = 650;
            if (mirror)
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, angle, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.FlipHorizontally, depth);
            }
            else
            {
                spriteBatch.Draw(part, positionZ, null, Color.White, angle, new Vector2(part.Width * scale, part.Height * scale), scale, SpriteEffects.None, depth);
            }

        }


        //***************DRAW HELPERS
        public Vector2 calculatePos(SkeletonPoint a)
        {
            //return new Vector2((((0.5f * a.X) + 0.5f) * (posH) - (texture.Width*scale*0.5f)), (((-0.5f * a.Y) + 0.5f) * (posW)- (texture.Height*scale*0.5f) ));
            return new Vector2((((0.5f * a.X) + 0.5f) * (posH)) + xOffset, (((-0.5f * a.Y) + 0.5f) * (posW)) + yOffset);
        }

        private float calculateDiff(SkeletonPoint a, SkeletonPoint b)
        {
            Vector2 va = new Vector2((((0.5f * a.X) + 0.5f) * (posH)) + xOffset, (((-0.5f * a.Y) + 0.5f) * (posW)) + yOffset);
            Vector2 vb = new Vector2((((0.5f * b.X) + 0.5f) * (posH)) + xOffset, (((-0.5f * b.Y) + 0.5f) * (posW)) + yOffset);
            return Vector2.Distance(va, vb);
        }

        private float calculateAngle(SkeletonPoint a, SkeletonPoint b)
        {
            float angle = -(float)Math.Atan2(a.Y - b.Y, a.X - b.X);
            angle -= (float)Math.PI / 2;
            return angle;
        }

        public Vector2 calculateMiddlePos(SkeletonPoint a, SkeletonPoint b, Texture2D texture, float scale)
        {
            //Vector2 tmp = new  Vector2((((0.5f * ((a.X + b.X)/2)) + 0.5f) * (posH) - (texture.Width * scale * 0.5f)), (((-0.5f * ((a.Y + b.Y)/2) + 0.5f) * (posW) - (texture.Height * scale * 0.5f))));
            Vector2 tmp = new Vector2((((0.5f * ((a.X + b.X) / 2)) + 0.5f) * (posH)) + xOffset, (((-0.5f * ((a.Y + b.Y) / 2) + 0.5f) * (posW)) + yOffset));
            return tmp;
        }


        public void changePosW(float val)
        {
            this.posW += val;
        }

        public void changePosH(float val)
        {
            this.posH += val;
        }
    }
}
