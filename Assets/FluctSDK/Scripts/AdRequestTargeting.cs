namespace Fluct
{
    public class AdRequestTargeting
    {
        public string UserId { get; private set; }
        public AdRequestTargeting(string userId = null)
        {
            UserId = userId;
        }
    }
}
