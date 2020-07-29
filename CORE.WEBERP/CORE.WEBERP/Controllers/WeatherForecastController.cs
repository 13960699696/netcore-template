using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Entity.Base_Manage;
using EFCore.Sharding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CORE.WEBERP.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDbAccessor _dbAccessor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbAccessor"></param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDbAccessor dbAccessor)
        {
            _logger = logger;
            _dbAccessor = dbAccessor;
        }
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var data = _dbAccessor.GetList<Base_User>();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
