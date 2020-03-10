namespace DAQToolbox.Models.Interfaces
{
    public interface IVoltageWriter<T>
    {
        public string PhysicalChannelName { get; set; }
        public bool IsInitialized { get; set; }
        void Initialize();
        void Write(T voltage);
    }
}
