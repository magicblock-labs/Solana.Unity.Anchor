using Solana.Unity.Anchor.Converters;
using Solana.Unity.Anchor.Models;
using Solana.Unity.Anchor.Models.Types;
using Solana.Unity.Rpc;
using Solana.Unity.Wallet;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Solana.Unity.Anchor
{
    public static class IdlParser
    {

        public static Idl Parse(string idl)
        {
            var res = JsonSerializer.Deserialize<Idl>(idl, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new IIdlSeedTypeConverter() }
            });
            PopulateFields(res);
            return res;
        }

        public static Idl ParseFile(string idlFile)
        {
            return Parse(File.ReadAllText(idlFile));
        }

        public static async Task<Idl> ParseProgram(PublicKey pk)
        {
            return await ParseProgram(pk, ClientFactory.GetClient(Cluster.MainNet));
        }

        public static async Task<Idl> ParseProgram(PublicKey pk, IRpcClient client)
        {
            var idlStr = await IdlRetriever.GetIdl(pk, client);
            var idl = Parse(idlStr);

            return idl;
        }
        
        // In the newer IDL spec the accounts types are not directly defined in "accounts", but "types"
        // This method will populate the accounts from the "types" field
        private static void PopulateFields(Idl res)
        {
            if(res?.Accounts == null || res.Accounts?.Length == 0 || res.Types == null || res.Types.Length == 0) return;
            for(int i = 0; i < res.Accounts.Length; i++)
            {
                var account = res.Accounts[i];
                foreach (var type in res.Types)
                {
                    if (type.Name == account.Name)
                    {
                        if (account is StructIdlTypeDefinition)
                        {
                            res.Accounts[i] = new StructIdlTypeDefinition()
                            {
                                Name = account.Name,
                                Fields = ((StructIdlTypeDefinition)type).Fields,
                                Discriminator = ((StructIdlTypeDefinition)account).Discriminator,
                            };
                        }
                    }
                }
            }
        }

    }
}