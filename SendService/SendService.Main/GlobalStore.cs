using DataContracts;
using System.Collections.Concurrent;

namespace SendService.Main
{
    /// <summary>
    /// Общее состояние экземпляра SendService. Его одновременно читают и меняют консьюмеры MassTransit
    /// (из пула потоков), HTTP-контроллеры и фоновый Worker, поэтому все операции потокобезопасны.
    /// Раньше синхронизация делалась через <c>lock (new object())</c> — у каждого вызова был свой объект
    /// блокировки, и потоки никогда не ждали друг друга.
    /// </summary>
    public static class GlobalStore
    {
        private static readonly object _sync = new();

        private static volatile HashSet<string> _ports = new HashSet<string>();

        private static readonly ConcurrentDictionary<int, int[]> Arrays = new();

        // volatile: флаги читаются в циклах ожидания, без него JIT вправе закешировать значение,
        // и цикл не увидит изменения из другого потока
        private static volatile bool _flag;
        private static volatile bool _isSendRequest;

        private static int _time;

        public static bool Flag
        {
            get => _flag;
            set => _flag = value;
        }

        public static bool IsSendRequest
        {
            get => _isSendRequest;
            set => _isSendRequest = value;
        }

        public static int GetTime()
        {
            return Volatile.Read(ref _time);
        }

        public static void SetPorts(HashSet<string> ports)
        {
            _ports = ports;
        }

        public static HashSet<string> GetPorts()
        {
            return _ports;
        }

        public static void AddArrays(int index, int[] array)
        {
            // повторная доставка того же сообщения не должна ронять консьюмер исключением о дубле ключа
            Arrays.TryAdd(index, array);
        }

        /// <summary>
        /// Забирает один массив из хранилища. Возвращает null, если массивов нет.
        /// </summary>
        public static ArrayMessage? GetArray()
        {
            lock (_sync)
            {
                foreach (var key in Arrays.Keys)
                {
                    if (Arrays.TryRemove(key, out var array))
                    {
                        Interlocked.Increment(ref _time);
                        return new ArrayMessage()
                        {
                            Array = array,
                            Index = key
                        };
                    }
                }
                return null;
            }
        }

        public static int GetArraysCount()
        {
            return Arrays.Count;
        }
    }
}
