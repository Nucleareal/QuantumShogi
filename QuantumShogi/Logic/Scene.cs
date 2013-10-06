using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumShogi.Logic
{
    public abstract class Scene
    {
        public abstract void Logic();

        public abstract void Draw();

        public abstract bool Processing();
    }
}
