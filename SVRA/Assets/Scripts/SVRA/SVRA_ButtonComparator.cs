using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct SVRA_ButtonComparator : IEqualityComparer<SVRA_ButtonAssistant>
{
    public bool Equals(SVRA_ButtonAssistant x, SVRA_ButtonAssistant y)
    {
        return x == y;
    }

    public int GetHashCode(SVRA_ButtonAssistant obj)
    {
        return (int)obj;
    }
}
