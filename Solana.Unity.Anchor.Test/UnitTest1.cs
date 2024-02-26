using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Solana.Unity.Anchor.Test
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestMethod]
        public void TestEmptyIdl()
        {
            var res = IdlParser.ParseFile("Resources/Empty.json");
            Assert.IsNotNull(res);

            ClientGenerator c = new();

            c.GenerateSyntaxTree(res);
            Assert.IsNotNull(c);

            var code = c.GenerateCode(res);
            Assert.IsNotNull(code);
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            var res = IdlParser.ParseFile("Resources/seq.json");
            Assert.IsNotNull(res);

            ClientGenerator c = new();

            c.GenerateSyntaxTree(res);
            Assert.IsNotNull(c);

            var code = c.GenerateCode(res);
            Assert.IsNotNull(code);
        }
        
        [TestMethod]
        public void TestIdlWithOptional()
        {
            var res = IdlParser.ParseFile("Resources/SolChess.json");
            Assert.IsNotNull(res);

            ClientGenerator c = new();

            c.GenerateSyntaxTree(res);
            Assert.IsNotNull(c);

            var code = c.GenerateCode(res);
            Assert.IsNotNull(code);
        }
    }
}

