using DAQToolbox.Models.Interfaces;
using NationalInstruments.DAQmx;
using System.Collections.Generic;

namespace DAQToolbox.Models
{
    public class DigitalOutputLineContainer : IDaqIO<bool>
    {
        public string[] Channels { get; set; }
        public List<IVoltageWriter<bool>> Writers { get; set; }

        public DigitalOutputLineContainer()
        {
            this.Channels = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External);
            GetWriters();

        }

        private void GetWriters()
        {
            foreach (var channel in Channels)
            {
                var writer = new DigitalVoltageWriter(channel);
                Writers.Add(writer);
            }
        }

        public void InitializeChannels()
        {
            if(Writers.Count > 0)
            {
                Writers.ForEach(writer => writer.Initialize());
            }
        }

        public void InitializeSingleChannel(string channelName)
        {
            var writer = Writers.Find(writer => writer.PhysicalChannelName == channelName);
            if(writer != null)
            {
                writer.Initialize();
            }
        }

        public void StartSingleChannelOutput(string channelName, bool outputValue)
        {
            var writer = Writers.Find(writer => writer.PhysicalChannelName == channelName);
            StartSingleChannelOutput(writer, outputValue);
        }

        public void StartSingleChannelOutput(IVoltageWriter<bool> writer, bool outputValue)
        {
            if (writer.IsInitialized)
            {
                writer.Write(outputValue);
            }
            else
            {
                writer.Initialize();
                writer.Write(outputValue);
            }
        }
    }
}
