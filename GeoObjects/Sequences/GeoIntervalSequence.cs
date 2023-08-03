using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.Sequences
{
    public class GeoIntervalSequence : GeoSequence
    {
        double start, end;
        public GeoIntervalSequence(double Start = 0, double End = 0)
        {
            start = Start;
            end = End;
        }

 

        public override IEnumerable<GeoObject> GetSequence()
        {
            double element = start;
            while (element < end)
                yield return new GeoNumber(element++);
        }

        public override GeoSequence GetSubsequence(int index)
        {
            if ((start + index) > end)
                return new GeoFiniteSequence(new List<GeoObject>());
            return new GeoIntervalSequence(start + index, end);
        }

        public override bool IsEmpty()
        {
            return false;
        }



    }
}
