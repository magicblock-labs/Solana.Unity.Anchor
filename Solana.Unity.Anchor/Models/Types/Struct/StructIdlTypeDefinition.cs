using System.Collections.Generic;

namespace Solana.Unity.Anchor.Models.Types
{
    public class StructIdlTypeDefinition : IIdlTypeDefinitionTy
    {
        public string Name { get; set; }

        public IdlField[] Fields { get; set; }
        public List<byte> Discriminator { get; set; }
    }
}