using RoomWatcher.Database;

namespace RoomWatcher
{
    public static class DoorService
    {
        public static List<Door> getDoors()
        {
            return Door.getDoors();
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
    }
}
