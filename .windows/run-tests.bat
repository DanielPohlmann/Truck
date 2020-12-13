cd ..

dotnet test --logger "trx;LogFileName=TestResults.trx" --logger "xunit;LogFileName=TestResults.xml" --results-directory ./BuildReports/UnitTests /p:CollectCoverage=true /p:CoverletOutput=BuildReports\Coverage\ /p:CoverletOutputFormat=opencover /p:Exclude=\"[xunit.],[.Tests?]\"  /p:ExcludeByFile=\"**/Controllers/**.cs,**/Data/**.cs,**/Configuration/**.cs,**/Migrations/**.cs,**/Program.cs,**/Startup.cs\"

reportgenerator "-reports:**\BuildReports\Coverage\coverage.opencover.xml" "-targetdir:.\BuildReports\Coverage" -reporttypes:HTML;HTMLSummary

start BuildReports\Coverage\index.htm

pause