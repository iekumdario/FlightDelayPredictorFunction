//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using Microsoft.ML.Data;

namespace FlightDelayPredictorFunctionML.Model.DataModels
{
    public class ModelInput
    {
        [ColumnName("AIRCRAFT"), LoadColumn(0)]
        public string AIRCRAFT { get; set; }


        [ColumnName("AIRCRAFT_TYPE"), LoadColumn(1)]
        public string AIRCRAFT_TYPE { get; set; }


        [ColumnName("STD"), LoadColumn(2)]
        public float STD { get; set; }


        [ColumnName("STA"), LoadColumn(3)]
        public float STA { get; set; }


        [ColumnName("OUT_OF_GATE"), LoadColumn(4)]
        public float OUT_OF_GATE { get; set; }


        [ColumnName("OFF_THE_GROUND"), LoadColumn(5)]
        public float OFF_THE_GROUND { get; set; }


        [ColumnName("ON_THE_GROUND"), LoadColumn(6)]
        public float ON_THE_GROUND { get; set; }


        [ColumnName("INTO_THE_GATE"), LoadColumn(7)]
        public float INTO_THE_GATE { get; set; }


        [ColumnName("DEP_DELAY_MIN"), LoadColumn(8)]
        public float DEP_DELAY_MIN { get; set; }


        [ColumnName("ARR_DELAY_MIN"), LoadColumn(9)]
        public float ARR_DELAY_MIN { get; set; }


        [ColumnName("DELAYED"), LoadColumn(10)]
        public bool DELAYED { get; set; }


        [ColumnName("FLIGHT_STATUS"), LoadColumn(11)]
        public string FLIGHT_STATUS { get; set; }


        [ColumnName("OD"), LoadColumn(12)]
        public string OD { get; set; }


        [ColumnName("FESTIVE_DAY_ORIGIN"), LoadColumn(13)]
        public string FESTIVE_DAY_ORIGIN { get; set; }


        [ColumnName("FESTIVE_DAY_DEST"), LoadColumn(14)]
        public string FESTIVE_DAY_DEST { get; set; }


        [ColumnName("MONTH"), LoadColumn(15)]
        public float MONTH { get; set; }


        [ColumnName("YEAR"), LoadColumn(16)]
        public float YEAR { get; set; }


        [ColumnName("WEEKDAY"), LoadColumn(17)]
        public float WEEKDAY { get; set; }


        [ColumnName("DAY"), LoadColumn(18)]
        public float DAY { get; set; }


        [ColumnName("QUARTER"), LoadColumn(19)]
        public float QUARTER { get; set; }


    }
}
