using System;
using System.Collections.Generic;
using System.Text;

namespace FileMerger
{
    public class MergeResolution
    {
        public long Offset { get; set; }
        public List<FrequencyPair> MergeChoices { get; set; }
    }
}
