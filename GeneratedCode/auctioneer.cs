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
using Auctioneer;
using Auctioneer.Program;
using Auctioneer.Errors;
using Auctioneer.Accounts;
using Auctioneer.Types;

namespace Auctioneer
{
    namespace Accounts
    {
        public partial class AuctioneerAuthority
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 920233374776249060UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{228, 74, 255, 245, 96, 83, 197, 12};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "fBjDC8APBxF";
            public byte Bump { get; set; }

            public static AuctioneerAuthority Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                AuctioneerAuthority result = new AuctioneerAuthority();
                result.Bump = _data.GetU8(offset);
                offset += 1;
                return result;
            }
        }

        public partial class ListingConfig
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 8338465850941686967UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{183, 196, 26, 41, 131, 46, 184, 115};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "Xjm7kw3Unhx";
            public ListingConfigVersion Version { get; set; }

            public long StartTime { get; set; }

            public long EndTime { get; set; }

            public Bid HighestBid { get; set; }

            public byte Bump { get; set; }

            public ulong ReservePrice { get; set; }

            public ulong MinBidIncrement { get; set; }

            public uint TimeExtPeriod { get; set; }

            public uint TimeExtDelta { get; set; }

            public bool AllowHighBidCancel { get; set; }

            public static ListingConfig Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                ListingConfig result = new ListingConfig();
                result.Version = (ListingConfigVersion)_data.GetU8(offset);
                offset += 1;
                result.StartTime = _data.GetS64(offset);
                offset += 8;
                result.EndTime = _data.GetS64(offset);
                offset += 8;
                offset += Bid.Deserialize(_data, offset, out var resultHighestBid);
                result.HighestBid = resultHighestBid;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                result.ReservePrice = _data.GetU64(offset);
                offset += 8;
                result.MinBidIncrement = _data.GetU64(offset);
                offset += 8;
                result.TimeExtPeriod = _data.GetU32(offset);
                offset += 4;
                result.TimeExtDelta = _data.GetU32(offset);
                offset += 4;
                result.AllowHighBidCancel = _data.GetBool(offset);
                offset += 1;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum AuctioneerErrorKind : uint
        {
            BumpSeedNotInHashMap = 6000U,
            AuctionNotStarted = 6001U,
            AuctionEnded = 6002U,
            AuctionActive = 6003U,
            BidTooLow = 6004U,
            SignerNotAuth = 6005U,
            NotHighestBidder = 6006U,
            BelowReservePrice = 6007U,
            BelowBidIncrement = 6008U,
            CannotCancelHighestBid = 6009U
        }
    }

    namespace Types
    {
        public partial class Bid
        {
            public ListingConfigVersion Version { get; set; }

            public ulong Amount { get; set; }

            public PublicKey BuyerTradeState { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8((byte)Version, offset);
                offset += 1;
                _data.WriteU64(Amount, offset);
                offset += 8;
                _data.WritePubKey(BuyerTradeState, offset);
                offset += 32;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Bid result)
            {
                int offset = initialOffset;
                result = new Bid();
                result.Version = (ListingConfigVersion)_data.GetU8(offset);
                offset += 1;
                result.Amount = _data.GetU64(offset);
                offset += 8;
                result.BuyerTradeState = _data.GetPubKey(offset);
                offset += 32;
                return offset - initialOffset;
            }
        }

        public enum ListingConfigVersion : byte
        {
            V0
        }
    }

    public partial class AuctioneerClient : TransactionalBaseClient<AuctioneerErrorKind>
    {
        public AuctioneerClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<AuctioneerAuthority>>> GetAuctioneerAuthoritysAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = AuctioneerAuthority.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<AuctioneerAuthority>>(res);
            List<AuctioneerAuthority> resultingAccounts = new List<AuctioneerAuthority>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => AuctioneerAuthority.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<AuctioneerAuthority>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ListingConfig>>> GetListingConfigsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = ListingConfig.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ListingConfig>>(res);
            List<ListingConfig> resultingAccounts = new List<ListingConfig>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => ListingConfig.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ListingConfig>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<AuctioneerAuthority>> GetAuctioneerAuthorityAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<AuctioneerAuthority>(res);
            var resultingAccount = AuctioneerAuthority.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<AuctioneerAuthority>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<ListingConfig>> GetListingConfigAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<ListingConfig>(res);
            var resultingAccount = ListingConfig.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<ListingConfig>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeAuctioneerAuthorityAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, AuctioneerAuthority> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                AuctioneerAuthority parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = AuctioneerAuthority.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeListingConfigAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, ListingConfig> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                ListingConfig parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = ListingConfig.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendAuthorizeAsync(AuthorizeAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctioneerProgram.Authorize(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendWithdrawAsync(WithdrawAccounts accounts, byte escrowPaymentBump, byte auctioneerAuthorityBump, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctioneerProgram.Withdraw(accounts, escrowPaymentBump, auctioneerAuthorityBump, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDepositAsync(DepositAccounts accounts, byte escrowPaymentBump, byte auctioneerAuthorityBump, ulong amount, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctioneerProgram.Deposit(accounts, escrowPaymentBump, auctioneerAuthorityBump, amount, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCancelAsync(CancelAccounts accounts, byte auctioneerAuthorityBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctioneerProgram.Cancel(accounts, auctioneerAuthorityBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendExecuteSaleAsync(ExecuteSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, byte auctioneerAuthorityBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctioneerProgram.ExecuteSale(accounts, escrowPaymentBump, freeTradeStateBump, programAsSignerBump, auctioneerAuthorityBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSellAsync(SellAccounts accounts, byte tradeStateBump, byte freeTradeStateBump, byte programAsSignerBump, byte auctioneerAuthorityBump, ulong tokenSize, long startTime, long endTime, ulong? reservePrice, ulong? minBidIncrement, uint? timeExtPeriod, uint? timeExtDelta, bool? allowHighBidCancel, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctioneerProgram.Sell(accounts, tradeStateBump, freeTradeStateBump, programAsSignerBump, auctioneerAuthorityBump, tokenSize, startTime, endTime, reservePrice, minBidIncrement, timeExtPeriod, timeExtDelta, allowHighBidCancel, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendBuyAsync(BuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, byte auctioneerAuthorityBump, ulong buyerPrice, ulong tokenSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.AuctioneerProgram.Buy(accounts, tradeStateBump, escrowPaymentBump, auctioneerAuthorityBump, buyerPrice, tokenSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<AuctioneerErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<AuctioneerErrorKind>>{{6000U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.BumpSeedNotInHashMap, "Bump seed not in hash map")}, {6001U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.AuctionNotStarted, "Auction has not started yet")}, {6002U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.AuctionEnded, "Auction has ended")}, {6003U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.AuctionActive, "Auction has not ended yet")}, {6004U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.BidTooLow, "The bid was lower than the highest bid")}, {6005U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.SignerNotAuth, "The signer must be the Auction House authority")}, {6006U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.NotHighestBidder, "Execute Sale must be run on the highest bidder")}, {6007U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.BelowReservePrice, "The bid price must be greater than the reserve price")}, {6008U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.BelowBidIncrement, "The bid must match the highest bid plus the minimum bid increment")}, {6009U, new ProgramError<AuctioneerErrorKind>(AuctioneerErrorKind.CannotCancelHighestBid, "The highest bidder is not allowed to cancel")}, };
        }
    }

    namespace Program
    {
        public class AuthorizeAccounts
        {
            public PublicKey Wallet { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class WithdrawAccounts
        {
            public PublicKey AuctionHouseProgram { get; set; }

            public PublicKey Wallet { get; set; }

            public PublicKey ReceiptAccount { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class DepositAccounts
        {
            public PublicKey AuctionHouseProgram { get; set; }

            public PublicKey Wallet { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey EscrowPaymentAccount { get; set; }

            public PublicKey TreasuryMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class CancelAccounts
        {
            public PublicKey AuctionHouseProgram { get; set; }

            public PublicKey ListingConfig { get; set; }

            public PublicKey Seller { get; set; }

            public PublicKey Wallet { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey TradeState { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ExecuteSaleAccounts
        {
            public PublicKey AuctionHouseProgram { get; set; }

            public PublicKey ListingConfig { get; set; }

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

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class SellAccounts
        {
            public PublicKey AuctionHouseProgram { get; set; }

            public PublicKey ListingConfig { get; set; }

            public PublicKey Wallet { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey AuctionHouse { get; set; }

            public PublicKey AuctionHouseFeeAccount { get; set; }

            public PublicKey SellerTradeState { get; set; }

            public PublicKey FreeSellerTradeState { get; set; }

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey ProgramAsSigner { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class BuyAccounts
        {
            public PublicKey AuctionHouseProgram { get; set; }

            public PublicKey ListingConfig { get; set; }

            public PublicKey Seller { get; set; }

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

            public PublicKey AuctioneerAuthority { get; set; }

            public PublicKey AhAuctioneerPda { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public static class AuctioneerProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction Authorize(AuthorizeAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(8678869534140449197UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Withdraw(WithdrawAccounts accounts, byte escrowPaymentBump, byte auctioneerAuthorityBump, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouseProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2495396153584390839UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU8(auctioneerAuthorityBump, offset);
                offset += 1;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Deposit(DepositAccounts accounts, byte escrowPaymentBump, byte auctioneerAuthorityBump, ulong amount, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouseProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13182846803881894898UL, offset);
                offset += 8;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU8(auctioneerAuthorityBump, offset);
                offset += 1;
                _data.WriteU64(amount, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Cancel(CancelAccounts accounts, byte auctioneerAuthorityBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouseProgram, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ListingConfig, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Seller, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13753127788127181800UL, offset);
                offset += 8;
                _data.WriteU8(auctioneerAuthorityBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ExecuteSale(ExecuteSaleAccounts accounts, byte escrowPaymentBump, byte freeTradeStateBump, byte programAsSignerBump, byte auctioneerAuthorityBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouseProgram, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ListingConfig, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Buyer, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Seller, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerPaymentReceiptAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerReceiptTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseTreasury, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
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
                _data.WriteU8(auctioneerAuthorityBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Sell(SellAccounts accounts, byte tradeStateBump, byte freeTradeStateBump, byte programAsSignerBump, byte auctioneerAuthorityBump, ulong tokenSize, long startTime, long endTime, ulong? reservePrice, ulong? minBidIncrement, uint? timeExtPeriod, uint? timeExtDelta, bool? allowHighBidCancel, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouseProgram, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ListingConfig, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Wallet, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FreeSellerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ProgramAsSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
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
                _data.WriteU8(auctioneerAuthorityBump, offset);
                offset += 1;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                _data.WriteS64(startTime, offset);
                offset += 8;
                _data.WriteS64(endTime, offset);
                offset += 8;
                if (reservePrice != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(reservePrice.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (minBidIncrement != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(minBidIncrement.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (timeExtPeriod != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU32(timeExtPeriod.Value, offset);
                    offset += 4;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (timeExtDelta != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU32(timeExtDelta.Value, offset);
                    offset += 4;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (allowHighBidCancel != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(allowHighBidCancel.Value, offset);
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

            public static Solana.Unity.Rpc.Models.TransactionInstruction Buy(BuyAccounts accounts, byte tradeStateBump, byte escrowPaymentBump, byte auctioneerAuthorityBump, ulong buyerPrice, ulong tokenSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouseProgram, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ListingConfig, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Seller, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Wallet, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EscrowPaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctionHouse, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.AuctionHouseFeeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.BuyerTradeState, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AuctioneerAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AhAuctioneerPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16927863322537952870UL, offset);
                offset += 8;
                _data.WriteU8(tradeStateBump, offset);
                offset += 1;
                _data.WriteU8(escrowPaymentBump, offset);
                offset += 1;
                _data.WriteU8(auctioneerAuthorityBump, offset);
                offset += 1;
                _data.WriteU64(buyerPrice, offset);
                offset += 8;
                _data.WriteU64(tokenSize, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}