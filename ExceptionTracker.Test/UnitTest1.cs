using ExceptionTracker.Mongo.Repository;
using ExceptionTracker.Test.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExceptionTracker.Test
{
    public class MongoRepositoryTest
    {
        private readonly MongoRepository<FooBar> _repository;

        public MongoRepositoryTest()
        {
            
            var mongoUrl = MongoUrl.Create("mongodb://127.0.0.1:27017");
            var settings = MongoClientSettings.FromUrl(mongoUrl);

            var client = new MongoClient(settings);
            var datebase = client.GetDatabase("logs");

            _repository = new MongoRepository<FooBar>(datebase);
        }

        [Fact]
        public void Test_Add()
        {
            _repository.Add(new FooBar
            {
                FooName = "This is FooName",
                FooTime = DateTime.Now,
                FooValue = 3.1415926535M,
                BarItems = new List<Bar>
                {
                    new Bar()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Length = 18.0M,
                        Width = 5.0M,
                        Height = 18.0M
                    }
                }
            });

            var repo = _repository.Find(e => e.FooName == "This is FooName");
        }
    }
}
