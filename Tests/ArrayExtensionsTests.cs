using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LappedTransform;
using NUnit.Framework;
using static System.Linq.Enumerable;

namespace Tests
{
    public class ArrayExtensionsTests
    {
        [Test]
        public void GetPartsTest()
        {
            // Arrange
            var arr = Range(1, 200).ToArray();
            // Act
            var parts = arr.GetOverlappingParts(100, 50).ToArray();
            // Assert
            Assert.AreEqual(3, parts.Length);
            Assert.That(parts.All(p => p.Length == 100), "All parts should have length equals to 100");
            Assert.AreEqual(200, parts.SelectMany(p => p).Distinct().Count(), "All numbers are included");
            Assert.AreEqual(new[] { 1, 51, 101 }, parts.Select(p => p[0]).ToArray());
        }

        [Test]
        public void SliceCenterTest()
        {
            var arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var sliced = arr.SliceCenter(2);
            Assert.AreEqual(new[] { 3, 4, 5, 6 }, sliced);
        }

        [Test]
        public void RemoveOverlapsTest()
        {
            // 1 2 3 4 3 + +
            //       + + 3 4 3 + + 
            //             + + 3 4 3 + + 
            //                   + + 3 4 3 2 1 
            // -------------------------------
            // 1 2 3 4 3 3 4 3 3 4 3 3 4 3 2 1
            var list = new List<int[]>()
            {
                new[] {1, 2, 3, 4, 3, 2, 1},
                new[] {1, 2, 3, 4, 3, 2, 1},
                new[] {1, 2, 3, 4, 3, 2, 1},
                new[] {1, 2, 3, 4, 3, 2, 1}
            };
            var cleaned = list.RemoveOverlaps(3).ToList();
            Assert.AreEqual(4, cleaned.Count);
            Assert.AreEqual(new[] { 1, 2, 3, 4, 3 }, cleaned[0]);
            for (int i = 1; i < cleaned.Count-1; i++)
            {
                Assert.AreEqual(new[] { 3, 4, 3 }, cleaned[i]);
            }
            Assert.AreEqual(new[] { 1, 2, 3, 4, 3 }, cleaned[cleaned.Count - 1]);
        }

        [Test]
        public void RemoveOverlapsOneTest()
        {
            // 1 2 3 4 3 + +
            //       + + 3 4 3 + + 
            //             + + 3 4 3 + + 
            //                   + + 3 4 3 2 1 
            // -------------------------------
            // 1 2 3 4 3 3 4 3 3 4 3 3 4 3 2 1
            var list = new List<int[]>()
            {
                new[] {1, 2, 3, 4, 3, 2, 1},
            };
            var cleaned = list.RemoveOverlaps(3).ToList();
            Assert.AreEqual(list.Count, cleaned.Count);
            Assert.AreEqual(new[] { 1, 2, 3, 4, 3, 2, 1 }, cleaned[0]);
        }
    }
}
