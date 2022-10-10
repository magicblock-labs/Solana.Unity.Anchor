using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solnet.Anchor;
using Solana.Unity.Rpc;
using System.IO;
using Solnet.Anchor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AnchorSourceGenerator
{
    static int Main(string[] args)
    {
        var parser = new Parser(x => x.CaseInsensitiveEnumValues = true);
        return parser.ParseArguments<CommandLineOptions>(args)
            .MapResult((CommandLineOptions opts) =>
            {
                string idlStr;

                if (opts.File != null)
                {
                    idlStr = File.ReadAllText(opts.File);
                }
                else
                {
                    idlStr = IdlRetriever.GetIdl(new(opts.Address), GetRpcClient(opts.Network));
                }

                if (idlStr == null)
                {
                    Console.WriteLine("Unable to read IDL from specified source. exiting");
                    return -2;
                }

                if (opts.Json != null)
                {
                    File.WriteAllText(opts.Json, idlStr);
                }

                var jsonParsed = JObject.Parse(idlStr);

                // Set UnixTimestamp to Int64 in accounts
                foreach (JToken account in (JArray)jsonParsed["accounts"])
                {
                    JToken accountType = account["type"];
                    foreach (JObject fields in (JArray)accountType["fields"])
                    {
                        if (fields["type"] != null && fields["type"].Type.ToString() == "Object" && fields["type"]["defined"] != null && fields["type"]["defined"].ToString() == "UnixTimestamp")
                        {
                            fields["type"].Replace("i64");
                        }
                    }
                }

                // Set UnixTimestamp to Int64 in instructions
                foreach (JObject instruction in (JArray)jsonParsed["instructions"])
                {
                    if ((JArray)instruction["args"] != null)
                    {
                        foreach (JObject arg in (JArray)instruction["args"])
                        {
                            var argType = arg["type"];
                            if (argType.Type.ToString() == "Object" && argType != null)
                            {
                                if (argType["defined"] != null && argType["defined"].ToString() == "UnixTimestamp")
                                {
                                    argType.Replace("i64");
                                }
                            }
                        }
                    }
                }
                
                Idl idl = IdlParser.Parse(jsonParsed.ToString());

                if (idl == null)
                {
                    Console.WriteLine("No IDL was generated. exiting");
                    return -3;
                }

                idl.DefaultProgramAddress = opts.Address;

                ClientGenerator cg = new ClientGenerator();

                var code = cg.GenerateCode(idl);

                Console.WriteLine(idl.NamePascalCase);

                if (!string.IsNullOrWhiteSpace(opts.Out))
                    File.WriteAllText(opts.Out, code);

                if (opts.StdOut)
                    Console.Write(code);

                return 0;
            },
            (IEnumerable<Error> errors) =>
            {
                if (!errors.Any(x => x is HelpRequestedError or VersionRequestedError or HelpVerbRequestedError)) return -1; // Invalid arguments
                return 0;
            });
    }

    public static IRpcClient GetRpcClient(Network network)
    => network switch
    {
        Network.Devnet => ClientFactory.GetClient(Cluster.DevNet),
        Network.Testnet => ClientFactory.GetClient(Cluster.TestNet),
        _ => ClientFactory.GetClient(Cluster.MainNet)
    };
}

public enum Network
{
    Mainnet,
    Devnet,
    Testnet
}

public class CommandLineOptions
{
    [Option('a', "address", Group = "source", HelpText = "Anchor Program Address")]
    public string Address { get; set; }

    [Option('n', " network", Group = "source",
            HelpText = "Network to fetch address based IDL, default: mainnet (mainnet, devnet, testnet)",
            Default = Network.Mainnet)]
    public Network Network { get; set; }


    [Option('i', "idl", Group = "source", HelpText = "Idl Source file")]
    public string File { get; set; }


    [Option('o', "output", Group = "output", HelpText = "File to write generated C# code")]
    public string Out { get; set; }

    [Option('s', "stdout", Group = "output", HelpText = "Write generated C# code to stdout")]
    public bool StdOut { get; set; }

    [Option('j', "json", Group = "output", HelpText = "File to write IDL json")]
    public string Json { get; set; }
}