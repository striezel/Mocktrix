# Mocktrix

Mocktrix _(working title, final name may change)_ will be an application to mock
a Matrix homeserver.

## Prerequisites

To run the program you need the .NET 8 runtime.
The current .NET 8 runtime can be downloaded from
<https://dotnet.microsoft.com/en-us/download/dotnet/8.0/runtime>.

## Usage

```
Mocktrix [OPTIONS]

Options:
  -? | --help     - Shows this help message and quits.
  -v | --version  - Shows the version of the program and quits.
  --conf file.xml - Loads the configuration from the given XML file.
```

## Known limitations

Since this application is currently not intended to be a full-featured Matrix
homeserver, it comes with some limitations. These are:

* Only the client-server API of the Matrix protocol is implemented. No
  federation API or identity server API is implemented.
* Supports only version r0.6.1 of the client-server API.
* Just a few relevant parts of the client-server API are currently implemented.
  These are basically login and logout, account and device management.
* At the moment, the server responses do not contain CORS headers.
* Any server data (like accounts, etc.) is kept in memory. This means that those
  are not persisted and are gone as soon as the server process shuts down.

## Copyright and Licensing

Copyright 2024  Dirk Stolle

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
