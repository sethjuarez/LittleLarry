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
        private const string TURNMODEL = "TurnModel";
        private IConnection _connection;
        public MachineLearningService(IConnection connection)
        {
            // register assembly for type information
            Register.Assembly(typeof(Data).GetTypeInfo().Assembly);

            _connection = connection;
            InitializeGenerators();
        }

        public IModel TurnModel { get; private set; }

        public Generator TurnGenerator { get; private set; }

        private void InitializeGenerators()
        {
            var t = Descriptor.For<Data>()
                            .With(d => d.Ain1)
                            .With(d => d.Ain2)
                            .With(d => d.Ain3)
                            .Learn(d => d.Direction);

            TurnGenerator = new DecisionTreeGenerator(descriptor: t,
                                                      depth: 10,
                                                      width: 4,
                                                      hint: 0);

            TurnModel = Load<DecisionTreeModel>(TURNMODEL);
        }

        public void Model()
        {
            var turnData = _connection.SQLiteConnection.Table<Data>()
                                                  .Where(d => d.Turn != 0)
                                                  .ToList();
            var idleData = _connection.SQLiteConnection.Table<Data>()
                                                  .Where(d => d.Turn == 0)
                                                  .ToList();
            // balance data
            if(idleData.Count() > turnData.Count())
                idleData = idleData.Take(turnData.Count()).ToList();

            turnData.AddRange(idleData);
            Model(turnData.OrderBy(d => d.Id));
        }

        public void Model(IEnumerable<Data> data)
        {
            if (data.Count() > 0)
                TurnModel = CreateModel(data, TurnGenerator, TURNMODEL);
        }

        public bool HasModel()
        {
            return TurnModel != null;
        }


        public (double speed, double turn) Predict(Data data)
        {
            if (HasModel())
            {
                var turn = (Turn)TurnModel.PredictValue(data);
                return (0.4, Data.DirectionToTurn(turn));
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
                    var val = new JsonReader(f).Read();
                    return (T)val;
                }
            }
            else
                return default(T);
        }
    }
}
