namespace DAQToolbox.Business
{
    public static class Constants
    {
        public static class ErrorMessages
        {
            public const string INVALID_STREAM = "Invalid stream";
            public const string INVALID_CHANNEL = "Channel name not recognized";
            public const string INVALID_WAVEFORM_TYPE = "Selected waveform type is not supported";
            public const string UNSUPPORTED_VOLTAGE_RANGE = "Voltage range not supported by the device, please input an accepted range.";
        }

        public static class DeviceProperties
        {
            public const int MAXIMUM_BUFFER_SIZE = 8191;
            public static double[] ALLOWED_MAX_VOLTAGES = { 10, 5, 1, 0.2 };
        }
    }
}
