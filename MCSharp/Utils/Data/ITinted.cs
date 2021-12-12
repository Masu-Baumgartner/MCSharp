using System;
using System.Collections.Generic;
using System.Text;

namespace MCSharp.Utils.Data
{
    public interface ITinted
    {
        TintType TintType { get; }
        Color TintColor { get; }
    }
}
