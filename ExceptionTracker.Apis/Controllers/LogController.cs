using ExceptionTracker.Apis.Models;
using ExceptionTracker.Apis.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExceptionTracker.Apis.Controllers
{
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly MongoRepository<BaseLog> repository;

        public LogController(IConfiguration configuration)
        {
            var address = configuration.GetValue<string>("MongoDB");
            var mongoUrl = MongoUrl.Create(address);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("etlogs");
            repository = new MongoRepository<BaseLog>(database);
        }

        // GET api/logs/{typeName}
        [HttpGet("/logs/{schema}/{id}")]
        public ActionResult<BsonDocument> Get(string schema,string id)
        {
            return repository.GetById(schema,id);
        }

        // GET api/logs/
        [HttpGet("/logs/{schema}")]
        public IActionResult GetAll(string schema)
        {
            return Ok(repository.GetAll());
        }

        // POST api/logs/{type}
        [HttpPost("/logs/{type}")]
        public ActionResult<List<string>> Post(string type)
        {
            using(var stream = new MemoryStream())
            {
                Request.Body.CopyTo(stream);
                stream.Position = 0;
                using(var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var document = BsonDocument.Parse(json);
                    return repository.Insert(type, document);
                }
            }
        }
    }
}
