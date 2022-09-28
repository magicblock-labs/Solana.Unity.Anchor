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
using TokenEntangler;
using TokenEntangler.Program;
using TokenEntangler.Errors;
using TokenEntangler.Accounts;

namespace TokenEntangler
{
    namespace Accounts
    {
        public partial class EntangledPair
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 8407153985841297029UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{133, 118, 20, 210, 1, 54, 172, 116};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "PKjzvQC4Wcj";
            public PublicKey TreasuryMint { get; set; }

            public PublicKey MintA { get; set; }

            public PublicKey MintB { get; set; }

            public PublicKey TokenAEscrow { get; set; }

            public PublicKey TokenBEscrow { get; set; }

            public PublicKey Authority { get; set; }

            public byte Bump { get; set; }

            public byte TokenAEscrowBump { get; set; }

            public byte TokenBEscrowBump { get; set; }

            public ulong Price { get; set; }

            public bool Paid { get; set; }

            public bool PaysEveryTime { get; set; }

            public static EntangledPair Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                EntangledPair result = new EntangledPair();
                result.TreasuryMint = _data.GetPubKey(offset);
                offset += 32;
                result.MintA = _data.GetPubKey(offset);
                offset += 32;
                result.MintB = _data.GetPubKey(offset);
                offset += 32;
                result.TokenAEscrow = _data.GetPubKey(offset);
                offset += 32;
                result.TokenBEscrow = _data.GetPubKey(offset);
                offset += 32;
                result.Authority = _data.GetPubKey(offset);
                offset += 32;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                result.TokenAEscrowBump = _data.GetU8(offset);
                offset += 1;
                result.TokenBEscrowBump = _data.GetU8(offset);
                offset += 1;
                result.Price = _data.GetU64(offset);
                offset += 8;
                result.Paid = _data.GetBool(offset);
                offset += 1;
                result.PaysEveryTime = _data.GetBool(offset);
                offset += 1;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum TokenEntanglerErrorKind : uint
        {
            PublicKeyMismatch = 6000U,
            InvalidMintAuthority = 6001U,
            UninitializedAccount = 6002U,
            IncorrectOwner = 6003U,
            PublicKeysShouldBeUnique = 6004U,
            StatementFalse = 6005U,
            NotRentExempt = 6006U,
            NumericalOverflow = 6007U,
            DerivedKeyInvalid = 6008U,
            MetadataDoesntExist = 6009U,
            EditionDoesntExist = 6010U,
            InvalidTokenAmount = 6011U,
            InvalidMint = 6012U,
            EntangledPairExists = 6013U,
            MustHaveSupplyOne = 6014U,
            BumpSeedNotInHashMap = 6015U
        }
    }

    public partial class TokenEntanglerClient : TransactionalBaseClient<TokenEntanglerErrorKind>
    {
        public TokenEntanglerClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<EntangledPair>>> GetEntangledPairsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = EntangledPair.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<EntangledPair>>(res);
            List<EntangledPair> resultingAccounts = new List<EntangledPair>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => EntangledPair.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<EntangledPair>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<EntangledPair>> GetEntangledPairAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<EntangledPair>(res);
            var resultingAccount = EntangledPair.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<EntangledPair>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeEntangledPairAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, EntangledPair> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                EntangledPair parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = EntangledPair.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendCreateEntangledPairAsync(CreateEntangledPairAccounts accounts, byte bump, byte reverseBump, byte tokenAEscrowBump, byte tokenBEscrowBump, ulong price, bool paysEveryTime, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.TokenEntanglerProgram.CreateEntangledPair(accounts, bump, reverseBump, tokenAEscrowBump, tokenBEscrowBump, price, paysEveryTime, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUpdateEntangledPairAsync(UpdateEntangledPairAccounts accounts, ulong price, bool paysEveryTime, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.TokenEntanglerProgram.UpdateEntangledPair(accounts, price, paysEveryTime, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSwapAsync(SwapAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.TokenEntanglerProgram.Swap(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<TokenEntanglerErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<TokenEntanglerErrorKind>>{{6000U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.PublicKeyMismatch, "PublicKeyMismatch")}, {6001U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.InvalidMintAuthority, "InvalidMintAuthority")}, {6002U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.UninitializedAccount, "UninitializedAccount")}, {6003U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.IncorrectOwner, "IncorrectOwner")}, {6004U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.PublicKeysShouldBeUnique, "PublicKeysShouldBeUnique")}, {6005U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.StatementFalse, "StatementFalse")}, {6006U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.NotRentExempt, "NotRentExempt")}, {6007U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.NumericalOverflow, "NumericalOverflow")}, {6008U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.DerivedKeyInvalid, "Derived key invalid")}, {6009U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.MetadataDoesntExist, "Metadata doesn't exist")}, {6010U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.EditionDoesntExist, "Edition doesn't exist")}, {6011U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.InvalidTokenAmount, "Invalid token amount")}, {6012U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.InvalidMint, "This token is not a valid mint for this entangled pair")}, {6013U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.EntangledPairExists, "This pair already exists as it's reverse")}, {6014U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.MustHaveSupplyOne, "Must have supply one!")}, {6015U, new ProgramError<TokenEntanglerErrorKind>(TokenEntanglerErrorKind.BumpSeedNotInHashMap, "Bump seed not in hash map")}, };
        }
    }

    namespace Program
    {
        public class CreateEntangledPairAccounts
        {
            public PublicKey TreasuryMint { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey MintA { get; set; }

            public PublicKey MetadataA { get; set; }

            public PublicKey EditionA { get; set; }

            public PublicKey MintB { get; set; }

            public PublicKey MetadataB { get; set; }

            public PublicKey EditionB { get; set; }

            public PublicKey TokenB { get; set; }

            public PublicKey TokenAEscrow { get; set; }

            public PublicKey TokenBEscrow { get; set; }

            public PublicKey EntangledPair { get; set; }

            public PublicKey ReverseEntangledPair { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class UpdateEntangledPairAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey NewAuthority { get; set; }

            public PublicKey EntangledPair { get; set; }
        }

        public class SwapAccounts
        {
            public PublicKey TreasuryMint { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey PaymentAccount { get; set; }

            public PublicKey PaymentTransferAuthority { get; set; }

            public PublicKey Token { get; set; }

            public PublicKey TokenMint { get; set; }

            public PublicKey ReplacementTokenMetadata { get; set; }

            public PublicKey ReplacementTokenMint { get; set; }

            public PublicKey ReplacementToken { get; set; }

            public PublicKey TransferAuthority { get; set; }

            public PublicKey TokenAEscrow { get; set; }

            public PublicKey TokenBEscrow { get; set; }

            public PublicKey EntangledPair { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public static class TokenEntanglerProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateEntangledPair(CreateEntangledPairAccounts accounts, byte bump, byte reverseBump, byte tokenAEscrowBump, byte tokenBEscrowBump, ulong price, bool paysEveryTime, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintA, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MetadataA, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.EditionA, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintB, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MetadataB, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.EditionB, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenB, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAEscrow, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenBEscrow, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EntangledPair, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReverseEntangledPair, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17352882407449062054UL, offset);
                offset += 8;
                _data.WriteU8(bump, offset);
                offset += 1;
                _data.WriteU8(reverseBump, offset);
                offset += 1;
                _data.WriteU8(tokenAEscrowBump, offset);
                offset += 1;
                _data.WriteU8(tokenBEscrowBump, offset);
                offset += 1;
                _data.WriteU64(price, offset);
                offset += 8;
                _data.WriteBool(paysEveryTime, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateEntangledPair(UpdateEntangledPairAccounts accounts, ulong price, bool paysEveryTime, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EntangledPair, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17603342113971855657UL, offset);
                offset += 8;
                _data.WriteU64(price, offset);
                offset += 8;
                _data.WriteBool(paysEveryTime, offset);
                offset += 1;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Swap(SwapAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreasuryMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PaymentAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PaymentTransferAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Token, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ReplacementTokenMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.ReplacementTokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReplacementToken, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TransferAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAEscrow, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenBEscrow, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EntangledPair, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(14449647541112719096UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}