using NationalInstruments.DAQmx;

namespace DAQToolbox.Business
{
    public abstract class Channel
    {
        protected readonly NationalInstruments.DAQmx.Task _daqTask = new Task();
        public string PhysicalChannelName { get; set; }
        public bool IsInitialized { get; set; }

        public abstract void TryInitialize();

        public abstract void Start();

        public void Stop()
        {
            this.IsInitialized = false;
            if (_daqTask != null)
            {
                _daqTask.Stop();
                _daqTask.Dispose();
            }

        }

        public void Stop(DaqException ex)
        {
            //TODO: Log exception
            Stop();
        }

    }
}
