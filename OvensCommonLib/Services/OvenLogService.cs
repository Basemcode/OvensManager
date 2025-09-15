public static class OvenLogService
{
    public static void SaveStatusChange(Oven oven, CycleSteps newStep)
    {
        using (var db = new MyDbContext())
        {
            var log = new OvenLog
            {
                OvenNumber = oven.Number,
                CycleStep = newStep.ToString(),
                Temperature = oven.Temperature,
                Timestamp = DateTime.Now,
            };

            db.OvenStatusLogs.Add(log);
            db.SaveChanges();
        }
    }
}
