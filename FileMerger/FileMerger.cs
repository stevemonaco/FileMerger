using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileMerger
{
    public class FileMerger
    {
        public List<MergeResolution> Resolutions { get; set; } = new List<MergeResolution>();
        private byte[] MergeBuffer;
        private long FilePosition;

        public void MergeFilesByMaxFrequency(List<FileStream> files, FileStream mergeOutput)
        {
            MergeBuffer = new byte[files.Count];
            var maxLength = files.Select(x => x.Length).Max();

            for(int i = 0; i < maxLength; i++)
            {
                var merged = MergeByteByMaxFrequency(files);
                FilePosition++;
                mergeOutput.WriteByte(merged);
            }
        }

        private byte MergeByteByMaxFrequency(List<FileStream> files)
        {
            var bufferPosition = 0;
            foreach (var file in files)
            {
                MergeBuffer[bufferPosition] = (byte)file.ReadByte();
                bufferPosition++;
            }

            var counts = from value in MergeBuffer
                         group value by value into countGroup
                         select new FrequencyPair
                         {
                             Value = countGroup.Key,
                             Count = countGroup.Count()
                         };

            var countList = counts.OrderByDescending(x => x.Count).ToList();

            if (countList.Count != 1)
            {
                var mr = new MergeResolution()
                {
                    Offset = FilePosition,
                    MergeChoices = countList
                };

                Resolutions.Add(mr);
            }

            return countList[0].Value;
        }
    }
}
