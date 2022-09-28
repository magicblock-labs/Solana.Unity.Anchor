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
using FixedPriceSale;
using FixedPriceSale.Program;
using FixedPriceSale.Errors;
using FixedPriceSale.Accounts;
using FixedPriceSale.Types;

namespace FixedPriceSale
{
    namespace Accounts
    {
        public partial class Store
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 1882152486802239618UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{130, 48, 247, 244, 182, 191, 30, 26};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "Nn25MFiXzvM";
            public PublicKey Admin { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public static Store Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Store result = new Store();
                result.Admin = _data.GetPubKey(offset);
                offset += 32;
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                return result;
            }
        }

        public partial class SellingResource
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 12038728708262273039UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{15, 32, 69, 235, 249, 39, 18, 167};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "3Xk2QTkUVFc";
            public PublicKey Store { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Resource { get; set; }

            public PublicKey Vault { get; set; }

            public PublicKey VaultOwner { get; set; }

            public ulong Supply { get; set; }

            public ulong? MaxSupply { get; set; }

            public SellingResourceState State { get; set; }

            public static SellingResource Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                SellingResource result = new SellingResource();
                result.Store = _data.GetPubKey(offset);
                offset += 32;
                result.Owner = _data.GetPubKey(offset);
                offset += 32;
                result.Resource = _data.GetPubKey(offset);
                offset += 32;
                result.Vault = _data.GetPubKey(offset);
                offset += 32;
                result.VaultOwner = _data.GetPubKey(offset);
                offset += 32;
                result.Supply = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.MaxSupply = _data.GetU64(offset);
                    offset += 8;
                }

                result.State = (SellingResourceState)_data.GetU8(offset);
                offset += 1;
                return result;
            }
        }

        public partial class Market
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 11152851117305872091UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{219, 190, 213, 55, 0, 227, 198, 154};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "dkokXHR3DTw";
            public PublicKey Store { get; set; }

            public PublicKey SellingResource { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey TreasuryHolder { get; set; }

            public PublicKey TreasuryOwner { get; set; }

            public PublicKey Owner { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public bool Mutable { get; set; }

            public ulong Price { get; set; }

            public ulong? PiecesInOneWallet { get; set; }

            public ulong StartDate { get; set; }

            public ulong? EndDate { get; set; }

            public MarketState State { get; set; }

            public ulong FundsCollected { get; set; }

            public GatingConfig Gatekeeper { get; set; }

            public static Market Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Market result = new Market();
                result.Store = _data.GetPubKey(offset);
                offset += 32;
                result.SellingResource = _data.GetPubKey(offset);
                offset += 32;
                result.TreasuryMint = _data.GetPubKey(offset);
                offset += 32;
                result.TreasuryHolder = _data.GetPubKey(offset);
                offset += 32;
                result.TreasuryOwner = _data.GetPubKey(offset);
                offset += 32;
                result.Owner = _data.GetPubKey(offset);
                offset += 32;
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                result.Mutable = _data.GetBool(offset);
                offset += 1;
                result.Price = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.PiecesInOneWallet = _data.GetU64(offset);
                    offset += 8;
                }

                result.StartDate = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.EndDate = _data.GetU64(offset);
                    offset += 8;
                }

                result.State = (MarketState)_data.GetU8(offset);
                offset += 1;
                result.FundsCollected = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    offset += GatingConfig.Deserialize(_data, offset, out var resultGatekeeper);
                    result.Gatekeeper = resultGatekeeper;
                }

                return result;
            }
        }

        public partial class TradeHistory
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 2970247384947914174UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{190, 117, 218, 114, 66, 112, 56, 41};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "YrhqeSGMUwr";
            public PublicKey Market { get; set; }

            public PublicKey Wallet { get; set; }

            public ulong AlreadyBought { get; set; }

            public static TradeHistory Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                TradeHistory result = new TradeHistory();
                result.Market = _data.GetPubKey(offset);
                offset += 32;
                result.Wallet = _data.GetPubKey(offset);
                offset += 32;
                result.AlreadyBought = _data.GetU64(offset);
                offset += 8;
                return result;
            }
        }

        public partial class PrimaryMetadataCreators
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 842597971910492994UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{66, 131, 48, 36, 100, 130, 177, 11};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "C8FjqkNeLVG";
            public Creator[] Creators { get; set; }

            public static PrimaryMetadataCreators Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PrimaryMetadataCreators result = new PrimaryMetadataCreators();
                int resultCreatorsLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Creators = new Creator[resultCreatorsLength];
                for (uint resultCreatorsIdx = 0; resultCreatorsIdx < resultCreatorsLength; resultCreatorsIdx++)
                {
                    offset += Creator.Deserialize(_data, offset, out var resultCreatorsresultCreatorsIdx);
                    result.Creators[resultCreatorsIdx] = resultCreatorsresultCreatorsIdx;
                }

                return result;
            }
        }

        public partial class PayoutTicket
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 5814033597987085977UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{153, 222, 52, 216, 192, 152, 175, 80};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "SjiK84BFFuZ";
            public bool Used { get; set; }

            public static PayoutTicket Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PayoutTicket result = new PayoutTicket();
                result.Used = _data.GetBool(offset);
                offset += 1;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum FixedPriceSaleErrorKind : uint
        {
            NoValidSignerPresent = 6000U,
            StringIsTooLong = 6001U,
            NameIsTooLong = 6002U,
            DescriptionIsTooLong = 6003U,
            SupplyIsGtThanAvailable = 6004U,
            SupplyIsNotProvided = 6005U,
            DerivedKeyInvalid = 6006U,
            SellingResourceOwnerInvalid = 6007U,
            PublicKeyMismatch = 6008U,
            PiecesInOneWalletIsTooMuch = 6009U,
            StartDateIsInPast = 6010U,
            EndDateIsEarlierThanBeginDate = 6011U,
            IncorrectOwner = 6012U,
            MarketIsNotStarted = 6013U,
            MarketIsEnded = 6014U,
            UserReachBuyLimit = 6015U,
            MathOverflow = 6016U,
            SupplyIsGtThanMaxSupply = 6017U,
            MarketDurationIsNotUnlimited = 6018U,
            MarketIsSuspended = 6019U,
            MarketIsImmutable = 6020U,
            MarketInInvalidState = 6021U,
            PriceIsZero = 6022U,
            FunderIsInvalid = 6023U,
            PayoutTicketExists = 6024U,
            InvalidFunderDestination = 6025U,
            TreasuryIsNotEmpty = 6026U,
            SellingResourceAlreadyTaken = 6027U,
            MetadataCreatorsIsEmpty = 6028U,
            UserWalletMustMatchUserTokenAccount = 6029U,
            MetadataShouldBeMutable = 6030U,
            PrimarySaleIsNotAllowed = 6031U,
            CreatorsIsGtThanAvailable = 6032U,
            CreatorsIsEmpty = 6033U,
            MarketOwnerDoesntHaveShares = 6034U,
            PrimaryMetadataCreatorsNotProvided = 6035U,
            GatingTokenMissing = 6036U,
            InvalidOwnerForGatingToken = 6037U,
            WrongGatingMetadataAccount = 6038U,
            WrongOwnerInTokenGatingAcc = 6039U,
            WrongGatingDate = 6040U,
            CollectionMintMissing = 6041U,
            WrongCollectionMintKey = 6042U,
            WrongGatingToken = 6043U
        }
    }

    namespace Types
    {
        public partial class GatingConfig
        {
            public PublicKey Collection { get; set; }

            public bool ExpireOnUse { get; set; }

            public ulong? GatingTime { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Collection, offset);
                offset += 32;
                _data.WriteBool(ExpireOnUse, offset);
                offset += 1;
                if (GatingTime != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(GatingTime.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out GatingConfig result)
            {
                int offset = initialOffset;
                result = new GatingConfig();
                result.Collection = _data.GetPubKey(offset);
                offset += 32;
                result.ExpireOnUse = _data.GetBool(offset);
                offset += 1;
                if (_data.GetBool(offset++))
                {
                    result.GatingTime = _data.GetU64(offset);
                    offset += 8;
                }

                return offset - initialOffset;
            }
        }

        public partial class Creator
        {
            public PublicKey Address { get; set; }

            public bool Verified { get; set; }

            public byte Share { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Address, offset);
                offset += 32;
                _data.WriteBool(Verified, offset);
                offset += 1;
                _data.WriteU8(Share, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Creator result)
            {
                int offset = initialOffset;
                result = new Creator();
                result.Address = _data.GetPubKey(offset);
                offset += 32;
                result.Verified = _data.GetBool(offset);
                offset += 1;
                result.Share = _data.GetU8(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public enum SellingResourceState : byte
        {
            Uninitialized,
            Created,
            InUse,
            Exhausted,
            Stopped
        }

        public enum MarketState : byte
        {
            Uninitialized,
            Created,
            Suspended,
            Active,
            Ended
        }
    }

    public partial class FixedPriceSaleClient : TransactionalBaseClient<FixedPriceSaleErrorKind>
    {
        public FixedPriceSaleClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Store>>> GetStoresAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Store.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Store>>(res);
            List<Store> resultingAccounts = new List<Store>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Store.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Store>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<SellingResource>>> GetSellingResourcesAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = SellingResource.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<SellingResource>>(res);
            List<SellingResource> resultingAccounts = new List<SellingResource>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => SellingResource.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<SellingResource>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Market>>> GetMarketsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Market.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Market>>(res);
            List<Market> resultingAccounts = new List<Market>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Market.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Market>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<TradeHistory>>> GetTradeHistorysAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = TradeHistory.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<TradeHistory>>(res);
            List<TradeHistory> resultingAccounts = new List<TradeHistory>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => TradeHistory.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<TradeHistory>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PrimaryMetadataCreators>>> GetPrimaryMetadataCreatorssAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PrimaryMetadataCreators.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PrimaryMetadataCreators>>(res);
            List<PrimaryMetadataCreators> resultingAccounts = new List<PrimaryMetadataCreators>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PrimaryMetadataCreators.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PrimaryMetadataCreators>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PayoutTicket>>> GetPayoutTicketsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PayoutTicket.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PayoutTicket>>(res);
            List<PayoutTicket> resultingAccounts = new List<PayoutTicket>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PayoutTicket.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PayoutTicket>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Store>> GetStoreAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Store>(res);
            var resultingAccount = Store.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Store>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<SellingResource>> GetSellingResourceAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<SellingResource>(res);
            var resultingAccount = SellingResource.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<SellingResource>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Market>> GetMarketAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Market>(res);
            var resultingAccount = Market.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Market>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<TradeHistory>> GetTradeHistoryAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<TradeHistory>(res);
            var resultingAccount = TradeHistory.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<TradeHistory>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PrimaryMetadataCreators>> GetPrimaryMetadataCreatorsAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PrimaryMetadataCreators>(res);
            var resultingAccount = PrimaryMetadataCreators.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PrimaryMetadataCreators>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PayoutTicket>> GetPayoutTicketAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PayoutTicket>(res);
            var resultingAccount = PayoutTicket.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PayoutTicket>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeStoreAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Store> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Store parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Store.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeSellingResourceAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, SellingResource> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                SellingResource parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = SellingResource.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeMarketAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Market> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Market parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Market.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeTradeHistoryAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, TradeHistory> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                TradeHistory parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = TradeHistory.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePrimaryMetadataCreatorsAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PrimaryMetadataCreators> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PrimaryMetadataCreators parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PrimaryMetadataCreators.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePayoutTicketAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PayoutTicket> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PayoutTicket parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PayoutTicket.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendInitSellingResourceAsync(InitSellingResourceAccounts accounts, byte masterEditionBump, byte vaultOwnerBump, ulong? maxSupply, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.InitSellingResource(accounts, masterEditionBump, vaultOwnerBump, maxSupply, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCreateStoreAsync(CreateStoreAccounts accounts, string name, string description, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.CreateStore(accounts, name, description, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendBuyAsync(BuyAccounts accounts, byte tradeHistoryBump, byte vaultOwnerBump, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.Buy(accounts, tradeHistoryBump, vaultOwnerBump, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCloseMarketAsync(CloseMarketAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.CloseMarket(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSuspendMarketAsync(SuspendMarketAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.SuspendMarket(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendChangeMarketAsync(ChangeMarketAccounts accounts, string newName, string newDescription, bool? mutable, ulong? newPrice, ulong? newPiecesInOneWallet, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.ChangeMarket(accounts, newName, newDescription, mutable, newPrice, newPiecesInOneWallet, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendResumeMarketAsync(ResumeMarketAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.ResumeMarket(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendWithdrawAsync(WithdrawAccounts accounts, byte treasuryOwnerBump, byte payoutTicketBump, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.Withdraw(accounts, treasuryOwnerBump, payoutTicketBump, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCreateMarketAsync(CreateMarketAccounts accounts, byte treasuryOwnerBump, string name, string description, bool mutable, ulong price, ulong? piecesInOneWallet, ulong startDate, ulong? endDate, GatingConfig gatingConfig, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.CreateMarket(accounts, treasuryOwnerBump, name, description, mutable, price, piecesInOneWallet, startDate, endDate, gatingConfig, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendClaimResourceAsync(ClaimResourceAccounts accounts, byte vaultOwnerBump, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.ClaimResource(accounts, vaultOwnerBump, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSavePrimaryMetadataCreatorsAsync(SavePrimaryMetadataCreatorsAccounts accounts, byte primaryMetadataCreatorsBump, Creator[] creators, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.FixedPriceSaleProgram.SavePrimaryMetadataCreators(accounts, primaryMetadataCreatorsBump, creators, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<FixedPriceSaleErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<FixedPriceSaleErrorKind>>{{6000U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.NoValidSignerPresent, "No valid signer present")}, {6001U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.StringIsTooLong, "Some string variable is longer than allowed")}, {6002U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.NameIsTooLong, "Name string variable is longer than allowed")}, {6003U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.DescriptionIsTooLong, "Description string variable is longer than allowed")}, {6004U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.SupplyIsGtThanAvailable, "Provided supply is gt than available")}, {6005U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.SupplyIsNotProvided, "Supply is not provided")}, {6006U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.DerivedKeyInvalid, "Derived key invalid")}, {6007U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.SellingResourceOwnerInvalid, "Invalid selling resource owner provided")}, {6008U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.PublicKeyMismatch, "PublicKeyMismatch")}, {6009U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.PiecesInOneWalletIsTooMuch, "Pieces in one wallet cannot be greater than Max Supply value")}, {6010U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.StartDateIsInPast, "StartDate cannot be in the past")}, {6011U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.EndDateIsEarlierThanBeginDate, "EndDate should not be earlier than StartDate")}, {6012U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.IncorrectOwner, "Incorrect account owner")}, {6013U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MarketIsNotStarted, "Market is not started")}, {6014U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MarketIsEnded, "Market is ended")}, {6015U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.UserReachBuyLimit, "User reach buy limit")}, {6016U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MathOverflow, "Math overflow")}, {6017U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.SupplyIsGtThanMaxSupply, "Supply is gt than max supply")}, {6018U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MarketDurationIsNotUnlimited, "Market duration is not unlimited")}, {6019U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MarketIsSuspended, "Market is suspended")}, {6020U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MarketIsImmutable, "Market is immutable")}, {6021U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MarketInInvalidState, "Market in invalid state")}, {6022U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.PriceIsZero, "Price is zero")}, {6023U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.FunderIsInvalid, "Funder is invalid")}, {6024U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.PayoutTicketExists, "Payout ticket exists")}, {6025U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.InvalidFunderDestination, "Funder provide invalid destination")}, {6026U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.TreasuryIsNotEmpty, "Treasury is not empty")}, {6027U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.SellingResourceAlreadyTaken, "Selling resource already taken by other market")}, {6028U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MetadataCreatorsIsEmpty, "Metadata creators is empty")}, {6029U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.UserWalletMustMatchUserTokenAccount, "User wallet must match user token account")}, {6030U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MetadataShouldBeMutable, "Metadata should be mutable")}, {6031U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.PrimarySaleIsNotAllowed, "Primary sale is not allowed")}, {6032U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.CreatorsIsGtThanAvailable, "Creators is gt than allowed")}, {6033U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.CreatorsIsEmpty, "Creators is empty")}, {6034U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.MarketOwnerDoesntHaveShares, "Market owner doesn't receive shares at primary sale")}, {6035U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.PrimaryMetadataCreatorsNotProvided, "PrimaryMetadataCreatorsNotProvided")}, {6036U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.GatingTokenMissing, "Gating token is missing")}, {6037U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.InvalidOwnerForGatingToken, "Invalid program owner for the gating token account")}, {6038U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.WrongGatingMetadataAccount, "Wrong Metadata account for the gating token")}, {6039U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.WrongOwnerInTokenGatingAcc, "Wrong owner in token gating account")}, {6040U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.WrongGatingDate, "Wrong gating date send")}, {6041U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.CollectionMintMissing, "Collection mint is missing")}, {6042U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.WrongCollectionMintKey, "Wrong collection mint key")}, {6043U, new ProgramError<FixedPriceSaleErrorKind>(FixedPriceSaleErrorKind.WrongGatingToken, "Wrong gating token")}, };
        }
    }

    namespace Program
    {
        public class InitSellingResourceAccounts
        {
            public PublicKey Store { get; set; }

            public PublicKey Admin { get; set; }

            public PublicKey SellingResource { get; set; }

            public PublicKey SellingResourceOwner { get; set; }

            public PublicKey ResourceMint { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Vault { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey ResourceToken { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class CreateStoreAccounts
        {
            public PublicKey Admin { get; set; }

            public PublicKey Store { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class BuyAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey SellingResource { get; set; }

            public PublicKey UserTokenAccount { get; set; }

            public PublicKey UserWallet { get; set; }

            public PublicKey TradeHistory { get; set; }

            public PublicKey TreasuryHolder { get; set; }

            public PublicKey NewMetadata { get; set; }

            public PublicKey NewEdition { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey NewMint { get; set; }

            public PublicKey EditionMarker { get; set; }

            public PublicKey Vault { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey NewTokenAccount { get; set; }

            public PublicKey MasterEditionMetadata { get; set; }

            public PublicKey Clock { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class CloseMarketAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Clock { get; set; }
        }

        public class SuspendMarketAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Clock { get; set; }
        }

        public class ChangeMarketAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Clock { get; set; }
        }

        public class ResumeMarketAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Clock { get; set; }
        }

        public class WithdrawAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey SellingResource { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TreasuryHolder { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Destination { get; set; }

            public PublicKey Funder { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey PayoutTicket { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey Clock { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey AssociatedTokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class CreateMarketAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey Store { get; set; }

            public PublicKey SellingResourceOwner { get; set; }

            public PublicKey SellingResource { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey TreasuryHolder { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class ClaimResourceAccounts
        {
            public PublicKey Market { get; set; }

            public PublicKey TreasuryHolder { get; set; }

            public PublicKey SellingResource { get; set; }

            public PublicKey SellingResourceOwner { get; set; }

            public PublicKey Vault { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Destination { get; set; }

            public PublicKey Clock { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class SavePrimaryMetadataCreatorsAccounts
        {
            public PublicKey Admin { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey PrimaryMetadataCreators { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public static class FixedPriceSaleProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction InitSellingResource(InitSellingResourceAccounts accounts, byte masterEditionBump, byte vaultOwnerBump, ulong? maxSupply, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Store, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Admin, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellingResource, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SellingResourceOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ResourceMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Vault, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ResourceToken, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10449703070204825400UL, offset);
                offset += 8;
                _data.WriteU8(masterEditionBump, offset);
                offset += 1;
                _data.WriteU8(vaultOwnerBump, offset);
                offset += 1;
                if (maxSupply != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(maxSupply.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateStore(CreateStoreAccounts accounts, string name, string description, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Admin, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Store, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(6007541800146409604UL, offset);
                offset += 8;
                offset += _data.WriteBorshString(name, offset);
                offset += _data.WriteBorshString(description, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Buy(BuyAccounts accounts, byte tradeHistoryBump, byte vaultOwnerBump, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Market, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellingResource, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UserTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UserWallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TradeHistory, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreasuryHolder, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EditionMarker, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Vault, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEditionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16927863322537952870UL, offset);
                offset += 8;
                _data.WriteU8(tradeHistoryBump, offset);
                offset += 1;
                _data.WriteU8(vaultOwnerBump, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CloseMarket(CloseMarketAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Market, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17616689969847900760UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SuspendMarket(SuspendMarketAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Market, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(8549455015641684982UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ChangeMarket(ChangeMarketAccounts accounts, string newName, string newDescription, bool? mutable, ulong? newPrice, ulong? newPiecesInOneWallet, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Market, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(6351731705877707650UL, offset);
                offset += 8;
                if (newName != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(newName, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newDescription != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(newDescription, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (mutable != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(mutable.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newPrice != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(newPrice.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newPiecesInOneWallet != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(newPiecesInOneWallet.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ResumeMarket(ResumeMarketAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Market, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10334748685051132102UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Withdraw(WithdrawAccounts accounts, byte treasuryOwnerBump, byte payoutTicketBump, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Market, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SellingResource, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreasuryHolder, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Destination, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Funder, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PayoutTicket, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AssociatedTokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2495396153584390839UL, offset);
                offset += 8;
                _data.WriteU8(treasuryOwnerBump, offset);
                offset += 1;
                _data.WriteU8(payoutTicketBump, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateMarket(CreateMarketAccounts accounts, byte treasuryOwnerBump, string name, string description, bool mutable, ulong price, ulong? piecesInOneWallet, ulong startDate, ulong? endDate, GatingConfig gatingConfig, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Market, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Store, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellingResourceOwner, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellingResource, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreasuryHolder, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(18373486675916612199UL, offset);
                offset += 8;
                _data.WriteU8(treasuryOwnerBump, offset);
                offset += 1;
                offset += _data.WriteBorshString(name, offset);
                offset += _data.WriteBorshString(description, offset);
                _data.WriteBool(mutable, offset);
                offset += 1;
                _data.WriteU64(price, offset);
                offset += 8;
                if (piecesInOneWallet != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(piecesInOneWallet.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                _data.WriteU64(startDate, offset);
                offset += 8;
                if (endDate != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(endDate.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (gatingConfig != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += gatingConfig.Serialize(_data, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ClaimResource(ClaimResourceAccounts accounts, byte vaultOwnerBump, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Market, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryHolder, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SellingResource, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SellingResourceOwner, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Vault, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Destination, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Clock, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(1966514949201371136UL, offset);
                offset += 8;
                _data.WriteU8(vaultOwnerBump, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SavePrimaryMetadataCreators(SavePrimaryMetadataCreatorsAccounts accounts, byte primaryMetadataCreatorsBump, Creator[] creators, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Admin, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PrimaryMetadataCreators, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(18356738847214530626UL, offset);
                offset += 8;
                _data.WriteU8(primaryMetadataCreatorsBump, offset);
                offset += 1;
                _data.WriteS32(creators.Length, offset);
                offset += 4;
                foreach (var creatorsElement in creators)
                {
                    offset += creatorsElement.Serialize(_data, offset);
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}