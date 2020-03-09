namespace DAQToolbox.Models.Interfaces
{
    public interface IVoltageWriter<T>
    {
        void Initialize();
        void Write(T voltage);
    }
}
