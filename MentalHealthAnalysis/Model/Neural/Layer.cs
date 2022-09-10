using System.Collections.Generic;

namespace MentalHealthAnalysis.Neural
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }

        public int NeuronCount => Neurons?.Count ?? 0;

        public NeuronType Type;

        public Layer(IEnumerable<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
            Neurons = (List<Neuron>)neurons;
            Type = type;
        }

        public IEnumerable<double> GetSignals()
        {
            List<double> result = new List<double>();

            foreach (Neuron neuron in Neurons)
            {
                result.Add(neuron.Output);
            }

            return result;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
