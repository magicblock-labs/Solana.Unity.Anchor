using Solana.Unity.Anchor.Models.Accounts;
using Solana.Unity.Anchor.Models.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Converters
{
    public class IIdlAccountItemConverter : JsonConverter<IIdlAccountItem[]>
    {
        public override IIdlAccountItem[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<IdlAccount> accountItems = JsonSerializer.Deserialize<List<IdlAccount>>(ref reader, options);
            return accountItems.Cast<IIdlAccountItem>().ToArray();
        }

        public override void Write(Utf8JsonWriter writer, IIdlAccountItem[] value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}