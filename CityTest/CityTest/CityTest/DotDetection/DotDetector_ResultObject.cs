using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KinectProject
{
    class DotDetector_ResultObject
    {
        public Vector2 usedHandPosition { get; set; }
        public Boolean dotDetected { get; set; }
        public List<Color> usedColorObjects { get; set;}
        public Boolean redMode { get; set; }
        public Boolean greenMode { get; set; }
        public Boolean blueMode { get; set; }
    }
}
