# Implementation status of the client-server API, version r0.6.1

The implementation status of Matrix's
[client-server API, version r0.6.1](https://spec.matrix.org/historical/client_server/r0.6.1.html)
is listed below.

Legend:

* ✅ - implemented
* 🚧 - partially implemented / work in progress
* ❌ - not implemented yet

Note: The parts that are marked as deprecated by the specification will not get
implemented.

## 2 API Standards

* ✅ 2.1 [GET /_matrix/client/versions](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-versions)

## 3 Web browser clients

* ❌ CORS headers

## 4 Server Discovery

### 4.1 Well-known URI

* ✅ 4.1.1 [GET /.well-known/matrix/client](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-well-known-matrix-client)

## 5 Client Authentication

### 5.5 Login

* ✅ 5.5.1 [GET /_matrix/client/r0/login](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-login)
* ✅ 5.5.2 [POST /_matrix/client/r0/login](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-login)
* ✅ 5.5.3 [POST /_matrix/client/r0/logout](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-logout)
* ✅ 5.5.4 [POST /_matrix/client/r0/logout/all](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-logout-all)

### 5.6 Account registration and management

* ✅ 5.6.1 [POST /_matrix/client/r0/register](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-register)
* ✅ 5.6.2 [POST /_matrix/client/r0/register/email/requestToken](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-register-email-requesttoken)
* ✅ 5.6.3 [POST /_matrix/client/r0/register/msisdn/requestToken](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-register-msisdn-requesttoken)
* ✅ 5.6.4 [POST /_matrix/client/r0/account/password](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-password)
* ✅ 5.6.5 [POST /_matrix/client/r0/account/password/email/requestToken](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-password-email-requesttoken)
* ✅ 5.6.6 [POST /_matrix/client/r0/account/password/msisdn/requestToken](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-password-msisdn-requesttoken)
* ✅ 5.6.7 [POST /_matrix/client/r0/account/deactivate](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-deactivate)
* ✅ 5.6.8 [GET /_matrix/client/r0/register/available](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-register-available)

#### 5.6.9 [Notes on password management](https://spec.matrix.org/historical/client_server/r0.6.1.html#notes-on-password-management)

The server currently rejects passwords with less than 12 characters as weak
passwords with error code `M_WEAK_PASSWORD`.

### 5.7 Adding Account Administrative Contact Information

* ✅ 5.7.1 [GET /_matrix/client/r0/account/3pid](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-account-3pid)
* ❌ 5.7.2 [Deprecated: POST /_matrix/client/r0/account/3pid](https://spec.matrix.org/historical/client_server/r0.6.1.html#deprecated-post-matrix-client-r0-account-3pid)
* ✅ 5.7.3 [POST /_matrix/client/r0/account/3pid/add](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-add)
* ✅ 5.7.4 [POST /_matrix/client/r0/account/3pid/bind](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-bind)
* ✅ 5.7.5 [POST /_matrix/client/r0/account/3pid/delete](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-delete)
* ✅ 5.7.6 [POST /_matrix/client/r0/account/3pid/unbind](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-unbind)
* ✅ 5.7.7 [POST /_matrix/client/r0/account/3pid/email/requestToken](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-email-requesttoken)
* ✅ 5.7.8 [POST /_matrix/client/r0/account/3pid/msisdn/requestToken](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-msisdn-requesttoken)

### 5.8 Current account information

* ✅ 5.8.1 [GET /_matrix/client/r0/account/whoami](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-account-whoami)

## 6 Capabilities negotiation

* ✅ 6.1 [GET /_matrix/client/r0/capabilities](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-capabilities)

## 7 Pagination

* ❌ Not implemented.

## 8 Filtering

### 8.1 Lazy-loading room members

* ❌ Not implemented.

### 8.2 API endpoints

* ❌ 8.2.1 [POST /_matrix/client/r0/user/{userId}/filter](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-user-userid-filter)
* ❌ 8.2.2 [GET /_matrix/client/r0/user/{userId}/filter/{filterId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-filter-filterid)

## 9 Events

### 9.1 Types of room events

* ❌ Not implemented.

### 9.2 Size limits

* ❌ Not implemented.

### 9.3 Room events

* ❌ Not implemented.

### 9.4 Syncing

* 🚧 9.4.1 [GET /_matrix/client/r0/sync](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-sync)
* ❌ 9.4.2 [Deprecated: GET /_matrix/client/r0/events](https://spec.matrix.org/historical/client_server/r0.6.1.html#deprecated-get-matrix-client-r0-events)
* ❌ 9.4.3 [Deprecated: GET /_matrix/client/r0/initialSync](https://spec.matrix.org/historical/client_server/r0.6.1.html#deprecated-get-matrix-client-r0-initialsync)
* ❌ 9.4.4 [Deprecated: GET /_matrix/client/r0/events/{eventId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#deprecated-get-matrix-client-r0-events-eventid)

### 9.5 Getting events for a room

* ❌ 9.5.1 [GET /_matrix/client/r0/rooms/{roomId}/event/{eventId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-event-eventid)
* ❌ 9.5.2 [GET /_matrix/client/r0/rooms/{roomId}/state/{eventType}/{stateKey}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-state-eventtype-statekey)
* ❌ 9.5.3 [GET /_matrix/client/r0/rooms/{roomId}/state](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-state)
* ❌ 9.5.4 [GET /_matrix/client/r0/rooms/{roomId}/members](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-members)
* ❌ 9.5.5 [GET /_matrix/client/r0/rooms/{roomId}/joined_members](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-joined-members)
* ❌ 9.5.6 [GET /_matrix/client/r0/rooms/{roomId}/messages](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-messages)
* ❌ 9.5.7 [Deprecated: GET /_matrix/client/r0/rooms/{roomId}/initialSync](https://spec.matrix.org/historical/client_server/r0.6.1.html#deprecated-get-matrix-client-r0-rooms-roomid-initialsync)

### 9.6 Sending events to a room

* ❌ 9.6.1 [PUT /_matrix/client/r0/rooms/{roomId}/state/{eventType}/{stateKey}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-rooms-roomid-state-eventtype-statekey)
* ❌ 9.6.2 [PUT /_matrix/client/r0/rooms/{roomId}/send/{eventType}/{txnId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-rooms-roomid-send-eventtype-txnid)

### 9.7 Redactions

#### 9.7.1 Events

* ❌ 9.7.1.1 [m.room.redaction](https://spec.matrix.org/historical/client_server/r0.6.1.html#id40)

#### 9.7.2 Client behaviour

* ❌ 9.7.2.1 [PUT /_matrix/client/r0/rooms/{roomId}/redact/{eventId}/{txnId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-rooms-roomid-redact-eventid-txnid)

## 10 Rooms

### 10.1 Creation

* ❌ 10.1.1 [POST /_matrix/client/r0/createRoom](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-createroom)

### 10.2 Room aliases

* ❌ 10.2.1 [PUT /_matrix/client/r0/directory/room/{roomAlias}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-directory-room-roomalias)
* ❌ 10.2.2 [GET /_matrix/client/r0/directory/room/{roomAlias}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-directory-room-roomalias)
* ❌ 10.2.3 [DELETE /_matrix/client/r0/directory/room/{roomAlias}](https://spec.matrix.org/historical/client_server/r0.6.1.html#delete-matrix-client-r0-directory-room-roomalias)
* ❌ 10.2.4 [GET /_matrix/client/r0/rooms/{roomId}/aliases](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-aliases)

### 10.3 Permissions

❌Not implemented.

### 10.4 Room membership

* ❌ 10.4.1 [GET /_matrix/client/r0/joined_rooms](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-joined-rooms)

#### 10.4.2 Joining rooms

* ❌ 10.4.2.1 [POST /_matrix/client/r0/rooms/{roomId}/invite](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-invite)
* ❌ 10.4.2.2 [POST /_matrix/client/r0/rooms/{roomId}/join](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-join)
* ❌ 10.4.2.3 [POST /_matrix/client/r0/join/{roomIdOrAlias}](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-join-roomidoralias)

#### 10.4.3 Leaving rooms

* ❌ 10.4.3.1 [POST /_matrix/client/r0/rooms/{roomId}/leave](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-leave)
* ❌ 10.4.3.2 [POST /_matrix/client/r0/rooms/{roomId}/forget](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-forget)
* ❌ 10.4.3.3 [POST /_matrix/client/r0/rooms/{roomId}/kick](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-kick)

#### 10.4.4 Banning users in a room

* ❌ 10.4.4.1 [POST /_matrix/client/r0/rooms/{roomId}/ban](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-ban)
* ❌ 10.4.4.2 [POST /_matrix/client/r0/rooms/{roomId}/unban](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-unban)

### 10.5 Listing rooms

* ❌ 10.5.1 [GET /_matrix/client/r0/directory/list/room/{roomId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-directory-list-room-roomid)
* ❌ 10.5.2 [PUT /_matrix/client/r0/directory/list/room/{roomId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-directory-list-room-roomid)
* ❌ 10.5.3 [GET /_matrix/client/r0/publicRooms](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-publicrooms)
* ❌ 10.5.4 [POST /_matrix/client/r0/publicRooms](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-publicrooms)

## 11 User Data

### 11.1 User Directory

* ❌ 11.1.1 [POST /_matrix/client/r0/user_directory/search](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-user-directory-search)

### 11.2 Profiles

* ✅ 11.2.1 [PUT /_matrix/client/r0/profile/{userId}/displayname](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-profile-userid-displayname)
* ✅ 11.2.2 [GET /_matrix/client/r0/profile/{userId}/displayname](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-profile-userid-displayname)
* ✅ 11.2.3 [PUT /_matrix/client/r0/profile/{userId}/avatar_url](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-profile-userid-avatar-url)
* ✅ 11.2.4 [GET /_matrix/client/r0/profile/{userId}/avatar_url](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-profile-userid-avatar-url)
* ✅ 11.2.5 [GET /_matrix/client/r0/profile/{userId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-profile-userid)
* ❌ 11.2.6 [Events on Change of Profile Information](https://spec.matrix.org/historical/client_server/r0.6.1.html#events-on-change-of-profile-information)

## 12 Security

### 12.1 Rate limiting

* ❌ Currently, the server does not have any kind of rate limiting.

## 13 Modules

### 13.1 to 13.7

TODO

### 13.8 Content repository

#### 13.8.1 Matrix Content (MXC) URIs

✅ Implemented.

#### 13.8.2 Client behaviour

* ✅ 13.8.2.1 [POST /_matrix/media/r0/upload](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-media-r0-upload)
* ✅ 13.8.2.2 [GET /_matrix/media/r0/download/{serverName}/{mediaId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-media-r0-download-servername-mediaid)
* ✅ 13.8.2.3 [GET /_matrix/media/r0/download/{serverName}/{mediaId}/{fileName}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-media-r0-download-servername-mediaid-filename)
* ❌ 13.8.2.4 [GET /_matrix/media/r0/thumbnail/{serverName}/{mediaId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-media-r0-thumbnail-servername-mediaid)
* ❌ 13.8.2.5 [GET /_matrix/media/r0/preview_url](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-media-r0-preview-url)
* ✅ 13.8.2.6 [GET /_matrix/media/r0/config](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-media-r0-config)
* ❌ 13.8.2.7 [Thumbnails](https://spec.matrix.org/historical/client_server/r0.6.1.html#thumbnails)

#### 13.8.3 Security considerations

* 🚧 Partially implemented.

### 13.9 to 13.33

TODO
