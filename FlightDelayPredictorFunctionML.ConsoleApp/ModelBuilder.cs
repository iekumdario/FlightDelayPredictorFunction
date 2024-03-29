//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using FlightDelayPredictorFunctionML.Model.DataModels;
using Microsoft.ML.Trainers.LightGbm;

namespace FlightDelayPredictorFunctionML.ConsoleApp
{
    public static class ModelBuilder
    {
        private static string MODEL_FILEPATH = @"../../../../FlightDelayPredictorFunctionML.Model/MLModel.zip";

        // Create MLContext to be shared across the model creation workflow objects 
        // Set a random seed for repeatable/deterministic results across multiple trainings.
        private static MLContext mlContext = new MLContext(seed: 1);

        public static void CreateModel(IDataView trainingDataView, IDataView testDataView)
        {
            // Build training pipeline
            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext);
            
            // Train Model
            ITransformer mlModel = TrainModel(mlContext, trainingDataView, trainingPipeline);

            // Evaluate quality of Model
            Evaluate(mlContext, testDataView, trainingPipeline);

            // Save model
            SaveModel(mlContext, mlModel, MODEL_FILEPATH, trainingDataView.Schema);
        }

        public static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = mlContext.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair("AIRCRAFT_TYPE", "AIRCRAFT_TYPE"), new InputOutputColumnPair("FLIGHT_STATUS", "FLIGHT_STATUS"), new InputOutputColumnPair("OD", "OD"), new InputOutputColumnPair("FESTIVE_DAY_ORIGIN", "FESTIVE_DAY_ORIGIN"), new InputOutputColumnPair("FESTIVE_DAY_DEST", "FESTIVE_DAY_DEST") })
                                      .Append(mlContext.Transforms.Categorical.OneHotHashEncoding(new[] { new InputOutputColumnPair("AIRCRAFT", "AIRCRAFT") }))
                                      .Append(mlContext.Transforms.Concatenate("Features", new[] { "AIRCRAFT_TYPE", "FLIGHT_STATUS", "OD", "FESTIVE_DAY_ORIGIN", "FESTIVE_DAY_DEST", "AIRCRAFT", "STD", "STA", "OUT_OF_GATE", "OFF_THE_GROUND", "ON_THE_GROUND", "INTO_THE_GATE", "DEP_DELAY_MIN", "ARR_DELAY_MIN", "MONTH", "YEAR", "WEEKDAY", "DAY", "QUARTER" }));

            // Set the training algorithm 
            var trainer = mlContext.BinaryClassification.Trainers.LightGbm(new LightGbmBinaryTrainer.Options() { NumberOfIterations = 150, LearningRate = 0.03023778f, NumberOfLeaves = 47, MinimumExampleCountPerLeaf = 1, UseCategoricalSplit = true, HandleMissingValue = true, MinimumExampleCountPerGroup = 50, MaximumCategoricalSplitPointCount = 32, CategoricalSmoothing = 1, L2CategoricalRegularization = 0.5, Booster = new GradientBooster.Options() { L2Regularization = 0.5, L1Regularization = 0 }, LabelColumnName = "DELAYED", FeatureColumnName = "Features" });
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }

        public static ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            Console.WriteLine("=============== Training  model ===============");

            ITransformer model = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine("=============== End of training process ===============");
            return model;
        }

        private static void Evaluate(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            // Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics
            Console.WriteLine("=============== Cross-validating to get model's accuracy metrics ===============");
            var crossValidationResults = mlContext.BinaryClassification.CrossValidateNonCalibrated(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: "DELAYED");
            PrintBinaryClassificationFoldsAverageMetrics(crossValidationResults);
        }
        private static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            Console.WriteLine($"=============== Saving the model  ===============");
            mlContext.Model.Save(mlModel, modelInputSchema, GetAbsolutePath(modelRelativePath));
            Console.WriteLine("The model is saved to {0}", GetAbsolutePath(modelRelativePath));
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

        public static void PrintBinaryClassificationMetrics(BinaryClassificationMetrics metrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*       Metrics for binary classification model      ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"*       Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"*       Auc:      {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"************************************************************");
        }


        public static void PrintBinaryClassificationFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<BinaryClassificationMetrics>> crossValResults)
        {
            var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics);

            var AccuracyValues = metricsInMultipleFolds.Select(m => m.Accuracy);
            var AccuracyAverage = AccuracyValues.Average();
            var AccuraciesStdDeviation = CalculateStandardDeviation(AccuracyValues);
            var AccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(AccuracyValues);


            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Binary Classification model      ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       Average Accuracy:    {AccuracyAverage:0.###}  - Standard deviation: ({AccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({AccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine($"*************************************************************************************************************");
        }

        public static double CalculateStandardDeviation(IEnumerable<double> values)
        {
            double average = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (values.Count() - 1));
            return standardDeviation;
        }

        public static double CalculateConfidenceInterval95(IEnumerable<double> values)
        {
            double confidenceInterval95 = 1.96 * CalculateStandardDeviation(values) / Math.Sqrt((values.Count() - 1));
            return confidenceInterval95;
        }
    }
}
