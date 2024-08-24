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
* ✅ 6.2 [m.change_password capability](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-change-password-capability)
* ✅ 6.3 [m.room_versions capability](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-versions-capability)

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

* ✅ 9.1.1 [Event Fields](https://spec.matrix.org/historical/client_server/r0.6.1.html#event-fields)
* ✅ 9.1.2 [Room Event Fields](https://spec.matrix.org/historical/client_server/r0.6.1.html#room-event-fields)
* ✅ 9.1.3 [State Event Fields](https://spec.matrix.org/historical/client_server/r0.6.1.html#state-event-fields)

### 9.2 Size limits

* ❌ Not implemented.

### 9.3 Room events

* ✅ 9.3.1 [m.room.canonical_alias](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-canonical-alias)
* ✅ 9.3.2 [m.room.create](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-create)
* ✅ 9.3.3 [m.room.join_rules](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-join-rules)
* ✅ 9.3.4 [m.room.member](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-member)
* ✅ 9.3.5 [m.room.power_levels](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-power-levels)
* ❌ 9.3.6 [m.room.redaction](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-redaction)
* ✅ 9.3.7 [Historical events](https://spec.matrix.org/historical/client_server/r0.6.1.html#historical-events)

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

* 🚧 10.1.1 [POST /_matrix/client/r0/createRoom](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-createroom)

### 10.2 Room aliases

* ❌ 10.2.1 [PUT /_matrix/client/r0/directory/room/{roomAlias}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-directory-room-roomalias)
* ❌ 10.2.2 [GET /_matrix/client/r0/directory/room/{roomAlias}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-directory-room-roomalias)
* ❌ 10.2.3 [DELETE /_matrix/client/r0/directory/room/{roomAlias}](https://spec.matrix.org/historical/client_server/r0.6.1.html#delete-matrix-client-r0-directory-room-roomalias)
* ❌ 10.2.4 [GET /_matrix/client/r0/rooms/{roomId}/aliases](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-aliases)

### 10.3 Permissions

❌Not implemented.

### 10.4 Room membership

* ✅ 10.4.1 [GET /_matrix/client/r0/joined_rooms](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-joined-rooms)

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

* ✅ 10.5.1 [GET /_matrix/client/r0/directory/list/room/{roomId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-directory-list-room-roomid)
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
* 🚧 11.2.6 [Events on Change of Profile Information](https://spec.matrix.org/historical/client_server/r0.6.1.html#events-on-change-of-profile-information)

## 12 Security

### 12.1 Rate limiting

* ❌ Currently, the server does not have any kind of rate limiting.

## 13 Modules

### 13.1 [Feature profiles](https://spec.matrix.org/historical/client_server/r0.6.1.html#feature-profiles)

TODO

### 13.2 Instant Messaging

#### 13.2.1 Events

* ✅ 13.2.1.1 [m.room.message](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-message)
* ✅ 13.2.1.2 [m.room.message.feedback](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-message-feedback)
* ✅ 13.2.1.3 [m.room.name](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-name)
* ✅ 13.2.1.4 [m.room.topic](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-topic)
* ✅ 13.2.1.5 [m.room.avatar](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-avatar)
* ✅ 13.2.1.6 [m.room.pinned_events](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-pinned-events)
* ✅ 13.2.1.7 [m.room.message msgtypes](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-message-msgtypes)
  * ✅ 13.2.1.7.1 [m.text](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-text)
  * ✅ 13.2.1.7.2 [m.emote](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-emote)
  * ✅ 13.2.1.7.3 [m.notice](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-notice)
  * ✅ 13.2.1.7.4 [m.image](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-image)
  * ✅ 13.2.1.7.5 [m.file](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-file)
  * ✅ 13.2.1.7.6 [m.audio](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-audio)
  * ✅ 13.2.1.7.7 [m.location](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-location)
  * ✅ 13.2.1.7.8 [m.video](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-video)

#### 13.2.2 [Client behaviour](https://spec.matrix.org/historical/client_server/r0.6.1.html#id44)

#### 13.2.3 [Server behaviour](https://spec.matrix.org/historical/client_server/r0.6.1.html#server-behaviour)

* ❌ Homeservers SHOULD reject `m.room.message` events which don't have a
    `msgtype` key, or which don't have a textual `body` key, with an HTTP status code of 400.

#### 13.2.4 [Security considerations](https://spec.matrix.org/historical/client_server/r0.6.1.html#security-considerations)

* ❌ Not implemented.

### 13.3 Voice over IP

#### 13.3.1 Events

* ✅ 13.3.1.1 [m.call.invite](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-call-invite)
* ✅ 13.3.1.2 [m.call.candidates](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-call-candidates)
* ✅ 13.3.1.3 [m.call.answer](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-call-answer)
* ✅ 13.3.1.4 [m.call.hangup](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-call-hangup)

#### 13.3.2 Client behaviour

##### 13.3.2.1 Glare

#### 13.3.3 Server behaviour

* ❌ 13.3.3.1 [GET /_matrix/client/r0/voip/turnServer](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-voip-turnserver)

#### 13.3.4 [Security considerations](https://spec.matrix.org/historical/client_server/r0.6.1.html#id48)

* ❌ Not implemented.

### 13.4 Typing Notifications

#### 13.4.1 Events

* ❌ 13.4.1.1 [m.typing](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-typing)

#### [13.4.2 Client behaviour](https://spec.matrix.org/historical/client_server/r0.6.1.html#id51)

* ❌ 13.4.2.1 [PUT /_matrix/client/r0/rooms/{roomId}/typing/{userId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-rooms-roomid-typing-userid)

#### [13.4.3 Security consideration](https://spec.matrix.org/historical/client_server/r0.6.1.html#id52)

### 13.5 Receipts

#### 13.5.1 Events

* ❌ 13.5.1.1 [m.receipt](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-receipt)

#### 13.5.2 Client behaviour

* ❌ 13.5.2.1 [POST /_matrix/client/r0/rooms/{roomId}/receipt/{receiptType}/{eventId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-receipt-receipttype-eventid)

#### 13.5.3 Server behaviour

* ❌ Not implemented yet.

#### 13.5.4 [Security considerations](https://spec.matrix.org/historical/client_server/r0.6.1.html#id57)

### 13.6 Fully read markers

#### 13.6.1 Events

* ❌ 13.6.1.1 [m.fully_read](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-fully-read)

#### 13.6.2 Client behaviour

* ❌ 13.6.2.1 [POST /_matrix/client/r0/rooms/{roomId}/read_markers](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-rooms-roomid-read-markers)

#### 13.6.3 Server behaviour

* ❌ Not implemented.

### 13.7 Presence

#### 13.7.1 Events

* ❌ 13.7.1.1 [m.presence](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-presence)

#### 13.7.2 Client behaviour

* ❌ 13.7.2.1 [PUT /_matrix/client/r0/presence/{userId}/status](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-presence-userid-status)
* ❌ 13.7.2.2 [GET /_matrix/client/r0/presence/{userId}/status](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-presence-userid-status)
* ❌ 13.7.2.3 [Last active ago](https://spec.matrix.org/historical/client_server/r0.6.1.html#last-active-ago)
* ❌ 13.7.2.4 [Idle timeout](https://spec.matrix.org/historical/client_server/r0.6.1.html#idle-timeout)

#### [13.7.3 Security considerations](https://spec.matrix.org/historical/client_server/r0.6.1.html#id65)

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

### 13.9 Send-to-Device messaging

#### [13.9.1 Client behaviour](https://spec.matrix.org/historical/client_server/r0.6.1.html#id71)

#### [13.9.2 Server behaviour](https://spec.matrix.org/historical/client_server/r0.6.1.html#id72)

* ❌ Not implemented.

#### 13.9.3 Protocol definitions

* ❌ 13.9.3.1 [PUT /_matrix/client/r0/sendToDevice/{eventType}/{txnId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-sendtodevice-eventtype-txnid)
* ❌ 13.9.3.2 [Extensions to /sync](https://spec.matrix.org/historical/client_server/r0.6.1.html#extensions-to-sync)

### 13.10 Device Management

#### 13.10.1 Client behaviour

* ✅ 13.10.1.1 [GET /_matrix/client/r0/devices](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-devices)
* ✅ 13.10.1.2 [GET /_matrix/client/r0/devices/{deviceId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-devices-deviceid)
* ✅ 13.10.1.3 [PUT /_matrix/client/r0/devices/{deviceId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-devices-deviceid)
* ✅ 13.10.1.4 [DELETE /_matrix/client/r0/devices/{deviceId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#delete-matrix-client-r0-devices-deviceid)
* ✅ 13.10.1.5 [POST /_matrix/client/r0/delete_devices](https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-delete-devices)

#### [13.10.2 Security considerations](https://spec.matrix.org/historical/client_server/r0.6.1.html#id75)

* ✅ User-interactive authentication is implemented for device deletion,
  requiring the user to re-submit the current password to allow deletion.

### 13.11 [End-to-End Encryption](https://spec.matrix.org/historical/client_server/r0.6.1.html#id76)

TODO

### 13.12 Room History Visibility

#### 13.12.1 Events

* ✅ 13.12.1.1 [m.room.history_visibility](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-room-history-visibility)

#### 13.12.2 Client behaviour

#### 13.12.3 [Server behaviour](https://spec.matrix.org/historical/client_server/r0.6.1.html#id87)

* ❌ Not implemented yet.

#### 13.12.4 [Security considerations](https://spec.matrix.org/historical/client_server/r0.6.1.html#id88)

### 13.13 to 13.17

TODO

### 13.18 Room Tagging

#### 13.18.1 Events

* ✅ 13.18.1.1 [m.tag](https://spec.matrix.org/historical/client_server/r0.6.1.html#m-tag)

#### 13.18.2 Client Behaviour

* ✅ 13.18.2.1 [GET /_matrix/client/r0/user/{userId}/rooms/{roomId}/tags](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-rooms-roomid-tags)
* ✅ 13.18.2.2 [PUT /_matrix/client/r0/user/{userId}/rooms/{roomId}/tags/{tag}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-user-userid-rooms-roomid-tags-tag)
* ✅ 13.18.2.3 [DELETE /_matrix/client/r0/user/{userId}/rooms/{roomId}/tags/{tag}](https://spec.matrix.org/historical/client_server/r0.6.1.html#delete-matrix-client-r0-user-userid-rooms-roomid-tags-tag)

### 13.19 Client Config

#### 13.19.1 Events

❌ Not implemented yet.

#### 13.19.2 Client Behaviour

* ❌ 13.19.2.1 [PUT /_matrix/client/r0/user/{userId}/account_data/{type}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-user-userid-account-data-type)
* ❌ 13.19.2.2 [GET /_matrix/client/r0/user/{userId}/account_data/{type}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-account-data-type)
* ❌ 13.19.2.3 [PUT /_matrix/client/r0/user/{userId}/rooms/{roomId}/account_data/{type}](https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-user-userid-rooms-roomid-account-data-type)
* ❌ 13.19.2.4 [GET /_matrix/client/r0/user/{userId}/rooms/{roomId}/account_data/{type}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-rooms-roomid-account-data-type)

#### 13.19.3 Server Behaviour

❌ Not implemented yet.

### 13.20 Server Administration

#### 13.20.1 Client behaviour

* ❌ 13.20.1.1 [GET /_matrix/client/r0/admin/whois/{userId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-admin-whois-userid)

### 13.21 Event Context

#### 13.21.1 Client behaviour

* ❌ 13.21.1.1 [GET /_matrix/client/r0/rooms/{roomId}/context/{eventId}](https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-rooms-roomid-context-eventid)

### 13.21.2 Security considerations

❌ Not implemented yet.

### 13.22 to 13.33

TODO
