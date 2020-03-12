using DAQToolbox.Models;
using DAQToolbox.Models.Interfaces;
using DAQToolbox.Models.Types;
using System;

namespace DAQToolbox.Business
{
    public class WriterFactory
    {
        public static IVoltageWriter NewVoltageWriter(Output outputType, string physicalChannelName) => outputType switch
        {
            Output.DigitalLine => new DigitalVoltageWriter(physicalChannelName),
            Output.AnalogSingleSample => new AnalogVoltageWriter(physicalChannelName),
            Output.AnalogFunction => new FunctionWriter(physicalChannelName),
            _ => throw new ArgumentException("Invalid parameter", paramName: outputType.ToString())
        };
    }
}
