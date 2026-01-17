namespace BeFitMAUI.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string Email { get; set; }
        // Add custom properties here if needed in the future
    }
}
