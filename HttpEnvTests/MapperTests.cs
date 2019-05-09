using HttpEnv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HttpEnvTests
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void TestMapping()
        {
            string pathToMap = "/foo";
            string codePath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            using (new MockHostingEnvironment(pathToMap, codePath))
            {
                var test = new Mapper();
                string result = test.Map(pathToMap);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void TestMapping2()
        {
            string pathToMap = "/bar";
            string codePath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            using (new MockHostingEnvironment(pathToMap, codePath))
            {
                var test = new Mapper();
                string result = test.Map(pathToMap);
                Assert.AreEqual(codePath, result);
            }
        }
    }
}
