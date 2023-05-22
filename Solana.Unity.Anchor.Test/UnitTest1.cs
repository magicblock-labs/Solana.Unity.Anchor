using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Solana.Unity.Anchor.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var res = IdlParser.ParseFile("Resources/ChatExample.json");
            var res = IdlParser.ParseFile("Resources/seq.json");
            Assert.IsNotNull(res);

            ClientGenerator c = new();

            c.GenerateSyntaxTree(res);
            Assert.IsNotNull(c);

            var code = c.GenerateCode(res);
            Assert.IsNotNull(code);
        }
    }
}

