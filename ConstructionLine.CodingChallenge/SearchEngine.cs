using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly List<ColorCount> _colorCount;
        private readonly List<SizeCount> _sizeCount;
        private readonly List<Shirt> _resultShirt;

        private readonly Dictionary<Size, List<Shirt>> _sizes;
        private readonly Dictionary<Color, List<Shirt>> _colors;
        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            // TODO: search logic goes here.
            _colorCount = new List<ColorCount>();
            _sizeCount = new List<SizeCount>();
            _resultShirt = new List<Shirt>();

            _sizes = _shirts.GroupBy(c => c.Size).ToDictionary(c => c.Key, x => x.ToList());
            _colors = _shirts.GroupBy(c => c.Color).ToDictionary(c => c.Key, x => x.ToList());
        }


        public SearchResults Search(SearchOptions options)
        {
            if (options.Colors.Count > 0 && options.Sizes.Count == 0)
            {
                options.Colors.ForEach((color) =>
                {
                    _resultShirt.AddRange(_colors[color].ToArray());
                });
            }
            else if (options.Sizes.Count > 0 && options.Colors.Count == 0)
            {
                options.Sizes.ForEach((size) =>
                {
                    _resultShirt.AddRange(_sizes[size].ToArray());
                });
            }
            else
            {
                options.Colors.ForEach((color) =>
                {
                    options.Sizes.ForEach((size) =>
                    {
                         _resultShirt.AddRange(_sizes[size].Where(c => c.Color == color).ToArray());
                    });
                });
            }

            Size.All.ForEach((size) => 
            {
                _sizeCount.Add(new SizeCount { Count = _resultShirt.Where(c => c.Size == size)?.Distinct()?.Count() ?? 0, Size = size });
            });

            Color.All.ForEach((color) =>
            {
                _colorCount.Add(new ColorCount { Count = _resultShirt.Where(c => c.Color == color)?.Distinct()?.Count() ?? 0, Color = color });
            });

            return new SearchResults
            {
                ColorCounts = _colorCount,
                SizeCounts = _sizeCount,
                Shirts = _resultShirt
            };
        }
    }
}