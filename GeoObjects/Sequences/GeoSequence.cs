using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.Sequences
{
    public abstract class GeoSequence : GeoObject
    {
        public abstract bool IsEmpty();
        abstract public IEnumerable<GeoObject> GetSequence();
        abstract public GeoSequence GetSubsequence(int index);
    }
}
