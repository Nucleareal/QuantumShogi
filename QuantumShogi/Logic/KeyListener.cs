using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using QuantumShogi.Environment;

namespace QuantumShogi.Logic
{
    class KeyListener
    {
        private byte[] state;
        private int[] Counts;
        private int KeyCount;

        private int[] MouseState;

        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event KeyEventHandler KeyPress;
        public event MousePointEventHandler OnPoint;
        public event MouseClickEventHandler MouseDown;
        public event MouseClickEventHandler MouseUp;
        public event MouseClickEventHandler MousePress;

        public KeyListener()
        {
            KeyCount = DXEnvironment.KeyCount;
            state = new byte[KeyCount];
            Counts = new int[KeyCount];
            MouseState = new int[8];

            KeyDown += delegate(int Code) { };
            KeyUp += delegate(int Code) { };
            KeyPress += delegate(int Code) { };
            OnPoint += delegate(int x, int y) { };
            MouseDown += delegate(int Code) { };
            MouseUp += delegate(int Code) { };
            MousePress += delegate(int Code) { };
        }

        public void Listen()
        {
            DX.GetHitKeyStateAll(out state[0]);

            for (int i = 0; i < KeyCount; i++)
            {
                if (state[i] == 1)
                {
                    if (Counts[i] == 0)
                        KeyDown(i);
                    else
                        KeyPress(i);
                    Counts[i]++;
                }
                else
                {
                    if (Counts[i] != 0)
                        KeyUp(i);
                    Counts[i] = 0;
                }
            }
            int x, y;
            DX.GetMousePoint(out x, out y);
            OnPoint(x, y);

            int State = DX.GetMouseInput();

            if ((State & DX.MOUSE_INPUT_LEFT) != 0)
            {
                if (MouseState[0] == 0)
                    MouseDown(0);
                else
                    MousePress(0);
                MouseState[0]++;
            }
            else
            {
                if (MouseState[0] != 0)
                    MouseUp(0);
                MouseState[0] = 0;
            }
            if ((State & DX.MOUSE_INPUT_RIGHT) != 0)
            {
                if (MouseState[1] == 0)
                    MouseDown(1);
                else
                    MousePress(1);
                MouseState[1]++;
            }
            else
            {
                if (MouseState[1] != 0)
                    MouseUp(1);
                MouseState[1] = 0;
            }
        }
    }
}
