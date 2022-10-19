using System;

namespace Solana.Unity.Anchor.Models.Types
{
    public struct UnixTimestamp : IIdlType
    {
        private Int64 _Value;

        public static implicit operator UnixTimestamp(Int64 value)
        {
            return new UnixTimestamp { _Value = value };
        }

        public static implicit operator Int64(UnixTimestamp value)
        {
            return value._Value;
        }
    }
}