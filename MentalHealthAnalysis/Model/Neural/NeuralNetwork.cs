using System;
using System.Collections.Generic;
using System.Linq;

namespace MentalHealthAnalysis.Neural
{
    public class NeuralNetwork
    {
        public List<Layer> Layers { get; }

        public Topology Topology { get; }

        public NeuralNetwork(Topology topology)
        {
            Topology = topology;

            Layers = new List<Layer>();

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        public Neuron FeedForward(params double[] inputSignals)
        {
            SendSignalsToInputNeurons(inputSignals);

            FeedForwardAllLayersAfterInput();

            if (Topology.OutputCount == 1)
            {
                return Layers.Last().Neurons[0];
            }
            else
            {
                return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
            }
        }

        public double Learn(List<Tuple<double, double[]>> dataset, int epoch)
        {
            double error = 0.0;

            for (int i = 0; i < epoch; i++)
            {
                foreach (Tuple<double, double[]> data in dataset)
                {
                    error += Backpropagation(data.Item1, data.Item2);
                }
            }

            double result = error / epoch;

            return result;
        }

        private double Backpropagation(double exprected, params double[] inputs)
        {
            double actual = FeedForward(inputs).Output;
            double difference = actual - exprected;

            foreach (Neuron neuron in Layers.Last().Neurons)
            {
                neuron.Learn(difference, Topology.LearningRate);
            }

            for (int j = Layers.Count - 2; j >= 0; j--)
            {
                Layer lauer = Layers[j];
                Layer previousLayer = Layers[j + 1];

                for (int i = 0; i < lauer.NeuronCount; i++)
                {
                    Neuron neuron = lauer.Neurons[i];

                    for (int k = 0; k < previousLayer.NeuronCount; k++)
                    {
                        Neuron previousNeuron = previousLayer.Neurons[k];

                        double error = previousNeuron.WeightList[i] * previousNeuron.Delta;

                        neuron.Learn(error, Topology.LearningRate);
                    }
                }
            }

            double result = difference * difference;

            return result;
        }

        private void FeedForwardAllLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                Layer layer = Layers[i];

                List<double> previousLayerSignals = (List<double>)Layers[i - 1].GetSignals();

                foreach (Neuron neoron in layer.Neurons)
                {
                    neoron.FeedForward(previousLayerSignals);
                }
            }
        }

        public void SendSignalsToInputNeurons(params double[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                List<double> signal = new List<double>() { inputSignals[i] };
                Neuron neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }
        private void CreateOutputLayer()
        {
            List<Neuron> outputNeurons = new List<Neuron>();
            Layer lastLayer = Layers.Last();

            for (int i = 0; i < Topology.OutputCount; i++)
            {
                Neuron neuron = new Neuron(lastLayer.NeuronCount, NeuronType.Output);

                outputNeurons.Add(neuron);
            }

            Layer outputLayer = new Layer(outputNeurons, NeuronType.Output);

            Layers.Add(outputLayer);
        }

        private void CreateHiddenLayers()
        {
            for (int j = 0; j < Topology.HiddenLayers.Count; j++)
            {
                List<Neuron> hiddenNeurons = new List<Neuron>();
                Layer lastLayer = Layers.Last();

                for (int i = 0; i < Topology.HiddenLayers[j]; i++)
                {
                    Neuron neuron = new Neuron(lastLayer.NeuronCount);

                    hiddenNeurons.Add(neuron);
                }

                Layer hiddenLayer = new Layer(hiddenNeurons);

                Layers.Add(hiddenLayer);
            }
        }

        private void CreateInputLayer()
        {
            List<Neuron> inputNeurons = new List<Neuron>();

            for (int i = 0; i < Topology.InputCount; i++)
            {
                Neuron neuron = new Neuron(1, NeuronType.Input);

                inputNeurons.Add(neuron);
            }

            Layer inputLayer = new Layer(inputNeurons, NeuronType.Input);

            Layers.Add(inputLayer);
        }
    }
}
