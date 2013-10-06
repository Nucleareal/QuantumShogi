using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

namespace QuantumShogi.Logic
{
    public class Scene_Play : Scene
    {
        private World world;
        private KeyListener klistener;
        private MousePointEventHandler Pointing;
        private MouseClickEventHandler Clicking;
        private bool IsRunning;

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

            IsRunning = true;
            klistener.KeyDown += delegate(int KeyCode) { if(KeyCode == DX.KEY_INPUT_ESCAPE) IsRunning = false; };
        }

        public override void Logic()
        {
            klistener.Listen();
        }

        public override void Draw()
        {
            world.Draw();
        }

        public override bool Processing()
        {
            return IsRunning;
        }
    }
}
