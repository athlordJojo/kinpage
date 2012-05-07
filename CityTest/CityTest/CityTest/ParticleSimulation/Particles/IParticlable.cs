using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kinampage.ParticleSimulation
{
    public interface IParticlable
    {
        void update(Vector2 gravity);
        void draw(SpriteBatch sp);
        int getLifeTime();
    }
}
