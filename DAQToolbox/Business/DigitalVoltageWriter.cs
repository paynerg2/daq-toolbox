using NationalInstruments.DAQmx;
using System;

namespace DAQToolbox.Business
{
    public class DigitalVoltageWriter : Channel
    {
        #region Fields
        protected DigitalSingleChannelWriter _writer;
        public bool AutoStart { get; set; }
        public bool OutputValue { get; set; }

        #endregion

        public DigitalVoltageWriter(string physicalChannelName)
        {
        }

        #region Public Methods
        public override void TryInitialize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PhysicalChannelName))
                {
                    throw new ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL, nameof(PhysicalChannelName));
                }
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

        public override void Start()
        {
            if (!IsInitialized) return;
            try
            {
                WriteDigitalSingleValueOutput(OutputValue);
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
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
