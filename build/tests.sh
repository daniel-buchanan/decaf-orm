#!/usr/bin/bash

dotnet build --no-incremental $1
dotnet test $1