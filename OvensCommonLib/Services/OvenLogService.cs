using Microsoft.EntityFrameworkCore;
using OvensCommonLib.Data;
using OvensCommonLib.Models;
using OvensCommonLib.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OvensCommonLib.Services
{
    public class OvenLogService
    {
        private readonly IDbContextFactory<OvensDbContext> _contextFactory;

        public OvenLogService(IDbContextFactory<OvensDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private OvenLog CreateLog(IOvenSnapshot oven, LogType logType)
        {
            return new OvenLog
            {
                OvenNumber = oven.Number,
                CycleStep = oven.CycleStep,
                Temperature = oven.Temperature,
                Timestamp = DateTime.Now,
                LogType = logType
            };
        }

        public async Task SaveStatusChangeAsync(IOvenSnapshot oven)
        {
            await using var db = _contextFactory.CreateDbContext();
            var log = CreateLog(oven, LogType.StatusChange);
            await db.OvenLogs.AddAsync(log);
            await db.SaveChangesAsync();
        }

        public async Task SaveTemperatureAsync(IOvenSnapshot oven)
        {
            await using var db = _contextFactory.CreateDbContext();
            var log = CreateLog(oven, LogType.Temperature);
            await db.OvenLogs.AddAsync(log);
            await db.SaveChangesAsync();
        }

        // add whole list of oven logs at once
        public async Task SaveTemperatureBatchAsync(IEnumerable<IOvenSnapshot> ovens)
        {
            await using var db = _contextFactory.CreateDbContext();

            var logs = new List<OvenLog>();
            foreach (var oven in ovens)
            {
                logs.Add(CreateLog(oven, LogType.Temperature));
            }

            await db.OvenLogs.AddRangeAsync(logs);
            await db.SaveChangesAsync();
        }
    }
}
