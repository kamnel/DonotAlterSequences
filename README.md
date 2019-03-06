# DonotAlterSequences


=========================
Installation instructions
=========================
1. Build the project in Visual studio
2. Create extensions folder "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\150\Extensions\DonotAlterSequences"
3. Copy contents of bin directory to extensions folder
4. Modify publish profile to include AdditionalDeploymentContributors. Example below

	<?xml version="1.0" encoding="utf-8"?>
	<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
	  <PropertyGroup>
		<TargetDatabaseName>****</TargetDatabaseName>
		<DeployScriptFileName>****.sql</DeployScriptFileName>
		<TargetConnectionString>Data Source=.;Integrated Security=True;Pooling=False</TargetConnectionString>
		<AdditionalDeploymentContributors>DonotAlterSequences</AdditionalDeploymentContributors>
	  </PropertyGroup>
	  <ItemGroup>
		<SqlCmdVariable Include="Environment">
		  <Value>local</Value>
		</SqlCmdVariable>
	  </ItemGroup>
	</Project>


5. Compile using msbuild
"C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/amd64/MSBuild.exe" /p:SqlPublishProfilePath=build.publish.xml /target:Publish C:/Code/****.sqlproj

=========================
References /Acknowledgements
=========================
This filter is inspired by awesome work done in https://github.com/GoEddie/DeploymentContributorFilterer 
