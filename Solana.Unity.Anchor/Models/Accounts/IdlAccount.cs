using Solana.Unity.Anchor.CodeGen;
using System.Text.Json.Serialization;

namespace Solana.Unity.Anchor.Models.Accounts
{
    public class IdlAccount : IIdlAccountItem
    {
        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public string Description { get; set; }

        public string NamePascalCase => Name.ToPascalCase();

        public bool IsMut { get; set; }
        
        [JsonPropertyName("writable")]
        public bool IsWritable { get; set; }
        
        [JsonIgnore]
        public bool Writable => IsMut || IsWritable;

        public bool IsSigner { get; set; }
        
        [JsonPropertyName("signer")]
        public bool IsSignerNew { get; set; }
        
        [JsonIgnore]
        public bool Signer => IsSigner || IsSignerNew;
        
        public bool IsOptional { get; set; }
        
        [JsonPropertyName("optional")]
        public bool IsOptionalNew { get; set; }
        
        [JsonIgnore]
        public bool Optional => IsOptional || IsOptionalNew;

        public IdlPda Pda { get; set; }

    }
}