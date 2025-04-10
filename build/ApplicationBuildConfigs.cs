using Cake.Common;
using Cake.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record SourceGeneratorProjectPaths(
    string ProjectName,
    string PathToSln,
    string ProjectFolder,
    string CsprojFile,
    string UnitTestProj,
    string OutDir,
    string NuGetFilePath)
{
    public static SourceGeneratorProjectPaths LoadFromContext(ICakeContext context, string buildConfiguration, string srcDirectory, string nugetVersion)
    {
        var projectName = "PublicInterfaceGenerator";
        var pathToSln = srcDirectory + $"/{projectName}.sln";
        var projectDir = srcDirectory + $"/{projectName}";
        var csProjFile = projectDir + $"/{projectName}.csproj";
        var unitTestsProj = srcDirectory + $"/UnitTests/UnitTests.csproj";
        var outDir = projectDir + $"/bin/{buildConfiguration}/cake-build-output/source-generator";
        var nugetFilePath = outDir + $"/*{nugetVersion}.nupkg";

        return new SourceGeneratorProjectPaths(
            projectName,
            pathToSln,
            projectDir,
            csProjFile,
            unitTestsProj,
            outDir,
            nugetFilePath);
    }
};

public record AttributesProjectPaths(
    string ProjectName,
    string ProjectFolder,
    string CsprojFile,
    string OutDir,
    string NuGetFilePath)
{
    public static AttributesProjectPaths LoadFromContext(ICakeContext context, string buildConfiguration, string srcDirectory, string nugetVersion)
    {
        var projectName = "PublicInterfaceGenerator.Attributes";
        var projectDir = srcDirectory + $"/{projectName}";
        var csProjFile = projectDir + $"/{projectName}.csproj";
        var outDir = projectDir + $"/bin/{buildConfiguration}/cake-build-output/attributes";
        var nugetFilePath = outDir + $"/*{nugetVersion}.nupkg";

        return new AttributesProjectPaths(
            projectName,
            projectDir,
            csProjFile,
            outDir,
            nugetFilePath);
    }
};
