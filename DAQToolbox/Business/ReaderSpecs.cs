using NationalInstruments.DAQmx;

namespace DAQToolbox.Business
{
    public class ReaderSpecs
    {
        public SymmetricVoltageRange Range { get; set; }
        public TimingConfig TimingConfig { get; set; }
        public AITerminalConfiguration TerminalConfig { get; set; } 
    }
}
