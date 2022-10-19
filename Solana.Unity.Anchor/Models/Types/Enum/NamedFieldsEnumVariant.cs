using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor.Models.Types
{
    public class NamedFieldsEnumVariant : IEnumVariant
    {
        public string Name { get; set; }

        public IdlField[] Fields { get; set; }
    }
}