using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinampage.ParticleSimulation
{
    public interface IEmitterable
    {
        void update();
        int getLifetime();
    }
}
