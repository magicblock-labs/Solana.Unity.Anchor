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
using SequenceEnforcer;
using SequenceEnforcer.Program;
using SequenceEnforcer.Errors;
using SequenceEnforcer.Accounts;

namespace SequenceEnforcer
{
    namespace Accounts
    {
        public partial class SequenceAccount
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 5679986792759222136UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{120, 223, 82, 231, 228, 93, 211, 78};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "MDca7Tv86gV";
            public ulong SequenceNum { get; set; }

            public PublicKey Authority { get; set; }

            public static SequenceAccount Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                SequenceAccount result = new SequenceAccount();
                result.SequenceNum = _data.GetU64(offset);
                offset += 8;
                result.Authority = _data.GetPubKey(offset);
                offset += 32;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum SequenceEnforcerErrorKind : uint
        {
            SequenceOutOfOrder = 6000U
        }
    }

    public partial class SequenceEnforcerClient : TransactionalBaseClient<SequenceEnforcerErrorKind>
    {
        public SequenceEnforcerClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<SequenceAccount>>> GetSequenceAccountsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = SequenceAccount.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<SequenceAccount>>(res);
            List<SequenceAccount> resultingAccounts = new List<SequenceAccount>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => SequenceAccount.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<SequenceAccount>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<SequenceAccount>> GetSequenceAccountAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<SequenceAccount>(res);
            var resultingAccount = SequenceAccount.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<SequenceAccount>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeSequenceAccountAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, SequenceAccount> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                SequenceAccount parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = SequenceAccount.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendInitializeAsync(InitializeAccounts accounts, byte bump, string sym, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.SequenceEnforcerProgram.Initialize(accounts, bump, sym, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendResetSequenceNumberAsync(ResetSequenceNumberAccounts accounts, ulong sequenceNum, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.SequenceEnforcerProgram.ResetSequenceNumber(accounts, sequenceNum, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCheckAndSetSequenceNumberAsync(CheckAndSetSequenceNumberAccounts accounts, ulong sequenceNum, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.SequenceEnforcerProgram.CheckAndSetSequenceNumber(accounts, sequenceNum, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<SequenceEnforcerErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<SequenceEnforcerErrorKind>>{{6000U, new ProgramError<SequenceEnforcerErrorKind>(SequenceEnforcerErrorKind.SequenceOutOfOrder, "Sequence out of order")}, };
        }
    }

    namespace Program
    {
        public class InitializeAccounts
        {
            public PublicKey SequenceAccount { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class ResetSequenceNumberAccounts
        {
            public PublicKey SequenceAccount { get; set; }

            public PublicKey Authority { get; set; }
        }

        public class CheckAndSetSequenceNumberAccounts
        {
            public PublicKey SequenceAccount { get; set; }

            public PublicKey Authority { get; set; }
        }

        public static class SequenceEnforcerProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction Initialize(InitializeAccounts accounts, byte bump, string sym, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SequenceAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17121445590508351407UL, offset);
                offset += 8;
                _data.WriteU8(bump, offset);
                offset += 1;
                offset += _data.WriteBorshString(sym, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ResetSequenceNumber(ResetSequenceNumberAccounts accounts, ulong sequenceNum, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SequenceAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11014749699822031972UL, offset);
                offset += 8;
                _data.WriteU64(sequenceNum, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CheckAndSetSequenceNumber(CheckAndSetSequenceNumberAccounts accounts, ulong sequenceNum, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SequenceAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(18443923197421745496UL, offset);
                offset += 8;
                _data.WriteU64(sequenceNum, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}