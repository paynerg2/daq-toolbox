using DAQToolbox.Business;
using System;

namespace DAQToolbox.Data
{
    public class Reading : BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public double[] Data { get; set; }
        public ReaderSpecs ReaderSpecs { get; set; }
        public string Label { get; set; }

        public Reading(DateTime timestamp, double[] data, string label, ReaderSpecs readerSpecs)
        {
            Timestamp = timestamp;
            Data = data;
            Label = label;
            ReaderSpecs = readerSpecs;
        }
    }
}
