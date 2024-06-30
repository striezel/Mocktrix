#!/bin/sh

# Utility script to handle artifact creation.
#
# Copyright (C) 2024  Dirk Stolle
#
# License: GNU GPL 3+

set -e

WORKSPACE=$(pwd)
VERSION=$(git describe --always)
VERSION=${VERSION:-unknown_version}
ARCHIVE_MTIME=$(git log -1 --format="%cI")
ARCHIVE_MTIME=${ARCHIVE_MTIME:-1970-01-01 00:00:00 +0000}
mkdir -p artifacts/publish
RIDS="linux-arm linux-arm64 linux-x64 linux-musl-x64 osx-x64 osx-arm64 win-x64 win-x86"
for RID in $RIDS
do
    # framework-dependent build
    DEST_NAME=Mocktrix-$VERSION-$RID-framework-dependent
    DESTINATION=$WORKSPACE/artifacts/publish/${DEST_NAME}
    dotnet publish ./Mocktrix/Mocktrix.csproj -c Release -r "$RID" -o "$DESTINATION"
    rm "$DESTINATION"/*.pdb
    cp LICENSE "$DESTINATION"/
    cp readme.md "$DESTINATION"/
    cp third-party.md "$DESTINATION"/
    cp ./Mocktrix/example.configuration.xml "$DESTINATION"/
    cd "$WORKSPACE/artifacts/publish" || exit 1
    tar cjf "${DEST_NAME}.tar.bz2" "$DEST_NAME" --sort=name --mtime="$ARCHIVE_MTIME" --owner=0 --group=0 --numeric-owner
    cd "$WORKSPACE" || exit 1

    # self-contained build
    DEST_NAME=Mocktrix-$VERSION-$RID-self-contained
    DESTINATION=$WORKSPACE/artifacts/publish/${DEST_NAME}
    dotnet publish ./Mocktrix/Mocktrix.csproj -c Release -r "$RID" -o "$DESTINATION" --self-contained true -p:PublishSingleFile=true
    rm "$DESTINATION"/*.pdb
    cp LICENSE "$DESTINATION"/
    cp readme.md "$DESTINATION"/
    cp third-party.md "$DESTINATION"/
    cp ./Mocktrix/example.configuration.xml "$DESTINATION"/
    cd "$WORKSPACE/artifacts/publish" || exit 1
    tar cjf "${DEST_NAME}.tar.bz2" "$DEST_NAME" --sort=name --mtime="$ARCHIVE_MTIME" --owner=0 --group=0 --numeric-owner
    cd "$WORKSPACE" || exit 1
done
