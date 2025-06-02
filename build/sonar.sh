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
dotnet build --no-incremental decaf-orm.sln;
dotnet publish decaf-orm.sln -c Debug --output ./output;

search_dir=./output;
entries=("$search_dir"/*);
last_file="";
count_files=$(find "$search_dir" -maxdepth 1 -type f | wc -c)

for (( i=0; i < count_files; i++ )); do
  entry=${entries[$i]};
  
  if [[ "$entry" != *"tests.dll" || "${entry}" == "" ]]; then
    continue;
  fi
  
  echo "Processing: ${entry}";
  
  format="json";
  file_extension="json";
  if [[ "$i" == "($count_files - 1)" ]]; then
    format="opencover";
    file_extension="xml";
  fi
  
  coverage_file="${entry}.coverage.${file_extension}";
  if [[ "${file_extension}" == "xml" ]]; then
    coverage_file="coverage.${file_extension}";
  fi
  
  echo "  -> Format: ${format}";
  echo "  -> Extension: ${file_extension}";
  echo "  -> Coverage File: ${coverage_file}";
  
  if [ $i == 0 ]; then
    ./.coverlet/coverlet "$entry" \
      --target "dotnet" \
      --targetargs "test --no-build" \
      -f=$format \
      -o="${coverage_file}"
  else
    merge_with=$last_file;
    ./.coverlet/coverlet "$entry" \
      --target "dotnet" \
      --targetargs "test --no-build" \
      -f=$format \
      -o="${coverage_file}" \
      --merge-with="${merge_with}";
  fi
  
  last_file=$coverage_file;
done

./.sonar/scanner/dotnet-sonarscanner end \
    /d:sonar.token="${SONAR_TOKEN}"