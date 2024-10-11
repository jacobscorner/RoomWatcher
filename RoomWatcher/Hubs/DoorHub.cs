using Microsoft.AspNetCore.SignalR;
using RoomWatcher.Database;

namespace RoomWatcher.Hubs
{
    public class DoorHub : Hub
    {
        public async Task InitSync()
        {
            List<Door> doors = Door.getDoors();
            if (doors.Count > 0)
            {
                var doorsSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(doors);
                await Clients.Caller.SendAsync("SyncAcked", doorsSerialized); // this is specifically sent to calling client to sync with other clients which are already in sync
            }
        }
        public async Task AddDoor(string doorLabel)
        {
            bool errorOccurred = false;
            string errorMessage = "";

            if (String.IsNullOrWhiteSpace(doorLabel))
            {
                errorOccurred = true;
                errorMessage = "Door label is mandatory";
            }
            if (!errorOccurred)
            {
                Door newDoor = Door.addDoor(doorLabel);
                if (newDoor == null)
                {
                    errorOccurred = true;
                    errorMessage = "Unexpected error";
                }
                else
                {
                    var newDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(newDoor);
                    await Clients.All.SendAsync("DoorAdded", newDoorSerialized);
                }
            }

            if (errorOccurred)
            {
                await Clients.Caller.SendAsync("HandleError", errorMessage);
            }
        }

        public async Task RemoveDoor(string doorId)
        {
            Door.removeDoor(doorId);
            await Clients.All.SendAsync("DoorRemoved", doorId);
        }

        public async Task LockDoor(string doorId)
        {
            Door updatedDoor = Door.LockDoor(doorId);
            var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
            await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
        }

        public async Task UnlockDoor(string doorId)
        {
            Door updatedDoor = Door.UnlockDoor(doorId);
            var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
            await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
        }

        public async Task OpenDoor(string doorId)
        {
            Door updatedDoor = Door.OpenDoor(doorId);
            var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
            await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
        }

        public async Task CloseDoor(string doorId)
        {
            Door updatedDoor = Door.CloseDoor(doorId);
            var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
            await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
        }
    }
}
