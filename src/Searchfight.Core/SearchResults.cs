using System;
using System.Collections.Generic;
using System.Text;

namespace Searchfight.Core
{
    public class SearchResult
    {
        public string Term { get; set; }
        public string Source { get; set; }
        public decimal Count { get; set; }
    }
}
