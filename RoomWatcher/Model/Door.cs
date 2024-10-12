namespace RoomWatcher.Model
{
    public class Door
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool IsClosed { get; set; }
        public bool IsLocked { get; set; }

        public Door(string doorLabel)
        {
            Id = Guid.NewGuid().ToString();
            Label = doorLabel;
        }

        public void LockDoor()
        {
            IsLocked = true;
        }

        public void UnlockDoor()
        {
            IsLocked = false;
        }

        public void OpenDoor()
        {
            IsClosed = false;
        }

        public void CloseDoor()
        {
            IsClosed = true;
        }
    }
}
