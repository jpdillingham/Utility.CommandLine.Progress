#!/bin/bash
set -e

dotnet test --no-build --configuration Release -p:CollectCoverage=true -p:CoverletOutput="./opencover.xml" -p:CoverletOutputFormat=opencover tests/Utility.CommandLine.Progress.Tests -p:Include="[Utility.CommandLine.Progress*]*" -p:Exclude="[*.Tests]*"