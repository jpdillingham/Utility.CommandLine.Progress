dotnet test -p:CollectCoverage=true -p:CoverletOutput='./coverage.xml' -p:CoverletOutputFormat=opencover tests/Utility.CommandLine.Progress.Tests /p:Exclude='[xunit*]*'