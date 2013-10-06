using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using QuantumShogi.Environment;

namespace QuantumShogi.Proxy
{
    /// <summary>
    /// @author つらみ
    /// </summary>
    public class DxLibProxy
    {
        private static readonly int DX_SUCCESS = 0;
        //private static readonly int DX_FAILED = -1;
        //private static readonly int DX_ERROR = 1;

        private static IEnumerable<int> InitalWork()
        {
            yield return DX.ChangeWindowMode(DX.TRUE);
            yield return DX.SetGraphMode(DXEnvironment.SX, DXEnvironment.SY, 32);
            yield return DX.DxLib_Init();
        }

        private static IEnumerable<int> FinalWork()
        {
            yield return DX.DxLib_End();
        }

        private static IEnumerable<int> RefWork()
        {
            yield return DX.ScreenFlip();
            yield return DX.ProcessMessage();
            yield return DX.ClearDrawScreen();
        }

        public static bool Init()
        {
            return InitalWork().All(x => x == DX_SUCCESS);
        }

        public static bool Fina()
        {
            return FinalWork().All(x => x == DX_SUCCESS);
        }

        public static bool Refresh()
        {
            return RefWork().All(x => x == DX_SUCCESS);
        }
    }
}
