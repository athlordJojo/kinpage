using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinampage.shader
{
    public class ShaderParameterController
    {

        public float bloodPower { get; set;}

        public ShaderParameterController()
        {
            bloodPower = -1;
        }

        public void updateBloodParameter(Boolean enable){
            if (enable)
            {
                bloodPower = 0f;
            }
            //erhoehen bis schwellenwert
            if (bloodPower != -1 && bloodPower <= 1f)
            {
                bloodPower += 0.05f;
            }
            else if (bloodPower >= 1)//schwellenwert uebwerschritten--> wieder ausschalten
            {
                bloodPower = -1;
            }
        }

    }
}
