using NationalInstruments.DAQmx;

namespace DAQToolbox.Models.Types
{
    public class TimingConfig
    {
        public int SampleRate { get; set; }
        public int SampleSize { get; set; }
        public SampleClockActiveEdge ActiveEdge { get; set; }
    }
}
