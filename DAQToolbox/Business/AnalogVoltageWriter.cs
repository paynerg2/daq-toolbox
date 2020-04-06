using NationalInstruments.DAQmx;

namespace DAQToolbox.Business
{
    public class AnalogVoltageWriter : Channel
    {
        #region Fields
        protected AnalogSingleChannelWriter _writer;

        private readonly int minimumValue = -10;
        private readonly int maximumValue = 10;
        private readonly AOVoltageUnits units = AOVoltageUnits.Volts;

        public double OutputValue { get; set; }
        #endregion

        // TODO: Inject the eventual logging interface through ctor
        public AnalogVoltageWriter(string physicalChannelName)
        {
            if (string.IsNullOrWhiteSpace(physicalChannelName))
            {
                throw new System.ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL, nameof(physicalChannelName));
            }
            this.PhysicalChannelName = physicalChannelName;
        }

        #region Public Methods
        public override void TryInitialize()
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(PhysicalChannelName))
                {
                    throw new System.ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL,
                        nameof(PhysicalChannelName));
                }

                _daqTask.AOChannels.CreateVoltageChannel(PhysicalChannelName, nameToAssignChannel: PhysicalChannelName, 
                    minimumValue, maximumValue, units);
                _daqTask.Control(TaskAction.Start);
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
            if (!IsInitialized) return;

            try
            {
                WriteAnalogSingleValueOutput(OutputValue);
            }
            catch (DaqException ex)
            {
                Stop(ex);
            }
        }

        #endregion

        #region Private Methods
        private void WriteAnalogSingleValueOutput(double outputValue)
        {
            _writer.WriteSingleSample(autoStart: true, outputValue);
        }
        #endregion
    }
}
