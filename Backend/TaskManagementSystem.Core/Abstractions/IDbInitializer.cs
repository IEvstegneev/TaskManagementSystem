namespace TaskManagementSystem.Core.Abstractions
{
    public interface IDbInitializer
    {
        void MigrateDb();
        void SeedDb();
    }
}