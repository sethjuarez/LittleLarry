using numl.Model;
using numl.Supervised.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            var descriptor = Descriptor.For<Tennis>()
                                       .With(t => t.Outlook)
                                       .With(t => t.Temperature)
                                       .With(t => t.Windy)
                                       .With(t => t.Humidity)
                                       .Learn(t => t.Play);

            Console.WriteLine(descriptor);

            var generator = new DecisionTreeGenerator(descriptor);
            var model = generator.Generate(Tennis.GetData());
            Console.WriteLine($"Model:\n{model}");

            var tn = new Tennis { Outlook = Outlook.Sunny,
                                 Temperature = Temperature.Cool,
                                 Humidity = Humidity.High,
                                 Windy = true };

            var tennis = model.PredictValue(tn);

            Console.WriteLine($"Play? {tennis}");

            Console.ReadKey();
        }
    }
}
