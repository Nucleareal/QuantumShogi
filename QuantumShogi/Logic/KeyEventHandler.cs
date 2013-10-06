using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantumShogi.Logic
{
    public delegate void KeyEventHandler(int KeyCode);

    public delegate void MousePointEventHandler(int x, int y);

    public delegate void MouseClickEventHandler(int Code);
}
