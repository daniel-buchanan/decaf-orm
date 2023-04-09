#!/usr/bin/bash

dotnet build --no-incremental pdq.sln
dotnet test pdq.sln