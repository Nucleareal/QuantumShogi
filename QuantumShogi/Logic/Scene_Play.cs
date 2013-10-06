using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumShogi.Logic
{
    public class Scene_Play : Scene
    {
        private World world;

        public Scene_Play()
        {
            world = new World();
            BoardInitializer.Init(world);
        }

        public override void Run()
        {
            world.Draw();
        }
    }
}
