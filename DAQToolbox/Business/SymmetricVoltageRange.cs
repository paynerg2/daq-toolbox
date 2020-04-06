using System;

namespace DAQToolbox.Business
{
    public class SymmetricVoltageRange
    {
        /* Describes a symmetric voltage range about 0, in volts (V). */

        private double maximumValue;
        private double minimumValue;
        public double MaximumValue 
        { 
            get => maximumValue; 
            set {
                if (value < 0) throw new ArgumentOutOfRangeException(paramName: "MaximumValue", "Maximum Value cannot be a negative voltage");
                VerifyIncomingValue(value);
                maximumValue = value;
                MinimumValue = -1 * value;
            } 
        }

        public double MinimumValue 
        { 
            get => minimumValue; 
            set {
                if (value > 0) throw new ArgumentOutOfRangeException(paramName: "MinimumValue", "Minimum Value cannot be a positive voltage");
                VerifyIncomingValue(value);
                minimumValue = value;
                maximumValue = -1 * value;
            } 
        }

        private void VerifyIncomingValue(double value)
        {
            bool valueIsAllowed = Array.Exists(Constants.DeviceProperties.ALLOWED_MAX_VOLTAGES, v => v == Math.Abs(value));
            if (valueIsAllowed) return;
            throw new ArgumentException(Constants.ErrorMessages.UNSUPPORTED_VOLTAGE_RANGE);
        }
    }
}
