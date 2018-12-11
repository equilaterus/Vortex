#load "./parameters.cake"

public class BuildPaths
{
    public BuildFiles Files { get; private set; }
    public BuildDirectories Directories { get; private set; }

    public static BuildPaths GetPaths(
		ICakeContext context, 
		BuildParameters parameters, 
		bool integrationTests)
    {
        var configuration =  parameters.Configuration;
        var buildDirectories = GetBuildDirectories(context, integrationTests);		

        var testAssemblies = buildDirectories
			.TestDirs
            .Select(dir => dir.Combine("bin")
                            .Combine(configuration)
                            .Combine(parameters.TargetFramework)
                            .CombineWithFilePath(dir.GetDirectoryName() + ".dll"))
            .ToList();

        var testProjects =  buildDirectories
			.TestDirs
			.Select(dir => dir.CombineWithFilePath(
							dir.GetDirectoryName() + ".csproj"))
			.ToList();

        var buildFiles = new BuildFiles(
            buildDirectories.RootDir.CombineWithFilePath("Equilaterus.Vortex.sln"),
            buildDirectories.TestResults.CombineWithFilePath("OpenCover.xml"),
            testAssemblies,
            testProjects);
        
        return new BuildPaths
        {
            Files = buildFiles,
            Directories = buildDirectories
        };
    }

    public static BuildDirectories GetBuildDirectories(
		ICakeContext context,
		bool integrationTests)
    {
        var rootDir = (DirectoryPath)context.Directory("../");
        var artifacts = rootDir.Combine(".artifacts");
        var testResults = artifacts.Combine("Test-Results");
		
        var testsPath = rootDir.Combine(context.Directory("test")); 
		var vortexTests = testsPath.Combine(
				context.Directory("Vortex.Tests"));	
		var vortexIntegrationTests = testsPath.Combine(
				context.Directory("Vortex.IntegrationTests"));
        var efCoreTests = testsPath.Combine(
                context.Directory("Vortex.Saturn.Data.EFCore.FunctionalTests")); 
        var mongoDBTests = testsPath.Combine(
                context.Directory("Vortex.Saturn.Data.MongoDB.FunctionalTests"));
		var azureFilesTests = testsPath.Combine(
				context.Directory("Vortex.Saturn.Files.Azure.FunctionalTests"));			
		var localFilesTests = testsPath.Combine(
				context.Directory("Vortex.Saturn.Files.Local.FunctionalTests"));			

        var srcPath = rootDir.Combine(context.Directory("src"));
        var efCore = srcPath.Combine(
                context.Directory("Vortex.Saturn.Data.EFCore"));
        var mongoDB = srcPath.Combine(
                context.Directory("Vortex.Saturn.Data.MongoDB"));
		var azureFiles = srcPath.Combine(
				context.Directory("Vortex.Saturn.Files.Azure"));
		var localFiles = srcPath.Combine(
				context.Directory("Vortex.Saturn.Files.Local"));
		
        var testDirs = new []{
								vortexTests								
                            };
		var integrationTestDirs = new []{
								vortexTests,
								vortexIntegrationTests,
                                efCoreTests,
                                mongoDBTests,
								azureFilesTests,
								localFilesTests
							};
        var toClean = new[] {
                                 testResults,   
								 
								 vortexTests.Combine("bin"),
                                 vortexTests.Combine("obj"),
                                 vortexIntegrationTests.Combine("bin"),
                                 vortexIntegrationTests.Combine("obj"),
                                 efCoreTests.Combine("bin"),
                                 efCoreTests.Combine("obj"),
                                 mongoDBTests.Combine("bin"),
                                 mongoDBTests.Combine("obj"),
								 azureFilesTests.Combine("bin"),
                                 azureFilesTests.Combine("obj"),
								 localFilesTests.Combine("bin"),
                                 localFilesTests.Combine("obj"),
								 
                                 efCore.Combine("bin"),
                                 efCore.Combine("obj"),
                                 mongoDB.Combine("bin"),
                                 mongoDB.Combine("obj"),
								 azureFiles.Combine("bin"),
                                 azureFiles.Combine("obj"),
								 localFiles.Combine("bin"),
                                 localFiles.Combine("obj")
                            };
        return new BuildDirectories(rootDir,
                                    artifacts,
                                    testResults,
                                    integrationTests ? integrationTestDirs : testDirs, 
                                    toClean);
    }
}

public class BuildFiles
{
    public FilePath Solution { get; private set; }
    public FilePath TestCoverageOutput { get; set;}
    public ICollection<FilePath> TestAssemblies { get; private set; }
    public ICollection<FilePath> TestProjects { get; private set; }

    public BuildFiles(FilePath solution,
                      FilePath testCoverageOutput,
                      ICollection<FilePath> testAssemblies,
                      ICollection<FilePath> testProjects)
    {
        Solution = solution;
        TestAssemblies = testAssemblies;
        TestCoverageOutput = testCoverageOutput;
        TestProjects = testProjects;
    }
}

public class BuildDirectories
{
    public DirectoryPath RootDir { get; private set; }
    public DirectoryPath Artifacts { get; private set; }
    public DirectoryPath TestResults { get; private set; }
    public ICollection<DirectoryPath> TestDirs { get; private set; }
    public ICollection<DirectoryPath> ToClean { get; private set; }

    public BuildDirectories(
        DirectoryPath rootDir,
        DirectoryPath artifacts,
        DirectoryPath testResults,
        ICollection<DirectoryPath> testDirs,
        ICollection<DirectoryPath> toClean)
    {
        RootDir = rootDir;
        Artifacts = artifacts;
        TestDirs = testDirs;
        ToClean = toClean;
        TestResults = testResults;
    }
}