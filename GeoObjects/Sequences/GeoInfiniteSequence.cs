using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.Sequences
{
    public class GeoInfiniteSequence:GeoSequence
    {
        double start;
        public GeoInfiniteSequence(double start)
        {
            this.start = start;
        }

 

        public override IEnumerable<GeoObject> GetSequence()
        {
            while(true)
                yield return new GeoNumber( start++);
        }

        public override GeoSequence GetSubsequence(int index)
        {
            
            return new GeoInfiniteSequence(start + index);
        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}
