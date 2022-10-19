using Solana.Unity.Anchor.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models.Types
{
    public class StructIdlTypeDefinition : IIdlTypeDefinitionTy
    {
        public string Name { get; set; }

        public IdlField[] Fields { get; set; }
    }
}