namespace DAQToolbox.Business
{
    public class Buffer
    {
        private readonly int maximumBufferSize = Constants.DeviceProperties.MAXIMUM_BUFFER_SIZE;

        public int CalculateBufferSize(int frequency)
        {
            // While f > 109Hz, we have to use the maximum sample rate: 900 kS/s, so we
            // adjust the number of points in the waveform (worst case: 9 points for 100kHz)
            return frequency > 109 ? maximumBufferSize :
                                     frequency / maximumBufferSize;
        }
    }
}
