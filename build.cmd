dotnet tool restore
dotnet test
dotnet build -c Release /p:SourceLinkCreate=true /v:n
.paket\paket.exe pack .build/nugets