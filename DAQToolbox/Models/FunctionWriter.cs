using DAQToolbox.Business;
using DAQToolbox.Models.Interfaces;
using NationalInstruments.DAQmx;
using System;

namespace DAQToolbox.Models
{
    public class FunctionWriter : IVoltageWriter
    {
        protected readonly NationalInstruments.DAQmx.Task _daqTask = new Task();
        protected AnalogSingleChannelWriter _writer;

        private readonly int minimumValue = -10;
        private readonly int maximumValue = 10;
        private readonly AOVoltageUnits units = AOVoltageUnits.Volts;

        public bool AutoStart { get; set; }
        public string PhysicalChannelName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsInitialized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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

        public void TryInitialize()
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

        public void Write()
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
            //Log exception
            Stop();
        }
    }
}
