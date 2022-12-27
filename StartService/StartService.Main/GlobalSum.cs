namespace StartService.Main
{
    public static class GlobalSum
    {
        public static long Sum { get; set; }

        public static int Count { get; set; } = 0;

        public static DateTime StartTime { get; set; }
        public static void AddCount()
        {
            Count++;
        }
    }
}
