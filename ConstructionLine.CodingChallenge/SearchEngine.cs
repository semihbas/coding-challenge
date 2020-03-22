using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine : ISearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;          
        }


        public SearchResults Search(SearchOptions options)
        {
            if (options == null || options.Colors == null || options.Sizes == null)
                throw new ArgumentException("Please provide valid search options!",nameof(SearchOptions));


            var filteredShirts = _shirts.Where(s => options.Colors.Any(c => c == s.Color && options.Sizes.Any(x => x == s.Size))).ToList();

            var groupByColourInFilteredShirts = filteredShirts.GroupBy(p => p.Color,
                 p => p.Color.Name,
                (key, g) => new ColorCount {  Color= key, Count = g.Count() }).ToList();

            foreach (var missingColor in Color.All.Where(color => !groupByColourInFilteredShirts.Any(x => x.Color == color)))
                groupByColourInFilteredShirts.Add(new ColorCount { Color = missingColor, Count = 0 });


            var groupBySizeInFilteredShirts = filteredShirts.GroupBy(p => p.Size,
             p => p.Size.Name,
            (key, g) => new SizeCount { Size = key, Count = g.Count() }).ToList();


            foreach (var missingSize in Size.All.Where(size => !groupBySizeInFilteredShirts.Any(x => x.Size == size)))
                groupBySizeInFilteredShirts.Add(new SizeCount { Size = missingSize, Count = 0 });

            return new SearchResults
            {
                Shirts= filteredShirts,
                ColorCounts= groupByColourInFilteredShirts,
                SizeCounts= groupBySizeInFilteredShirts
            };
        }
    }
}