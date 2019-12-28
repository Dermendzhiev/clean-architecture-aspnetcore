namespace CleanArchitecture.FunctionalTests.Helpers
{
    using CleanArchitecture.Persistence;

    internal static class DatabaseInitializer
    {
        internal static void InitializeDbForTests(CleanArchitectureDbContext db)
        {
            PredefinedData.InitializePolls();
            db.Polls.AddRange(PredefinedData.Polls);

            db.SaveChanges();
        }

        internal static void ReinitializeDbForTests(CleanArchitectureDbContext db)
        {
            db.Polls.RemoveRange(db.Polls);

            InitializeDbForTests(db);
        }
    }
}