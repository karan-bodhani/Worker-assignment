using System.Collections.Generic;

namespace Worker
{
    public class ConsistentHashing
    {
        private readonly List<string> _workers = new();

        public void UpdateWorkers(IEnumerable<string> workers)
        {
            _workers.Clear();
            _workers.AddRange(workers);
        }

        public string GetWorkerForItem(string itemId)
        {
            // Example consistent hashing logic (simple modulo for demonstration)
            int hash = itemId.GetHashCode();
            return _workers[hash % _workers.Count];
        }
    }
}
