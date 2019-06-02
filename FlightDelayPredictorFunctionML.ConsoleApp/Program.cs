//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using FlightDelayPredictorFunctionML.Model.DataModels;
using static Microsoft.ML.DataOperationsCatalog;
using System.Collections.Generic;

namespace FlightDelayPredictorFunctionML.ConsoleApp
{
    class Program
    {
        //Machine Learning model to load and use for predictions
        private const string MODEL_FILEPATH = @"MLModel.zip";

        //Dataset to use for predictions 
        private const string DATA_FILEPATH = @"C:\Users\Mukei\Google Drive\hackathoncopa\delay dataset work\delays_clean_fixed_FINAL.tsv";

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: DATA_FILEPATH,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            // Training code used by ML.NET CLI and AutoML to generate the model
            ModelBuilder.CreateModel(splitDataView.TrainSet, splitDataView.TestSet);

            ITransformer mlModel = mlContext.Model.Load(GetAbsolutePath(MODEL_FILEPATH), out DataViewSchema inputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            // Create sample data to do a single prediction with it 
            IEnumerable<ModelInput> sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(splitDataView.TestSet, false);

            int errorCount = 0;

            Console.WriteLine($"Predicting delayed status for {sampleForPrediction.Count()} flights from test data sample. (Only errors will be printed)");

            foreach (var sampleData in sampleForPrediction)
            {
                // Try a single prediction
                ModelOutput predictionResult = predEngine.Predict(sampleData);
                if (sampleData.DELAYED != predictionResult.Prediction)
                {
                    errorCount++;
                    Console.WriteLine($"Delay Prediction --> Actual value (will delay): {sampleData.DELAYED} | Predicted value (will delay) : {predictionResult.Prediction}");
                }
            }

            Console.WriteLine($"Printed {errorCount} failed predictions out of {sampleForPrediction.Count()} records correctly predicted");

            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
