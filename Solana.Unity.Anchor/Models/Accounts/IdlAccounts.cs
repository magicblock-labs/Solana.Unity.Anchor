using Solana.Unity.Anchor.Converters;
using Solana.Unity.Anchor.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models.Accounts
{
    public class IdlAccounts : IIdlAccountItem
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public string FullName { get; set; }

        public string NamePascalCase => Name.ToPascalCase();

        [JsonConverter(typeof(IIdlAccountItemConverter))]
        public IIdlAccountItem[] Accounts { get; set; }


    }
}