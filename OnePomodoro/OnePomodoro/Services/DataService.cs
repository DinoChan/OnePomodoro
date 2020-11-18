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
        public readonly static string DbName = "pomodoro.db";

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
            await CreateTheDatabaseAsync();
            using (var context = new PeriodContext())
            {
                var time = DateTime.Now;
                var periods = await context.Periods.Where(p => p.From > time).ToListAsync();
                context.Periods.RemoveRange(periods);
                int records = await context.SaveChangesAsync();
            }
        }

        public static async Task AddPeriodsAsync(IEnumerable<Period> periods)
        {
            await CreateTheDatabaseAsync();
            using (var context = new PeriodContext())
            {
                context.Periods.AddRange(periods);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Period>> QueryAllPeriodsAsync()
        {
            await CreateTheDatabaseAsync();
            using (var context = new PeriodContext())
            {
                var result = await context.Periods.ToListAsync();
                return result;
            }
        }


        public static async Task AddFuturePeriodsAsync(bool isInPomodoro, DateTime startTime, bool autoStartOfNextPomodoro, bool autoStartOfBreak,
          int completedPomodoros, int longBreakAfter, TimeSpan pomodoroLength, TimeSpan shortBreakLength, TimeSpan longBreakLength)
        {
            var periods = new List<Period>();
            if (isInPomodoro)
                completedPomodoros++;

            int count = 0;
            while (count < 8)
            {
                isInPomodoro = isInPomodoro == false;
                if (isInPomodoro)
                {
                    if (autoStartOfNextPomodoro == false)
                        break;

                    startTime += pomodoroLength;
                    periods.Add(new Period { From = startTime - pomodoroLength, HasFinished = true, To = startTime, IsFocus = true });
                    completedPomodoros++;
                }
                else
                {
                    if (autoStartOfBreak == false)
                        break;

                    var breakLength = (completedPomodoros % longBreakAfter == 0) ? longBreakLength : shortBreakLength;
                    startTime += breakLength;
                    periods.Add(new Period { From = startTime - breakLength, HasFinished = true, To = startTime, IsFocus = false });
                }

                count++;
            }

            await AddPeriodsAsync(periods);
        }


        public static async Task AddPeriodAsync(bool isInPomodoro, bool hasFinished, DateTime startTime, DateTime endTime)
        {
            await CreateTheDatabaseAsync();
            using (var context = new PeriodContext())
            {
                context.Periods.Add(new Period { From = startTime, HasFinished = hasFinished, To = endTime, IsFocus = isInPomodoro });
                await context.SaveChangesAsync();
            }
        }
    }
}
