using LittleLarry.Model.Hardware;
using numl;
using numl.Model;
using numl.Serialization;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using numl.Supervised.NaiveBayes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LittleLarry.Model.Services
{
    public class MachineLearningService
    {
        public IModel TurnModel { get; private set; }
        public Generator TurnGenerator { get; private set; }

        private const string TURNMODEL = "TurnModel";
        private string _modelPath;
        public IDataService _dataService;

        public MachineLearningService(IDataService dataService)
        {
            // register assembly for type information
            Register.Assembly(typeof(Data).GetTypeInfo().Assembly);
            _dataService = dataService;
            _modelPath = Path.Combine(_dataService.DataPath, $"{TURNMODEL}.json");
            InitializeGenerators();
        }

        

        private void InitializeGenerators()
        {
            var t = Descriptor.For<Data>()
                                .With(d => d.Ain1)
                                .With(d => d.Ain2)
                                .With(d => d.Ain3)
                                .Learn(d => d.Direction);

            TurnGenerator = new DecisionTreeGenerator(descriptor: t, depth: 10, width: 4, hint: 0);
            
            if (File.Exists(_modelPath))
                TurnModel = JsonReader.Read<DecisionTreeModel>(_modelPath);
        }

        public void Model()
        {
            var data = _dataService.GetData();
            var turnData = data
                            .Where(d => d.Turn != 0)
                            .ToList();

            var idleData = data
                            .Where(d => d.Turn == 0)
                            .ToList();

            // balance data
            if (idleData.Count() > turnData.Count())
                idleData = idleData.Take(turnData.Count()).ToList();

            turnData.AddRange(idleData);
            Model(turnData.OrderBy(d => d.Id));
        }

        public void Model(IEnumerable<Data> data)
        {
            if (data.Count() > 0)
                TurnModel = CreateModel(data, TurnGenerator);
        }

        public bool HasModel()
        {
            return TurnModel != null;
        }

        public string GetModelString()
        {
            if (HasModel())
                return TurnModel.ToString();
            else
                return "N/A";
        }


        public (double speed, double turn) Predict(Data data)
        {
            if (HasModel())
            {
                var turn = (Turn)TurnModel.PredictValue(data);
                return (Motor.Speed, Data.DirectionToTurn(turn));
            }
            else return (0, 0);
        }

        private IModel CreateModel(IEnumerable<Data> data, Generator generator)
        {
            var model = generator.Generate(data);

            // save model
            if (File.Exists(_modelPath)) File.Delete(_modelPath);
            model.Save(_modelPath);

            return model;
        }
    }
}
