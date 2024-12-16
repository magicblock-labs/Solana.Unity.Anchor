using Solana.Unity.Anchor.Models.Types.Enum;
using Solana.Unity.Anchor.CodeGen;
using Solana.Unity.Anchor.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models.Types
{
    public class EnumIdlTypeDefinition : IIdlTypeDefinitionTy
    {
        public string Name { get; set; }
        public string Address { get; set;  }
        
        public bool Writable { get; set;  }

        public IEnumVariant[] Variants { get; set; }


        public bool IsPureEnum()
        {
            return Variants.All(x => x is SimpleEnumVariant);
        }
    }
}