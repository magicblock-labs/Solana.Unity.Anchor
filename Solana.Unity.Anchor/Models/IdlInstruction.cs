using Solana.Unity.Anchor.Converters;
using Solana.Unity.Anchor.Models.Accounts;
using Solana.Unity.Anchor.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models
{

    public class IdlInstruction
    {
        public string Name { get; set; }

        public ulong InstructionSignatureHash { get; set; }

        [JsonConverter(typeof(IIdlAccountItemConverter))]
        public IIdlAccountItem[] Accounts { get; set; }

        public IdlField[] Args { get; set; }
    }
}