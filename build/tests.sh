#!/usr/bin/bash

dotnet build --no-incremental pdq.sql
dotnet test pdq.sln