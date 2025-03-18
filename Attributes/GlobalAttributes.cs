namespace AssemblyService.Attributes
{
    public static class GlobalAttributes
    {
        public static MySQLConfig mySQLConfig = new MySQLConfig();
    }

    public class MySQLConfig
    {
        public string? connectionString { get; set; }
    }
}
