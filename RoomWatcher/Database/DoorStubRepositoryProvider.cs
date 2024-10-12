using RoomWatcher.Model;

namespace RoomWatcher.Database
{
    public static class DoorStubRepositoryProvider
    {
        private static Dictionary<string, Door> DoorsDict = new Dictionary<string, Door>();

        public static Door getDoor(string doorId)
        {
            Door door;
            DoorsDict.TryGetValue(doorId, out door);
            return door;
        }

        public static List<Door> getDoors()
        {
            List<Door> doors = new List<Door>();
            foreach (KeyValuePair<string, Door> entry in DoorsDict)
            {
                doors.Add(entry.Value);
            }
            return doors;
        }

        public static Door addDoor(string doorLabel)
        {
            Door newDoor = new Door(doorLabel);
            DoorsDict.Add(newDoor.Id, newDoor);
            return newDoor;
        }

        public static void removeDoor(string doorId)
        {
            Door door = getDoor(doorId);
            if (door != null)
            {
                DoorsDict.Remove(doorId);
            }
        }

        public static Door LockDoor(string doorId)
        {
            Door door = getDoor(doorId);
            if (door != null)
            {
                door.LockDoor();
            }
            return door;
        }

        public static Door UnlockDoor(string doorId)
        {
            Door door = getDoor(doorId);
            if (door != null)
            {
                door.UnlockDoor();
            }
            return door;
        }

        public static Door OpenDoor(string doorId)
        {
            Door door = getDoor(doorId);
            if (door != null)
            {
                door.OpenDoor();
            }
            return door;
        }

        public static Door CloseDoor(string doorId)
        {
            Door door = getDoor(doorId);
            if (door != null)
            {
                door.CloseDoor();
            }
            return door;
        }
    }
}
