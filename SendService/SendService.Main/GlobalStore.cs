using DataContracts;

namespace SendService.Main
{
    public static class GlobalStore
    {
        private static HashSet<string> Ports = new HashSet<string>();

        private static Dictionary<int, int[]> Arrays = new();
        public static bool Flag { get; set; }

        public static bool IsSendRequest { get; set; } = false;

        private static int Time = 0;

        public static int GetTime()
        {
            return Time;
        }
        public static void SetPorts(HashSet<string> ports)
        {
            Ports = ports;
        }
        public static HashSet<string> GetPorts()
        {
            return Ports;
        }
        public static void AddArrays(int index, int[] array)
        {
            Arrays.Add(index, array);
        }
        public static ArrayMessage GetArray()
        {
            var array = Arrays.FirstOrDefault();
            if (array.Value != null) {
                Arrays.Remove(array.Key);
                Time++;
            }
            return new ArrayMessage()
            {
                Array = array.Value,
                Index = array.Key
            };
        }
        public static int GetArraysCount()
        {
            return Arrays.Count;
        }
    }
}
