﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FMY.Core.RedisClient;
using FMY.TestMongo;
using MongoDB.Driver;

namespace FMY.Core.TestClient
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestRedis()
        {
            RedisBase.Core.SetValue("d", "d");

            RedisBase.Core.SetValues(new System.Collections.Generic.Dictionary<string, string>() {
                { "e","e"}
                ,{ "f","f"}
            });
            string key = "Users";
            RedisBase.Core.FlushAll();
            RedisBase.Core.AddItemToList(key, "FMY1");
            RedisBase.Core.AddItemToList(key, "FMY2");
            RedisBase.Core.Add<string>("mykey", "123456");
            RedisString.Set("mykey1", "abcdef");
            Console.ReadLine();
        }

        [TestMethod]
        public void TestRedisLua()
        {
            string lua2 = @"
local a= redis.call('setnx',KEYS[1],ARGV[1])
redis.call('expire',KEYS[1],ARGV[2])
return a
";
            var result = RedisBase.Core.ExecLua(lua2, new string[] { "key4nx" }, new string[] { "val4nx", "3000" });
        }

        [TestMethod]
        public void TestMongo()
        {
            var database = "log4net";
            var collection = "FMY.WEB.UI8";
            var db = new MongoClient("mongodb://127.0.0.1:27018/?socketTimeout=1s").GetDatabase(database);
            var coll = db.GetCollection<Model2>(collection);
            coll.InsertOneAsync(new Model2() { NameExt = "123", LastName = "LastName" }).Wait();//.ConfigureAwait(false);

            MongoDBHelper helper = new MongoDBHelper();
            //helper.Insert<Model1>(new Model1() { Name = "name", Addr = "123" });
            helper.InsertAsync<Model1>("FMY.WEB.UI7", new Model1() { Name = "name2", Addr = "122" }).Wait();
        }
    }

    public class Model1
    {
        public string Name { get; set; }
        public string Addr { get; set; }

        public int Age { get; set; }
    }

    public class Model2
    {
        public string LastName { get; set; }
        public string NameExt { get; set; }

    }
}
