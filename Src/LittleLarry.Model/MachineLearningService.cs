using numl;
using numl.Model;
using numl.Serialization;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Model
{
    public class MachineLearningService
    {
        private const string SPEEDMODEL = "SpeedModel";
        private const string TURNMODEL = "TurnModel";
        private DecisionTreeGenerator _speedGenerator;
        private DecisionTreeGenerator _turnGenerator;
        public MachineLearningService()
        {
            InitializeGenerators();
        }

        public IModel SpeedModel { get; private set; }

        public IModel TurnModel { get; private set; }

        private void InitializeGenerators()
        {
            var s = Descriptor.For<Data>()
                            .With(d => d.Ain1)
                            .With(d => d.Ain2)
                            .With(d => d.Ain3)
                            .Learn(d => d.Speed);

            _speedGenerator = new DecisionTreeGenerator(descriptor: s,
                                                      depth: 10,
                                                      width: 5,
                                                      hint: 0);

            var t = Descriptor.For<Data>()
                            .With(d => d.Ain1)
                            .With(d => d.Ain2)
                            .With(d => d.Ain3)
                            .Learn(d => d.Turn);

            _turnGenerator = new DecisionTreeGenerator(descriptor: t,
                                                      depth: 10,
                                                      width: 5,
                                                      hint: 0);

            SpeedModel = Load<IModel>(SPEEDMODEL);
            TurnModel = Load<IModel>(TURNMODEL);
        }

        public void Model(IEnumerable<Data> data)
        {
            SpeedModel = CreateModel(data, _speedGenerator, SPEEDMODEL);
            TurnModel = CreateModel(data, _turnGenerator, TURNMODEL);
        }

        public bool HasModel()
        {
            return SpeedModel == null || TurnModel == null;
        }
        public Data Predict(Data data)
        {
            if (HasModel())
            {
                data.Speed = 0;
                data.Turn = 0;
            }
            else
            {
                SpeedModel.Predict(data);
                TurnModel.Predict(data);
            }
            return data;
        }

        private string GetFilePath(string name)
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                                 $"{name}.json");
        }

        private IModel CreateModel(IEnumerable<Data> data, IGenerator generator, string name)
        {
            var learned = Learner.Learn(data, .8, 1000, generator);
            Save(learned.Model, name);
            Save(learned.Score, name + "Score");
            return learned.Model;
        }

        private void Save<T>(T model, string name)
        {
            var file = GetFilePath(name);
            if (File.Exists(file)) File.Delete(file);

            using (var fs = new FileStream(file, FileMode.CreateNew))
            using (var f = new StreamWriter(fs))
                new JsonWriter(f).Write(model);
        }

        private T Load<T>(string name)
        {
            var file = GetFilePath(name);
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open))
                using (var f = new StreamReader(fs))
                {
                    try
                    {
                        return (T)new JsonReader(f).Read();
                    }
                    catch
                    {
                        return default(T);
                    }
                }
            }
            else
                return default(T);
        }

    }
}
