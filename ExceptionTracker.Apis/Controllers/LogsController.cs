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
using ExceptionTracker.Apis.Utils;
using Newtonsoft.Json;

namespace ExceptionTracker.Apis.Controllers
{
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly MongoRepository<BaseLog> repository;

        public LogsController(IConfiguration configuration)
        {
            var address = configuration.GetValue<string>("MongoDB");
            var mongoUrl = MongoUrl.Create(address);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("etlogs");
            repository = new MongoRepository<BaseLog>(database);
        }

        // GET api/logs/{schemaName}/{id}
        [HttpGet("/logs/{schemaName}/{id}")]
        public string Get(string schemaName, string id)
        {
            var records = repository.GetById(schemaName, id);
            var ss = records.ToJson();
            return records.FirstOrDefault().ToJson();
        }

        // GET api/logs/{schemaName}
        [HttpGet("/logs/{schemaName}")]
        public IActionResult GetAll(string schemaName)
        {
            return Ok(repository.GetAll());
        }

        // POST api/logs/{schemaName}
        [HttpPost("/logs/{schemaName}")]
        public ActionResult<List<BsonDocument>> Post(string schemaName)
        {
            var json = Request.ReadAsString();
            var document = BsonDocument.Parse(json);
            return repository.Insert(schemaName, document).ToList();
        }

        // PUT api/logs/{schemaName}/{id}
        [HttpPut("/logs/{schemaName}/{id}")]
        public ActionResult<List<BsonDocument>> Put(string schemaName, string id)
        {
            var json = Request.ReadAsString();
            var document = BsonDocument.Parse(json);
            return repository.Update(schemaName, id, document).ToList();
        }

        // DELETE api/logs/{schemaName}/{id}
        [HttpDelete("/logs/{schemaName}/{id}")]
        public ActionResult Delete(string schemaName, string id)
        {
            var json = Request.ReadAsString();
            repository.Delete(schemaName, id);
            return Ok();
        }
    }
}
