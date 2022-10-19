using Solana.Unity.Anchor.Converters;
using Solana.Unity.Anchor.Models;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Utilities;
using Solana.Unity.Wallet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.IO.Compression;
using System.Text;
using Solana.Unity.Programs.Utilities;

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
            return res;
        }

        public static Idl ParseFile(string idlFile)
        {
            return Parse(File.ReadAllText(idlFile));
        }

        public static Idl ParseProgram(PublicKey pk)
        {
            return ParseProgram(pk, ClientFactory.GetClient(Cluster.MainNet));
        }

        public static Idl ParseProgram(PublicKey pk, IRpcClient client)
        {
            var idlStr = IdlRetriever.GetIdl(pk, client);
            var idl = Parse(idlStr);

            return idl;
        }

    }
}