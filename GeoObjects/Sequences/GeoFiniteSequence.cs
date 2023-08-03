using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoObjects.Sequences
{
    public class GeoFiniteSequence: GeoSequence
    {
        public List<GeoObject> Elements { get; private set; }
        public GeoFiniteSequence(List<GeoObject> elements)
        {
            Elements = elements;
            
        }

        public override bool IsEmpty()
        {
            return Elements.Count == 0;
        }

        
        public override IEnumerable<GeoObject> GetSequence()
        {
            return Elements;
        }

        public override GeoSequence GetSubsequence(int index)
        {
            List<GeoObject> subsequence = new List<GeoObject>();
            for (int i = index; i < Elements.Count; i++)
                subsequence.Add(Elements[i]);
            return new GeoFiniteSequence(subsequence);
        }

    
    }
}
