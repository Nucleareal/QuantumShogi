using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumShogi.Logic
{
    public class Scene_Play : Scene
    {
        private World world;
        private KeyListener klistener;
        private MousePointEventHandler Pointing;
        private MouseClickEventHandler Clicking;

        public Scene_Play()
        {
            world = new World();
            klistener = new KeyListener();

            world.InitBoard();
            world.InitBoard(); //1回だけだと例外が発生する
            BoardInitializer.Init(world);

            Pointing = delegate(int x, int y) { world.OnFocus(x, y); };
            Clicking = delegate(int Code) { world.OnClick(Code); };

            klistener.OnPoint += Pointing;
            klistener.MouseDown += Clicking;
        }

        public override void Logic()
        {
            klistener.Listen();
        }

        public override void Draw()
        {
            world.Draw();
        }
    }
}
