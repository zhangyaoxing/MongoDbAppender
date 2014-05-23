using System;
using System.Configuration;
using System.Threading;
using System.Xml;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Log4net.Appender.MongoDb;
using MongoDB.Driver.Builders;

namespace MongoDbAppernderTest
{
    
    
    /// <summary>
    ///This is a test class for MongoDbAppenderTest and is intended
    ///to contain all MongoDbAppenderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MongoDbAppenderTest
    {
        private const int THREAD_COUNT = 20;
        private const int LOG_COUNT = 100;
        private Semaphore sem;

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for MongoDbAppender Constructor.
        ///</summary>
        [TestMethod()]
        public void MongoDbAppenderConstructorTest()
        {
            InitTestDB();
            var log = LogManager.GetLogger(typeof(MongoDbAppenderTest));
            var random = new Random();
            var rNum = random.Next().ToString();
            log.Warn(string.Format("{0}: Warning happened.", rNum));
            var doc = SearchMongoDbForLog("WARN");
            Assert.IsTrue(doc.GetElement("message").Value.AsString.StartsWith(rNum + ":"));
            rNum = random.Next().ToString();
            log.Error(string.Format("{0}: Error happened", rNum));
            doc = SearchMongoDbForLog("ERROR");
            Assert.IsTrue(doc.GetElement("message").Value.AsString.StartsWith(rNum + ":"));
        }
        [TestMethod()]
        public void MongoDbAppenderStressTest()
        {
            InitTestDB();
            this.sem = new Semaphore(0, THREAD_COUNT);
            var start = DateTime.Now;
            for (var i = 0; i < THREAD_COUNT; i++)
            {
                Thread thread = new Thread(new ThreadStart(this.WritingThread));
                thread.Start();
            }
            for (var i = 0; i < THREAD_COUNT; i++)
            {
                this.sem.WaitOne();
            }
            var spent = (DateTime.Now - start).Duration().TotalMilliseconds / 1000;
            //Console.WriteLine(spent);
            Console.WriteLine((double)THREAD_COUNT * LOG_COUNT / spent);
            var collection = GetLogCollection();
            var count = collection.Count();
            Assert.AreEqual(THREAD_COUNT * LOG_COUNT, count);
            var groups = collection.Group(
                Query.Null,
                "thread",
                new BsonDocument("count", 0),
                new BsonJavaScript("function(curr, result) { result.count++; }"),
                null);
            foreach (var group in groups)
            {
                Assert.AreEqual(LOG_COUNT, group.GetElement("count").Value.AsDouble);
            }
        }

        private void WritingThread()
        {
            var logger = LogManager.GetLogger(typeof(MongoDbAppenderTest));
            for (var i = 0; i < LOG_COUNT; i++)
            {
                logger.Warn("Info logged", new ArgumentException());
            }
            this.sem.Release();
        }

        private void InitTestDB()
        {
            var collection = GetLogCollection();
            collection.Drop();
        }
        private BsonDocument SearchMongoDbForLog(string level)
        {
            var collection = GetLogCollection();
            var query = Query.EQ("level", new BsonString(level));

            var docs = collection.FindAs<BsonDocument>(query)
                .SetSortOrder(SortBy.Descending("timestamp"));
            foreach (var doc in docs)
            {
                return doc;
            }
            return null;
        }
        private MongoCollection GetLogCollection()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = path + @"\config\logging.config";

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            var rootNode = doc.SelectSingleNode("/log4net/appender");
            var connStrNameNode = rootNode.SelectSingleNode("connectionStringName/@value");
            var connStrNode = rootNode.SelectSingleNode("connectionString/@value");
            var collectionNameNode = rootNode.SelectSingleNode("collectionName/@value");
            var dbNode = rootNode.SelectSingleNode("dbName/@value");
            string connStr;
            if (connStrNode == null)
            {
                connStr = ConfigurationManager.ConnectionStrings[connStrNameNode.InnerText].ConnectionString;
            }
            else
            {
                connStr = connStrNode.InnerText;
            }
            var url = new MongoUrl(connStr);
            var client = new MongoClient(url);
            var server = client.GetServer();
            var db = server.GetDatabase(dbNode == null ? (string.IsNullOrWhiteSpace(url.DatabaseName) ? MongoDbAppender.DEFAULT_DATABASE : url.DatabaseName) : dbNode.InnerText);
            var collection = db.GetCollection("logs_" + collectionNameNode.InnerText);
            return collection;
        }
    }
}
