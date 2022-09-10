using System;
using System.Collections.Generic;

namespace MentalHealthAnalysis.Neural
{
    public class Neuron
    {
        public List<double> WeightList { get; }
        public List<double> InputsList { get; }

        public NeuronType Type { get; }

        public double Output { get; set; }
        public double Delta { get; set; }

        public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
        {
            Type = type;

            WeightList = new List<double>();
            InputsList = new List<double>();

            InitWeigtsRandomVaue(inputCount);
        }

        private void InitWeigtsRandomVaue(int inputCount)
        {
            Random random = new Random();

            for (int i = 0; i < inputCount; i++)
            {
                WeightList.Add(random.NextDouble());

                InputsList.Add(0);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                InputsList[i] = inputs[i];
            }

            double sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * WeightList[i];
            }

            if (Type != NeuronType.Input)
            {
                Output = Sigmoid(sum);
            }
            else
            {
                Output = sum;
            }

            return Output;
        }

        private double Sigmoid(double x)
        {
            double result = 1.0 / (1.0 + Math.Pow(Math.E, -x));

            return result;
        }

        private double SigmoidDX(double x)
        {
            double sigmoid = Sigmoid(x);
            double result = sigmoid / (1 - sigmoid);

            return result;
        }

        public override string ToString()
        {
            return Output.ToString();
        }

        public void SetWeigts(params double[] weights)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                WeightList[i] = weights[i];
            }
        }

        public void Learn(double error, double learningRate)
        {
            if (Type == NeuronType.Input)
            {
                return;
            }

            Delta = error * SigmoidDX(Output);

            for (int i = 0; i < WeightList.Count; i++)
            {
                double weight = WeightList[i];
                double input = InputsList[i];

                double newWeigth = weight - (input * Delta * learningRate);

                WeightList[i] = newWeigth;
            }
        }
    }
}
