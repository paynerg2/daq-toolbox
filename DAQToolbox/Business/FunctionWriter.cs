using NationalInstruments.DAQmx;
using System;

namespace DAQToolbox.Business
{
    public class FunctionWriter : Channel
    {
        protected AnalogSingleChannelWriter _writer;

        private readonly int minimumValue = -10;
        private readonly int maximumValue = 10;
        private readonly AOVoltageUnits units = AOVoltageUnits.Volts;

        public bool AutoStart { get; set; }
        public double[] OutputValues { get; set; }

        public FunctionWriter(string physicalChannelName)
        {
            if (string.IsNullOrWhiteSpace(physicalChannelName))
            {
                throw new ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL,
                    nameof(physicalChannelName));
            }

            PhysicalChannelName = physicalChannelName;
        }

        public override void TryInitialize()
        {
            try
            {
                _daqTask.AOChannels.CreateVoltageChannel(PhysicalChannelName, nameToAssignChannel: PhysicalChannelName,
                    minimumValue, maximumValue, units);
                DaqStream stream = _daqTask.Stream ?? throw new DaqException(Constants.ErrorMessages.INVALID_STREAM);

                _writer = new AnalogSingleChannelWriter(stream);
                this.IsInitialized = true;
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
        }

        public override void Start()
        {
            if (!IsInitialized) TryInitialize();
            try
            {
                WriteAnalogFunction(OutputValues);
            }
            catch (DaqException ex)
            {
                Stop(ex);            }
        }

        private void WriteAnalogFunction(double[] outputValues)
        {
            _writer.WriteMultiSample(AutoStart, outputValues);
        }
    }
}
