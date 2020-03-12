﻿using DAQToolbox.Business;
using DAQToolbox.Models.Interfaces;
using NationalInstruments.DAQmx;

namespace DAQToolbox.Models
{
    public class AnalogVoltageWriter : IVoltageWriter
    {
        #region Fields
        protected readonly NationalInstruments.DAQmx.Task _daqTask = new Task();
        protected AnalogSingleChannelWriter _writer;

        private readonly int minimumValue = -10;
        private readonly int maximumValue = 10;
        private readonly AOVoltageUnits units = AOVoltageUnits.Volts;

        public bool AutoStart { get; set; } = true;
        public string PhysicalChannelName { get; set; }
        public bool IsInitialized { get; set; } = false;
        public double OutputValue { get; set; }
        #endregion

        // TODO: Inject the eventual logging interface through ctor
        public AnalogVoltageWriter(string physicalChannelName)
        {
            if (string.IsNullOrWhiteSpace(physicalChannelName))
            {
                throw new System.ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL, 
                    nameof(physicalChannelName));
            }

            this.PhysicalChannelName = physicalChannelName;
        }

        #region Public Methods
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
                WriteAnalogSingleValueOutput(OutputValue);
            }
            catch (DaqException ex)
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

        #region Private Methods
        private void WriteAnalogSingleValueOutput(double outputValue)
        {
            _writer.WriteSingleSample(AutoStart, outputValue);
        }
        #endregion
    }
}
