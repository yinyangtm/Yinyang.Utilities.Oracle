using System;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yinyang.Utilities.Oracle.Core.Test
{
    [TestClass]
    public class TestOracle
    {
        private readonly string _connectionString;

        private IConfiguration Configuration { get; }

        public TestOracle()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<TestOracle>()
                .AddEnvironmentVariables()
                .Build();

            _connectionString = Configuration["Oracle"];
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException();
            }
        }

        [TestMethod]
        public void EasySelect()
        {
            OracleDatabase.ConnectionString = _connectionString;
            using (var Oracle = new OracleDatabase(_connectionString))
            {
                var result = Oracle.EasySelect<EntityTest>("select * from yinyang.\"test\" where \"id\" = 1").First();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.id);
                Assert.AreEqual(answer.key, result.key);
                Assert.AreEqual(answer.value, result.value);
                Oracle.Close();
            }
        }

        [TestMethod]
        public void ExecuteReaderFirst()
        {
            using (var Oracle = new OracleDatabase(_connectionString))
            {
                Oracle.Open();
                Oracle.CommandText = "select * from yinyang.\"test\" where \"id\" = :id";
                Oracle.AddParameter(":id", 1);
                var result = Oracle.ExecuteReaderFirst<EntityTest>();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.id);
                Assert.AreEqual(answer.key, result.key);
                Assert.AreEqual(answer.value, result.value);
                Oracle.Close();
            }
        }

        [TestMethod]
        public void Select()
        {
            using (var Oracle = new OracleDatabase(_connectionString))
            {
                Oracle.Open();
                Oracle.CommandText = "select * from yinyang.\"test\" where \"id\" = 1";
                var result = Oracle.ExecuteReader<EntityTest>().First();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.id);
                Assert.AreEqual(answer.key, result.key);
                Assert.AreEqual(answer.value, result.value);
                Oracle.Close();
            }
        }

        [TestMethod]
        public void SelectCount()
        {
            using (var Oracle = new OracleDatabase(_connectionString))
            {
                Oracle.Open();
                Oracle.CommandText = "select count(*) from yinyang.\"test\" where \"id\" = 1";
                var result = Oracle.ExecuteScalarToInt();

                Assert.AreEqual(1, result);
                Oracle.Close();
            }
        }

        [TestMethod]
        public void Function()
        {
            using (var Oracle = new OracleDatabase(_connectionString))
            {
                Oracle.Open();
                Oracle.ChangeCommandType(CommandType.Text);
                Oracle.CommandText = "select * from table(GetTestData(:id))";
                Oracle.AddParameter(":id", 1);
                var result = Oracle.ExecuteReaderFirst<EntityFunction>();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.ID);
                Assert.AreEqual(answer.key, result.KEY);
                Assert.AreEqual(answer.value, result.VALUE);

                Oracle.Close();
            }
        }

        [TestMethod]
        public void TableRowsCount()
        {
            using (var Oracle = new OracleDatabase(_connectionString))
            {
                Oracle.Open();
                Assert.AreEqual(1, Oracle.TableRowsCount("yinyang.\"test\""));
                Oracle.Close();
            }
        }
    }
}
