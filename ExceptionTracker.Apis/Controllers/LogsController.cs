using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ExceptionTracker.Apis.Models;
using MongoDB.Bson;
using MongoDB.Driver;


namespace ExceptionTracker.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly MongoDBConfig _config;
        private readonly MongoClient _mongoClient;

        public LogsController(IConfiguration configuration)
        {
            _config = configuration.GetSection("MongoBD").Get<MongoDBConfig>();
            _mongoClient = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress(_config.HostName),
                UseSsl = _config.UseSSL,
                ConnectTimeout = TimeSpan.FromMilliseconds(_config.Timeout),
                DefaultCredentials = MongoCredentials.Create(_config.UserName, _config.Password),
                MaxConnectionPoolSize = _config.MaxPoolSize,
                ReadPreference = new ReadPreference(ReadPreferenceMode.Primary)
            });
        }

        // GET api/logs
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
