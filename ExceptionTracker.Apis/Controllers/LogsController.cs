using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;


namespace ExceptionTracker.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        public LogsController(IConfiguration configuration)
        {
            var address = configuration.GetValue<string>("MongoDB");
            var mongoUrl = MongoUrl.Create(address);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            _mongoClient = new MongoClient(settings);
            _database = _mongoClient.GetDatabase("logs");
        }

        // GET api/logs/{typeName}
        [HttpGet]
        public string Get(string typeName)
        {
           return string.Empty;
        }

        // GET api/logs/{typeName}/{id}
        [HttpGet("{typeName}/{id}")]
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
