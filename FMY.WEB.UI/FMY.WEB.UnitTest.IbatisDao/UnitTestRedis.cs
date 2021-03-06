﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FMY.RedisClient;

namespace FMY.WEB.UnitTest.IbatisDao
{
    /// <summary>
    /// UnitTestRedis 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTestRedis
    {
        public UnitTestRedis()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO:  在此处添加测试逻辑
            //
            string key = "Users";
            RedisBase.Core.FlushAll();
            TestLua();
            RedisBase.Core.AddItemToList(key, "FMY1");
            RedisBase.Core.AddItemToList(key, "FMY2");
            RedisBase.Core.Add<string>("mykey", "123456");
            RedisString.Set("mykey1", "abcdef");
            Console.ReadLine();
        }


        public static void TestLua()
        {
            var luaBody = @"
    local val = redis.call('zrange', KEYS[1], 0, ARGV[1]-1)
    if val then redis.call('zremrangebyrank', KEYS[1], 0, ARGV[1]-1) end
    return val";

            //var i = 0;
            //var alphabet = 26.Times(c => ((char)('A' + c)).ToString());
            //alphabet.ForEach(x => Redis.AddItemToSortedSet("zalphabet", x, i++));
            for (int i = 0; i < 26; i++)
            {
                RedisBase.Core.AddItemToSortedSet("zalphabet", i.ToString(), i);
            }

            //Remove the letters with the lowest rank from the sorted set 'zalphabet'
            var letters = RedisBase.Core.ExecLuaAsList(luaBody, keys: new[] { "zalphabet" }, args: new[] { "3" });
            //letters.PrintDump(); //[A, B, C]
        }
    }
}
