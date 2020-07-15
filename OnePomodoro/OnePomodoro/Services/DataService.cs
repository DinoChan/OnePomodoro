using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OnePomodoro.Services
{
    public class DataService
    {
        public readonly static string DbName= "pomodoro.db";

        public static async Task<bool> IsFilePresent()
        {
            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(DbName);
            return item != null;
        }

        public static async Task CreateTheDatabaseAsync()
        {
            PeriodContext.Filename = Path.Combine(ApplicationData.Current.LocalFolder.Path, DbName);
            if (await IsFilePresent() == false)
            {
                var context = new PeriodContext();
                await context.Database.EnsureCreatedAsync();
            }
        }

        public static async Task RemoveFuturePeriodsAsync()
        {
            using (var context = new PeriodContext())
            {
                var time = DateTime.Now;
                var periods = await context.Periods.Where(p => p.From > time).ToListAsync();
                context.Periods.RemoveRange(periods);
                int records = await context.SaveChangesAsync();
            }
        }

        public async Task AddPeriodsAsync(IEnumerable<Period> periods)
        {
            using (var context = new PeriodContext())
            {
                context.Periods.AddRange(periods);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Period>> QueryAllPeriodsAsync()
        {
            using (var context = new PeriodContext())
            {
                var result = await context.Periods.ToListAsync();
                return result;
            }
        }
    }
}
