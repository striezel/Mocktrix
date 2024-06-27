#!/bin/sh

# Utility script to handle artifact creation.
#
# Copyright (C) 2024  Dirk Stolle
#
# License: GNU GPL 3+

set -e

WORKSPACE=$(pwd)
mkdir -p artifacts/publish
RIDS="linux-arm linux-arm64 linux-x64 linux-musl-x64 osx-x64 osx-arm64 win-x64 win-x86"
for RID in $RIDS
do
    # framework-dependent build
    DEST_NAME=Mocktrix-$RID
    DESTINATION=$WORKSPACE/artifacts/publish/${DEST_NAME}
    dotnet publish ./Mocktrix/Mocktrix.csproj -c Release -r "$RID" -o "$DESTINATION"
    rm "$DESTINATION"/*.pdb
    cp LICENSE "$DESTINATION"/
    cp readme.md "$DESTINATION"/
    cp third-party.md "$DESTINATION"/
    cd "$WORKSPACE/artifacts/publish" || exit 1
    tar cjf "${DEST_NAME}.tar.bz2" "$DEST_NAME"
    cd "$WORKSPACE" || exit 1

    # self-contained build
    DEST_NAME=Mocktrix-$RID-self-contained
    DESTINATION=$WORKSPACE/artifacts/publish/${DEST_NAME}
    dotnet publish ./Mocktrix/Mocktrix.csproj -c Release -r "$RID" -o "$DESTINATION" --self-contained true
    rm "$DESTINATION"/*.pdb
    cp LICENSE "$DESTINATION"/
    cp readme.md "$DESTINATION"/
    cp third-party.md "$DESTINATION"/
    cd "$WORKSPACE/artifacts/publish" || exit 1
    tar cjf "${DEST_NAME}.tar.bz2" "$DEST_NAME"
    cd "$WORKSPACE" || exit 1
done
