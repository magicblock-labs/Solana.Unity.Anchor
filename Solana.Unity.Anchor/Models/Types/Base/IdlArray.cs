using Solana.Unity.Anchor.Converters;
using Solana.Unity.Anchor.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models.Types.Base
{
    public class IdlArray : IIdlType
    {

        [JsonConverter(typeof(IIdlTypeConverter))]
        public IIdlType ValuesType { get; set; }

        public int? Size { get; set; }

    }
}