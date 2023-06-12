namespace BIP.InternalCRM.Infrastructure.Options
{
    public class CorsOptions
    {
        public bool Enabled { get; set; }
        
        public string[] ClientOrigins { get; set; }
    }
}
