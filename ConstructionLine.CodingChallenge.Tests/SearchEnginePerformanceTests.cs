using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConstructionLine.CodingChallenge.Tests.SampleData;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEnginePerformanceTests : SearchEngineTestsBase
    {
        private List<Shirt> _shirts;
        private SearchEngine _searchEngine;

        [SetUp]
        public void Setup()
        {
            
            var dataBuilder = new SampleDataBuilder(50000);

            _shirts = dataBuilder.CreateShirts();

            _searchEngine = new SearchEngine(_shirts);
        }


        [Test]
        public void Search_Should_Return_Valid_Response()
        {
            var sw = new Stopwatch();
            sw.Start();

            var options = new SearchOptions
            {
                Colors = new List<Color> { Color.Red },
                Sizes = new List<Size> { Size.Small }
            };


            var results = _searchEngine.Search(options);

            sw.Stop();
            Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

            Assert.LessOrEqual(sw.ElapsedMilliseconds,100);
            AssertResults(results.Shirts, options);
            AssertSizeCounts(_shirts, options, results.SizeCounts);
            AssertColorCounts(_shirts, options, results.ColorCounts);
        }

        [Test]
        public void Search_Should_ThrowsArgumentException_When_SearchOptions_Null()
        {
            SearchOptions options = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _searchEngine.Search(options);
            });
        }

        [Test]
        public void Search_Should_ThrowsArgumentException_When_SearchOptions_Colour_Null()
        {
            var options = new SearchOptions
            {
                Colors = null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _searchEngine.Search(options);
            });
        }

        [Test]
        public void Search_Should_ThrowsArgumentException_When_SearchOptions_Size_Null()
        {
            var options = new SearchOptions
            {
                Sizes = null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _searchEngine.Search(options);
            });
        }
    }
}
