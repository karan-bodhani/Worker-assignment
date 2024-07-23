using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Worker
{
    public class DataAccess
    {
        private readonly AppDbContext _context;

        public DataAccess(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DatabaseItem>> GetItemsForWorkerAsync(string workerName)
        {
            return await _context.Items.Where(item => item.CurrentWorker == workerName).ToListAsync();
        }

        public async Task UpdateItemValueAsync(string itemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
            if (item != null)
            {
                item.Value++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignItemsToWorkersAsync(IEnumerable<string> workerNames)
        {
            var items = await _context.Items.ToListAsync();
            var workerCount = workerNames.Count();
            for (int i = 0; i < items.Count; i++)
            {
                var worker = workerNames.ElementAt(i % workerCount);
                items[i].CurrentWorker = worker;
            }

            _context.Items.UpdateRange(items);
            await _context.SaveChangesAsync();
        }
    }

    public class DatabaseItem
    {
        public string Id { get; set; }
        public int Value { get; set; }
        public string CurrentWorker { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DatabaseItem> Items { get; set; }
    }
}
