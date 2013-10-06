using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using QuantumShogi.Proxy;
using QuantumShogi.Logic;

namespace QuantumShogi.Main
{
    public class MainThread
    {
        public static void Run()
        {
            Scene s = new Scene_Play();

            while (DxLibProxy.Refresh())
            {
                s.Draw();
                s.Logic();
            }
        }
    }
}
