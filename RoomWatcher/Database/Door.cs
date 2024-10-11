namespace RoomWatcher.Database
{
    public class Door
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public bool IsClosed { get; set; }

        public bool IsLocked { get; set; }

        static Dictionary<string, Door> DoorsDict = new Dictionary<string, Door>();

        private Door(string doorLabel)
        {
            Id = Guid.NewGuid().ToString();
            this.Label = doorLabel;
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
            DoorsDict.Remove(doorId);
        }

        public static Door LockDoor(string doorId)
        {
            Door door;
            DoorsDict.TryGetValue(doorId, out door);
            if (door != null && door.IsClosed)
            {
                door.IsLocked = true;
            }
            return door;
        }

        public static Door UnlockDoor(string doorId)
        {
            Door door;
            DoorsDict.TryGetValue(doorId, out door);
            if (door != null && door.IsLocked)
            {
                door.IsLocked = false;
            }
            return door;
        }

        public static Door OpenDoor(string doorId)
        {
            Door door;
            DoorsDict.TryGetValue(doorId, out door);
            if (door != null && door.IsClosed)
            {
                door.IsClosed = false;
            }
            return door;
        }

        public static Door CloseDoor(string doorId)
        {
            Door door;
            DoorsDict.TryGetValue(doorId, out door);
            if (door != null && !door.IsClosed)
            {
                door.IsClosed = true;
            }
            return door;
        }
    }
}
