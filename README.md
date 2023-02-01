<p align="center">
    <img src="https://raw.githubusercontent.com/garbles-labs/Solana.Unity.Anchor/master/assets/icon.png" margin="auto" height="175"/>
</p>

<p align="center">
    <a href="https://github.com/garbles-labs/Solana.Unity.Anchor/actions/workflows/dotnet.yml">
        <img src="https://github.com/garbles-labs/Solana.Unity.Anchor/actions/workflows/dotnet.yml/badge.svg"
            alt="Build" ></a>
    <a href="https://github.com/garbles-labs/Solana.Unity.Anchor/actions/workflows/publish.yml">
       <img src="https://github.com/garbles-labs/Solana.Unity.Anchor/actions/workflows/publish.yml/badge.svg" 
            alt="Release"></a>
    <a href="">
        <img src="https://img.shields.io/github/license/garbles-labs/Solana.Unity-Core?style=flat-square"
            alt="Code License"></a>
    <a href="https://discord.gg/cReXaBReZt">
       <img alt="Discord" src="https://img.shields.io/discord/849407317761064961?style=flat-square"
            alt="Join the discussion!"></a>
</p>

<div style="text-align:center">

<p>

# Solana.Unity.Anchor

Solana.Unity.Anchor is a fork of Solnet.Anchor that generates code compatible with Unity dotnet standard 2.1.

Solana.Unity.Anchor rely on [Solana.Unity-Core](https://github.com/garbles-labs/Solana.Unity-Core) libraries and can be used with the [Solana.Unity-SDK](https://github.com/garbles-labs/Solana.Unity-SDK)

## Features

This repo contains 3 main projects:
- Solnet.Anchor: IDL parsing and code generation
- Solnet.Anchor.Tool: dotnet tool executable that interfaces with the project above.
- Solnet.Anchor.SourceGenerator: Roslyn source generator that depends on the tool above to automatically generate code from IDL in your IDE.

Currently covers all of IDL features with the exception of events and seeds.

## Requirements

Solnet.Anchor and Solnet.Anchor.Tool are compiled and run in net6. Could be easily backported to net5.
Solnet.Anchor.SourceGenerator is compiled in netstandard2.1 to be able to be used as a Roslyn Source Generator. However, machine needs net6 as it just calls Solnet.Anchor.Tool that requires net6.

Generated code can be run using net5 or net6, and the respective Solnet version >=5.0.3 or >=6.0.3 libraries.

## Instructions

To compile this project:

`dotnet build`

To use the generator tool:

- `dotnet tool install Solana.Unity.Anchor.Tool` (if you use `-g` option, you won't need to do this for every project')
- `dotnet anchorgen -i idl/file.json -o src/ProgramCode.cs`  
   - You can generate from a live program that uploaded its idl using flag `-a`, or add the default program address if used alongside `-i`
   - You stdout output is supported using `-s` flag

To use the source generator roslyn plugin:

- In your .csproj add `<PackageReference Include="Solnet.Anchor.SourceGenerator" OutputItemType="Analyzer" ReferenceOutputAssembly="false"/>`
- For each project you want to generate sources from an address, add inside `PropertyGroup`: `<AnchorGenerator>address1,address2</AnchorGenerator>`
- For each project you want to generate sources from IDL, add `<AdditionalFiles Include="IdlFile.json" Address="GDDMwNyyx8uB6zrqwBFHjLLG3TBYk2F8Az4yrQC5RzMp" AnchorGenerate="true" />`


# Support

Consider supporting us:

* Sol Address: **oaksGKfwkFZwCniyCF35ZVxHDPexQ3keXNTiLa7RCSp**
* [Mango Ref Link](https://trade.mango.markets/?ref=MangoSharp)

## Contribution

We encourage everyone to contribute, submit issues, PRs, discuss. Every kind of help is welcome.

## Maintainers

* **Hugo** - [murlokito](https://github.com/murlokito)
* **Tiago** - [tiago](https://github.com/tiago18c)

See also the list of [contributors](https://github.com/bmresearch/Solnet.Anchor/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/bmresearch/Solnet.Anchor/blob/master/LICENSE) file for details
