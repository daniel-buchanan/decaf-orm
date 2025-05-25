#!/usr/bin/bash

echo "SONAR_TOKEN=${SONAR_TOKEN}";

./.sonar/scanner/dotnet-sonarscanner begin \
    /k:"daniel-buchanan_pdq" \
    /o:"daniel-buchanan" \
    /d:sonar.token="${SONAR_TOKEN}" \
    /d:sonar.host.url="https://sonarcloud.io" \
    /d:sonar.cs.opencover.reportsPaths=coverage.xml \
    /d:sonar.scanner.scanAll=false

dotnet build --no-incremental decaf-orm.sln 
dotnet publish decaf-orm.sln --output ./output
./.coverlet/coverlet ./output \
  --target "dotnet" \
  --target-args "test --no-build" \
  -f=opencover \
  -o "coverage.xml"

./.sonar/scanner/dotnet-sonarscanner end \
    /d:sonar.token="${SONAR_TOKEN}"