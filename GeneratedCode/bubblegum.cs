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
using Bubblegum;
using Bubblegum.Program;
using Bubblegum.Errors;
using Bubblegum.Accounts;
using Bubblegum.Events;
using Bubblegum.Types;

namespace Bubblegum
{
    namespace Accounts
    {
        public partial class TreeConfig
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 14915960087858115962UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{122, 245, 175, 248, 171, 34, 0, 207};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "MZs54B3fwcz";
            public PublicKey TreeCreator { get; set; }

            public PublicKey TreeDelegate { get; set; }

            public ulong TotalMintCapacity { get; set; }

            public ulong NumMinted { get; set; }

            public static TreeConfig Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                TreeConfig result = new TreeConfig();
                result.TreeCreator = _data.GetPubKey(offset);
                offset += 32;
                result.TreeDelegate = _data.GetPubKey(offset);
                offset += 32;
                result.TotalMintCapacity = _data.GetU64(offset);
                offset += 8;
                result.NumMinted = _data.GetU64(offset);
                offset += 8;
                return result;
            }
        }

        public partial class Voucher
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 4687585125344857279UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{191, 204, 149, 234, 213, 165, 13, 65};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "Z5h9LgqQQwJ";
            public LeafSchema LeafSchema { get; set; }

            public uint Index { get; set; }

            public PublicKey MerkleTree { get; set; }

            public static Voucher Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Voucher result = new Voucher();
                offset += LeafSchema.Deserialize(_data, offset, out var resultLeafSchema);
                result.LeafSchema = resultLeafSchema;
                result.Index = _data.GetU32(offset);
                offset += 4;
                result.MerkleTree = _data.GetPubKey(offset);
                offset += 32;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum BubblegumErrorKind : uint
        {
            AssetOwnerMismatch = 6000U,
            PublicKeyMismatch = 6001U,
            HashingMismatch = 6002U,
            UnsupportedSchemaVersion = 6003U,
            CreatorShareTotalMustBe100 = 6004U,
            DuplicateCreatorAddress = 6005U,
            CreatorDidNotVerify = 6006U,
            CreatorNotFound = 6007U,
            NoCreatorsPresent = 6008U,
            CreatorHashMismatch = 6009U,
            DataHashMismatch = 6010U,
            CreatorsTooLong = 6011U,
            MetadataNameTooLong = 6012U,
            MetadataSymbolTooLong = 6013U,
            MetadataUriTooLong = 6014U,
            MetadataBasisPointsTooHigh = 6015U,
            TreeAuthorityIncorrect = 6016U,
            InsufficientMintCapacity = 6017U,
            NumericalOverflowError = 6018U,
            IncorrectOwner = 6019U,
            CollectionCannotBeVerifiedInThisInstruction = 6020U,
            CollectionNotFound = 6021U,
            AlreadyVerified = 6022U,
            AlreadyUnverified = 6023U,
            UpdateAuthorityIncorrect = 6024U,
            LeafAuthorityMustSign = 6025U
        }
    }

    namespace Events
    {
    }

    namespace Types
    {
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

        public partial class Uses
        {
            public UseMethod UseMethod { get; set; }

            public ulong Remaining { get; set; }

            public ulong Total { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8((byte)UseMethod, offset);
                offset += 1;
                _data.WriteU64(Remaining, offset);
                offset += 8;
                _data.WriteU64(Total, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Uses result)
            {
                int offset = initialOffset;
                result = new Uses();
                result.UseMethod = (UseMethod)_data.GetU8(offset);
                offset += 1;
                result.Remaining = _data.GetU64(offset);
                offset += 8;
                result.Total = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class Collection
        {
            public bool Verified { get; set; }

            public PublicKey Key { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteBool(Verified, offset);
                offset += 1;
                _data.WritePubKey(Key, offset);
                offset += 32;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Collection result)
            {
                int offset = initialOffset;
                result = new Collection();
                result.Verified = _data.GetBool(offset);
                offset += 1;
                result.Key = _data.GetPubKey(offset);
                offset += 32;
                return offset - initialOffset;
            }
        }

        public partial class MetadataArgs
        {
            public string Name { get; set; }

            public string Symbol { get; set; }

            public string Uri { get; set; }

            public ushort SellerFeeBasisPoints { get; set; }

            public bool PrimarySaleHappened { get; set; }

            public bool IsMutable { get; set; }

            public byte? EditionNonce { get; set; }

            public TokenStandard TokenStandard { get; set; }

            public Collection Collection { get; set; }

            public Uses Uses { get; set; }

            public TokenProgramVersion TokenProgramVersion { get; set; }

            public Creator[] Creators { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Name, offset);
                offset += _data.WriteBorshString(Symbol, offset);
                offset += _data.WriteBorshString(Uri, offset);
                _data.WriteU16(SellerFeeBasisPoints, offset);
                offset += 2;
                _data.WriteBool(PrimarySaleHappened, offset);
                offset += 1;
                _data.WriteBool(IsMutable, offset);
                offset += 1;
                if (EditionNonce != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU8(EditionNonce.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (TokenStandard != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU8((byte)TokenStandard, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (Collection != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += Collection.Serialize(_data, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (Uses != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += Uses.Serialize(_data, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                _data.WriteU8((byte)TokenProgramVersion, offset);
                offset += 1;
                _data.WriteS32(Creators.Length, offset);
                offset += 4;
                foreach (var creatorsElement in Creators)
                {
                    offset += creatorsElement.Serialize(_data, offset);
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out MetadataArgs result)
            {
                int offset = initialOffset;
                result = new MetadataArgs();
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                offset += _data.GetBorshString(offset, out var resultSymbol);
                result.Symbol = resultSymbol;
                offset += _data.GetBorshString(offset, out var resultUri);
                result.Uri = resultUri;
                result.SellerFeeBasisPoints = _data.GetU16(offset);
                offset += 2;
                result.PrimarySaleHappened = _data.GetBool(offset);
                offset += 1;
                result.IsMutable = _data.GetBool(offset);
                offset += 1;
                if (_data.GetBool(offset++))
                {
                    result.EditionNonce = _data.GetU8(offset);
                    offset += 1;
                }

                if (_data.GetBool(offset++))
                {
                    result.TokenStandard = (TokenStandard)_data.GetU8(offset);
                    offset += 1;
                }

                if (_data.GetBool(offset++))
                {
                    offset += Collection.Deserialize(_data, offset, out var resultCollection);
                    result.Collection = resultCollection;
                }

                if (_data.GetBool(offset++))
                {
                    offset += Uses.Deserialize(_data, offset, out var resultUses);
                    result.Uses = resultUses;
                }

                result.TokenProgramVersion = (TokenProgramVersion)_data.GetU8(offset);
                offset += 1;
                int resultCreatorsLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Creators = new Creator[resultCreatorsLength];
                for (uint resultCreatorsIdx = 0; resultCreatorsIdx < resultCreatorsLength; resultCreatorsIdx++)
                {
                    offset += Creator.Deserialize(_data, offset, out var resultCreatorsresultCreatorsIdx);
                    result.Creators[resultCreatorsIdx] = resultCreatorsresultCreatorsIdx;
                }

                return offset - initialOffset;
            }
        }

        public enum LeafSchemaType : byte
        {
            V1
        }

        public partial class V1Type
        {
            public PublicKey Id { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Delegate { get; set; }

            public ulong Nonce { get; set; }

            public byte[] DataHash { get; set; }

            public byte[] CreatorHash { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out V1Type result)
            {
                int offset = initialOffset;
                result = new V1Type();
                result.Id = _data.GetPubKey(offset);
                offset += 32;
                result.Owner = _data.GetPubKey(offset);
                offset += 32;
                result.Delegate = _data.GetPubKey(offset);
                offset += 32;
                result.Nonce = _data.GetU64(offset);
                offset += 8;
                result.DataHash = _data.GetBytes(offset, 32);
                offset += 32;
                result.CreatorHash = _data.GetBytes(offset, 32);
                offset += 32;
                return offset - initialOffset;
            }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Id, offset);
                offset += 32;
                _data.WritePubKey(Owner, offset);
                offset += 32;
                _data.WritePubKey(Delegate, offset);
                offset += 32;
                _data.WriteU64(Nonce, offset);
                offset += 8;
                _data.WriteSpan(DataHash, offset);
                offset += DataHash.Length;
                _data.WriteSpan(CreatorHash, offset);
                offset += CreatorHash.Length;
                return offset - initialOffset;
            }
        }

        public partial class LeafSchema
        {
            public V1Type V1Value { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8((byte)Type, offset);
                offset += 1;
                switch (Type)
                {
                    case LeafSchemaType.V1:
                        offset += V1Value.Serialize(_data, offset);
                        break;
                }

                return offset - initialOffset;
            }

            public LeafSchemaType Type { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out LeafSchema result)
            {
                int offset = initialOffset;
                result = new LeafSchema();
                result.Type = (LeafSchemaType)_data.GetU8(offset);
                offset += 1;
                switch (result.Type)
                {
                    case LeafSchemaType.V1:
                    {
                        V1Type tmpV1Value = new V1Type();
                        offset += V1Type.Deserialize(_data, offset, out tmpV1Value);
                        result.V1Value = tmpV1Value;
                        break;
                    }
                }

                return offset - initialOffset;
            }
        }

        public enum TokenProgramVersion : byte
        {
            Original,
            Token2022
        }

        public enum TokenStandard : byte
        {
            NonFungible,
            FungibleAsset,
            Fungible,
            NonFungibleEdition
        }

        public enum UseMethod : byte
        {
            Burn,
            Multiple,
            Single
        }
    }

    public partial class BubblegumClient : TransactionalBaseClient<BubblegumErrorKind>
    {
        public BubblegumClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<TreeConfig>>> GetTreeConfigsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = TreeConfig.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<TreeConfig>>(res);
            List<TreeConfig> resultingAccounts = new List<TreeConfig>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => TreeConfig.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<TreeConfig>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Voucher>>> GetVouchersAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Voucher.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Voucher>>(res);
            List<Voucher> resultingAccounts = new List<Voucher>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Voucher.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Voucher>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<TreeConfig>> GetTreeConfigAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<TreeConfig>(res);
            var resultingAccount = TreeConfig.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<TreeConfig>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Voucher>> GetVoucherAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Voucher>(res);
            var resultingAccount = Voucher.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Voucher>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeTreeConfigAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, TreeConfig> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                TreeConfig parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = TreeConfig.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeVoucherAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Voucher> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Voucher parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Voucher.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendCreateTreeAsync(CreateTreeAccounts accounts, uint maxDepth, uint maxBufferSize, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.CreateTree(accounts, maxDepth, maxBufferSize, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSetTreeDelegateAsync(SetTreeDelegateAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.SetTreeDelegate(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendMintV1Async(MintV1Accounts accounts, MetadataArgs message, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.MintV1(accounts, message, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendVerifyCreatorAsync(VerifyCreatorAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.VerifyCreator(accounts, root, dataHash, creatorHash, nonce, index, message, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUnverifyCreatorAsync(UnverifyCreatorAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.UnverifyCreator(accounts, root, dataHash, creatorHash, nonce, index, message, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendVerifyCollectionAsync(VerifyCollectionAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.VerifyCollection(accounts, root, dataHash, creatorHash, nonce, index, message, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUnverifyCollectionAsync(UnverifyCollectionAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.UnverifyCollection(accounts, root, dataHash, creatorHash, nonce, index, message, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSetAndVerifyCollectionAsync(SetAndVerifyCollectionAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, byte[] collection, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.SetAndVerifyCollection(accounts, root, dataHash, creatorHash, nonce, index, message, collection, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendTransferAsync(TransferAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.Transfer(accounts, root, dataHash, creatorHash, nonce, index, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDelegateAsync(DelegateAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.Delegate(accounts, root, dataHash, creatorHash, nonce, index, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendBurnAsync(BurnAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.Burn(accounts, root, dataHash, creatorHash, nonce, index, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendRedeemAsync(RedeemAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.Redeem(accounts, root, dataHash, creatorHash, nonce, index, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCancelRedeemAsync(CancelRedeemAccounts accounts, byte[] root, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.CancelRedeem(accounts, root, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDecompressV1Async(DecompressV1Accounts accounts, MetadataArgs metadata, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.DecompressV1(accounts, metadata, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCompressAsync(CompressAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.BubblegumProgram.Compress(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<BubblegumErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<BubblegumErrorKind>>{{6000U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.AssetOwnerMismatch, "Asset Owner Does not match")}, {6001U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.PublicKeyMismatch, "PublicKeyMismatch")}, {6002U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.HashingMismatch, "Hashing Mismatch Within Leaf Schema")}, {6003U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.UnsupportedSchemaVersion, "Unsupported Schema Version")}, {6004U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.CreatorShareTotalMustBe100, "Creator shares must sum to 100")}, {6005U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.DuplicateCreatorAddress, "No duplicate creator addresses in metadata")}, {6006U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.CreatorDidNotVerify, "Creator did not verify the metadata")}, {6007U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.CreatorNotFound, "Creator not found in creator Vec")}, {6008U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.NoCreatorsPresent, "No creators in creator Vec")}, {6009U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.CreatorHashMismatch, "User-provided creator Vec must result in same user-provided creator hash")}, {6010U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.DataHashMismatch, "User-provided metadata must result in same user-provided data hash")}, {6011U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.CreatorsTooLong, "Creators list too long")}, {6012U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.MetadataNameTooLong, "Name in metadata is too long")}, {6013U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.MetadataSymbolTooLong, "Symbol in metadata is too long")}, {6014U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.MetadataUriTooLong, "Uri in metadata is too long")}, {6015U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.MetadataBasisPointsTooHigh, "Basis points in metadata cannot exceed 10000")}, {6016U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.TreeAuthorityIncorrect, "Tree creator or tree delegate must sign.")}, {6017U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.InsufficientMintCapacity, "Not enough unapproved mints left")}, {6018U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.NumericalOverflowError, "NumericalOverflowError")}, {6019U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.IncorrectOwner, "Incorrect account owner")}, {6020U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.CollectionCannotBeVerifiedInThisInstruction, "Cannot Verify Collection in this Instruction")}, {6021U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.CollectionNotFound, "Collection Not Found on Metadata")}, {6022U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.AlreadyVerified, "Collection item is already verified.")}, {6023U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.AlreadyUnverified, "Collection item is already unverified.")}, {6024U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.UpdateAuthorityIncorrect, "Incorrect leaf metadata update authority.")}, {6025U, new ProgramError<BubblegumErrorKind>(BubblegumErrorKind.LeafAuthorityMustSign, "This transaction must be signed by either the leaf owner or leaf delegate")}, };
        }
    }

    namespace Program
    {
        public class CreateTreeAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TreeCreator { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class SetTreeDelegateAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey TreeCreator { get; set; }

            public PublicKey NewTreeDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }
        }

        public class MintV1Accounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TreeDelegate { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }
        }

        public class VerifyCreatorAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Creator { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }
        }

        public class UnverifyCreatorAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Creator { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }
        }

        public class VerifyCollectionAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TreeDelegate { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey CollectionMetadata { get; set; }

            public PublicKey EditionAccount { get; set; }

            public PublicKey BubblegumSigner { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }
        }

        public class UnverifyCollectionAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TreeDelegate { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey CollectionMetadata { get; set; }

            public PublicKey EditionAccount { get; set; }

            public PublicKey BubblegumSigner { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }
        }

        public class SetAndVerifyCollectionAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TreeDelegate { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey CollectionMetadata { get; set; }

            public PublicKey EditionAccount { get; set; }

            public PublicKey BubblegumSigner { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }
        }

        public class TransferAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey NewLeafOwner { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }
        }

        public class DelegateAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey PreviousLeafDelegate { get; set; }

            public PublicKey NewLeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }
        }

        public class BurnAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }
        }

        public class RedeemAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Voucher { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class CancelRedeemAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey Voucher { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }
        }

        public class DecompressV1Accounts
        {
            public PublicKey Voucher { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey SysvarRent { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey AssociatedTokenProgram { get; set; }
        }

        public class CompressAccounts
        {
            public PublicKey TreeAuthority { get; set; }

            public PublicKey LeafOwner { get; set; }

            public PublicKey LeafDelegate { get; set; }

            public PublicKey MerkleTree { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey LogWrapper { get; set; }

            public PublicKey CompressionProgram { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public static class BubblegumProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateTree(CreateTreeAccounts accounts, uint maxDepth, uint maxBufferSize, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeCreator, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15866122498241745829UL, offset);
                offset += 8;
                _data.WriteU32(maxDepth, offset);
                offset += 4;
                _data.WriteU32(maxBufferSize, offset);
                offset += 4;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SetTreeDelegate(SetTreeDelegateAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeCreator, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewTreeDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MerkleTree, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(7393276431020750589UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction MintV1(MintV1Accounts accounts, MetadataArgs message, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeDelegate, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(7527366247671947921UL, offset);
                offset += 8;
                offset += message.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction VerifyCreator(VerifyCreatorAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Creator, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(14003103321588502836UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                offset += message.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UnverifyCreator(UnverifyCreatorAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Creator, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10984406386623492715UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                offset += message.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction VerifyCollection(VerifyCollectionAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.EditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.BubblegumSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12212134156261749048UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                offset += message.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UnverifyCollection(UnverifyCollectionAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.EditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.BubblegumSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12158180955007941626UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                offset += message.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SetAndVerifyCollection(SetAndVerifyCollectionAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, MetadataArgs message, byte[] collection, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.EditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.BubblegumSigner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16912400468640658155UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                offset += message.Serialize(_data, offset);
                _data.WriteSpan(collection, offset);
                offset += collection.Length;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Transfer(TransferAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewLeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13422138168166593699UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Delegate(DelegateAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PreviousLeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewLeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9873113408189731674UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Burn(BurnAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(6713419448098582132UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Redeem(RedeemAccounts accounts, byte[] root, byte[] dataHash, byte[] creatorHash, ulong nonce, uint index, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.LeafOwner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Voucher, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16240477538706918584UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                _data.WriteSpan(dataHash, offset);
                offset += dataHash.Length;
                _data.WriteSpan(creatorHash, offset);
                offset += creatorHash.Length;
                _data.WriteU64(nonce, offset);
                offset += 8;
                _data.WriteU32(index, offset);
                offset += 4;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CancelRedeem(CancelRedeemAccounts accounts, byte[] root, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.LeafOwner, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Voucher, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17451641138953342063UL, offset);
                offset += 8;
                _data.WriteSpan(root, offset);
                offset += root.Length;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DecompressV1(DecompressV1Accounts accounts, MetadataArgs metadata, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Voucher, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.LeafOwner, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SysvarRent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AssociatedTokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(5883102871591605558UL, offset);
                offset += 8;
                offset += metadata.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Compress(CompressAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TreeAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafOwner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LeafDelegate, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MerkleTree, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.LogWrapper, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CompressionProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(18262964761550438738UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}