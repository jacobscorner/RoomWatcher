using Microsoft.AspNetCore.SignalR;
using RoomWatcher.Database;

namespace RoomWatcher.Hubs
{
    public class DoorHub : Hub
    {
        public async Task InitSynchronizeClient()
        {
            List<Door> doors = DoorService.getDoors();
            if (doors.Count > 0)
            {
                var doorsSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(doors);
                await Clients.Caller.SendAsync("SynchronizeClient", doorsSerialized);
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
            Door doorToUpdate = Door.getDoor(doorId);
            if (DoorService.IsAlreadyLocked(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleWarning", "Selected door is already locked");
            }
            else if (!DoorService.CanDoorBeLocked(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleError", "Selected door cannot be locked");
            }
            else
            {
                Door updatedDoor = Door.LockDoor(doorId);
                var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
                await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
            }            
        }

        public async Task UnlockDoor(string doorId)
        {
            Door doorToUpdate = Door.getDoor(doorId);
            if (DoorService.IsAlreadyUnlocked(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleWarning", "Selected door is already unlocked");
            }
            else if (!DoorService.CanDoorBeUnLocked(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleError", "Selected door cannot be unlocked");
            }
            else
            {
                Door updatedDoor = Door.UnlockDoor(doorId);
                var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
                await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
            }
        }

        public async Task OpenDoor(string doorId)
        {
            Door doorToUpdate = Door.getDoor(doorId);
            if (DoorService.IsAlreadyOpened(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleWarning", "Selected door is already opened");
            }
            else if (!DoorService.CanDoorBeOpened(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleError", "Selected door cannot be opened");
            }
            else
            {
                Door updatedDoor = Door.OpenDoor(doorId);
                var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
                await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
            }
        }

        public async Task CloseDoor(string doorId)
        {
            Door doorToUpdate = Door.getDoor(doorId);
            if (DoorService.IsAlreadyClosed(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleWarning", "Selected door is already closed");
            }
            else if (!DoorService.CanDoorBeClosed(doorToUpdate))
            {
                await Clients.Caller.SendAsync("HandleError", "Selected door cannot be closed");
            }
            else
            {
                Door updatedDoor = Door.CloseDoor(doorId);
                var updatedDoorSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(updatedDoor);
                await Clients.All.SendAsync("DoorUpdated", updatedDoorSerialized);
            }
        }
    }
}
