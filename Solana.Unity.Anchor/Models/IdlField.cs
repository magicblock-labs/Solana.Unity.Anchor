using Solana.Unity.Anchor.Converters;
using Solana.Unity.Anchor.Models.Types;
using Solana.Unity.Anchor.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models
{
    public class IdlField
    {
        public string Name { get; set; }

        [JsonConverter(typeof(IIdlTypeConverter))]
        public IIdlType Type { get; set; }
    }
}