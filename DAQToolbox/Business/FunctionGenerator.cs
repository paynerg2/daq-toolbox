using System;
using System.Linq;

namespace DAQToolbox.Business
{
    public class FunctionGenerator
    {
        protected Buffer buffer = new Buffer();
        protected FunctionSpecs FunctionSpecs { get; set; }

        public double[] GetWaveform()
        {
            int bufferSize = buffer.CalculateBufferSize(FunctionSpecs.Frequency);

            double[] waveformData = FunctionSpecs.WaveformType switch
            {
                Waveform.Sine     => GenerateSineWave(bufferSize),
                Waveform.Square   => GenerateSquareWave(bufferSize),
                Waveform.Triangle => GenerateTriangleWave(bufferSize),
                _                 => throw new ArgumentException(Constants.ErrorMessages.INVALID_WAVEFORM_TYPE)
            };

            return ClipToTenVolts(waveformData);
        }

        private double[] GenerateSineWave(int bufferSize)
        {
            double[] sineWave = new double[bufferSize];

            // Each subsequent array index represents a point on the time axis with delta = 1 / bufferSize.
            // Buffer size is directly dependent on sample rate because the device's internal clock will output values at this rate.
            double dt = 1 / (double)bufferSize;

            //Generate sinewave data at each point
            for (int i = 0; i < bufferSize; i++)
            {
                sineWave[i] = FunctionSpecs.Amplitude * Math.Sin(2 * Math.PI * FunctionSpecs.Frequency * (double)i * dt) + FunctionSpecs.Offset;
            }

            return sineWave;
        }

        private double[] GenerateSquareWave(int bufferSize)
        {
            //Match the size of the output data array
            double[] squareWave = new double[bufferSize];

            int lowtime = (int)(bufferSize * FunctionSpecs.DutyCycle);
            for (int i = 0; i < lowtime; i++)
            {
                //Output HIGH for the high-time
                squareWave[i] = FunctionSpecs.Amplitude + FunctionSpecs.Offset;
            }
            for (int i = lowtime; i < bufferSize; i++)
            {
                //Output LOW for the low-time
                squareWave[i] = -FunctionSpecs.Amplitude + FunctionSpecs.Offset;
            }

            return squareWave;
        }

        private double[] GenerateTriangleWave(int bufferSize)
        {
            double[] triangleWave = new double[bufferSize];

            //Period is based on the number of samples, not the actual frequency
            double period = (double)bufferSize;
            int lowtime = (int)(bufferSize * FunctionSpecs.DutyCycle);

            //Note: Sawtooth is equivalent to a triangle wave with 100% duty cycle 
            double initialValue = FunctionSpecs.Amplitude + FunctionSpecs.Offset;
            for (int i = 0; i < lowtime; i++)
            {
                triangleWave[i] = 2 * FunctionSpecs.Amplitude * ((double)i) / (FunctionSpecs.DutyCycle * period) - initialValue;
            }

            for (int i = lowtime; i < bufferSize; i++)
            {
                triangleWave[i] = -2 * FunctionSpecs.Amplitude * ((double)i - (double)lowtime) / ((1 - FunctionSpecs.DutyCycle) * period) + initialValue;
            }

            return triangleWave;
        }

        private double Clip(double element)
        {
            if (element > 10)
            {
                element = 10;
            }
            else if (element < -10)
            {
                element = -10;
            }

            return element;
        }

        private double[] ClipToTenVolts(double[] data)
        {
            double[] result = new double[data.Length];
            result = data.Select(dataPoint => Clip(dataPoint)).ToArray();
            return result;
        }
    }
}
