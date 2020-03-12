using DAQToolbox.Business;
using DAQToolbox.Models.Interfaces;
using NationalInstruments.DAQmx;
using System;

namespace DAQToolbox.Models
{
    public class DigitalVoltageWriter : IVoltageWriter
    {
        #region Fields
        protected readonly NationalInstruments.DAQmx.Task _daqTask = new Task();
        protected DigitalSingleChannelWriter _writer;
        public bool AutoStart { get; set; }
        public string PhysicalChannelName { get; set; }
        public bool IsInitialized { get; set; } = false;
        public bool OutputValue { get; set; }

        #endregion

        public DigitalVoltageWriter(string physicalChannelName)
        {
            if (string.IsNullOrWhiteSpace(physicalChannelName))
            {
                throw new ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL, nameof(physicalChannelName));
            }

            this.PhysicalChannelName = physicalChannelName;
        }

        #region Public Methods
        public void TryInitialize()
        {
            try
            {
                _daqTask.DOChannels.CreateChannel(PhysicalChannelName, nameToAssign: PhysicalChannelName,
                        ChannelLineGrouping.OneChannelForEachLine);
                DaqStream stream = _daqTask.Stream ?? throw new DaqException(Constants.ErrorMessages.INVALID_STREAM);

                _writer = new DigitalSingleChannelWriter(stream);
                this.IsInitialized = true;
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
        }

        public void Write()
        {
            if (!IsInitialized) TryInitialize();
            try
            {
                WriteDigitalSingleValueOutput(OutputValue);
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
        }

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
        #endregion

        #region Private Methods
        private void WriteDigitalSingleValueOutput(bool outputValue)
        {
            _writer.WriteSingleSampleSingleLine(AutoStart, outputValue);
        }
        #endregion
    }
}
