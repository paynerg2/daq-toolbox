using DAQToolbox.Models.Interfaces;
using NationalInstruments.DAQmx;

namespace DAQToolbox.Models
{
    public class AnalogVoltageWriter : IVoltageWriter<double>
    {
        #region Fields
        private readonly NationalInstruments.DAQmx.Task _daqTask = new Task();
        private readonly string physicalChannelName;

        private readonly int minimumValue = -10;
        private readonly int maximumValue = 10;
        private readonly AOVoltageUnits units = AOVoltageUnits.Volts;

        public bool AutoStart { get; set; } = true;
        #endregion

        // TODO: Inject the eventual logging interface through ctor
        public AnalogVoltageWriter(string physicalChannelName)
        {
            if (string.IsNullOrWhiteSpace(physicalChannelName))
            {
                throw new System.ArgumentException("message", nameof(physicalChannelName));
            }

            this.physicalChannelName = physicalChannelName;
        }

        #region Public Methods
        public void Initialize()
        {
            try
            {
                _daqTask.AOChannels.CreateVoltageChannel(physicalChannelName, nameToAssignChannel: "",
                minimumValue, maximumValue, units);
                _daqTask.Control(TaskAction.Verify);
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
        }

        public void Write(double voltage)
        {
            try
            {
                var stream = _daqTask.Stream;
                if (stream == null) return;

                var writer = new AnalogSingleChannelWriter(stream);
                writer.WriteSingleSample(AutoStart, voltage);
            }
            catch(DaqException ex)
            {
                Stop(ex);
            }
        }

        public void Stop()
        {
            if(_daqTask != null)
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
        #endregion
    }
}
