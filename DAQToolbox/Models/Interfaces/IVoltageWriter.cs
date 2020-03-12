namespace DAQToolbox.Models.Interfaces
{
    public interface IVoltageWriter
    {
        public string PhysicalChannelName { get; set; }
        public bool IsInitialized { get; set; }
        void TryInitialize();
        void Write();
        void Stop();
    }
}
