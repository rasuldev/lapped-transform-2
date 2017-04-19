using System;
using System.Collections.Generic;
using System.Linq;

namespace LappedTransform
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T[]> GetOverlappingParts<T>(this T[] arr, int partLength, int step)
        {
            var startIndex = 0;
            while (startIndex + partLength <= arr.Length)
            {
                var part = new T[partLength];
                Array.Copy(arr, startIndex, part, 0, partLength);
                startIndex += step;
                yield return part;
            }
        }

        public static T[] Slice<T>(this T[] source, int index, int length)
        {
            T[] slice = new T[length];
            Array.Copy(source, index, slice, 0, length);
            return slice;
        }

        public static T[] SliceCenter<T>(this T[] source, int trimSize)
        {
            return source.Slice(trimSize, source.Length - 2 * trimSize);
        }

        /// <summary>
        /// Given a list of arrays (parts) and a shift (<paramref name="step"/>) of that arrays relative to each other,
        /// we remove from each array overlapping parts.
        /// For example, if overlapping is 4 items we remove two items from the ends of parts:
        /// [--------------++]
        ///             [++------------++]
        ///                         [++--------------]
        /// + is removed item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parts">All parts should have the same length</param>
        /// <param name="step">Part length - Step must be even</param>
        /// <returns></returns>
        public static IEnumerable<T[]> RemoveOverlaps<T>(this IEnumerable<T[]> parts, int step)
        {
            var iterator = parts.GetEnumerator();
            if (!iterator.MoveNext())
                // we have empty collection
                yield break;

            //if (step % 2 != 0)
            //    throw new ArgumentException("Step should be even");
            var firstPart = iterator.Current;
            var partLength = firstPart.Length;
            var overlap = partLength - step;
            if (overlap % 2 != 0)
                throw new ArgumentException("Overlap must be even (overlap = part length - step)");

            if (!iterator.MoveNext())
            {
                // when parts count is one return this single part unchanged
                yield return firstPart;
                yield break;
            }
            yield return firstPart.Slice(0, partLength - overlap / 2);
            
            while (true)
            {
                var part = iterator.Current;
                if (iterator.MoveNext())
                    yield return part.Slice(overlap / 2, partLength - overlap);
                else
                {
                    // last part
                    yield return part.Slice(overlap / 2, partLength - overlap / 2);
                    yield break;
                }
            }
        }

        public static IEnumerable<T[]> RemoveOverlaps<T>(this IList<T[]> parts, int step)
        {
            if (!parts.Any())
                yield break;
            if (parts.Count == 1)
            {
                yield return parts[0];
                yield break;
            }
            var partLength = parts[0].Length;
            var overlap = partLength - step;
            if (overlap % 2 != 0)
                throw new ArgumentException("Overlap must be even (overlap = part length - step)");

            yield return parts[0].Slice(0, partLength - overlap / 2);
            for (int i = 1; i < parts.Count-1; i++)
            {
                yield return parts[i].Slice(overlap / 2, partLength - overlap);
            }
            yield return parts[parts.Count-1].Slice(0, partLength - overlap / 2);
        }

    }
}