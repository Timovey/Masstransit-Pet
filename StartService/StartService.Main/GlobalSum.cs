namespace StartService.Main
{
    /// <summary>
    /// Накопитель результатов. ArrayReadyConsumer вызывается MassTransit параллельно, поэтому
    /// обновления атомарные: обычные <c>Sum += x</c> и <c>Count++</c> теряли часть результатов.
    /// </summary>
    public static class GlobalSum
    {
        private static long _sum;
        private static int _count;

        public static long Sum => Interlocked.Read(ref _sum);

        public static int Count => Volatile.Read(ref _count);

        public static DateTime StartTime { get; set; }

        public static void Add(long value)
        {
            Interlocked.Add(ref _sum, value);
            Interlocked.Increment(ref _count);
        }

        public static void Reset()
        {
            Interlocked.Exchange(ref _sum, 0);
            Interlocked.Exchange(ref _count, 0);
        }
    }
}
