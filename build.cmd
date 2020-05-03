dotnet tool restore
dotnet test
dotnet build -c Release /p:SourceLinkCreate=true /v:n
dotnet paket pack .build/nugets