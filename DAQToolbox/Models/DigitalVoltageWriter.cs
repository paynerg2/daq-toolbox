using DAQToolbox.Models.Interfaces;
using NationalInstruments.DAQmx;
using System;

namespace DAQToolbox.Models
{
    public class DigitalVoltageWriter : IVoltageWriter<bool>
    {
        #region Fields
        private readonly NationalInstruments.DAQmx.Task _daqTask = new Task();

        private readonly string physicalChannelName;

        public bool AutoStart { get; set; }

        #endregion

        public DigitalVoltageWriter(string physicalChannelName)
        {
            if (string.IsNullOrWhiteSpace(physicalChannelName))
            {
                throw new ArgumentException("Channel name not recognized", nameof(physicalChannelName));
            }

            this.physicalChannelName = physicalChannelName;
        }

        #region Public Methods
        public void Initialize()
        {
            try
            {
                _daqTask.DOChannels.CreateChannel(physicalChannelName, nameToAssign: "",
                        ChannelLineGrouping.OneChannelForEachLine);
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
        }

        public void Write(bool outputValue)
        {
            try
            {
                WriteDigitalSingleValueOutput(outputValue);
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
        }

        public void Stop()
        {
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
        #endregion

        #region Private Methods
        private void WriteDigitalSingleValueOutput(bool outputValue)
        {
            DaqStream stream = _daqTask.Stream;
            if (stream == null) return;

            var writer = new DigitalSingleChannelWriter(stream);
            writer.WriteSingleSampleSingleLine(AutoStart, outputValue);
        }
        #endregion
    }
}
