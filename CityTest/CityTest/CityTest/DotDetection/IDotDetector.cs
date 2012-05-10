using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectProject
{
    interface IDotDetectorAlgorithm
    {

        void addColor(Microsoft.Xna.Framework.Color c);

        Boolean isDot();
        void reset();
    }
}
