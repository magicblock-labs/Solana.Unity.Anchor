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
using Hydra;
using Hydra.Program;
using Hydra.Errors;
using Hydra.Accounts;
using Hydra.Types;

namespace Hydra
{
    namespace Accounts
    {
        public partial class Fanout
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 11262111641372878244UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{164, 101, 210, 92, 222, 14, 75, 156};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "UVs7a3vhzRR";
            public PublicKey Authority { get; set; }

            public string Name { get; set; }

            public PublicKey AccountKey { get; set; }

            public ulong TotalShares { get; set; }

            public ulong TotalMembers { get; set; }

            public ulong TotalInflow { get; set; }

            public ulong LastSnapshotAmount { get; set; }

            public byte BumpSeed { get; set; }

            public byte AccountOwnerBumpSeed { get; set; }

            public ulong TotalAvailableShares { get; set; }

            public MembershipModel MembershipModel { get; set; }

            public PublicKey MembershipMint { get; set; }

            public ulong? TotalStakedShares { get; set; }

            public static Fanout Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Fanout result = new Fanout();
                result.Authority = _data.GetPubKey(offset);
                offset += 32;
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                result.AccountKey = _data.GetPubKey(offset);
                offset += 32;
                result.TotalShares = _data.GetU64(offset);
                offset += 8;
                result.TotalMembers = _data.GetU64(offset);
                offset += 8;
                result.TotalInflow = _data.GetU64(offset);
                offset += 8;
                result.LastSnapshotAmount = _data.GetU64(offset);
                offset += 8;
                result.BumpSeed = _data.GetU8(offset);
                offset += 1;
                result.AccountOwnerBumpSeed = _data.GetU8(offset);
                offset += 1;
                result.TotalAvailableShares = _data.GetU64(offset);
                offset += 8;
                result.MembershipModel = (MembershipModel)_data.GetU8(offset);
                offset += 1;
                if (_data.GetBool(offset++))
                {
                    result.MembershipMint = _data.GetPubKey(offset);
                    offset += 32;
                }

                if (_data.GetBool(offset++))
                {
                    result.TotalStakedShares = _data.GetU64(offset);
                    offset += 8;
                }

                return result;
            }
        }

        public partial class FanoutMint
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 15635030446569071666UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{50, 164, 42, 108, 90, 201, 250, 216};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "9UHTdNxQjN7";
            public PublicKey Mint { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey TokenAccount { get; set; }

            public ulong TotalInflow { get; set; }

            public ulong LastSnapshotAmount { get; set; }

            public byte BumpSeed { get; set; }

            public static FanoutMint Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                FanoutMint result = new FanoutMint();
                result.Mint = _data.GetPubKey(offset);
                offset += 32;
                result.Fanout = _data.GetPubKey(offset);
                offset += 32;
                result.TokenAccount = _data.GetPubKey(offset);
                offset += 32;
                result.TotalInflow = _data.GetU64(offset);
                offset += 8;
                result.LastSnapshotAmount = _data.GetU64(offset);
                offset += 8;
                result.BumpSeed = _data.GetU8(offset);
                offset += 1;
                return result;
            }
        }

        public partial class FanoutMembershipVoucher
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 9057475975415742137UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{185, 62, 74, 60, 105, 158, 178, 125};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "Xz6MLQQgWtt";
            public PublicKey Fanout { get; set; }

            public ulong TotalInflow { get; set; }

            public ulong LastInflow { get; set; }

            public byte BumpSeed { get; set; }

            public PublicKey MembershipKey { get; set; }

            public ulong Shares { get; set; }

            public static FanoutMembershipVoucher Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                FanoutMembershipVoucher result = new FanoutMembershipVoucher();
                result.Fanout = _data.GetPubKey(offset);
                offset += 32;
                result.TotalInflow = _data.GetU64(offset);
                offset += 8;
                result.LastInflow = _data.GetU64(offset);
                offset += 8;
                result.BumpSeed = _data.GetU8(offset);
                offset += 1;
                result.MembershipKey = _data.GetPubKey(offset);
                offset += 32;
                result.Shares = _data.GetU64(offset);
                offset += 8;
                return result;
            }
        }

        public partial class FanoutMembershipMintVoucher
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 13078016346526458297UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{185, 33, 118, 173, 147, 114, 126, 181};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "XxzzXkeD1WL";
            public PublicKey Fanout { get; set; }

            public PublicKey FanoutMint { get; set; }

            public ulong LastInflow { get; set; }

            public byte BumpSeed { get; set; }

            public static FanoutMembershipMintVoucher Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                FanoutMembershipMintVoucher result = new FanoutMembershipMintVoucher();
                result.Fanout = _data.GetPubKey(offset);
                offset += 32;
                result.FanoutMint = _data.GetPubKey(offset);
                offset += 32;
                result.LastInflow = _data.GetU64(offset);
                offset += 8;
                result.BumpSeed = _data.GetU8(offset);
                offset += 1;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum HydraErrorKind : uint
        {
            BadArtithmetic = 6000U,
            InvalidAuthority = 6001U,
            InsufficientShares = 6002U,
            SharesArentAtMax = 6003U,
            NewMintAccountRequired = 6004U,
            MintAccountRequired = 6005U,
            InvalidMembershipModel = 6006U,
            InvalidMembershipVoucher = 6007U,
            MintDoesNotMatch = 6008U,
            InvalidHoldingAccount = 6009U,
            HoldingAccountMustBeAnATA = 6010U,
            DerivedKeyInvalid = 6011U,
            IncorrectOwner = 6012U,
            WalletDoesNotOwnMembershipToken = 6013U,
            InvalidMetadata = 6014U,
            NumericalOverflow = 6015U,
            InsufficientBalanceToDistribute = 6016U,
            InvalidFanoutForMint = 6017U,
            MustDistribute = 6018U,
            InvalidStakeAta = 6019U,
            CannotTransferToSelf = 6020U,
            TransferNotSupported = 6021U,
            RemoveNotSupported = 6022U,
            RemoveSharesMustBeZero = 6023U,
            InvalidCloseAccountDestination = 6024U
        }
    }

    namespace Types
    {
        public partial class AddMemberArgs
        {
            public ulong Shares { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(Shares, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out AddMemberArgs result)
            {
                int offset = initialOffset;
                result = new AddMemberArgs();
                result.Shares = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class InitializeFanoutArgs
        {
            public byte BumpSeed { get; set; }

            public byte NativeAccountBumpSeed { get; set; }

            public string Name { get; set; }

            public ulong TotalShares { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8(BumpSeed, offset);
                offset += 1;
                _data.WriteU8(NativeAccountBumpSeed, offset);
                offset += 1;
                offset += _data.WriteBorshString(Name, offset);
                _data.WriteU64(TotalShares, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out InitializeFanoutArgs result)
            {
                int offset = initialOffset;
                result = new InitializeFanoutArgs();
                result.BumpSeed = _data.GetU8(offset);
                offset += 1;
                result.NativeAccountBumpSeed = _data.GetU8(offset);
                offset += 1;
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                result.TotalShares = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public enum MembershipModel : byte
        {
            Wallet,
            Token,
            NFT
        }
    }

    public partial class HydraClient : TransactionalBaseClient<HydraErrorKind>
    {
        public HydraClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Fanout>>> GetFanoutsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Fanout.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Fanout>>(res);
            List<Fanout> resultingAccounts = new List<Fanout>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Fanout.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Fanout>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMint>>> GetFanoutMintsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = FanoutMint.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMint>>(res);
            List<FanoutMint> resultingAccounts = new List<FanoutMint>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => FanoutMint.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMint>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMembershipVoucher>>> GetFanoutMembershipVouchersAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = FanoutMembershipVoucher.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMembershipVoucher>>(res);
            List<FanoutMembershipVoucher> resultingAccounts = new List<FanoutMembershipVoucher>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => FanoutMembershipVoucher.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMembershipVoucher>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMembershipMintVoucher>>> GetFanoutMembershipMintVouchersAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = FanoutMembershipMintVoucher.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMembershipMintVoucher>>(res);
            List<FanoutMembershipMintVoucher> resultingAccounts = new List<FanoutMembershipMintVoucher>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => FanoutMembershipMintVoucher.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<FanoutMembershipMintVoucher>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Fanout>> GetFanoutAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Fanout>(res);
            var resultingAccount = Fanout.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Fanout>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMint>> GetFanoutMintAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMint>(res);
            var resultingAccount = FanoutMint.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMint>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMembershipVoucher>> GetFanoutMembershipVoucherAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMembershipVoucher>(res);
            var resultingAccount = FanoutMembershipVoucher.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMembershipVoucher>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMembershipMintVoucher>> GetFanoutMembershipMintVoucherAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMembershipMintVoucher>(res);
            var resultingAccount = FanoutMembershipMintVoucher.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<FanoutMembershipMintVoucher>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeFanoutAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Fanout> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Fanout parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Fanout.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeFanoutMintAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, FanoutMint> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                FanoutMint parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = FanoutMint.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeFanoutMembershipVoucherAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, FanoutMembershipVoucher> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                FanoutMembershipVoucher parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = FanoutMembershipVoucher.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeFanoutMembershipMintVoucherAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, FanoutMembershipMintVoucher> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                FanoutMembershipMintVoucher parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = FanoutMembershipMintVoucher.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendProcessInitAsync(ProcessInitAccounts accounts, InitializeFanoutArgs args, MembershipModel model, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessInit(accounts, args, model, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessInitForMintAsync(ProcessInitForMintAccounts accounts, byte bumpSeed, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessInitForMint(accounts, bumpSeed, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessAddMemberWalletAsync(ProcessAddMemberWalletAccounts accounts, AddMemberArgs args, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessAddMemberWallet(accounts, args, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessAddMemberNftAsync(ProcessAddMemberNftAccounts accounts, AddMemberArgs args, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessAddMemberNft(accounts, args, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessSetTokenMemberStakeAsync(ProcessSetTokenMemberStakeAccounts accounts, ulong shares, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessSetTokenMemberStake(accounts, shares, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessSetForTokenMemberStakeAsync(ProcessSetForTokenMemberStakeAccounts accounts, ulong shares, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessSetForTokenMemberStake(accounts, shares, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessDistributeNftAsync(ProcessDistributeNftAccounts accounts, bool distributeForMint, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessDistributeNft(accounts, distributeForMint, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessDistributeWalletAsync(ProcessDistributeWalletAccounts accounts, bool distributeForMint, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessDistributeWallet(accounts, distributeForMint, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessDistributeTokenAsync(ProcessDistributeTokenAccounts accounts, bool distributeForMint, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessDistributeToken(accounts, distributeForMint, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessSignMetadataAsync(ProcessSignMetadataAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessSignMetadata(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessTransferSharesAsync(ProcessTransferSharesAccounts accounts, ulong shares, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessTransferShares(accounts, shares, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessUnstakeAsync(ProcessUnstakeAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessUnstake(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendProcessRemoveMemberAsync(ProcessRemoveMemberAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.HydraProgram.ProcessRemoveMember(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<HydraErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<HydraErrorKind>>{{6000U, new ProgramError<HydraErrorKind>(HydraErrorKind.BadArtithmetic, "Encountered an arithmetic error")}, {6001U, new ProgramError<HydraErrorKind>(HydraErrorKind.InvalidAuthority, "Invalid authority")}, {6002U, new ProgramError<HydraErrorKind>(HydraErrorKind.InsufficientShares, "Not Enough Available Shares")}, {6003U, new ProgramError<HydraErrorKind>(HydraErrorKind.SharesArentAtMax, "All available shares must be assigned to a member")}, {6004U, new ProgramError<HydraErrorKind>(HydraErrorKind.NewMintAccountRequired, "A New mint account must be provided")}, {6005U, new ProgramError<HydraErrorKind>(HydraErrorKind.MintAccountRequired, "A Token type Fanout requires a Membership Mint")}, {6006U, new ProgramError<HydraErrorKind>(HydraErrorKind.InvalidMembershipModel, "Invalid Membership Model")}, {6007U, new ProgramError<HydraErrorKind>(HydraErrorKind.InvalidMembershipVoucher, "Invalid Membership Voucher")}, {6008U, new ProgramError<HydraErrorKind>(HydraErrorKind.MintDoesNotMatch, "Invalid Mint for the config")}, {6009U, new ProgramError<HydraErrorKind>(HydraErrorKind.InvalidHoldingAccount, "Holding account does not match the config")}, {6010U, new ProgramError<HydraErrorKind>(HydraErrorKind.HoldingAccountMustBeAnATA, "A Mint holding account must be an ata for the mint owned by the config")}, {6013U, new ProgramError<HydraErrorKind>(HydraErrorKind.WalletDoesNotOwnMembershipToken, "Wallet Does not Own Membership Token")}, {6014U, new ProgramError<HydraErrorKind>(HydraErrorKind.InvalidMetadata, "The Metadata specified is not valid Token Metadata")}, {6016U, new ProgramError<HydraErrorKind>(HydraErrorKind.InsufficientBalanceToDistribute, "Not enough new balance to distribute")}, {6018U, new ProgramError<HydraErrorKind>(HydraErrorKind.MustDistribute, "This operation must be the instruction right after a distrobution on the same accounts.")}, {6021U, new ProgramError<HydraErrorKind>(HydraErrorKind.TransferNotSupported, "Transfer is not supported on this membership model")}, {6022U, new ProgramError<HydraErrorKind>(HydraErrorKind.RemoveNotSupported, "Remove is not supported on this membership model")}, {6023U, new ProgramError<HydraErrorKind>(HydraErrorKind.RemoveSharesMustBeZero, "Before you remove a wallet or NFT member please transfer the shares to another member")}, {6024U, new ProgramError<HydraErrorKind>(HydraErrorKind.InvalidCloseAccountDestination, "Sending Sol to a SPL token destination will render the sol unusable")}, };
        }
    }

    namespace Program
    {
        public class ProcessInitAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey HoldingAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey MembershipMint { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ProcessInitForMintAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey FanoutForMint { get; set; }

            public PublicKey MintHoldingAccount { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class ProcessAddMemberWalletAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Member { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey MembershipAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ProcessAddMemberNftAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey MembershipAccount { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ProcessSetTokenMemberStakeAccounts
        {
            public PublicKey Member { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey MembershipVoucher { get; set; }

            public PublicKey MembershipMint { get; set; }

            public PublicKey MembershipMintTokenAccount { get; set; }

            public PublicKey MemberStakeAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ProcessSetForTokenMemberStakeAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Member { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey MembershipVoucher { get; set; }

            public PublicKey MembershipMint { get; set; }

            public PublicKey MembershipMintTokenAccount { get; set; }

            public PublicKey MemberStakeAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ProcessDistributeNftAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey Member { get; set; }

            public PublicKey MembershipMintTokenAccount { get; set; }

            public PublicKey MembershipKey { get; set; }

            public PublicKey MembershipVoucher { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey HoldingAccount { get; set; }

            public PublicKey FanoutForMint { get; set; }

            public PublicKey FanoutForMintMembershipVoucher { get; set; }

            public PublicKey FanoutMint { get; set; }

            public PublicKey FanoutMintMemberTokenAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ProcessDistributeWalletAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey Member { get; set; }

            public PublicKey MembershipVoucher { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey HoldingAccount { get; set; }

            public PublicKey FanoutForMint { get; set; }

            public PublicKey FanoutForMintMembershipVoucher { get; set; }

            public PublicKey FanoutMint { get; set; }

            public PublicKey FanoutMintMemberTokenAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ProcessDistributeTokenAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey Member { get; set; }

            public PublicKey MembershipMintTokenAccount { get; set; }

            public PublicKey MembershipVoucher { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey HoldingAccount { get; set; }

            public PublicKey FanoutForMint { get; set; }

            public PublicKey FanoutForMintMembershipVoucher { get; set; }

            public PublicKey FanoutMint { get; set; }

            public PublicKey FanoutMintMemberTokenAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey MembershipMint { get; set; }

            public PublicKey MemberStakeAccount { get; set; }
        }

        public class ProcessSignMetadataAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey HoldingAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }
        }

        public class ProcessTransferSharesAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey FromMember { get; set; }

            public PublicKey ToMember { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey FromMembershipAccount { get; set; }

            public PublicKey ToMembershipAccount { get; set; }
        }

        public class ProcessUnstakeAccounts
        {
            public PublicKey Member { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey MembershipVoucher { get; set; }

            public PublicKey MembershipMint { get; set; }

            public PublicKey MembershipMintTokenAccount { get; set; }

            public PublicKey MemberStakeAccount { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey Instructions { get; set; }
        }

        public class ProcessRemoveMemberAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Member { get; set; }

            public PublicKey Fanout { get; set; }

            public PublicKey MembershipAccount { get; set; }

            public PublicKey Destination { get; set; }
        }

        public static class HydraProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessInit(ProcessInitAccounts accounts, InitializeFanoutArgs args, MembershipModel model, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.HoldingAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17091898729950414252UL, offset);
                offset += 8;
                offset += args.Serialize(_data, offset);
                _data.WriteU8((byte)model, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessInitForMint(ProcessInitForMintAccounts accounts, byte bumpSeed, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutForMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MintHoldingAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12259883806397863564UL, offset);
                offset += 8;
                _data.WriteU8(bumpSeed, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessAddMemberWallet(ProcessAddMemberWalletAccounts accounts, AddMemberArgs args, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Member, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16995588035153955273UL, offset);
                offset += 8;
                offset += args.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessAddMemberNft(ProcessAddMemberNftAccounts accounts, AddMemberArgs args, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(505292774059933532UL, offset);
                offset += 8;
                offset += args.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessSetTokenMemberStake(ProcessSetTokenMemberStakeAccounts accounts, ulong shares, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Member, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMintTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MemberStakeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10302478017813552551UL, offset);
                offset += 8;
                _data.WriteU64(shares, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessSetForTokenMemberStake(ProcessSetForTokenMemberStakeAccounts accounts, ulong shares, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Member, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMintTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MemberStakeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(7897712870329559250UL, offset);
                offset += 8;
                _data.WriteU64(shares, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessDistributeNft(ProcessDistributeNftAccounts accounts, bool distributeForMint, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Member, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMintTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MembershipKey, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.HoldingAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutForMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutForMintMembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.FanoutMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutMintMemberTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11041229315756060780UL, offset);
                offset += 8;
                _data.WriteBool(distributeForMint, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessDistributeWallet(ProcessDistributeWalletAccounts accounts, bool distributeForMint, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Member, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.HoldingAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutForMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutForMintMembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.FanoutMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutMintMemberTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11796837448697751804UL, offset);
                offset += 8;
                _data.WriteBool(distributeForMint, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessDistributeToken(ProcessDistributeTokenAccounts accounts, bool distributeForMint, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Member, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMintTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.HoldingAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutForMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutForMintMembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.FanoutMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FanoutMintMemberTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MemberStakeAccount, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15309182213261519230UL, offset);
                offset += 8;
                _data.WriteBool(distributeForMint, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessSignMetadata(ProcessSignMetadataAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.HoldingAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(6431023720485307324UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessTransferShares(ProcessTransferSharesAccounts accounts, ulong shares, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.FromMember, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ToMember, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.FromMembershipAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ToMembershipAccount, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(6276916604536401859UL, offset);
                offset += 8;
                _data.WriteU64(shares, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessUnstake(ProcessUnstakeAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Member, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipVoucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipMintTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MemberStakeAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Instructions, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9605965342803796185UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ProcessRemoveMember(ProcessRemoveMemberAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Member, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Fanout, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MembershipAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Destination, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(6167161775199628553UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}