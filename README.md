# WeGotLoggedChatRoomsYo
Learning how to make and log chat rooms with SignalR, MongoDb, and Blazor.

Duplicate the tab in browser and join a room (or group, idr what I called it) to view and send messages in that room.
  Idk if it helps, my DB was PracticeDB > ChatDB > ChatLog > Document.

Documents look like this:
```{
  "_id": {
    "$oid": "6458133b67305841455b7720"
  },
  "User": "dad IS THE BEST ",
  "Message": "MOM WOULD BE SO PROUND ",
  "Group": "HAHA YOU CLOWNS "
}
