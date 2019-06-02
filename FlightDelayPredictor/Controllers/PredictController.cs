using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using FlightDelayPredictorFunctionML.Model.DataModels;

namespace FlightDelayPredictor.Controllers
{
    [Route("api/delayed")]
    [ApiController]
    public class PredictController : ControllerBase
    {
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

        public PredictController(PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        [HttpPost]
        public ActionResult<dynamic> Post([FromBody] ModelInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ModelOutput prediction = _predictionEnginePool.Predict(input);

            dynamic delayed = new {
                prediction = prediction.Prediction,
                confidence = CalculatePercentage(prediction.Score)
            };

            return Ok(delayed);
        }

        public float CalculatePercentage(double value)
        {
            return 100 * (1.0f / (1.0f + (float)Math.Exp(-value)));
        }
    }
}