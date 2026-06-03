#!/bin/sh

dotnet new nugetconfig > /dev/null 2>&1
dotnet nuget add source https://sda-iatec.pkgs.visualstudio.com/_packaging/IATec.Community/nuget/v3/index.json -n PrivateFeed1 -u #{Username}# -p #{Password}# --store-password-in-clear-text > /dev/null 2>&1