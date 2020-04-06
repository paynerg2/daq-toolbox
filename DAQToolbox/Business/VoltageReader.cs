using NationalInstruments.DAQmx;

namespace DAQToolbox.Business
{
    public class VoltageReader : Channel
    {
        protected AnalogSingleChannelReader _reader;
        private readonly AIVoltageUnits units = AIVoltageUnits.Volts;

        public int NumberOfSamples { get; set; }

        public ReaderSpecs ReaderSpecs { get; set; }

        public double[] Data { get; set; }

        public VoltageReader(string physicalChannelName)
        {
            if (string.IsNullOrWhiteSpace(physicalChannelName))
            {
                throw new System.ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL, nameof(physicalChannelName));
            }

            PhysicalChannelName = physicalChannelName;
        }
        public override void TryInitialize()
        {
            try
            {
                _daqTask.AIChannels.CreateVoltageChannel(PhysicalChannelName, 
                    nameToAssignChannel: PhysicalChannelName,
                    ReaderSpecs.TerminalConfig, 
                    ReaderSpecs.Range.MinimumValue, 
                    ReaderSpecs.Range.MaximumValue, 
                    units);

                _daqTask.Timing.ConfigureSampleClock("", ReaderSpecs.TimingConfig.SampleRate, 
                    ReaderSpecs.TimingConfig.ActiveEdge, 
                    SampleQuantityMode.FiniteSamples, 
                    ReaderSpecs.TimingConfig.SampleSize);

                DaqStream stream = _daqTask.Stream ?? throw new DaqException(Constants.ErrorMessages.INVALID_STREAM);

                _reader = new AnalogSingleChannelReader(stream);
                this.IsInitialized = true;
            }
            catch (DaqException ex)
            {
                // Log Exception
                Stop(ex);
            }
        }

        public override void Start()
        {
            try
            {
                Data = _reader.ReadMultiSample(NumberOfSamples);
            }
            catch (DaqException ex)
            {
                // Log Exception
                this.IsInitialized = false;
                Stop(ex);
            }
        }
    }
}