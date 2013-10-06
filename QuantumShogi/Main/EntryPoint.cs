using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantumShogi.Proxy;
using QuantumShogi.Image;

namespace QuantumShogi.Main
{
    public class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            DxLibProxy.Init();
            ImageLoader.ShowLoading();
            ImageLoader.Load();
            MainThread.Run();
            DxLibProxy.Fina();
        }
    }
}
