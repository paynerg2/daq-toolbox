using DAQToolbox.Business;
using DAQToolbox.Models.Interfaces;
using DAQToolbox.Models.Types;
using System;

namespace DAQToolbox.Models
{
    public class Channel
    {
        protected IVoltageWriter _writer;
        public Output OutputType { get; set; }
        public string PhysicalChannelName { get; set; }

        public Channel(Output outputType)
        {
            OutputType = outputType;
            _writer = WriterFactory.NewVoltageWriter(OutputType, PhysicalChannelName);
        }

        public void Start() => _writer.Write();

        public void Stop() => _writer.Stop();

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
