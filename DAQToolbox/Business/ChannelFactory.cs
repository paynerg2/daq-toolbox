using System;

namespace DAQToolbox.Business
{
    public static class ChannelFactory
    {
        public static Channel NewChannel(ChannelType channelType, string physicalChannel) => channelType switch
        {
            ChannelType.AnalogVoltageWriter  => new AnalogVoltageWriter(physicalChannel),
            ChannelType.DigitalVoltageWriter => new DigitalVoltageWriter(physicalChannel),
            ChannelType.FunctionWriter       => new FunctionWriter(physicalChannel),
            _                                => throw new ArgumentException(Constants.ErrorMessages.INVALID_CHANNEL, nameof(physicalChannel))
        };
    }
}
