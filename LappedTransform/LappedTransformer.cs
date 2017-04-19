using System;
using System.Collections.Generic;

namespace LappedTransform
{
    public class LappedTransformer
    {
        private readonly double[] _df;
        private readonly int _partLength;
        private readonly Func<double[], int, double[]> _partTransformer;
        public LappedTransformer(double[] df, int partLength, Func<double[], int, double[]> partTransformer)
        {
            _df = df;
            _partLength = partLength;
            _partTransformer = partTransformer;
        }

        public void Run(int partTransformerOrder)
        {
            var step = _partLength / 4;
            var parts = _df.GetOverlappingParts(_partLength, step);

            foreach (var part in parts)
            {
                var transformed = _partTransformer(part, partTransformerOrder);
            }

        }

        
    }
}