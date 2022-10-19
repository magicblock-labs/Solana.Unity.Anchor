using Solana.Unity.Anchor.CodeGen;
using Solana.Unity.Anchor.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models.Types.Base
{
    public class IdlTuple : IIdlType
    {

        // [JsonConverter(typeof(IIdlTypeConverter))]
        public IIdlType[] ValuesType { get; set; }
    }
}