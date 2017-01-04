using numl;
using numl.Model;
using numl.Serialization;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Model
{
    public class MachineLearningService
    {
        private const string SPEEDMODEL = "SpeedModel";
        private const string TURNMODEL = "TurnModel";
        private IConnection _connection;
        public MachineLearningService(IConnection connection)
        {
            // register assembly for type information
            Register.Assembly(typeof(Data).GetTypeInfo().Assembly);

            _connection = connection;
            InitializeGenerators();
        }

        public IModel SpeedModel { get; private set; }

        public IModel TurnModel { get; private set; }

        public Generator SpeedGenerator { get; private set; }

        public Generator TurnGenerator { get; private set; }

        private void InitializeGenerators()
        {
            var s = Descriptor.For<Data>()
                            .With(d => d.Ain1)
                            .With(d => d.Ain2)
                            .With(d => d.Ain3)
                            .Learn(d => d.Forward);

            SpeedGenerator = new DecisionTreeGenerator(descriptor: s,
                                                      depth: 10,
                                                      width: 4,
                                                      hint: 0);

            var t = Descriptor.For<Data>()
                            .With(d => d.Ain1)
                            .With(d => d.Ain2)
                            .With(d => d.Ain3)
                            .Learn(d => d.Direction);

            TurnGenerator = new DecisionTreeGenerator(descriptor: t,
                                                      depth: 10,
                                                      width: 4,
                                                      hint: 0);

            SpeedModel = Load<DecisionTreeModel>(SPEEDMODEL);
            TurnModel = Load<DecisionTreeModel>(TURNMODEL);
        }

        public void Model()
        {
            var data = _connection.SQLiteConnection.Table<Data>()
                                                  .Where(d => d.Speed >= 0)
                                                  .OrderBy(d => d.Id)
                                                  .ToList();
            Model(data);
        }

        public void Model(IEnumerable<Data> data)
        {
            SpeedModel = CreateModel(data, SpeedGenerator, SPEEDMODEL);
            TurnModel = CreateModel(data, TurnGenerator, TURNMODEL);
        }

        public bool HasModel()
        {
            return SpeedModel != null && TurnModel != null;
        }


        public (double speed, double turn) Predict(Data data)
        {
            if (HasModel())
            {
                var speed = (Speed)SpeedModel.PredictValue(data);
                var turn = (Turn)TurnModel.PredictValue(data);
                return (Data.ForwardToSpeed(speed), Data.DirectionToTurn(turn));
            }
            else return (0, 0);
        }

        private IModel CreateModel(IEnumerable<Data> data, Generator generator, string name)
        {
            var model = generator.Generate(data);
            Save(model, name);
            return model;
        }

        private void Save(object model, string name)
        {
            var file = Path.Combine(_connection.DataPath, $"{name}.json");
            if (File.Exists(file)) File.Delete(file);

            using (var fs = new FileStream(file, FileMode.CreateNew))
            using (var f = new StreamWriter(fs))
                new JsonWriter(f).Write(model);
        }

        private T Load<T>(string name)
        {
            var file = Path.Combine(_connection.DataPath, $"{name}.json");
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open))
                using (var f = new StreamReader(fs))
                {
                    //try
                    //{
                        var val = new JsonReader(f).Read();
                        return (T)val;
                    //}
                    //catch(Exception e)
                    //{
                    //    return default(T);
                    //}
                }
            }
            else
                return default(T);
        }

    }
}
