using Fastkart.Models.Entities;

public class UserExternalLogin
{
    public int Uid { get; set; }
    public int UserUid { get; set; }
    public string LoginProvider { get; set; } // "Google", "Facebook"
    public string ProviderKey { get; set; } 
    public string? ProviderDisplayName { get; set; }
    public DateTime CreatedAt { get; set; }

    public Users User { get; set; }
}