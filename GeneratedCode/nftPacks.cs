using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Solana.Unity;
using Solana.Unity.Programs.Abstract;
using Solana.Unity.Programs.Utilities;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Core.Sockets;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;
using MplNftPacks;
using MplNftPacks.Program;
using MplNftPacks.Errors;
using MplNftPacks.Accounts;
using MplNftPacks.Types;

namespace MplNftPacks
{
    namespace Accounts
    {
        public partial class PackCard
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 13623684537658831159UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{55, 241, 34, 215, 231, 12, 17, 189};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "AMhzvWMggY4";
            public AccountType AccountType { get; set; }

            public PublicKey PackSet { get; set; }

            public PublicKey Master { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenAccount { get; set; }

            public uint MaxSupply { get; set; }

            public ushort Weight { get; set; }

            public static PackCard Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PackCard result = new PackCard();
                result.AccountType = (AccountType)_data.GetU8(offset);
                offset += 1;
                result.PackSet = _data.GetPubKey(offset);
                offset += 32;
                result.Master = _data.GetPubKey(offset);
                offset += 32;
                result.Metadata = _data.GetPubKey(offset);
                offset += 32;
                result.TokenAccount = _data.GetPubKey(offset);
                offset += 32;
                result.MaxSupply = _data.GetU32(offset);
                offset += 4;
                result.Weight = _data.GetU16(offset);
                offset += 2;
                return result;
            }
        }

        public partial class PackConfig
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 12489845269522874748UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{124, 1, 129, 61, 25, 216, 84, 173};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "Mk1iZgFikeQ";
            public AccountType AccountType { get; set; }

            public (uint,uint,uint)[] Weights { get; set; }

            public CleanUpActions ActionToDo { get; set; }

            public static PackConfig Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PackConfig result = new PackConfig();
                result.AccountType = (AccountType)_data.GetU8(offset);
                offset += 1;
                int resultWeightsLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Weights = new (uint,uint,uint)[resultWeightsLength];
                for (uint resultWeightsIdx = 0; resultWeightsIdx < resultWeightsLength; resultWeightsIdx++)
                {
                }

                offset += CleanUpActions.Deserialize(_data, offset, out var resultActionToDo);
                result.ActionToDo = resultActionToDo;
                return result;
            }
        }

        public partial class PackSet
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 16709777104449447271UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{103, 37, 170, 138, 190, 13, 229, 231};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "JFf58zWf1yQ";
            public AccountType AccountType { get; set; }

            public PublicKey Store { get; set; }

            public PublicKey Authority { get; set; }

            public string Description { get; set; }

            public string Uri { get; set; }

            public byte[] Name { get; set; }

            public uint PackCards { get; set; }

            public uint PackVouchers { get; set; }

            public ulong TotalWeight { get; set; }

            public ulong TotalEditions { get; set; }

            public bool Mutable { get; set; }

            public PackSetState PackState { get; set; }

            public PackDistributionType DistributionType { get; set; }

            public uint AllowedAmountToRedeem { get; set; }

            public ulong RedeemStartDate { get; set; }

            public ulong? RedeemEndDate { get; set; }

            public static PackSet Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PackSet result = new PackSet();
                result.AccountType = (AccountType)_data.GetU8(offset);
                offset += 1;
                result.Store = _data.GetPubKey(offset);
                offset += 32;
                result.Authority = _data.GetPubKey(offset);
                offset += 32;
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                offset += _data.GetBorshString(offset, out var resultUri);
                result.Uri = resultUri;
                result.Name = _data.GetBytes(offset, 32);
                offset += 32;
                result.PackCards = _data.GetU32(offset);
                offset += 4;
                result.PackVouchers = _data.GetU32(offset);
                offset += 4;
                result.TotalWeight = _data.GetU64(offset);
                offset += 8;
                result.TotalEditions = _data.GetU64(offset);
                offset += 8;
                result.Mutable = _data.GetBool(offset);
                offset += 1;
                result.PackState = (PackSetState)_data.GetU8(offset);
                offset += 1;
                result.DistributionType = (PackDistributionType)_data.GetU8(offset);
                offset += 1;
                result.AllowedAmountToRedeem = _data.GetU32(offset);
                offset += 4;
                result.RedeemStartDate = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.RedeemEndDate = _data.GetU64(offset);
                    offset += 8;
                }

                return result;
            }
        }

        public partial class PackVoucher
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 3314027365130790130UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{242, 100, 139, 130, 84, 202, 253, 45};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "hYWmtj8ZDEU";
            public AccountType AccountType { get; set; }

            public PublicKey PackSet { get; set; }

            public PublicKey Master { get; set; }

            public PublicKey Metadata { get; set; }

            public static PackVoucher Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PackVoucher result = new PackVoucher();
                result.AccountType = (AccountType)_data.GetU8(offset);
                offset += 1;
                result.PackSet = _data.GetPubKey(offset);
                offset += 32;
                result.Master = _data.GetPubKey(offset);
                offset += 32;
                result.Metadata = _data.GetPubKey(offset);
                offset += 32;
                return result;
            }
        }

        public partial class ProvingProcess
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 6866135481422306887UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{71, 222, 225, 141, 215, 105, 73, 95};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "D2Edz3uA2h4";
            public AccountType AccountType { get; set; }

            public PublicKey WalletKey { get; set; }

            public bool IsExhausted { get; set; }

            public PublicKey VoucherMint { get; set; }

            public PublicKey PackSet { get; set; }

            public uint CardsRedeemed { get; set; }

            public (uint,uint) CardsToRedeem { get; set; }

            public static ProvingProcess Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                ProvingProcess result = new ProvingProcess();
                result.AccountType = (AccountType)_data.GetU8(offset);
                offset += 1;
                result.WalletKey = _data.GetPubKey(offset);
                offset += 32;
                result.IsExhausted = _data.GetBool(offset);
                offset += 1;
                result.VoucherMint = _data.GetPubKey(offset);
                offset += 32;
                result.PackSet = _data.GetPubKey(offset);
                offset += 32;
                result.CardsRedeemed = _data.GetU32(offset);
                offset += 4;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum MplNftPacksErrorKind : uint
        {
            WrongAllowedAmountToRedeem = 0U,
            WrongRedeemDate = 1U,
            CardProbabilityMissing = 2U,
            WrongCardProbability = 3U,
            CardShouldntHaveProbabilityValue = 4U,
            ProvedVouchersMismatchPackVouchers = 5U,
            PackIsAlreadyEnded = 6U,
            PackSetNotConfigured = 7U,
            CantActivatePack = 8U,
            PackSetNotActivated = 9U,
            ProvingPackProcessCompleted = 10U,
            ProvingVoucherProcessCompleted = 11U,
            WrongEdition = 12U,
            WrongEditionMint = 13U,
            Overflow = 14U,
            Underflow = 15U,
            NotEmptyPackSet = 16U,
            WrongPackState = 17U,
            ImmutablePackSet = 18U,
            CantSetTheSameValue = 19U,
            WrongMaxSupply = 20U,
            WrongVoucherSupply = 21U,
            CardDoesntHaveEditions = 22U,
            UserRedeemedAllCards = 23U,
            UriTooLong = 24U,
            CardDoesntHaveMaxSupply = 25U,
            WrongMasterSupply = 26U,
            MissingEditionsInPack = 27U,
            AlreadySetNextCardToRedeem = 28U,
            EndDateNotArrived = 29U,
            DescriptionTooLong = 30U,
            WhitelistedCreatorInactive = 31U,
            WrongWhitelistedCreator = 32U,
            WrongVoucherOwner = 33U,
            CardShouldntHaveSupplyValue = 34U,
            PackIsFullWithCards = 35U,
            WeightsNotCleanedUp = 36U,
            CardAlreadyRedeemed = 37U,
            UserCantRedeemThisCard = 38U,
            InvalidWeightPosition = 39U
        }
    }

    namespace Types
    {
        public partial class AddCardToPackArgs
        {
            public uint MaxSupply { get; set; }

            public ushort Weight { get; set; }

            public uint Index { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU32(MaxSupply, offset);
                offset += 4;
                _data.WriteU16(Weight, offset);
                offset += 2;
                _data.WriteU32(Index, offset);
                offset += 4;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out AddCardToPackArgs result)
            {
                int offset = initialOffset;
                result = new AddCardToPackArgs();
                result.MaxSupply = _data.GetU32(offset);
                offset += 4;
                result.Weight = _data.GetU16(offset);
                offset += 2;
                result.Index = _data.GetU32(offset);
                offset += 4;
                return offset - initialOffset;
            }
        }

        public partial class InitPackSetArgs
        {
            public byte[] Name { get; set; }

            public string Description { get; set; }

            public string Uri { get; set; }

            public bool Mutable { get; set; }

            public PackDistributionType DistributionType { get; set; }

            public uint AllowedAmountToRedeem { get; set; }

            public ulong? RedeemStartDate { get; set; }

            public ulong? RedeemEndDate { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteSpan(Name, offset);
                offset += Name.Length;
                offset += _data.WriteBorshString(Description, offset);
                offset += _data.WriteBorshString(Uri, offset);
                _data.WriteBool(Mutable, offset);
                offset += 1;
                _data.WriteU8((byte)DistributionType, offset);
                offset += 1;
                _data.WriteU32(AllowedAmountToRedeem, offset);
                offset += 4;
                if (RedeemStartDate != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(RedeemStartDate.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (RedeemEndDate != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(RedeemEndDate.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out InitPackSetArgs result)
            {
                int offset = initialOffset;
                result = new InitPackSetArgs();
                result.Name = _data.GetBytes(offset, 32);
                offset += 32;
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                offset += _data.GetBorshString(offset, out var resultUri);
                result.Uri = resultUri;
                result.Mutable = _data.GetBool(offset);
                offset += 1;
                result.DistributionType = (PackDistributionType)_data.GetU8(offset);
                offset += 1;
                result.AllowedAmountToRedeem = _data.GetU32(offset);
                offset += 4;
                if (_data.GetBool(offset++))
                {
                    result.RedeemStartDate = _data.GetU64(offset);
                    offset += 8;
                }

                if (_data.GetBool(offset++))
                {
                    result.RedeemEndDate = _data.GetU64(offset);
                    offset += 8;
                }

                return offset - initialOffset;
            }
        }

        public partial class EditPackSetArgs
        {
            public byte[] Name { get; set; }

            public string Description { get; set; }

            public string Uri { get; set; }

            public bool? Mutable { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                if (Name != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteSpan(Name, offset);
                    offset += Name.Length;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (Description != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(Description, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (Uri != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(Uri, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (Mutable != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(Mutable.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out EditPackSetArgs result)
            {
                int offset = initialOffset;
                result = new EditPackSetArgs();
                if (_data.GetBool(offset++))
                {
                    result.Name = _data.GetBytes(offset, 32);
                    offset += 32;
                }

                if (_data.GetBool(offset++))
                {
                    offset += _data.GetBorshString(offset, out var resultDescription);
                    result.Description = resultDescription;
                }

                if (_data.GetBool(offset++))
                {
                    offset += _data.GetBorshString(offset, out var resultUri);
                    result.Uri = resultUri;
                }

                if (_data.GetBool(offset++))
                {
                    result.Mutable = _data.GetBool(offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }
        }

        public partial class ClaimPackArgs
        {
            public uint Index { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU32(Index, offset);
                offset += 4;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out ClaimPackArgs result)
            {
                int offset = initialOffset;
                result = new ClaimPackArgs();
                result.Index = _data.GetU32(offset);
                offset += 4;
                return offset - initialOffset;
            }
        }

        public partial class RequestCardToRedeemArgs
        {
            public uint Index { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU32(Index, offset);
                offset += 4;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out RequestCardToRedeemArgs result)
            {
                int offset = initialOffset;
                result = new RequestCardToRedeemArgs();
                result.Index = _data.GetU32(offset);
                offset += 4;
                return offset - initialOffset;
            }
        }

        public enum CleanUpActionsType : byte
        {
            Change,
            Sort,
            None
        }

        public partial class CleanUpActions
        {
            public Tuple<uint, uint> ChangeValue { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8((byte)Type, offset);
                offset += 1;
                switch (Type)
                {
                    case CleanUpActionsType.Change:
                        _data.WriteU32(ChangeValue.Item1, offset);
                        offset += 4;
                        _data.WriteU32(ChangeValue.Item2, offset);
                        offset += 4;
                        break;
                }

                return offset - initialOffset;
            }

            public CleanUpActionsType Type { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out CleanUpActions result)
            {
                int offset = initialOffset;
                result = new CleanUpActions();
                result.Type = (CleanUpActionsType)_data.GetU8(offset);
                offset += 1;
                switch (result.Type)
                {
                    case CleanUpActionsType.Change:
                    {
                        uint ChangeItem1;
                        ChangeItem1 = _data.GetU32(offset);
                        offset += 4;
                        uint ChangeItem2;
                        ChangeItem2 = _data.GetU32(offset);
                        offset += 4;
                        result.ChangeValue = Tuple.Create(ChangeItem1, ChangeItem2);
                        break;
                    }
                }

                return offset - initialOffset;
            }
        }

        public enum PackSetState : byte
        {
            NotActivated,
            Activated,
            Deactivated,
            Ended
        }

        public enum PackDistributionType : byte
        {
            MaxSupply,
            Fixed,
            Unlimited
        }

        public enum AccountType : byte
        {
            Uninitialized,
            PackSet,
            PackCard,
            PackVoucher,
            ProvingProcess,
            PackConfig
        }
    }

    public partial class MplNftPacksClient : TransactionalBaseClient<MplNftPacksErrorKind>
    {
        public MplNftPacksClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackCard>>> GetPackCardsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PackCard.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackCard>>(res);
            List<PackCard> resultingAccounts = new List<PackCard>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PackCard.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackCard>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackConfig>>> GetPackConfigsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PackConfig.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackConfig>>(res);
            List<PackConfig> resultingAccounts = new List<PackConfig>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PackConfig.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackConfig>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackSet>>> GetPackSetsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PackSet.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackSet>>(res);
            List<PackSet> resultingAccounts = new List<PackSet>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PackSet.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackSet>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackVoucher>>> GetPackVouchersAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PackVoucher.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackVoucher>>(res);
            List<PackVoucher> resultingAccounts = new List<PackVoucher>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PackVoucher.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PackVoucher>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ProvingProcess>>> GetProvingProcesssAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = ProvingProcess.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ProvingProcess>>(res);
            List<ProvingProcess> resultingAccounts = new List<ProvingProcess>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => ProvingProcess.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ProvingProcess>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PackCard>> GetPackCardAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PackCard>(res);
            var resultingAccount = PackCard.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PackCard>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PackConfig>> GetPackConfigAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PackConfig>(res);
            var resultingAccount = PackConfig.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PackConfig>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PackSet>> GetPackSetAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PackSet>(res);
            var resultingAccount = PackSet.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PackSet>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PackVoucher>> GetPackVoucherAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PackVoucher>(res);
            var resultingAccount = PackVoucher.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PackVoucher>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<ProvingProcess>> GetProvingProcessAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<ProvingProcess>(res);
            var resultingAccount = ProvingProcess.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<ProvingProcess>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribePackCardAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PackCard> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PackCard parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PackCard.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePackConfigAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PackConfig> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PackConfig parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PackConfig.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePackSetAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PackSet> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PackSet parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PackSet.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePackVoucherAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PackVoucher> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PackVoucher parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PackVoucher.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeProvingProcessAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, ProvingProcess> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                ProvingProcess parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = ProvingProcess.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendInitPackAsync(InitPackAccounts accounts, InitPackSetArgs initPackSetArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.InitPack(accounts, initPackSetArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAddCardToPackAsync(AddCardToPackAccounts accounts, AddCardToPackArgs addCardToPackArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.AddCardToPack(accounts, addCardToPackArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAddVoucherToPackAsync(AddVoucherToPackAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.AddVoucherToPack(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendActivateAsync(ActivateAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.Activate(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeactivateAsync(DeactivateAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.Deactivate(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendClosePackAsync(ClosePackAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.ClosePack(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendClaimPackAsync(ClaimPackAccounts accounts, ClaimPackArgs claimPackArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.ClaimPack(accounts, claimPackArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendTransferPackAuthorityAsync(TransferPackAuthorityAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.TransferPackAuthority(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeletePackAsync(DeletePackAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.DeletePack(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeletePackCardAsync(DeletePackCardAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.DeletePackCard(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeletePackVoucherAsync(DeletePackVoucherAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.DeletePackVoucher(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendEditPackAsync(EditPackAccounts accounts, EditPackSetArgs editPackSetArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.EditPack(accounts, editPackSetArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendRequestCardForRedeemAsync(RequestCardForRedeemAccounts accounts, RequestCardToRedeemArgs requestCardToRedeemArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.RequestCardForRedeem(accounts, requestCardToRedeemArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCleanUpAsync(CleanUpAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.CleanUp(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeletePackConfigAsync(DeletePackConfigAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplNftPacksProgram.DeletePackConfig(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<MplNftPacksErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<MplNftPacksErrorKind>>{{0U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongAllowedAmountToRedeem, "Allowed amount to redeem should be more then 0")}, {1U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongRedeemDate, "Wrong redeem date")}, {2U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CardProbabilityMissing, "Card probability is missing")}, {3U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongCardProbability, "Wrong card probability value")}, {4U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CardShouldntHaveProbabilityValue, "Cards for this pack shouldn't have probability value")}, {5U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.ProvedVouchersMismatchPackVouchers, "Proved vouchers mismatch pack vouchers")}, {6U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.PackIsAlreadyEnded, "Pack is already ended")}, {7U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.PackSetNotConfigured, "NFT pack set not fully configured")}, {8U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CantActivatePack, "Can't activate NFT pack in current state")}, {9U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.PackSetNotActivated, "Pack set should be activated")}, {10U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.ProvingPackProcessCompleted, "Proving process for this pack is completed")}, {11U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.ProvingVoucherProcessCompleted, "Proving process for this voucher is completed")}, {12U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongEdition, "Received edition from wrong master")}, {13U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongEditionMint, "Received wrong edition mint")}, {14U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.Overflow, "Overflow")}, {15U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.Underflow, "Underflow")}, {16U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.NotEmptyPackSet, "Pack set should be empty to delete it")}, {17U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongPackState, "Wrong pack state to change data")}, {18U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.ImmutablePackSet, "Pack set is immutable")}, {19U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CantSetTheSameValue, "Can't set the same value")}, {20U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongMaxSupply, "Wrong max supply value")}, {21U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongVoucherSupply, "Voucher should have supply greater then 0")}, {22U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CardDoesntHaveEditions, "Card ran out of editions")}, {23U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.UserRedeemedAllCards, "User redeemed all allowed cards")}, {24U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.UriTooLong, "URI too long")}, {25U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CardDoesntHaveMaxSupply, "Card doesn't have max supply")}, {26U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongMasterSupply, "Master edition should have unlimited supply")}, {27U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.MissingEditionsInPack, "Pack set doesn't have total editions")}, {28U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.AlreadySetNextCardToRedeem, "User already got next card to redeem")}, {29U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.EndDateNotArrived, "Can't close the pack before end date")}, {30U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.DescriptionTooLong, "Pack description too long")}, {31U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WhitelistedCreatorInactive, "Whitelisted creator inactive")}, {32U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongWhitelistedCreator, "Wrong whitelisted creator address")}, {33U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WrongVoucherOwner, "Voucher owner mismatch")}, {34U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CardShouldntHaveSupplyValue, "Cards for this pack shouldn't have supply value")}, {35U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.PackIsFullWithCards, "Pack is already full of cards")}, {36U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.WeightsNotCleanedUp, "Card weights should be cleaned up")}, {37U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.CardAlreadyRedeemed, "User already redeemed this card")}, {38U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.UserCantRedeemThisCard, "User can't redeem this card")}, {39U, new ProgramError<MplNftPacksErrorKind>(MplNftPacksErrorKind.InvalidWeightPosition, "Invalid weight position")}, };
        }
    }

    namespace Program
    {
        public class InitPackAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Store { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey Clock { get; set; }

            public PublicKey WhitelistedCreator { get; set; }
        }

        public class AddCardToPackAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey PackConfig { get; set; }

            public PublicKey PackCard { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey MasterMetadata { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey Source { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey ProgramAuthority { get; set; }

            public PublicKey Store { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class AddVoucherToPackAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey PackVoucher { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey VoucherOwner { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey MasterMetadata { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey Source { get; set; }

            public PublicKey Store { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ActivateAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey Authority { get; set; }
        }

        public class DeactivateAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey Authority { get; set; }
        }

        public class ClosePackAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Clock { get; set; }
        }

        public class ClaimPackAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey ProvingProcess { get; set; }

            public PublicKey UserWallet { get; set; }

            public PublicKey PackCard { get; set; }

            public PublicKey UserToken { get; set; }

            public PublicKey NewMetadata { get; set; }

            public PublicKey NewEdition { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey NewMint { get; set; }

            public PublicKey NewMintAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey MetadataMint { get; set; }

            public PublicKey EditionMarker { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class TransferPackAuthorityAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey CurrentAuthority { get; set; }

            public PublicKey NewAuthority { get; set; }
        }

        public class DeletePackAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Refunder { get; set; }
        }

        public class DeletePackCardAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey PackCard { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Refunder { get; set; }

            public PublicKey NewMasterEditionOwner { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey ProgramAuthority { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class DeletePackVoucherAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey PackVoucher { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Refunder { get; set; }
        }

        public class EditPackAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey Authority { get; set; }
        }

        public class RequestCardForRedeemAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey PackConfig { get; set; }

            public PublicKey Store { get; set; }

            public PublicKey Edition { get; set; }

            public PublicKey EditionMint { get; set; }

            public PublicKey PackVoucher { get; set; }

            public PublicKey ProvingProcess { get; set; }

            public PublicKey UserWallet { get; set; }

            public PublicKey RecentSlothashes { get; set; }

            public PublicKey Clock { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey UserToken { get; set; }
        }

        public class CleanUpAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey PackConfig { get; set; }
        }

        public class DeletePackConfigAccounts
        {
            public PublicKey PackSet { get; set; }

            public PublicKey PackConfig { get; set; }

            public PublicKey Refunder { get; set; }

            public PublicKey Authority { get; set; }
        }

        public static class MplNftPacksProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction InitPack(InitPackAccounts accounts, InitPackSetArgs initPackSetArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Store, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.WhitelistedCreator, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16612344585933497787UL, offset);
                offset += 8;
                offset += initPackSetArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AddCardToPack(AddCardToPackAccounts accounts, AddCardToPackArgs addCardToPackArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackConfig, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackCard, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Source, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Store, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10715713915699352625UL, offset);
                offset += 8;
                offset += addCardToPackArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AddVoucherToPack(AddVoucherToPackAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.VoucherOwner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Source, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Store, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9930993401291164338UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Activate(ActivateAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(5956634580510559170UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Deactivate(DeactivateAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(976749443730731052UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ClosePack(ClosePackAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(5120577759566018788UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ClaimPack(ClaimPackAccounts accounts, ClaimPackArgs claimPackArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ProvingProcess, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UserWallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackCard, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UserToken, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewMintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MetadataMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.EditionMarker, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13820783862001108365UL, offset);
                offset += 8;
                offset += claimPackArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction TransferPackAuthority(TransferPackAuthorityAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CurrentAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewAuthority, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(7979011765883631419UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeletePack(DeletePackAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Refunder, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(4017892160323630793UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeletePackCard(DeletePackCardAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackCard, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Refunder, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMasterEditionOwner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13971289452132694793UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeletePackVoucher(DeletePackVoucherAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Refunder, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10716619178292402215UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction EditPack(EditPackAccounts accounts, EditPackSetArgs editPackSetArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(7613488030204588040UL, offset);
                offset += 8;
                offset += editPackSetArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction RequestCardForRedeem(RequestCardForRedeemAccounts accounts, RequestCardToRedeemArgs requestCardToRedeemArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackConfig, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Store, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Edition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.EditionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PackVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ProvingProcess, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UserWallet, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.RecentSlothashes, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UserToken, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(351482052384419204UL, offset);
                offset += 8;
                offset += requestCardToRedeemArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CleanUp(CleanUpAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackConfig, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(18076755479828346376UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeletePackConfig(DeletePackConfigAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PackSet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PackConfig, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Refunder, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11494728808567257267UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}