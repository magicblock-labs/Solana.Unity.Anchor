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
using AuctionHouse;
using AuctionHouse.Program;
using AuctionHouse.Errors;
using AuctionHouse.Accounts;
using AuctionHouse.Types;

namespace AuctionHouse
{
    namespace Accounts
    {
        public partial class BidReceipt
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 7144813729942443706UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{186, 150, 141, 135, 59, 122, 39, 99};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "YD929MgXGrE";
            public PublicKey TradeState { get; set; }

            public PublicKey Bookkeeper { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey Buyer { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey PurchaseReceipt { get; set; }

            public ulong Price { get; set; }

            public ulong TokenSize { get; set; }

            public byte Bump { get; set; }

            public byte TradeStateBump { get; set; }

            public long CreatedAt { get; set; }

            public long? CanceledAt { get; set; }

            public static BidReceipt Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                BidReceipt result = new BidReceipt();
                result.TradeState = _data.GetPubKey(offset);
                offset += 32;
                result.Bookkeeper = _data.GetPubKey(offset);
                offset += 32;
                result.AuctionHouse = _data.GetPubKey(offset);
                offset += 32;
                result.Buyer = _data.GetPubKey(offset);
                offset += 32;
                result.Metadata = _data.GetPubKey(offset);
                offset += 32;
                if (_data.GetBool(offset++))
                {
                    result.TokenAccount = _data.GetPubKey(offset);
                    offset += 32;
                }

                if (_data.GetBool(offset++))
                {
                    result.PurchaseReceipt = _data.GetPubKey(offset);
                    offset += 32;
                }

                result.Price = _data.GetU64(offset);
                offset += 8;
                result.TokenSize = _data.GetU64(offset);
                offset += 8;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                result.TradeStateBump = _data.GetU8(offset);
                offset += 1;
                result.CreatedAt = _data.GetS64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.CanceledAt = _data.GetS64(offset);
                    offset += 8;
                }

                return result;
            }
        }

        public partial class ListingReceipt
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 16669031444762413040UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{240, 71, 225, 94, 200, 75, 84, 231};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "hC2RgjuNXEv";
            public PublicKey TradeState { get; set; }

            public PublicKey Bookkeeper { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey Seller { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey PurchaseReceipt { get; set; }

            public ulong Price { get; set; }

            public ulong TokenSize { get; set; }

            public byte Bump { get; set; }

            public byte TradeStateBump { get; set; }

            public long CreatedAt { get; set; }

            public long? CanceledAt { get; set; }

            public static ListingReceipt Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                ListingReceipt result = new ListingReceipt();
                result.TradeState = _data.GetPubKey(offset);
                offset += 32;
                result.Bookkeeper = _data.GetPubKey(offset);
                offset += 32;
                result.AuctionHouse = _data.GetPubKey(offset);
                offset += 32;
                result.Seller = _data.GetPubKey(offset);
                offset += 32;
                result.Metadata = _data.GetPubKey(offset);
                offset += 32;
                if (_data.GetBool(offset++))
                {
                    result.PurchaseReceipt = _data.GetPubKey(offset);
                    offset += 32;
                }

                result.Price = _data.GetU64(offset);
                offset += 8;
                result.TokenSize = _data.GetU64(offset);
                offset += 8;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                result.TradeStateBump = _data.GetU8(offset);
                offset += 1;
                result.CreatedAt = _data.GetS64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.CanceledAt = _data.GetS64(offset);
                    offset += 8;
                }

                return result;
            }
        }

        public partial class PurchaseReceipt
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 9698083547350204239UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{79, 127, 222, 137, 154, 131, 150, 134};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "EJFBwhcydJm";
            public PublicKey Bookkeeper { get; set; }

            public PublicKey Buyer { get; set; }

            public PublicKey Seller { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey Metadata { get; set; }

            public ulong TokenSize { get; set; }

            public ulong Price { get; set; }

            public byte Bump { get; set; }

            public long CreatedAt { get; set; }

            public static PurchaseReceipt Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PurchaseReceipt result = new PurchaseReceipt();
                result.Bookkeeper = _data.GetPubKey(offset);
                offset += 32;
                result.Buyer = _data.GetPubKey(offset);
                offset += 32;
                result.Seller = _data.GetPubKey(offset);
                offset += 32;
                result.AuctionHouse = _data.GetPubKey(offset);
                offset += 32;
                result.Metadata = _data.GetPubKey(offset);
                offset += 32;
                result.TokenSize = _data.GetU64(offset);
                offset += 8;
                result.Price = _data.GetU64(offset);
                offset += 8;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                result.CreatedAt = _data.GetS64(offset);
                offset += 8;
                return result;
            }
        }

        public partial class AuctionHouse
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 3527820258240326696UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{40, 108, 215, 107, 213, 85, 245, 48};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "7mB8j9Ym6fH";
            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctionHouseTreasury { get; set; }

            public PublicKey TreasuryWithdrawalDestination { get; set; }

            public PublicKey FeeWithdrawalDestination { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Creator { get; set; }

            public byte Bump { get; set; }

            public byte TreasuryBump { get; set; }

            public byte FeePayerBump { get; set; }

            public ushort SellerFeeBasisPoints { get; set; }

            public bool RequiresSignOff { get; set; }

            public bool CanChangeSalePrice { get; set; }

            public byte EscrowPaymentBump { get; set; }

            public bool HasAuctioneer { get; set; }

            public PublicKey AuctioneerAddress { get; set; }

            public bool[] Scopes { get; set; }

            public static AuctionHouse Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                AuctionHouse result = new AuctionHouse();
                result.AuctionHouseFeeAccount = _data.GetPubKey(offset);
                offset += 32;
                result.AuctionHouseTreasury = _data.GetPubKey(offset);
                offset += 32;
                result.TreasuryWithdrawalDestination = _data.GetPubKey(offset);
                offset += 32;
                result.FeeWithdrawalDestination = _data.GetPubKey(offset);
                offset += 32;
                result.TreasuryMint = _data.GetPubKey(offset);
                offset += 32;
                result.Authority = _data.GetPubKey(offset);
                offset += 32;
                result.Creator = _data.GetPubKey(offset);
                offset += 32;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                result.TreasuryBump = _data.GetU8(offset);
                offset += 1;
                result.FeePayerBump = _data.GetU8(offset);
                offset += 1;
                result.SellerFeeBasisPoints = _data.GetU16(offset);
                offset += 2;
                result.RequiresSignOff = _data.GetBool(offset);
                offset += 1;
                result.CanChangeSalePrice = _data.GetBool(offset);
                offset += 1;
                result.EscrowPaymentBump = _data.GetU8(offset);
                offset += 1;
                result.HasAuctioneer = _data.GetBool(offset);
                offset += 1;
                result.AuctioneerAddress = _data.GetPubKey(offset);
                offset += 32;
                result.Scopes = new bool[7];
                for (uint resultScopesIdx = 0; resultScopesIdx < 7; resultScopesIdx++)
                {
                    result.Scopes[resultScopesIdx] = _data.GetBool(offset);
                    offset += 1;
                }

                return result;
            }
        }

        public partial class Auctioneer
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 8715906234422420782UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{46, 101, 92, 150, 138, 30, 245, 120};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "8m6jHCBkyno";
            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public byte Bump { get; set; }

            public static Auctioneer Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Auctioneer result = new Auctioneer();
                result.AuctioneerAuthority = _data.GetPubKey(offset);
                offset += 32;
                result.AuctionHouse = _data.GetPubKey(offset);
                offset += 32;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum AuctionHouseErrorKind : uint
        {
            PublicKeyMismatch = 6000U,
            InvalidMintAuthority = 6001U,
            UninitializedAccount = 6002U,
            IncorrectOwner = 6003U,
            PublicKeysShouldBeUnique = 6004U,
            StatementFalse = 6005U,
            NotRentExempt = 6006U,
            NumericalOverflow = 6007U,
            ExpectedSolAccount = 6008U,
            CannotExchangeSOLForSol = 6009U,
            SOLWalletMustSign = 6010U,
            CannotTakeThisActionWithoutAuctionHouseSignOff = 6011U,
            NoPayerPresent = 6012U,
            DerivedKeyInvalid = 6013U,
            MetadataDoesntExist = 6014U,
            InvalidTokenAmount = 6015U,
            BothPartiesNeedToAgreeToSale = 6016U,
            CannotMatchFreeSalesWithoutAuctionHouseOrSellerSignoff = 6017U,
            SaleRequiresSigner = 6018U,
            OldSellerNotInitialized = 6019U,
            SellerATACannotHaveDelegate = 6020U,
            BuyerATACannotHaveDelegate = 6021U,
            NoValidSignerPresent = 6022U,
            InvalidBasisPoints = 6023U,
            TradeStateDoesntExist = 6024U,
            TradeStateIsNotEmpty = 6025U,
            ReceiptIsEmpty = 6026U,
            InstructionMismatch = 6027U,
            InvalidAuctioneer = 6028U,
            MissingAuctioneerScope = 6029U,
            MustUseAuctioneerHandler = 6030U,
            NoAuctioneerProgramSet = 6031U,
            TooManyScopes = 6032U,
            AuctionHouseNotDelegated = 6033U,
            BumpSeedNotInHashMap = 6034U,
            EscrowUnderRentExemption = 6035U,
            InvalidSeedsOrAuctionHouseNotDelegated = 6036U,
            BuyerTradeStateNotValid = 6037U,
            MissingElementForPartialOrder = 6038U,
            NotEnoughTokensAvailableForPurchase = 6039U,
            PartialPriceMismatch = 6040U,
            AuctionHouseAlreadyDelegated = 6041U,
            AuctioneerAuthorityMismatch = 6042U,
            InsufficientFunds = 6043U
        }
    }

    namespace Types
    {
        public enum AuthorityScope : byte
        {
            Deposit,
            Buy,
            PublicBuy,
            ExecuteSale,
            Sell,
            Cancel,
            Withdraw
        }
    }

    public partial class AuctionHouseClient : TransactionalBaseClient<AuctionHouseErrorKind>
    {
        public AuctionHouseClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<BidReceipt>>> GetBidReceiptsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = BidReceipt.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<BidReceipt>>(res);
            List<BidReceipt> resultingAccounts = new List<BidReceipt>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => BidReceipt.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<BidReceipt>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ListingReceipt>>> GetListingReceiptsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = ListingReceipt.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ListingReceipt>>(res);
            List<ListingReceipt> resultingAccounts = new List<ListingReceipt>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => ListingReceipt.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ListingReceipt>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PurchaseReceipt>>> GetPurchaseReceiptsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PurchaseReceipt.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PurchaseReceipt>>(res);
            List<PurchaseReceipt> resultingAccounts = new List<PurchaseReceipt>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PurchaseReceipt.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PurchaseReceipt>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<AuctionHouse.Accounts.AuctionHouse>>> GetAuctionHousesAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = AuctionHouse.Accounts.AuctionHouse.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<AuctionHouse.Accounts.AuctionHouse>>(res);
            List<AuctionHouse.Accounts.AuctionHouse> resultingAccounts = new List<AuctionHouse.Accounts.AuctionHouse>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => AuctionHouse.Accounts.AuctionHouse.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<AuctionHouse.Accounts.AuctionHouse>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Auctioneer>>> GetAuctioneersAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Auctioneer.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Auctioneer>>(res);
            List<Auctioneer> resultingAccounts = new List<Auctioneer>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Auctioneer.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Auctioneer>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<BidReceipt>> GetBidReceiptAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<BidReceipt>(res);
            var resultingAccount = BidReceipt.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<BidReceipt>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<ListingReceipt>> GetListingReceiptAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<ListingReceipt>(res);
            var resultingAccount = ListingReceipt.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<ListingReceipt>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PurchaseReceipt>> GetPurchaseReceiptAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PurchaseReceipt>(res);
            var resultingAccount = PurchaseReceipt.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PurchaseReceipt>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<AuctionHouse.Accounts.AuctionHouse>> GetAuctionHouseAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<AuctionHouse.Accounts.AuctionHouse>(res);
            var resultingAccount = AuctionHouse.Accounts.AuctionHouse.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<AuctionHouse.Accounts.AuctionHouse>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Auctioneer>> GetAuctioneerAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Auctioneer>(res);
            var resultingAccount = Auctioneer.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Auctioneer>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeBidReceiptAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, BidReceipt> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                BidReceipt parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = BidReceipt.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeListingReceiptAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, ListingReceipt> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                ListingReceipt parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = ListingReceipt.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePurchaseReceiptAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PurchaseReceipt> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PurchaseReceipt parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PurchaseReceipt.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeAuctionHouseAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, AuctionHouse.Accounts.AuctionHouse> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                AuctionHouse.Accounts.AuctionHouse parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = AuctionHouse.Accounts.AuctionHouse.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeAuctioneerAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Auctioneer> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Auctioneer parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Auctioneer.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendWithdrawFromFeeAsync(WithdrawFromFeeAccounts accounts, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.WithdrawFromFee(accounts, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendWithdrawFromTreasuryAsync(WithdrawFromTreasuryAccounts accounts, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.WithdrawFromTreasury(accounts, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUpdateAuctionHouseAsync(UpdateAuctionHouseAccounts accounts, ushort? sellerFeeBasisPoints, bool? requiresSignOff, bool? canChangeSalePrice, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.UpdateAuctionHouse(accounts, sellerFeeBasisPoints, requiresSignOff, canChangeSalePrice, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCreateAuctionHouseAsync(CreateAuctionHouseAccounts accounts, byte bump, byte feePayerBump, byte treasuryBump, ushort sellerFeeBasisPoints, bool requiresSignOff, bool canChangeSalePrice, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.CreateAuctionHouse(accounts, bump, feePayerBump, treasuryBump, sellerFeeBasisPoints, requiresSignOff, canChangeSalePrice, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendBuyAsync(BuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.Buy(accounts, tradeStateBump, escrowPaymentBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerBuyAsync(AuctioneerBuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerBuy(accounts, tradeStateBump, escrowPaymentBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendPublicBuyAsync(PublicBuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.PublicBuy(accounts, tradeStateBump, escrowPaymentBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerPublicBuyAsync(AuctioneerPublicBuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerPublicBuy(accounts, tradeStateBump, escrowPaymentBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCancelAsync(CancelAccounts accounts, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.Cancel(accounts, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerCancelAsync(AuctioneerCancelAccounts accounts, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerCancel(accounts, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDepositAsync(DepositAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.Deposit(accounts, escrowPaymentBump, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerDepositAsync(AuctioneerDepositAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerDeposit(accounts, escrowPaymentBump, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendExecuteSaleAsync(ExecuteSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.ExecuteSale(accounts, escrowPaymentBump, freeTradeStateBump, programAsSignerBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendExecutePartialSaleAsync(ExecutePartialSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, ulong? partialOrderSize, ulong? partialOrderPrice, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.ExecutePartialSale(accounts, escrowPaymentBump, freeTradeStateBump, programAsSignerBump, buyerPrice, tokenSize, partialOrderSize, partialOrderPrice, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerExecuteSaleAsync(AuctioneerExecuteSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerExecuteSale(accounts, escrowPaymentBump, freeTradeStateBump, programAsSignerBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerExecutePartialSaleAsync(AuctioneerExecutePartialSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, ulong? partialOrderSize, ulong? partialOrderPrice, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerExecutePartialSale(accounts, escrowPaymentBump, freeTradeStateBump, programAsSignerBump, buyerPrice, tokenSize, partialOrderSize, partialOrderPrice, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSellAsync(SellAccounts accounts, byte tradeStateBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.Sell(accounts, tradeStateBump, freeTradeStateBump, programAsSignerBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerSellAsync(AuctioneerSellAccounts accounts, byte tradeStateBump, byte freeTradeStateBump, byte programAsSignerBump, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerSell(accounts, tradeStateBump, freeTradeStateBump, programAsSignerBump, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendWithdrawAsync(WithdrawAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.Withdraw(accounts, escrowPaymentBump, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendAuctioneerWithdrawAsync(AuctioneerWithdrawAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.AuctioneerWithdraw(accounts, escrowPaymentBump, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCloseEscrowAccountAsync(CloseEscrowAccountAccounts accounts, byte escrowPaymentBump, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.CloseEscrowAccount(accounts, escrowPaymentBump, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDelegateAuctioneerAsync(DelegateAuctioneerAccounts accounts, AuthorityScope[] scopes, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.DelegateAuctioneer(accounts, scopes, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUpdateAuctioneerAsync(UpdateAuctioneerAccounts accounts, AuthorityScope[] scopes, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.UpdateAuctioneer(accounts, scopes, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendPrintListingReceiptAsync(PrintListingReceiptAccounts accounts, byte receiptBump, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.PrintListingReceipt(accounts, receiptBump, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCancelListingReceiptAsync(CancelListingReceiptAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.CancelListingReceipt(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendPrintBidReceiptAsync(PrintBidReceiptAccounts accounts, byte receiptBump, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.PrintBidReceipt(accounts, receiptBump, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCancelBidReceiptAsync(CancelBidReceiptAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.CancelBidReceipt(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendPrintPurchaseReceiptAsync(PrintPurchaseReceiptAccounts accounts, byte purchaseReceiptBump, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctionHouseProgram.PrintPurchaseReceipt(accounts, purchaseReceiptBump, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<AuctionHouseErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<AuctionHouseErrorKind>>{{6000U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.PublicKeyMismatch, "PublicKeyMismatch")}, {6001U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.InvalidMintAuthority, "InvalidMintAuthority")}, {6002U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.UninitializedAccount, "UninitializedAccount")}, {6003U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.IncorrectOwner, "IncorrectOwner")}, {6004U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.PublicKeysShouldBeUnique, "PublicKeysShouldBeUnique")}, {6005U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.StatementFalse, "StatementFalse")}, {6006U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.NotRentExempt, "NotRentExempt")}, {6007U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.NumericalOverflow, "NumericalOverflow")}, {6008U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.ExpectedSolAccount, "Expected a sol account but got an spl token account instead")}, {6009U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.CannotExchangeSOLForSol, "Cannot exchange sol for sol")}, {6010U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.SOLWalletMustSign, "If paying with sol, sol wallet must be signer")}, {6011U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.CannotTakeThisActionWithoutAuctionHouseSignOff, "Cannot take this action without auction house signing too")}, {6012U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.NoPayerPresent, "No payer present on this txn")}, {6013U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.DerivedKeyInvalid, "Derived key invalid")}, {6014U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.MetadataDoesntExist, "Metadata doesn't exist")}, {6015U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.InvalidTokenAmount, "Invalid token amount")}, {6016U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.BothPartiesNeedToAgreeToSale, "Both parties need to agree to this sale")}, {6017U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.CannotMatchFreeSalesWithoutAuctionHouseOrSellerSignoff, "Cannot match free sales unless the auction house or seller signs off")}, {6018U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.SaleRequiresSigner, "This sale requires a signer")}, {6019U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.OldSellerNotInitialized, "Old seller not initialized")}, {6020U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.SellerATACannotHaveDelegate, "Seller ata cannot have a delegate set")}, {6021U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.BuyerATACannotHaveDelegate, "Buyer ata cannot have a delegate set")}, {6022U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.NoValidSignerPresent, "No valid signer present")}, {6023U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.InvalidBasisPoints, "BP must be less than or equal to 10000")}, {6024U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.TradeStateDoesntExist, "The trade state account does not exist")}, {6025U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.TradeStateIsNotEmpty, "The trade state is not empty")}, {6026U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.ReceiptIsEmpty, "The receipt is empty")}, {6027U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.InstructionMismatch, "The instruction does not match")}, {6028U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.InvalidAuctioneer, "Invalid Auctioneer for this Auction House instance.")}, {6029U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.MissingAuctioneerScope, "The Auctioneer does not have the correct scope for this action.")}, {6030U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.MustUseAuctioneerHandler, "Must use auctioneer handler.")}, {6031U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.NoAuctioneerProgramSet, "No Auctioneer program set.")}, {6032U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.TooManyScopes, "Too many scopes.")}, {6033U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.AuctionHouseNotDelegated, "Auction House not delegated.")}, {6034U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.BumpSeedNotInHashMap, "Bump seed not in hash map.")}, {6035U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.EscrowUnderRentExemption, "The instruction would drain the escrow below rent exemption threshold")}, {6036U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.InvalidSeedsOrAuctionHouseNotDelegated, "Invalid seeds or Auction House not delegated")}, {6037U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.BuyerTradeStateNotValid, "The buyer trade state was unable to be initialized.")}, {6038U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.MissingElementForPartialOrder, "Partial order size and price must both be provided in a partial buy.")}, {6039U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.NotEnoughTokensAvailableForPurchase, "Amount of tokens available for purchase is less than the partial order amount.")}, {6040U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.PartialPriceMismatch, "Calculated partial price does not not partial price that was provided.")}, {6041U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.AuctionHouseAlreadyDelegated, "Auction House already delegated.")}, {6042U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.AuctioneerAuthorityMismatch, "Auctioneer Authority Mismatch")}, {6043U, new ProgramError<AuctionHouseErrorKind>(AuctionHouseErrorKind.InsufficientFunds, "Insufficient funds in escrow account to purchase.")}, };
        }
    }

    namespace Program
    {
        public class WithdrawFromFeeAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey FeeWithdrawalDestination { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class WithdrawFromTreasuryAccounts
        {
            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey TreasuryWithdrawalDestination { get; set; }

            public PublicKey AuctionHouseTreasury { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class UpdateAuctionHouseAccounts
        {
            public PublicKey TreasuryMint { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey NewAuthority { get; set; }

            public PublicKey FeeWithdrawalDestination { get; set; }

            public PublicKey TreasuryWithdrawalDestination { get; set; }

            public PublicKey TreasuryWithdrawalDestinationOwner { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class CreateAuctionHouseAccounts
        {
            public PublicKey TreasuryMint { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey FeeWithdrawalDestination { get; set; }

            public PublicKey TreasuryWithdrawalDestination { get; set; }

            public PublicKey TreasuryWithdrawalDestinationOwner { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctionHouseTreasury { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class BuyAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class AuctioneerBuyAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class PublicBuyAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class AuctioneerPublicBuyAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class CancelAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey TradeState { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class AuctioneerCancelAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey TradeState { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class DepositAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class AuctioneerDepositAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class ExecuteSaleAccounts
        {
            public PublicKey Buyer { get; set; }

            public PublicKey Seller { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey SellerPaymentReceiptAccount { get; set; }

            public PublicKey BuyerReceiptTokenAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctionHouseTreasury { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey SellerTradeState { get; set; }

            public PublicKey FreeTradeState { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class ExecutePartialSaleAccounts
        {
            public PublicKey Buyer { get; set; }

            public PublicKey Seller { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey SellerPaymentReceiptAccount { get; set; }

            public PublicKey BuyerReceiptTokenAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctionHouseTreasury { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey SellerTradeState { get; set; }

            public PublicKey FreeTradeState { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class AuctioneerExecuteSaleAccounts
        {
            public PublicKey Buyer { get; set; }

            public PublicKey Seller { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey SellerPaymentReceiptAccount { get; set; }

            public PublicKey BuyerReceiptTokenAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctionHouseTreasury { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey SellerTradeState { get; set; }

            public PublicKey FreeTradeState { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class AuctioneerExecutePartialSaleAccounts
        {
            public PublicKey Buyer { get; set; }

            public PublicKey Seller { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey SellerPaymentReceiptAccount { get; set; }

            public PublicKey BuyerReceiptTokenAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctionHouseTreasury { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public PublicKey SellerTradeState { get; set; }

            public PublicKey FreeTradeState { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class SellAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey SellerTradeState { get; set; }

            public PublicKey FreeSellerTradeState { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class AuctioneerSellAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey SellerTradeState { get; set; }

            public PublicKey FreeSellerTradeState { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class WithdrawAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey ReceiptAccount { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class AuctioneerWithdrawAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey ReceiptAccount { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class CloseEscrowAccountAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class DelegateAuctioneerAccounts
        {
            public PublicKey AuctionHouse { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class UpdateAuctioneerAccounts
        {
            public PublicKey AuctionHouse { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class PrintListingReceiptAccounts
        {
            public PublicKey Receipt { get; set; }

            public PublicKey Bookkeeper { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey Instruction { get; set; }
        }

        public class CancelListingReceiptAccounts
        {
            public PublicKey Receipt { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Instruction { get; set; }
        }

        public class PrintBidReceiptAccounts
        {
            public PublicKey Receipt { get; set; }

            public PublicKey Bookkeeper { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey Instruction { get; set; }
        }

        public class CancelBidReceiptAccounts
        {
            public PublicKey Receipt { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Instruction { get; set; }
        }

        public class PrintPurchaseReceiptAccounts
        {
            public PublicKey PurchaseReceipt { get; set; }

            public PublicKey ListingReceipt { get; set; }

            public PublicKey BidReceipt { get; set; }

            public PublicKey Bookkeeper { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey Instruction { get; set; }
        }

        public static class AuctionHouseProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction WithdrawFromFee(WithdrawFromFeeAccounts accounts, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FeeWithdrawalDestination, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(4256943025411772595UL, offset);
                offset += 8;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction WithdrawFromTreasury(WithdrawFromTreasuryAccounts accounts, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreasuryWithdrawalDestination, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseTreasury, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12253248092804391936UL, offset);
                offset += 8;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateAuctionHouse(UpdateAuctionHouseAccounts accounts, ushort? sellerFeeBasisPoints, bool? requiresSignOff, bool? canChangeSalePrice, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FeeWithdrawalDestination, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreasuryWithdrawalDestination, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryWithdrawalDestinationOwner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15849575501573314388UL, offset);
                offset += 8;
                if (sellerFeeBasisPoints != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU16(sellerFeeBasisPoints.Value, offset);
                    offset += 2;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (requiresSignOff != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(requiresSignOff.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (canChangeSalePrice != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(canChangeSalePrice.Value, offset);
                    offset += 1;
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

            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateAuctionHouse(CreateAuctionHouseAccounts accounts, byte bump, byte feePayerBump, byte treasuryBump, ushort sellerFeeBasisPoints, bool requiresSignOff, bool canChangeSalePrice, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FeeWithdrawalDestination, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreasuryWithdrawalDestination, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryWithdrawalDestinationOwner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseTreasury, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17403825381545493213UL, offset);
                offset += 8;
                _data.WriteU8(bump, offset);
                offset += 1;
                _data.WriteU8(feePayerBump, offset);
                offset += 1;
                _data.WriteU8(treasuryBump, offset);
                offset += 1;
                _data.WriteU16(sellerFeeBasisPoints, offset);
                offset += 2;
                _data.WriteBool(requiresSignOff, offset);
                offset += 1;
                _data.WriteBool(canChangeSalePrice, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Buy(BuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16927863322537952870UL, offset);
                offset += 8;
                _data.WriteU8(tradeStateBump, offset);
                offset += 1;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerBuy(AuctioneerBuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15000699694727129617UL, offset);
                offset += 8;
                _data.WriteU8(tradeStateBump, offset);
                offset += 1;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction PublicBuy(PublicBuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12326578860498506921UL, offset);
                offset += 8;
                _data.WriteU8(tradeStateBump, offset);
                offset += 1;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerPublicBuy(AuctioneerPublicBuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9139261969753436125UL, offset);
                offset += 8;
                _data.WriteU8(tradeStateBump, offset);
                offset += 1;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Cancel(CancelAccounts accounts, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13753127788127181800UL, offset);
                offset += 8;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerCancel(AuctioneerCancelAccounts accounts, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15510621914255614405UL, offset);
                offset += 8;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Deposit(DepositAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13182846803881894898UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerDeposit(AuctioneerDepositAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9167549250117401167UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ExecuteSale(ExecuteSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Buyer, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Seller, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerPaymentReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerReceiptTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseTreasury, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(442251406432881189UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU8(freeTradeStateBump, offset);
                offset += 1;
                _data.WriteU8(programAsSignerBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ExecutePartialSale(ExecutePartialSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, ulong? partialOrderSize, ulong? partialOrderPrice, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Buyer, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Seller, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerPaymentReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerReceiptTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseTreasury, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9640979960313352867UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU8(freeTradeStateBump, offset);
                offset += 1;
                _data.WriteU8(programAsSignerBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                if (partialOrderSize != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(partialOrderSize.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (partialOrderPrice != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(partialOrderPrice.Value, offset);
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

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerExecuteSale(AuctioneerExecuteSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Buyer, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Seller, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerPaymentReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerReceiptTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseTreasury, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(3828952466324487492UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU8(freeTradeStateBump, offset);
                offset += 1;
                _data.WriteU8(programAsSignerBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerExecutePartialSale(AuctioneerExecutePartialSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, ulong? partialOrderSize, ulong? partialOrderPrice, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Buyer, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Seller, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerPaymentReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerReceiptTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseTreasury, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(3897178974466223113UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU8(freeTradeStateBump, offset);
                offset += 1;
                _data.WriteU8(programAsSignerBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                if (partialOrderSize != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(partialOrderSize.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (partialOrderPrice != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(partialOrderPrice.Value, offset);
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

            public static Solana.Unity.Rpc.Models.TransactionInstruction Sell(SellAccounts accounts, byte tradeStateBump, byte freeTradeStateBump, byte programAsSignerBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeSellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12502976635542562355UL, offset);
                offset += 8;
                _data.WriteU8(tradeStateBump, offset);
                offset += 1;
                _data.WriteU8(freeTradeStateBump, offset);
                offset += 1;
                _data.WriteU8(programAsSignerBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerSell(AuctioneerSellAccounts accounts, byte tradeStateBump, byte freeTradeStateBump, byte programAsSignerBump, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeSellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13194081782167649531UL, offset);
                offset += 8;
                _data.WriteU8(tradeStateBump, offset);
                offset += 1;
                _data.WriteU8(freeTradeStateBump, offset);
                offset += 1;
                _data.WriteU8(programAsSignerBump, offset);
                offset += 1;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Withdraw(WithdrawAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2495396153584390839UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AuctioneerWithdraw(AuctioneerWithdrawAccounts accounts, byte escrowPaymentBump, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17056415642336077397UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CloseEscrowAccount(CloseEscrowAccountAccounts accounts, byte escrowPaymentBump, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(3103629459430845137UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DelegateAuctioneer(DelegateAuctioneerAccounts accounts, AuthorityScope[] scopes, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16067626630961214058UL, offset);
                offset += 8;
                _data.WriteS32(scopes.Length, offset);
                offset += 4;
                foreach (var scopesElement in scopes)
                {
                    _data.WriteU8((byte)scopesElement, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateAuctioneer(UpdateAuctioneerAccounts accounts, AuthorityScope[] scopes, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15035329336285658983UL, offset);
                offset += 8;
                _data.WriteS32(scopes.Length, offset);
                offset += 4;
                foreach (var scopesElement in scopes)
                {
                    _data.WriteU8((byte)scopesElement, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction PrintListingReceipt(PrintListingReceiptAccounts accounts, byte receiptBump, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Receipt, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Bookkeeper, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Instruction, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2000687075873811407UL, offset);
                offset += 8;
                _data.WriteU8(receiptBump, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CancelListingReceipt(CancelListingReceiptAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Receipt, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Instruction, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(818456623680469931UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction PrintBidReceipt(PrintBidReceiptAccounts accounts, byte receiptBump, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Receipt, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Bookkeeper, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Instruction, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15727767197790697822UL, offset);
                offset += 8;
                _data.WriteU8(receiptBump, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CancelBidReceipt(CancelBidReceiptAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Receipt, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Instruction, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(3148063267756928246UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction PrintPurchaseReceipt(PrintPurchaseReceiptAccounts accounts, byte purchaseReceiptBump, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PurchaseReceipt, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ListingReceipt, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BidReceipt, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Bookkeeper, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Instruction, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10332445790973958883UL, offset);
                offset += 8;
                _data.WriteU8(purchaseReceiptBump, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}