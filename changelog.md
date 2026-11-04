# Version history of Mocktrix

_(Note: This changelog focuses on the major changes between the different
versions. Therefore, it may not contain all changes. Especially smaller fixes or
improvements may be omitted.)_

## Next Version

This version uses .NET 10 (instead of .NET 8 as in the previous version).

It also adds some new features:

* Client configuration values ("account data") are implemented, both on global
  and on room-based scope.
* Room tagging endpoints are added.
* Device deletion will now use the user-interactive authentication API.
* Basic room creation and listing of of rooms is implemented.

## Version 0.1.0

Initial release.

Since this version is not intended to be a full-featured Matrix homeserver, it
comes with some limitations. These are:

* Only the client-server API of the Matrix protocol is implemented. No
  federation API or identity server API is implemented.
* Supports only version r0.6.1 of the client-server API.
* Just a few relevant parts of the client-server API are currently implemented.
  These are basically login and logout, account and device management, and the
  upload to and download from the Matrix content repository.
* Rate-limiting is not implemented.
* At the moment, the server responses do not contain CORS headers.
* Any server data (like accounts, uploaded content, etc.) is kept in memory.
  This means that those are not persisted and are gone as soon as the server
  process shuts down.
