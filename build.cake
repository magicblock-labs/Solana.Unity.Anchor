#module nuget:?package=Cake.DotNetTool.Module&version=0.4.0
#addin nuget:?package=Cake.Coverlet&version=2.5.4
#tool dotnet:?package=dotnet-reportgenerator-globaltool&version=4.8.7

var testProjectsRelativePaths = new string[]
{
    "./Solana.Unity.Anchor.Test/Solana.Unity.Anchor.Test.csproj",
};


var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");
var solutionFolder = "./";
var artifactsDir = MakeAbsolute(Directory("artifacts"));

var reportTypes = "HtmlInline";
var coverageFolder = "./code_coverage";
var coverageFolderIntegration = "./code_coverage_integration";

var coberturaFileName = "results";
var coverageFilePath = Directory(coverageFolder) + File(coberturaFileName + ".info");
var jsonFilePath = Directory(coverageFolder) + File(coberturaFileName + ".json");

var packagesDir = artifactsDir.Combine(Directory("packages"));

var deliverables = new[] {"Solana.Unity.Anchor.Tool", "Solana.Unity.Anchor.SourceGenerator"};

Task("Clean")
    .Does(() => {
        CleanDirectory(artifactsDir);
    });

Task("Restore")
    .Does(() => {
    DotNetCoreRestore(solutionFolder);
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild(solutionFolder, new DotNetCoreBuildSettings
        {
            NoRestore = true,
            Configuration = configuration
        });
    });


Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
    
        var coverletSettings = new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputDirectory = coverageFolder,
            CoverletOutputName = coberturaFileName
        };

        var testSettings = new DotNetCoreTestSettings
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true,
            ArgumentCustomization = args => args.Append($"--logger trx")
        };

        DotNetCoreTest(testProjectsRelativePaths[0], testSettings, coverletSettings);

        coverletSettings.MergeWithFile = jsonFilePath;
        for (int i = 1; i < testProjectsRelativePaths.Length; i++)
        {
            if (i == testProjectsRelativePaths.Length - 1)
            {
                coverletSettings.CoverletOutputFormat = CoverletOutputFormat.lcov;
            }
            DotNetCoreTest(testProjectsRelativePaths[i], testSettings, coverletSettings);
        }
    });

Task("Publish")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCorePublish(solutionFolder, new DotNetCorePublishSettings
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true,
            OutputDirectory = artifactsDir
        });
    });

Task("Pack")
    .IsDependentOn("Publish")
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            IncludeSymbols = true,
            OutputDirectory = packagesDir,
        };

        foreach(var deliverable in deliverables)
        {
            var f = File(deliverable + "/" + deliverable + ".csproj").Path;

            DotNetCorePack(f.FullPath, settings);
        }
    });

RunTarget(target);