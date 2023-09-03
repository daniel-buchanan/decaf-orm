#!/usr/bin/bash

./.sonar/scanner/dotnet-sonarscanner begin \
    /k:"daniel-buchanan_pdq" \
    /o:"daniel-buchanan" \
    /d:sonar.token="${SONAR_TOKEN}" \
    /d:sonar.host.url="https://sonarcloud.io" \
    /d:sonar.cs.opencover.reportsPaths=coverage.xml

dotnet build --no-incremental pdq.sln 

./.coverlet/coverlet ./tests/pdq.db.common.tests/bin/Debug/net7.0/pdq.db.common.tests.dll \
    --target "dotnet" \
    --targetargs "test --no-build" \
    -f=json \
    -o="db-common.json"

./.coverlet/coverlet ./tests/pdq.services.tests/bin/Debug/net7.0/pdq.services.tests.dll \
    --target "dotnet" \
    --targetargs "test --no-build" \
    --merge-with "db-common.json" \
    -f=json \
    -o="services-merged.json"

./.coverlet/coverlet ./tests/pdq.npgsql.tests/bin/Debug/net7.0/pdq.npgsql.tests.dll \
    --target "dotnet" \
    --targetargs "test --no-build" \
    --merge-with "services-merged.json" \
    -f=json \
    -o="npgsql-merged.json"

./.coverlet/coverlet ./tests/pdq.sqlserver.tests/bin/Debug/net7.0/pdq.sqlserver.tests.dll \
    --target "dotnet" \
    --targetargs "test --no-build" \
    --merge-with "npgsql-merged.json" \
    -f=json \
    -o="sqlserver-merged.json"

./.coverlet/coverlet ./tests/pdq.core-tests/bin/Debug/net7.0/pdq.core-tests.dll \
    --target "dotnet" \
    --targetargs "test --no-build" \
    --merge-with "sqlserver-merged.json" \
    -f=opencover \
    -o="coverage.xml"

./.sonar/scanner/dotnet-sonarscanner end \
    /d:sonar.token="${SONAR_TOKEN}"