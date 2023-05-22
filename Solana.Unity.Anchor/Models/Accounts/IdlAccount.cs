using Solana.Unity.Anchor.CodeGen;

namespace Solana.Unity.Anchor.Models.Accounts
{
    public class IdlAccount : IIdlAccountItem
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string NamePascalCase => Name.ToPascalCase();

        public bool IsMut { get; set; }

        public bool IsSigner { get; set; }
        
        public bool IsOptional { get; set; }

        public IdlPda Pda { get; set; }

    }
}