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
using MplTokenMetadata;
using MplTokenMetadata.Program;
using MplTokenMetadata.Errors;
using MplTokenMetadata.Accounts;
using MplTokenMetadata.Types;

namespace MplTokenMetadata
{
    namespace Accounts
    {
        public partial class UseAuthorityRecord
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 3651512152485185763UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{227, 200, 230, 197, 244, 198, 172, 50};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "f6oGBa9cgAM";
            public Key Key { get; set; }

            public ulong AllowedUses { get; set; }

            public byte Bump { get; set; }

            public static UseAuthorityRecord Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                UseAuthorityRecord result = new UseAuthorityRecord();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.AllowedUses = _data.GetU64(offset);
                offset += 8;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                return result;
            }
        }

        public partial class CollectionAuthorityRecord
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 12134065000149692572UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{156, 48, 108, 31, 212, 219, 100, 168};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "T8EN5aq1mQb";
            public Key Key { get; set; }

            public byte Bump { get; set; }

            public static CollectionAuthorityRecord Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                CollectionAuthorityRecord result = new CollectionAuthorityRecord();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.Bump = _data.GetU8(offset);
                offset += 1;
                return result;
            }
        }

        public partial class Metadata
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 6725481107337841480UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{72, 11, 121, 26, 111, 181, 85, 93};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "D3vebSFQVUx";
            public Key Key { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey Mint { get; set; }

            public Data Data { get; set; }

            public bool PrimarySaleHappened { get; set; }

            public bool IsMutable { get; set; }

            public byte? EditionNonce { get; set; }

            public TokenStandard TokenStandard { get; set; }

            public Collection Collection { get; set; }

            public Uses Uses { get; set; }

            public CollectionDetails CollectionDetails { get; set; }

            public static Metadata Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Metadata result = new Metadata();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.UpdateAuthority = _data.GetPubKey(offset);
                offset += 32;
                result.Mint = _data.GetPubKey(offset);
                offset += 32;
                offset += Data.Deserialize(_data, offset, out var resultData);
                result.Data = resultData;
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

                if (_data.GetBool(offset++))
                {
                    offset += CollectionDetails.Deserialize(_data, offset, out var resultCollectionDetails);
                    result.CollectionDetails = resultCollectionDetails;
                }

                return result;
            }
        }

        public partial class MasterEditionV2
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 11505026815943195493UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{101, 59, 163, 207, 238, 16, 170, 159};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "Hw62L9xA8RU";
            public Key Key { get; set; }

            public ulong Supply { get; set; }

            public ulong? MaxSupply { get; set; }

            public static MasterEditionV2 Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                MasterEditionV2 result = new MasterEditionV2();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.Supply = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.MaxSupply = _data.GetU64(offset);
                    offset += 8;
                }

                return result;
            }
        }

        public partial class MasterEditionV1
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 13370553651352413519UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{79, 165, 41, 167, 180, 191, 141, 185};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "EKfA8UPC9Qk";
            public Key Key { get; set; }

            public ulong Supply { get; set; }

            public ulong? MaxSupply { get; set; }

            public PublicKey PrintingMint { get; set; }

            public PublicKey OneTimePrintingAuthorizationMint { get; set; }

            public static MasterEditionV1 Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                MasterEditionV1 result = new MasterEditionV1();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.Supply = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.MaxSupply = _data.GetU64(offset);
                    offset += 8;
                }

                result.PrintingMint = _data.GetPubKey(offset);
                offset += 32;
                result.OneTimePrintingAuthorizationMint = _data.GetPubKey(offset);
                offset += 32;
                return result;
            }
        }

        public partial class Edition
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 12099873706834753002UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{234, 117, 249, 74, 7, 99, 235, 167};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "gDZhcmRet9c";
            public Key Key { get; set; }

            public PublicKey Parent { get; set; }

            public ulong EditionField { get; set; }

            public static Edition Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Edition result = new Edition();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.Parent = _data.GetPubKey(offset);
                offset += 32;
                result.EditionField = _data.GetU64(offset);
                offset += 8;
                return result;
            }
        }

        public partial class ReservationListV2
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 15737696910135388609UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{193, 233, 97, 55, 245, 135, 103, 218};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "ZSBn4UrShPf";
            public Key Key { get; set; }

            public PublicKey MasterEdition { get; set; }

            public ulong? SupplySnapshot { get; set; }

            public Reservation[] Reservations { get; set; }

            public ulong TotalReservationSpots { get; set; }

            public ulong CurrentReservationSpots { get; set; }

            public static ReservationListV2 Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                ReservationListV2 result = new ReservationListV2();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.MasterEdition = _data.GetPubKey(offset);
                offset += 32;
                if (_data.GetBool(offset++))
                {
                    result.SupplySnapshot = _data.GetU64(offset);
                    offset += 8;
                }

                int resultReservationsLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Reservations = new Reservation[resultReservationsLength];
                for (uint resultReservationsIdx = 0; resultReservationsIdx < resultReservationsLength; resultReservationsIdx++)
                {
                    offset += Reservation.Deserialize(_data, offset, out var resultReservationsresultReservationsIdx);
                    result.Reservations[resultReservationsIdx] = resultReservationsresultReservationsIdx;
                }

                result.TotalReservationSpots = _data.GetU64(offset);
                offset += 8;
                result.CurrentReservationSpots = _data.GetU64(offset);
                offset += 8;
                return result;
            }
        }

        public partial class ReservationListV1
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 10088513367238791151UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{239, 79, 12, 206, 116, 153, 1, 140};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "h2bWfZfSaH1";
            public Key Key { get; set; }

            public PublicKey MasterEdition { get; set; }

            public ulong? SupplySnapshot { get; set; }

            public ReservationV1[] Reservations { get; set; }

            public static ReservationListV1 Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                ReservationListV1 result = new ReservationListV1();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.MasterEdition = _data.GetPubKey(offset);
                offset += 32;
                if (_data.GetBool(offset++))
                {
                    result.SupplySnapshot = _data.GetU64(offset);
                    offset += 8;
                }

                int resultReservationsLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Reservations = new ReservationV1[resultReservationsLength];
                for (uint resultReservationsIdx = 0; resultReservationsIdx < resultReservationsLength; resultReservationsIdx++)
                {
                    offset += ReservationV1.Deserialize(_data, offset, out var resultReservationsresultReservationsIdx);
                    result.Reservations[resultReservationsIdx] = resultReservationsresultReservationsIdx;
                }

                return result;
            }
        }

        public partial class EditionMarker
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 16872081252924132073UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{233, 10, 18, 230, 129, 172, 37, 234};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "fymsTg55qG5";
            public Key Key { get; set; }

            public byte[] Ledger { get; set; }

            public static EditionMarker Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                EditionMarker result = new EditionMarker();
                result.Key = (Key)_data.GetU8(offset);
                offset += 1;
                result.Ledger = _data.GetBytes(offset, 31);
                offset += 31;
                return result;
            }
        }
    }

    namespace Errors
    {
        public enum MplTokenMetadataErrorKind : uint
        {
            InstructionUnpackError = 0U,
            InstructionPackError = 1U,
            NotRentExempt = 2U,
            AlreadyInitialized = 3U,
            Uninitialized = 4U,
            InvalidMetadataKey = 5U,
            InvalidEditionKey = 6U,
            UpdateAuthorityIncorrect = 7U,
            UpdateAuthorityIsNotSigner = 8U,
            NotMintAuthority = 9U,
            InvalidMintAuthority = 10U,
            NameTooLong = 11U,
            SymbolTooLong = 12U,
            UriTooLong = 13U,
            UpdateAuthorityMustBeEqualToMetadataAuthorityAndSigner = 14U,
            MintMismatch = 15U,
            EditionsMustHaveExactlyOneToken = 16U,
            MaxEditionsMintedAlready = 17U,
            TokenMintToFailed = 18U,
            MasterRecordMismatch = 19U,
            DestinationMintMismatch = 20U,
            EditionAlreadyMinted = 21U,
            PrintingMintDecimalsShouldBeZero = 22U,
            OneTimePrintingAuthorizationMintDecimalsShouldBeZero = 23U,
            EditionMintDecimalsShouldBeZero = 24U,
            TokenBurnFailed = 25U,
            TokenAccountOneTimeAuthMintMismatch = 26U,
            DerivedKeyInvalid = 27U,
            PrintingMintMismatch = 28U,
            OneTimePrintingAuthMintMismatch = 29U,
            TokenAccountMintMismatch = 30U,
            TokenAccountMintMismatchV2 = 31U,
            NotEnoughTokens = 32U,
            PrintingMintAuthorizationAccountMismatch = 33U,
            AuthorizationTokenAccountOwnerMismatch = 34U,
            Disabled = 35U,
            CreatorsTooLong = 36U,
            CreatorsMustBeAtleastOne = 37U,
            MustBeOneOfCreators = 38U,
            NoCreatorsPresentOnMetadata = 39U,
            CreatorNotFound = 40U,
            InvalidBasisPoints = 41U,
            PrimarySaleCanOnlyBeFlippedToTrue = 42U,
            OwnerMismatch = 43U,
            NoBalanceInAccountForAuthorization = 44U,
            ShareTotalMustBe100 = 45U,
            ReservationExists = 46U,
            ReservationDoesNotExist = 47U,
            ReservationNotSet = 48U,
            ReservationAlreadyMade = 49U,
            BeyondMaxAddressSize = 50U,
            NumericalOverflowError = 51U,
            ReservationBreachesMaximumSupply = 52U,
            AddressNotInReservation = 53U,
            CannotVerifyAnotherCreator = 54U,
            CannotUnverifyAnotherCreator = 55U,
            SpotMismatch = 56U,
            IncorrectOwner = 57U,
            PrintingWouldBreachMaximumSupply = 58U,
            DataIsImmutable = 59U,
            DuplicateCreatorAddress = 60U,
            ReservationSpotsRemainingShouldMatchTotalSpotsAtStart = 61U,
            InvalidTokenProgram = 62U,
            DataTypeMismatch = 63U,
            BeyondAlottedAddressSize = 64U,
            ReservationNotComplete = 65U,
            TriedToReplaceAnExistingReservation = 66U,
            InvalidOperation = 67U,
            InvalidOwner = 68U,
            PrintingMintSupplyMustBeZeroForConversion = 69U,
            OneTimeAuthMintSupplyMustBeZeroForConversion = 70U,
            InvalidEditionIndex = 71U,
            ReservationArrayShouldBeSizeOne = 72U,
            IsMutableCanOnlyBeFlippedToFalse = 73U,
            CollectionCannotBeVerifiedInThisInstruction = 74U,
            Removed = 75U,
            MustBeBurned = 76U,
            InvalidUseMethod = 77U,
            CannotChangeUseMethodAfterFirstUse = 78U,
            CannotChangeUsesAfterFirstUse = 79U,
            CollectionNotFound = 80U,
            InvalidCollectionUpdateAuthority = 81U,
            CollectionMustBeAUniqueMasterEdition = 82U,
            UseAuthorityRecordAlreadyExists = 83U,
            UseAuthorityRecordAlreadyRevoked = 84U,
            Unusable = 85U,
            NotEnoughUses = 86U,
            CollectionAuthorityRecordAlreadyExists = 87U,
            CollectionAuthorityDoesNotExist = 88U,
            InvalidUseAuthorityRecord = 89U,
            InvalidCollectionAuthorityRecord = 90U,
            InvalidFreezeAuthority = 91U,
            InvalidDelegate = 92U,
            CannotAdjustVerifiedCreator = 93U,
            CannotRemoveVerifiedCreator = 94U,
            CannotWipeVerifiedCreators = 95U,
            NotAllowedToChangeSellerFeeBasisPoints = 96U,
            EditionOverrideCannotBeZero = 97U,
            InvalidUser = 98U,
            RevokeCollectionAuthoritySignerIncorrect = 99U,
            TokenCloseFailed = 100U,
            UnsizedCollection = 101U,
            SizedCollection = 102U,
            MissingCollectionMetadata = 103U,
            NotAMemberOfCollection = 104U,
            NotVerifiedMemberOfCollection = 105U,
            NotACollectionParent = 106U,
            CouldNotDetermineTokenStandard = 107U,
            MissingEditionAccount = 108U,
            NotAMasterEdition = 109U,
            MasterEditionHasPrints = 110U,
            BorshDeserializationError = 111U,
            CannotUpdateVerifiedCollection = 112U,
            CollectionMasterEditionAccountInvalid = 113U,
            AlreadyVerified = 114U,
            AlreadyUnverified = 115U
        }
    }

    namespace Types
    {
        public partial class MintPrintingTokensViaTokenArgs
        {
            public ulong Supply { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(Supply, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out MintPrintingTokensViaTokenArgs result)
            {
                int offset = initialOffset;
                result = new MintPrintingTokensViaTokenArgs();
                result.Supply = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class SetReservationListArgs
        {
            public Reservation[] Reservations { get; set; }

            public ulong? TotalReservationSpots { get; set; }

            public ulong Offset { get; set; }

            public ulong TotalSpotOffset { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteS32(Reservations.Length, offset);
                offset += 4;
                foreach (var reservationsElement in Reservations)
                {
                    offset += reservationsElement.Serialize(_data, offset);
                }

                if (TotalReservationSpots != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(TotalReservationSpots.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                _data.WriteU64(Offset, offset);
                offset += 8;
                _data.WriteU64(TotalSpotOffset, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out SetReservationListArgs result)
            {
                int offset = initialOffset;
                result = new SetReservationListArgs();
                int resultReservationsLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Reservations = new Reservation[resultReservationsLength];
                for (uint resultReservationsIdx = 0; resultReservationsIdx < resultReservationsLength; resultReservationsIdx++)
                {
                    offset += Reservation.Deserialize(_data, offset, out var resultReservationsresultReservationsIdx);
                    result.Reservations[resultReservationsIdx] = resultReservationsresultReservationsIdx;
                }

                if (_data.GetBool(offset++))
                {
                    result.TotalReservationSpots = _data.GetU64(offset);
                    offset += 8;
                }

                result.Offset = _data.GetU64(offset);
                offset += 8;
                result.TotalSpotOffset = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class UpdateMetadataAccountArgs
        {
            public Data Data { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public bool? PrimarySaleHappened { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                if (Data != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += Data.Serialize(_data, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (UpdateAuthority != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WritePubKey(UpdateAuthority, offset);
                    offset += 32;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (PrimarySaleHappened != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(PrimarySaleHappened.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out UpdateMetadataAccountArgs result)
            {
                int offset = initialOffset;
                result = new UpdateMetadataAccountArgs();
                if (_data.GetBool(offset++))
                {
                    offset += Data.Deserialize(_data, offset, out var resultData);
                    result.Data = resultData;
                }

                if (_data.GetBool(offset++))
                {
                    result.UpdateAuthority = _data.GetPubKey(offset);
                    offset += 32;
                }

                if (_data.GetBool(offset++))
                {
                    result.PrimarySaleHappened = _data.GetBool(offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }
        }

        public partial class UpdateMetadataAccountArgsV2
        {
            public DataV2 Data { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public bool? PrimarySaleHappened { get; set; }

            public bool? IsMutable { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                if (Data != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += Data.Serialize(_data, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (UpdateAuthority != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WritePubKey(UpdateAuthority, offset);
                    offset += 32;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (PrimarySaleHappened != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(PrimarySaleHappened.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (IsMutable != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(IsMutable.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out UpdateMetadataAccountArgsV2 result)
            {
                int offset = initialOffset;
                result = new UpdateMetadataAccountArgsV2();
                if (_data.GetBool(offset++))
                {
                    offset += DataV2.Deserialize(_data, offset, out var resultData);
                    result.Data = resultData;
                }

                if (_data.GetBool(offset++))
                {
                    result.UpdateAuthority = _data.GetPubKey(offset);
                    offset += 32;
                }

                if (_data.GetBool(offset++))
                {
                    result.PrimarySaleHappened = _data.GetBool(offset);
                    offset += 1;
                }

                if (_data.GetBool(offset++))
                {
                    result.IsMutable = _data.GetBool(offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }
        }

        public partial class CreateMetadataAccountArgs
        {
            public Data Data { get; set; }

            public bool IsMutable { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += Data.Serialize(_data, offset);
                _data.WriteBool(IsMutable, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out CreateMetadataAccountArgs result)
            {
                int offset = initialOffset;
                result = new CreateMetadataAccountArgs();
                offset += Data.Deserialize(_data, offset, out var resultData);
                result.Data = resultData;
                result.IsMutable = _data.GetBool(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public partial class CreateMetadataAccountArgsV2
        {
            public DataV2 Data { get; set; }

            public bool IsMutable { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += Data.Serialize(_data, offset);
                _data.WriteBool(IsMutable, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out CreateMetadataAccountArgsV2 result)
            {
                int offset = initialOffset;
                result = new CreateMetadataAccountArgsV2();
                offset += DataV2.Deserialize(_data, offset, out var resultData);
                result.Data = resultData;
                result.IsMutable = _data.GetBool(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public partial class CreateMetadataAccountArgsV3
        {
            public DataV2 Data { get; set; }

            public bool IsMutable { get; set; }

            public CollectionDetails CollectionDetails { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += Data.Serialize(_data, offset);
                _data.WriteBool(IsMutable, offset);
                offset += 1;
                if (CollectionDetails != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += CollectionDetails.Serialize(_data, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out CreateMetadataAccountArgsV3 result)
            {
                int offset = initialOffset;
                result = new CreateMetadataAccountArgsV3();
                offset += DataV2.Deserialize(_data, offset, out var resultData);
                result.Data = resultData;
                result.IsMutable = _data.GetBool(offset);
                offset += 1;
                if (_data.GetBool(offset++))
                {
                    offset += CollectionDetails.Deserialize(_data, offset, out var resultCollectionDetails);
                    result.CollectionDetails = resultCollectionDetails;
                }

                return offset - initialOffset;
            }
        }

        public partial class CreateMasterEditionArgs
        {
            public ulong? MaxSupply { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                if (MaxSupply != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(MaxSupply.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out CreateMasterEditionArgs result)
            {
                int offset = initialOffset;
                result = new CreateMasterEditionArgs();
                if (_data.GetBool(offset++))
                {
                    result.MaxSupply = _data.GetU64(offset);
                    offset += 8;
                }

                return offset - initialOffset;
            }
        }

        public partial class MintNewEditionFromMasterEditionViaTokenArgs
        {
            public ulong Edition { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(Edition, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out MintNewEditionFromMasterEditionViaTokenArgs result)
            {
                int offset = initialOffset;
                result = new MintNewEditionFromMasterEditionViaTokenArgs();
                result.Edition = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class ApproveUseAuthorityArgs
        {
            public ulong NumberOfUses { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(NumberOfUses, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out ApproveUseAuthorityArgs result)
            {
                int offset = initialOffset;
                result = new ApproveUseAuthorityArgs();
                result.NumberOfUses = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class UtilizeArgs
        {
            public ulong NumberOfUses { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(NumberOfUses, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out UtilizeArgs result)
            {
                int offset = initialOffset;
                result = new UtilizeArgs();
                result.NumberOfUses = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class SetCollectionSizeArgs
        {
            public ulong Size { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(Size, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out SetCollectionSizeArgs result)
            {
                int offset = initialOffset;
                result = new SetCollectionSizeArgs();
                result.Size = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class Data
        {
            public string Name { get; set; }

            public string Symbol { get; set; }

            public string Uri { get; set; }

            public ushort SellerFeeBasisPoints { get; set; }

            public Creator[] Creators { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Name, offset);
                offset += _data.WriteBorshString(Symbol, offset);
                offset += _data.WriteBorshString(Uri, offset);
                _data.WriteU16(SellerFeeBasisPoints, offset);
                offset += 2;
                if (Creators != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteS32(Creators.Length, offset);
                    offset += 4;
                    foreach (var creatorsElement in Creators)
                    {
                        offset += creatorsElement.Serialize(_data, offset);
                    }
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Data result)
            {
                int offset = initialOffset;
                result = new Data();
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                offset += _data.GetBorshString(offset, out var resultSymbol);
                result.Symbol = resultSymbol;
                offset += _data.GetBorshString(offset, out var resultUri);
                result.Uri = resultUri;
                result.SellerFeeBasisPoints = _data.GetU16(offset);
                offset += 2;
                if (_data.GetBool(offset++))
                {
                    int resultCreatorsLength = (int)_data.GetU32(offset);
                    offset += 4;
                    result.Creators = new Creator[resultCreatorsLength];
                    for (uint resultCreatorsIdx = 0; resultCreatorsIdx < resultCreatorsLength; resultCreatorsIdx++)
                    {
                        offset += Creator.Deserialize(_data, offset, out var resultCreatorsresultCreatorsIdx);
                        result.Creators[resultCreatorsIdx] = resultCreatorsresultCreatorsIdx;
                    }
                }

                return offset - initialOffset;
            }
        }

        public partial class DataV2
        {
            public string Name { get; set; }

            public string Symbol { get; set; }

            public string Uri { get; set; }

            public ushort SellerFeeBasisPoints { get; set; }

            public Creator[] Creators { get; set; }

            public Collection Collection { get; set; }

            public Uses Uses { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Name, offset);
                offset += _data.WriteBorshString(Symbol, offset);
                offset += _data.WriteBorshString(Uri, offset);
                _data.WriteU16(SellerFeeBasisPoints, offset);
                offset += 2;
                if (Creators != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteS32(Creators.Length, offset);
                    offset += 4;
                    foreach (var creatorsElement in Creators)
                    {
                        offset += creatorsElement.Serialize(_data, offset);
                    }
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

                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out DataV2 result)
            {
                int offset = initialOffset;
                result = new DataV2();
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                offset += _data.GetBorshString(offset, out var resultSymbol);
                result.Symbol = resultSymbol;
                offset += _data.GetBorshString(offset, out var resultUri);
                result.Uri = resultUri;
                result.SellerFeeBasisPoints = _data.GetU16(offset);
                offset += 2;
                if (_data.GetBool(offset++))
                {
                    int resultCreatorsLength = (int)_data.GetU32(offset);
                    offset += 4;
                    result.Creators = new Creator[resultCreatorsLength];
                    for (uint resultCreatorsIdx = 0; resultCreatorsIdx < resultCreatorsLength; resultCreatorsIdx++)
                    {
                        offset += Creator.Deserialize(_data, offset, out var resultCreatorsresultCreatorsIdx);
                        result.Creators[resultCreatorsIdx] = resultCreatorsresultCreatorsIdx;
                    }
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

        public partial class Reservation
        {
            public PublicKey Address { get; set; }

            public ulong SpotsRemaining { get; set; }

            public ulong TotalSpots { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Address, offset);
                offset += 32;
                _data.WriteU64(SpotsRemaining, offset);
                offset += 8;
                _data.WriteU64(TotalSpots, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out Reservation result)
            {
                int offset = initialOffset;
                result = new Reservation();
                result.Address = _data.GetPubKey(offset);
                offset += 32;
                result.SpotsRemaining = _data.GetU64(offset);
                offset += 8;
                result.TotalSpots = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class ReservationV1
        {
            public PublicKey Address { get; set; }

            public byte SpotsRemaining { get; set; }

            public byte TotalSpots { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Address, offset);
                offset += 32;
                _data.WriteU8(SpotsRemaining, offset);
                offset += 1;
                _data.WriteU8(TotalSpots, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out ReservationV1 result)
            {
                int offset = initialOffset;
                result = new ReservationV1();
                result.Address = _data.GetPubKey(offset);
                offset += 32;
                result.SpotsRemaining = _data.GetU8(offset);
                offset += 1;
                result.TotalSpots = _data.GetU8(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public enum Key : byte
        {
            Uninitialized,
            EditionV1,
            MasterEditionV1,
            ReservationListV1,
            MetadataV1,
            ReservationListV2,
            MasterEditionV2,
            EditionMarker,
            UseAuthorityRecord,
            CollectionAuthorityRecord
        }

        public enum UseMethod : byte
        {
            Burn,
            Multiple,
            Single
        }

        public enum CollectionDetailsType : byte
        {
            V1
        }

        public partial class V1Type
        {
            public ulong Size { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out V1Type result)
            {
                int offset = initialOffset;
                result = new V1Type();
                result.Size = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(Size, offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class CollectionDetails
        {
            public V1Type V1Value { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8((byte)Type, offset);
                offset += 1;
                switch (Type)
                {
                    case CollectionDetailsType.V1:
                        offset += V1Value.Serialize(_data, offset);
                        break;
                }

                return offset - initialOffset;
            }

            public CollectionDetailsType Type { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out CollectionDetails result)
            {
                int offset = initialOffset;
                result = new CollectionDetails();
                result.Type = (CollectionDetailsType)_data.GetU8(offset);
                offset += 1;
                switch (result.Type)
                {
                    case CollectionDetailsType.V1:
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

        public enum TokenStandard : byte
        {
            NonFungible,
            FungibleAsset,
            Fungible,
            NonFungibleEdition
        }
    }

    public partial class MplTokenMetadataClient : TransactionalBaseClient<MplTokenMetadataErrorKind>
    {
        public MplTokenMetadataClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<UseAuthorityRecord>>> GetUseAuthorityRecordsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = UseAuthorityRecord.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<UseAuthorityRecord>>(res);
            List<UseAuthorityRecord> resultingAccounts = new List<UseAuthorityRecord>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => UseAuthorityRecord.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<UseAuthorityRecord>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<CollectionAuthorityRecord>>> GetCollectionAuthorityRecordsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = CollectionAuthorityRecord.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<CollectionAuthorityRecord>>(res);
            List<CollectionAuthorityRecord> resultingAccounts = new List<CollectionAuthorityRecord>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => CollectionAuthorityRecord.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<CollectionAuthorityRecord>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Metadata>>> GetMetadatasAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Metadata.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Metadata>>(res);
            List<Metadata> resultingAccounts = new List<Metadata>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Metadata.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Metadata>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<MasterEditionV2>>> GetMasterEditionV2sAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = MasterEditionV2.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<MasterEditionV2>>(res);
            List<MasterEditionV2> resultingAccounts = new List<MasterEditionV2>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => MasterEditionV2.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<MasterEditionV2>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<MasterEditionV1>>> GetMasterEditionV1sAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = MasterEditionV1.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<MasterEditionV1>>(res);
            List<MasterEditionV1> resultingAccounts = new List<MasterEditionV1>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => MasterEditionV1.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<MasterEditionV1>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Edition>>> GetEditionsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Edition.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Edition>>(res);
            List<Edition> resultingAccounts = new List<Edition>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Edition.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Edition>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ReservationListV2>>> GetReservationListV2sAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = ReservationListV2.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ReservationListV2>>(res);
            List<ReservationListV2> resultingAccounts = new List<ReservationListV2>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => ReservationListV2.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ReservationListV2>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ReservationListV1>>> GetReservationListV1sAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = ReservationListV1.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ReservationListV1>>(res);
            List<ReservationListV1> resultingAccounts = new List<ReservationListV1>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => ReservationListV1.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<ReservationListV1>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<EditionMarker>>> GetEditionMarkersAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = EditionMarker.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<EditionMarker>>(res);
            List<EditionMarker> resultingAccounts = new List<EditionMarker>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => EditionMarker.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<EditionMarker>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<UseAuthorityRecord>> GetUseAuthorityRecordAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<UseAuthorityRecord>(res);
            var resultingAccount = UseAuthorityRecord.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<UseAuthorityRecord>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<CollectionAuthorityRecord>> GetCollectionAuthorityRecordAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<CollectionAuthorityRecord>(res);
            var resultingAccount = CollectionAuthorityRecord.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<CollectionAuthorityRecord>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Metadata>> GetMetadataAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Metadata>(res);
            var resultingAccount = Metadata.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Metadata>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<MasterEditionV2>> GetMasterEditionV2Async(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<MasterEditionV2>(res);
            var resultingAccount = MasterEditionV2.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<MasterEditionV2>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<MasterEditionV1>> GetMasterEditionV1Async(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<MasterEditionV1>(res);
            var resultingAccount = MasterEditionV1.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<MasterEditionV1>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Edition>> GetEditionAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Edition>(res);
            var resultingAccount = Edition.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Edition>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<ReservationListV2>> GetReservationListV2Async(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<ReservationListV2>(res);
            var resultingAccount = ReservationListV2.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<ReservationListV2>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<ReservationListV1>> GetReservationListV1Async(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<ReservationListV1>(res);
            var resultingAccount = ReservationListV1.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<ReservationListV1>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<EditionMarker>> GetEditionMarkerAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<EditionMarker>(res);
            var resultingAccount = EditionMarker.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<EditionMarker>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeUseAuthorityRecordAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, UseAuthorityRecord> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                UseAuthorityRecord parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = UseAuthorityRecord.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeCollectionAuthorityRecordAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, CollectionAuthorityRecord> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                CollectionAuthorityRecord parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = CollectionAuthorityRecord.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeMetadataAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Metadata> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Metadata parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Metadata.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeMasterEditionV2Async(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, MasterEditionV2> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                MasterEditionV2 parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = MasterEditionV2.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeMasterEditionV1Async(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, MasterEditionV1> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                MasterEditionV1 parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = MasterEditionV1.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeEditionAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Edition> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Edition parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Edition.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeReservationListV2Async(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, ReservationListV2> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                ReservationListV2 parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = ReservationListV2.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeReservationListV1Async(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, ReservationListV1> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                ReservationListV1 parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = ReservationListV1.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeEditionMarkerAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, EditionMarker> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                EditionMarker parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = EditionMarker.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<RequestResult<string>> SendCreateMetadataAccountAsync(CreateMetadataAccountAccounts accounts, CreateMetadataAccountArgs createMetadataAccountArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.CreateMetadataAccount(accounts, createMetadataAccountArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUpdateMetadataAccountAsync(UpdateMetadataAccountAccounts accounts, UpdateMetadataAccountArgs updateMetadataAccountArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.UpdateMetadataAccount(accounts, updateMetadataAccountArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeprecatedCreateMasterEditionAsync(DeprecatedCreateMasterEditionAccounts accounts, CreateMasterEditionArgs createMasterEditionArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.DeprecatedCreateMasterEdition(accounts, createMasterEditionArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeprecatedMintNewEditionFromMasterEditionViaPrintingTokenAsync(DeprecatedMintNewEditionFromMasterEditionViaPrintingTokenAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.DeprecatedMintNewEditionFromMasterEditionViaPrintingToken(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUpdatePrimarySaleHappenedViaTokenAsync(UpdatePrimarySaleHappenedViaTokenAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.UpdatePrimarySaleHappenedViaToken(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeprecatedSetReservationListAsync(DeprecatedSetReservationListAccounts accounts, SetReservationListArgs setReservationListArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.DeprecatedSetReservationList(accounts, setReservationListArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeprecatedCreateReservationListAsync(DeprecatedCreateReservationListAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.DeprecatedCreateReservationList(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSignMetadataAsync(SignMetadataAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.SignMetadata(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeprecatedMintPrintingTokensViaTokenAsync(DeprecatedMintPrintingTokensViaTokenAccounts accounts, MintPrintingTokensViaTokenArgs mintPrintingTokensViaTokenArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.DeprecatedMintPrintingTokensViaToken(accounts, mintPrintingTokensViaTokenArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendDeprecatedMintPrintingTokensAsync(DeprecatedMintPrintingTokensAccounts accounts, MintPrintingTokensViaTokenArgs mintPrintingTokensViaTokenArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.DeprecatedMintPrintingTokens(accounts, mintPrintingTokensViaTokenArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCreateMasterEditionAsync(CreateMasterEditionAccounts accounts, CreateMasterEditionArgs createMasterEditionArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.CreateMasterEdition(accounts, createMasterEditionArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendMintNewEditionFromMasterEditionViaTokenAsync(MintNewEditionFromMasterEditionViaTokenAccounts accounts, MintNewEditionFromMasterEditionViaTokenArgs mintNewEditionFromMasterEditionViaTokenArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.MintNewEditionFromMasterEditionViaToken(accounts, mintNewEditionFromMasterEditionViaTokenArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendConvertMasterEditionV1ToV2Async(ConvertMasterEditionV1ToV2Accounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.ConvertMasterEditionV1ToV2(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendMintNewEditionFromMasterEditionViaVaultProxyAsync(MintNewEditionFromMasterEditionViaVaultProxyAccounts accounts, MintNewEditionFromMasterEditionViaTokenArgs mintNewEditionFromMasterEditionViaTokenArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.MintNewEditionFromMasterEditionViaVaultProxy(accounts, mintNewEditionFromMasterEditionViaTokenArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendPuffMetadataAsync(PuffMetadataAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.PuffMetadata(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUpdateMetadataAccountV2Async(UpdateMetadataAccountV2Accounts accounts, UpdateMetadataAccountArgsV2 updateMetadataAccountArgsV2, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.UpdateMetadataAccountV2(accounts, updateMetadataAccountArgsV2, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCreateMetadataAccountV2Async(CreateMetadataAccountV2Accounts accounts, CreateMetadataAccountArgsV2 createMetadataAccountArgsV2, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.CreateMetadataAccountV2(accounts, createMetadataAccountArgsV2, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCreateMasterEditionV3Async(CreateMasterEditionV3Accounts accounts, CreateMasterEditionArgs createMasterEditionArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.CreateMasterEditionV3(accounts, createMasterEditionArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendVerifyCollectionAsync(VerifyCollectionAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.VerifyCollection(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUtilizeAsync(UtilizeAccounts accounts, UtilizeArgs utilizeArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.Utilize(accounts, utilizeArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendApproveUseAuthorityAsync(ApproveUseAuthorityAccounts accounts, ApproveUseAuthorityArgs approveUseAuthorityArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.ApproveUseAuthority(accounts, approveUseAuthorityArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendRevokeUseAuthorityAsync(RevokeUseAuthorityAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.RevokeUseAuthority(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUnverifyCollectionAsync(UnverifyCollectionAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.UnverifyCollection(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendApproveCollectionAuthorityAsync(ApproveCollectionAuthorityAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.ApproveCollectionAuthority(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendRevokeCollectionAuthorityAsync(RevokeCollectionAuthorityAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.RevokeCollectionAuthority(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSetAndVerifyCollectionAsync(SetAndVerifyCollectionAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.SetAndVerifyCollection(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendFreezeDelegatedAccountAsync(FreezeDelegatedAccountAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.FreezeDelegatedAccount(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendThawDelegatedAccountAsync(ThawDelegatedAccountAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.ThawDelegatedAccount(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendRemoveCreatorVerificationAsync(RemoveCreatorVerificationAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.RemoveCreatorVerification(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendBurnNftAsync(BurnNftAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.BurnNft(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendVerifySizedCollectionItemAsync(VerifySizedCollectionItemAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.VerifySizedCollectionItem(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendUnverifySizedCollectionItemAsync(UnverifySizedCollectionItemAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.UnverifySizedCollectionItem(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSetAndVerifySizedCollectionItemAsync(SetAndVerifySizedCollectionItemAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.SetAndVerifySizedCollectionItem(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendCreateMetadataAccountV3Async(CreateMetadataAccountV3Accounts accounts, CreateMetadataAccountArgsV3 createMetadataAccountArgsV3, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.CreateMetadataAccountV3(accounts, createMetadataAccountArgsV3, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSetCollectionSizeAsync(SetCollectionSizeAccounts accounts, SetCollectionSizeArgs setCollectionSizeArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.SetCollectionSize(accounts, setCollectionSizeArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendSetTokenStandardAsync(SetTokenStandardAccounts accounts, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.SetTokenStandard(accounts, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        public async Task<RequestResult<string>> SendBubblegumSetCollectionSizeAsync(BubblegumSetCollectionSizeAccounts accounts, SetCollectionSizeArgs setCollectionSizeArgs, PublicKey feePayer, Func<byte[], PublicKey, byte[]> signingCallback, PublicKey programId)
        {
            Solana.Unity.Rpc.Models.TransactionInstruction instr = Program.MplTokenMetadataProgram.BubblegumSetCollectionSize(accounts, setCollectionSizeArgs, programId);
            return await SignAndSendTransaction(instr, feePayer, signingCallback);
        }

        protected override Dictionary<uint, ProgramError<MplTokenMetadataErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<MplTokenMetadataErrorKind>>{{0U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InstructionUnpackError, "Failed to unpack instruction data")}, {1U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InstructionPackError, "Failed to pack instruction data")}, {2U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotRentExempt, "Lamport balance below rent-exempt threshold")}, {3U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.AlreadyInitialized, "Already initialized")}, {4U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.Uninitialized, "Uninitialized")}, {5U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidMetadataKey, " Metadata's key must match seed of ['metadata', program id, mint] provided")}, {6U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidEditionKey, "Edition's key must match seed of ['metadata', program id, name, 'edition'] provided")}, {7U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.UpdateAuthorityIncorrect, "Update Authority given does not match")}, {8U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.UpdateAuthorityIsNotSigner, "Update Authority needs to be signer to update metadata")}, {9U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotMintAuthority, "You must be the mint authority and signer on this transaction")}, {10U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidMintAuthority, "Mint authority provided does not match the authority on the mint")}, {11U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NameTooLong, "Name too long")}, {12U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.SymbolTooLong, "Symbol too long")}, {13U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.UriTooLong, "URI too long")}, {14U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.UpdateAuthorityMustBeEqualToMetadataAuthorityAndSigner, "Update authority must be equivalent to the metadata's authority and also signer of this transaction")}, {15U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MintMismatch, "Mint given does not match mint on Metadata")}, {16U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.EditionsMustHaveExactlyOneToken, "Editions must have exactly one token")}, {17U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MaxEditionsMintedAlready, "Maximum editions printed already")}, {18U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.TokenMintToFailed, "Token mint to failed")}, {19U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MasterRecordMismatch, "The master edition record passed must match the master record on the edition given")}, {20U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.DestinationMintMismatch, "The destination account does not have the right mint")}, {21U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.EditionAlreadyMinted, "An edition can only mint one of its kind!")}, {22U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.PrintingMintDecimalsShouldBeZero, "Printing mint decimals should be zero")}, {23U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.OneTimePrintingAuthorizationMintDecimalsShouldBeZero, "OneTimePrintingAuthorization mint decimals should be zero")}, {24U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.EditionMintDecimalsShouldBeZero, "EditionMintDecimalsShouldBeZero")}, {25U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.TokenBurnFailed, "Token burn failed")}, {26U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.TokenAccountOneTimeAuthMintMismatch, "The One Time authorization mint does not match that on the token account!")}, {27U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.DerivedKeyInvalid, "Derived key invalid")}, {28U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.PrintingMintMismatch, "The Printing mint does not match that on the master edition!")}, {29U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.OneTimePrintingAuthMintMismatch, "The One Time Printing Auth mint does not match that on the master edition!")}, {30U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.TokenAccountMintMismatch, "The mint of the token account does not match the Printing mint!")}, {31U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.TokenAccountMintMismatchV2, "The mint of the token account does not match the master metadata mint!")}, {32U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotEnoughTokens, "Not enough tokens to mint a limited edition")}, {33U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.PrintingMintAuthorizationAccountMismatch, "The mint on your authorization token holding account does not match your Printing mint!")}, {34U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.AuthorizationTokenAccountOwnerMismatch, "The authorization token account has a different owner than the update authority for the master edition!")}, {35U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.Disabled, "This feature is currently disabled.")}, {36U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CreatorsTooLong, "Creators list too long")}, {37U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CreatorsMustBeAtleastOne, "Creators must be at least one if set")}, {38U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MustBeOneOfCreators, "If using a creators array, you must be one of the creators listed")}, {39U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NoCreatorsPresentOnMetadata, "This metadata does not have creators")}, {40U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CreatorNotFound, "This creator address was not found")}, {41U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidBasisPoints, "Basis points cannot be more than 10000")}, {42U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.PrimarySaleCanOnlyBeFlippedToTrue, "Primary sale can only be flipped to true and is immutable")}, {43U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.OwnerMismatch, "Owner does not match that on the account given")}, {44U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NoBalanceInAccountForAuthorization, "This account has no tokens to be used for authorization")}, {45U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ShareTotalMustBe100, "Share total must equal 100 for creator array")}, {46U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationExists, "This reservation list already exists!")}, {47U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationDoesNotExist, "This reservation list does not exist!")}, {48U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationNotSet, "This reservation list exists but was never set with reservations")}, {49U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationAlreadyMade, "This reservation list has already been set!")}, {50U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.BeyondMaxAddressSize, "Provided more addresses than max allowed in single reservation")}, {51U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NumericalOverflowError, "NumericalOverflowError")}, {52U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationBreachesMaximumSupply, "This reservation would go beyond the maximum supply of the master edition!")}, {53U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.AddressNotInReservation, "Address not in reservation!")}, {54U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotVerifyAnotherCreator, "You cannot unilaterally verify another creator, they must sign")}, {55U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotUnverifyAnotherCreator, "You cannot unilaterally unverify another creator")}, {56U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.SpotMismatch, "In initial reservation setting, spots remaining should equal total spots")}, {57U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.IncorrectOwner, "Incorrect account owner")}, {58U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.PrintingWouldBreachMaximumSupply, "printing these tokens would breach the maximum supply limit of the master edition")}, {59U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.DataIsImmutable, "Data is immutable")}, {60U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.DuplicateCreatorAddress, "No duplicate creator addresses")}, {61U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationSpotsRemainingShouldMatchTotalSpotsAtStart, "Reservation spots remaining should match total spots when first being created")}, {62U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidTokenProgram, "Invalid token program")}, {63U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.DataTypeMismatch, "Data type mismatch")}, {64U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.BeyondAlottedAddressSize, "Beyond alotted address size in reservation!")}, {65U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationNotComplete, "The reservation has only been partially alotted")}, {66U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.TriedToReplaceAnExistingReservation, "You cannot splice over an existing reservation!")}, {67U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidOperation, "Invalid operation")}, {68U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidOwner, "Invalid Owner")}, {69U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.PrintingMintSupplyMustBeZeroForConversion, "Printing mint supply must be zero for conversion")}, {70U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.OneTimeAuthMintSupplyMustBeZeroForConversion, "One Time Auth mint supply must be zero for conversion")}, {71U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidEditionIndex, "You tried to insert one edition too many into an edition mark pda")}, {72U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.ReservationArrayShouldBeSizeOne, "In the legacy system the reservation needs to be of size one for cpu limit reasons")}, {73U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.IsMutableCanOnlyBeFlippedToFalse, "Is Mutable can only be flipped to false")}, {74U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CollectionCannotBeVerifiedInThisInstruction, "Cannont Verify Collection in this Instruction")}, {75U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.Removed, "This instruction was deprecated in a previous release and is now removed")}, {76U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MustBeBurned, "This token use method is burn and there are no remaining uses, it must be burned")}, {77U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidUseMethod, "This use method is invalid")}, {78U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotChangeUseMethodAfterFirstUse, "Cannot Change Use Method after the first use")}, {79U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotChangeUsesAfterFirstUse, "Cannot Change Remaining or Available uses after the first use")}, {80U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CollectionNotFound, "Collection Not Found on Metadata")}, {81U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidCollectionUpdateAuthority, "Collection Update Authority is invalid")}, {82U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CollectionMustBeAUniqueMasterEdition, "Collection Must Be a Unique Master Edition v2")}, {83U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.UseAuthorityRecordAlreadyExists, "The Use Authority Record Already Exists, to modify it Revoke, then Approve")}, {84U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.UseAuthorityRecordAlreadyRevoked, "The Use Authority Record is empty or already revoked")}, {85U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.Unusable, "This token has no uses")}, {86U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotEnoughUses, "There are not enough Uses left on this token.")}, {87U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CollectionAuthorityRecordAlreadyExists, "This Collection Authority Record Already Exists.")}, {88U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CollectionAuthorityDoesNotExist, "This Collection Authority Record Does Not Exist.")}, {89U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidUseAuthorityRecord, "This Use Authority Record is invalid.")}, {90U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidCollectionAuthorityRecord, "This Collection Authority Record is invalid.")}, {91U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidFreezeAuthority, "Metadata does not match the freeze authority on the mint")}, {92U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidDelegate, "All tokens in this account have not been delegated to this user.")}, {93U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotAdjustVerifiedCreator, "Creator can not be adjusted once they are verified.")}, {94U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotRemoveVerifiedCreator, "Verified creators cannot be removed.")}, {95U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotWipeVerifiedCreators, "Can not wipe verified creators.")}, {96U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotAllowedToChangeSellerFeeBasisPoints, "Not allowed to change seller fee basis points.")}, {97U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.EditionOverrideCannotBeZero, "Edition override cannot be zero")}, {98U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.InvalidUser, "Invalid User")}, {99U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.RevokeCollectionAuthoritySignerIncorrect, "Revoke Collection Authority signer is incorrect")}, {100U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.TokenCloseFailed, "Token close failed")}, {101U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.UnsizedCollection, "Can't use this function on unsized collection")}, {102U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.SizedCollection, "Can't use this function on a sized collection")}, {103U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MissingCollectionMetadata, "Can't burn a verified member of a collection w/o providing collection metadata account")}, {104U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotAMemberOfCollection, "This NFT is not a member of the specified collection.")}, {105U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotVerifiedMemberOfCollection, "This NFT is not a verified member of the specified collection.")}, {106U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotACollectionParent, "This NFT is not a collection parent NFT.")}, {107U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CouldNotDetermineTokenStandard, "Could not determine a TokenStandard type.")}, {108U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MissingEditionAccount, "This mint account has an edition but none was provided.")}, {109U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.NotAMasterEdition, "This edition is not a Master Edition")}, {110U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.MasterEditionHasPrints, "This Master Edition has existing prints")}, {111U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.BorshDeserializationError, "Borsh Deserialization Error")}, {112U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CannotUpdateVerifiedCollection, "Cannot update a verified colleciton in this command")}, {113U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.CollectionMasterEditionAccountInvalid, "Edition account aoesnt match collection ")}, {114U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.AlreadyVerified, "Item is already verified.")}, {115U, new ProgramError<MplTokenMetadataErrorKind>(MplTokenMetadataErrorKind.AlreadyUnverified, "Item is already unverified.")}, };
        }
    }

    namespace Program
    {
        public class CreateMetadataAccountAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class UpdateMetadataAccountAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey UpdateAuthority { get; set; }
        }

        public class DeprecatedCreateMasterEditionAccounts
        {
            public PublicKey Edition { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey PrintingMint { get; set; }

            public PublicKey OneTimePrintingAuthorizationMint { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey PrintingMintAuthority { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey OneTimePrintingAuthorizationMintAuthority { get; set; }
        }

        public class DeprecatedMintNewEditionFromMasterEditionViaPrintingTokenAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Edition { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey PrintingMint { get; set; }

            public PublicKey MasterTokenAccount { get; set; }

            public PublicKey EditionMarker { get; set; }

            public PublicKey BurnAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey MasterUpdateAuthority { get; set; }

            public PublicKey MasterMetadata { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey ReservationList { get; set; }
        }

        public class UpdatePrimarySaleHappenedViaTokenAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Token { get; set; }
        }

        public class DeprecatedSetReservationListAccounts
        {
            public PublicKey MasterEdition { get; set; }

            public PublicKey ReservationList { get; set; }

            public PublicKey Resource { get; set; }
        }

        public class DeprecatedCreateReservationListAccounts
        {
            public PublicKey ReservationList { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey Resource { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class SignMetadataAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Creator { get; set; }
        }

        public class DeprecatedMintPrintingTokensViaTokenAccounts
        {
            public PublicKey Destination { get; set; }

            public PublicKey Token { get; set; }

            public PublicKey OneTimePrintingAuthorizationMint { get; set; }

            public PublicKey PrintingMint { get; set; }

            public PublicKey BurnAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class DeprecatedMintPrintingTokensAccounts
        {
            public PublicKey Destination { get; set; }

            public PublicKey PrintingMint { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class CreateMasterEditionAccounts
        {
            public PublicKey Edition { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class MintNewEditionFromMasterEditionViaTokenAccounts
        {
            public PublicKey NewMetadata { get; set; }

            public PublicKey NewEdition { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey NewMint { get; set; }

            public PublicKey EditionMarkPda { get; set; }

            public PublicKey NewMintAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey TokenAccountOwner { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey NewMetadataUpdateAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class ConvertMasterEditionV1ToV2Accounts
        {
            public PublicKey MasterEdition { get; set; }

            public PublicKey OneTimeAuth { get; set; }

            public PublicKey PrintingMint { get; set; }
        }

        public class MintNewEditionFromMasterEditionViaVaultProxyAccounts
        {
            public PublicKey NewMetadata { get; set; }

            public PublicKey NewEdition { get; set; }

            public PublicKey MasterEdition { get; set; }

            public PublicKey NewMint { get; set; }

            public PublicKey EditionMarkPda { get; set; }

            public PublicKey NewMintAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey VaultAuthority { get; set; }

            public PublicKey SafetyDepositStore { get; set; }

            public PublicKey SafetyDepositBox { get; set; }

            public PublicKey Vault { get; set; }

            public PublicKey NewMetadataUpdateAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey TokenVaultProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class PuffMetadataAccounts
        {
            public PublicKey Metadata { get; set; }
        }

        public class UpdateMetadataAccountV2Accounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey UpdateAuthority { get; set; }
        }

        public class CreateMetadataAccountV2Accounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class CreateMasterEditionV3Accounts
        {
            public PublicKey Edition { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class VerifyCollectionAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey Collection { get; set; }

            public PublicKey CollectionMasterEditionAccount { get; set; }
        }

        public class UtilizeAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey UseAuthority { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey AtaProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }

            public PublicKey UseAuthorityRecord { get; set; }

            public PublicKey Burner { get; set; }
        }

        public class ApproveUseAuthorityAccounts
        {
            public PublicKey UseAuthorityRecord { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey User { get; set; }

            public PublicKey OwnerTokenAccount { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey Burner { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class RevokeUseAuthorityAccounts
        {
            public PublicKey UseAuthorityRecord { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey User { get; set; }

            public PublicKey OwnerTokenAccount { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class UnverifyCollectionAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey Collection { get; set; }

            public PublicKey CollectionMasterEditionAccount { get; set; }

            public PublicKey CollectionAuthorityRecord { get; set; }
        }

        public class ApproveCollectionAuthorityAccounts
        {
            public PublicKey CollectionAuthorityRecord { get; set; }

            public PublicKey NewCollectionAuthority { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class RevokeCollectionAuthorityAccounts
        {
            public PublicKey CollectionAuthorityRecord { get; set; }

            public PublicKey DelegateAuthority { get; set; }

            public PublicKey RevokeAuthority { get; set; }

            public PublicKey Metadata { get; set; }

            public PublicKey Mint { get; set; }
        }

        public class SetAndVerifyCollectionAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey Collection { get; set; }

            public PublicKey CollectionMasterEditionAccount { get; set; }

            public PublicKey CollectionAuthorityRecord { get; set; }
        }

        public class FreezeDelegatedAccountAccounts
        {
            public PublicKey Delegate { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Edition { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class ThawDelegatedAccountAccounts
        {
            public PublicKey Delegate { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey Edition { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey TokenProgram { get; set; }
        }

        public class RemoveCreatorVerificationAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Creator { get; set; }
        }

        public class BurnNftAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Owner { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey TokenAccount { get; set; }

            public PublicKey MasterEditionAccount { get; set; }

            public PublicKey SplTokenProgram { get; set; }

            public PublicKey CollectionMetadata { get; set; }
        }

        public class VerifySizedCollectionItemAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey Collection { get; set; }

            public PublicKey CollectionMasterEditionAccount { get; set; }

            public PublicKey CollectionAuthorityRecord { get; set; }
        }

        public class UnverifySizedCollectionItemAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey Collection { get; set; }

            public PublicKey CollectionMasterEditionAccount { get; set; }

            public PublicKey CollectionAuthorityRecord { get; set; }
        }

        public class SetAndVerifySizedCollectionItemAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey Collection { get; set; }

            public PublicKey CollectionMasterEditionAccount { get; set; }

            public PublicKey CollectionAuthorityRecord { get; set; }
        }

        public class CreateMetadataAccountV3Accounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey MintAuthority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class SetCollectionSizeAccounts
        {
            public PublicKey CollectionMetadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey CollectionAuthorityRecord { get; set; }
        }

        public class SetTokenStandardAccounts
        {
            public PublicKey Metadata { get; set; }

            public PublicKey UpdateAuthority { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey Edition { get; set; }
        }

        public class BubblegumSetCollectionSizeAccounts
        {
            public PublicKey CollectionMetadata { get; set; }

            public PublicKey CollectionAuthority { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey BubblegumSigner { get; set; }

            public PublicKey CollectionAuthorityRecord { get; set; }
        }

        public static class MplTokenMetadataProgram
        {
            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateMetadataAccount(CreateMetadataAccountAccounts accounts, CreateMetadataAccountArgs createMetadataAccountArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(8178469667106277707UL, offset);
                offset += 8;
                offset += createMetadataAccountArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateMetadataAccount(UpdateMetadataAccountAccounts accounts, UpdateMetadataAccountArgs updateMetadataAccountArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12481094111162994317UL, offset);
                offset += 8;
                offset += updateMetadataAccountArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeprecatedCreateMasterEdition(DeprecatedCreateMasterEditionAccounts accounts, CreateMasterEditionArgs createMasterEditionArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Edition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PrintingMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.OneTimePrintingAuthorizationMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PrintingMintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.OneTimePrintingAuthorizationMintAuthority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(1535548169317089179UL, offset);
                offset += 8;
                offset += createMasterEditionArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeprecatedMintNewEditionFromMasterEditionViaPrintingToken(DeprecatedMintNewEditionFromMasterEditionViaPrintingTokenAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Edition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PrintingMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EditionMarker, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.BurnAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterUpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReservationList, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16472848840885413018UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdatePrimarySaleHappenedViaToken(UpdatePrimarySaleHappenedViaTokenAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Token, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(7130185429074936236UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeprecatedSetReservationList(DeprecatedSetReservationListAccounts accounts, SetReservationListArgs setReservationListArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReservationList, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Resource, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10285882053547400260UL, offset);
                offset += 8;
                offset += setReservationListArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeprecatedCreateReservationList(DeprecatedCreateReservationListAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.ReservationList, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Resource, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(5217895164288295851UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SignMetadata(SignMetadataAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Creator, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15125896718475720114UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeprecatedMintPrintingTokensViaToken(DeprecatedMintPrintingTokensViaTokenAccounts accounts, MintPrintingTokensViaTokenArgs mintPrintingTokensViaTokenArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Destination, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Token, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.OneTimePrintingAuthorizationMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PrintingMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.BurnAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16070022771934503508UL, offset);
                offset += 8;
                offset += mintPrintingTokensViaTokenArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction DeprecatedMintPrintingTokens(DeprecatedMintPrintingTokensAccounts accounts, MintPrintingTokensViaTokenArgs mintPrintingTokensViaTokenArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Destination, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PrintingMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(8734044823844056002UL, offset);
                offset += 8;
                offset += mintPrintingTokensViaTokenArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateMasterEdition(CreateMasterEditionAccounts accounts, CreateMasterEditionArgs createMasterEditionArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Edition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(4994238245997957811UL, offset);
                offset += 8;
                offset += createMasterEditionArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction MintNewEditionFromMasterEditionViaToken(MintNewEditionFromMasterEditionViaTokenAccounts accounts, MintNewEditionFromMasterEditionViaTokenArgs mintNewEditionFromMasterEditionViaTokenArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EditionMarkPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewMintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccountOwner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewMetadataUpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(8538056878863670012UL, offset);
                offset += 8;
                offset += mintNewEditionFromMasterEditionViaTokenArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ConvertMasterEditionV1ToV2(ConvertMasterEditionV1ToV2Accounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.OneTimeAuth, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PrintingMint, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17196852476832914137UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction MintNewEditionFromMasterEditionViaVaultProxy(MintNewEditionFromMasterEditionViaVaultProxyAccounts accounts, MintNewEditionFromMasterEditionViaTokenArgs mintNewEditionFromMasterEditionViaTokenArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.EditionMarkPda, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewMintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.VaultAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SafetyDepositStore, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SafetyDepositBox, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Vault, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewMetadataUpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenVaultProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(3441352618856609346UL, offset);
                offset += 8;
                offset += mintNewEditionFromMasterEditionViaTokenArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction PuffMetadata(PuffMetadataAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(8234812580625242455UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateMetadataAccountV2(UpdateMetadataAccountV2Accounts accounts, UpdateMetadataAccountArgsV2 updateMetadataAccountArgsV2, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15315011533438223562UL, offset);
                offset += 8;
                offset += updateMetadataAccountArgsV2.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateMetadataAccountV2(CreateMetadataAccountV2Accounts accounts, CreateMetadataAccountArgsV2 createMetadataAccountArgsV2, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(18357391354724174104UL, offset);
                offset += 8;
                offset += createMetadataAccountArgsV2.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateMasterEditionV3(CreateMasterEditionV3Accounts accounts, CreateMasterEditionArgs createMasterEditionArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Edition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17109885589388367251UL, offset);
                offset += 8;
                offset += createMasterEditionArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction VerifyCollection(VerifyCollectionAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Collection, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMasterEditionAccount, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12212134156261749048UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction Utilize(UtilizeAccounts accounts, UtilizeArgs utilizeArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UseAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Owner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AtaProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UseAuthorityRecord, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Burner, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11797652773333537384UL, offset);
                offset += 8;
                offset += utilizeArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ApproveUseAuthority(ApproveUseAuthorityAccounts accounts, ApproveUseAuthorityArgs approveUseAuthorityArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UseAuthorityRecord, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.OwnerTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Burner, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(17016032427477107726UL, offset);
                offset += 8;
                offset += approveUseAuthorityArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction RevokeUseAuthority(RevokeUseAuthorityAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UseAuthorityRecord, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.OwnerTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(6083762275981771468UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UnverifyCollection(UnverifyCollectionAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Collection, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMasterEditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthorityRecord, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12158180955007941626UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ApproveCollectionAuthority(ApproveCollectionAuthorityAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionAuthorityRecord, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.NewCollectionAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UpdateAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(8006065610189474046UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction RevokeCollectionAuthority(RevokeCollectionAuthorityAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionAuthorityRecord, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.DelegateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.RevokeAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11141958382557563679UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SetAndVerifyCollection(SetAndVerifyCollectionAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Collection, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMasterEditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthorityRecord, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16912400468640658155UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction FreezeDelegatedAccount(FreezeDelegatedAccountAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Delegate, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Edition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9178357432550494222UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ThawDelegatedAccount(ThawDelegatedAccountAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Delegate, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Edition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(12307995700928682223UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction RemoveCreatorVerification(RemoveCreatorVerificationAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Creator, true)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(471646898047730217UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction BurnNft(BurnNftAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Owner, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MasterEditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SplTokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionMetadata, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2244749479137185143UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction VerifySizedCollectionItem(VerifySizedCollectionItemAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Collection, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMasterEditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthorityRecord, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(10643240745204412246UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UnverifySizedCollectionItem(UnverifySizedCollectionItemAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Collection, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMasterEditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthorityRecord, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15965430685053926305UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SetAndVerifySizedCollectionItem(SetAndVerifySizedCollectionItemAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Collection, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionMasterEditionAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthorityRecord, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(4894946615504759224UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction CreateMetadataAccountV3(CreateMetadataAccountV3Accounts accounts, CreateMetadataAccountArgsV3 createMetadataAccountArgsV3, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.MintAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.UpdateAuthority, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11221894932101794859UL, offset);
                offset += 8;
                offset += createMetadataAccountArgsV3.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SetCollectionSize(SetCollectionSizeAccounts accounts, SetCollectionSizeArgs setCollectionSizeArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthorityRecord, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2866505066048061085UL, offset);
                offset += 8;
                offset += setCollectionSizeArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SetTokenStandard(SetTokenStandardAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Metadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UpdateAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Edition, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9282387356091602067UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction BubblegumSetCollectionSize(BubblegumSetCollectionSizeAccounts accounts, SetCollectionSizeArgs setCollectionSizeArgs, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionAuthority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.BubblegumSigner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionAuthorityRecord, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(448315544931129318UL, offset);
                offset += 8;
                offset += setCollectionSizeArgs.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}