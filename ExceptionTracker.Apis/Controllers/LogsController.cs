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
        public ActionResult Get(string schemaName, string id)
        {
            var records = repository.GetById(schemaName, id);
            return new JsonResult(records.FirstOrDefault().ToJsonEx());
        }

        // GET api/logs/{schemaName}
        [HttpGet("/logs/{schemaName}")]
        public ActionResult GetAll(string schemaName)
        {
            var records = repository.GetAll(schemaName).ToJsonEx();
            return new JsonResult(records);
        }

        // POST api/logs/{schemaName}
        [HttpPost("/logs/{schemaName}")]
        public ActionResult Post(string schemaName)
        {
            var json = Request.Body.ReadAsString();
            var document = BsonDocument.Parse(json);
            var records =  repository.Insert(schemaName, document).ToJsonEx();
            return new JsonResult(records);
        }

        // PUT api/logs/{schemaName}/{id}
        [HttpPut("/logs/{schemaName}/{id}")]
        public ActionResult Put(string schemaName, string id)
        {
            var json = Request.Body.ReadAsString();
            var document = BsonDocument.Parse(json);
            var records =  repository.Update(schemaName, id, document).ToJsonEx();
            return new JsonResult(records);
        }

        // DELETE api/logs/{schemaName}/{id}
        [HttpDelete("/logs/{schemaName}/{id}")]
        public ActionResult Delete(string schemaName, string id)
        {
            repository.Delete(schemaName, id);
            return Ok();
        }
    }
}
