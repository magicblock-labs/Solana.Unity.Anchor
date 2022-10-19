using Solana.Unity.Anchor.Converters;
using Solana.Unity.Anchor.Models.Types;
using Solana.Unity.Anchor.CodeGen;
using Solana.Unity.Anchor.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models
{
    public class Idl
    {
        [JsonIgnore]
        public string DefaultProgramAddress { get; set; }

        public string Version { get; set; }
        public string Name { get; set; }

        public string NamePascalCase => Name.ToPascalCase();

        public IdlInstruction[] Instructions { get; set; }


        [JsonConverter(typeof(IIdlTypeDefinitionTyConverter))]
        public IIdlTypeDefinitionTy[] Accounts { get; set; }

        [JsonConverter(typeof(IIdlTypeDefinitionTyConverter))]
        public IIdlTypeDefinitionTy[] Types { get; set; }

        public IdlErrorCode[] Errors { get; set; }

        public IdlEvent[] Events { get; set; }

    }

}