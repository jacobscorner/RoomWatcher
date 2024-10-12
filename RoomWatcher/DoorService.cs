using RoomWatcher.Database;
using RoomWatcher.Model;

namespace RoomWatcher
{
    public static class DoorService
    {
        public static List<Door> getDoors()
        {
            return DoorStubRepositoryProvider.getDoors();
        }

        public static Door getDoor(string doorId)
        {
            return DoorStubRepositoryProvider.getDoor(doorId);
        }

        public static Door addDoor(string doorLabel)
        {
            return DoorStubRepositoryProvider.addDoor(doorLabel);
        }

        public static void removeDoor(string doorId)
        {
            DoorStubRepositoryProvider.removeDoor(doorId);
        }

        public static bool IsAlreadyOpened(Door door)
        {
            bool result = false;
            if (door != null && !door.IsClosed)
            {
                result = true;
            }
            return result;
        }

        public static bool CanDoorBeOpened(Door door)
        {
            bool result = false;
            if (door != null && door.IsClosed && !door.IsLocked)
            {
                result = true;
            }
            return result;
        }

        public static Door OpenDoor(string doorId)
        {
            return DoorStubRepositoryProvider.OpenDoor(doorId);
        }

        public static bool IsAlreadyClosed(Door door)
        {
            bool result = false;
            if (door != null && door.IsClosed)
            {
                result = true;
            }
            return result;
        }

        public static bool CanDoorBeClosed(Door door)
        {
            bool result = false;
            if (door != null && !door.IsClosed)
            {
                result = true;
            }
            return result;
        }

        public static Door CloseDoor(string doorId)
        {
            return DoorStubRepositoryProvider.CloseDoor(doorId);
        }

        public static bool IsAlreadyLocked(Door door)
        {
            bool result = false;
            if (door != null && door.IsLocked)
            {
                result = true;
            }
            return result;
        }

        public static bool CanDoorBeLocked(Door door)
        {   bool result = false;
            if (door != null && door.IsClosed)
            {
                result = true;
            }
            return result;
        }

        public static Door LockDoor(string doorId)
        {
            return DoorStubRepositoryProvider.LockDoor(doorId);
        }

        public static bool IsAlreadyUnlocked(Door door)
        {
            bool result = false;
            if (door != null && !door.IsLocked)
            {
                result = true;
            }
            return result;
        }

        public static bool CanDoorBeUnLocked(Door door)
        {
            bool result = false;
            if (door != null && door.IsLocked)
            {
                result = true;
            }
            return result;
        }

        public static Door UnlockDoor(string doorId)
        {
            return DoorStubRepositoryProvider.UnlockDoor(doorId);
        }
    }
}
