using NationalInstruments.DAQmx;

namespace DAQToolbox.Business
{
    public class TimingConfig
    {
        public int SampleRate { get; set; }
        public int SampleSize { get; set; }
        public SampleClockActiveEdge ActiveEdge { get; set; }
    }
}
