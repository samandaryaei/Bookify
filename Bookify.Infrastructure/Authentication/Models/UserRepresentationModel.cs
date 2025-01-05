using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Authentication.Models;

public class UserRepresentationModel
{
    // public string? Id { get; set; }  // optional  
    // public string? Username { get; set; }  // optional  
    // public string? FirstName { get; set; }  // optional  
    // public string? LastName { get; set; }  // optional  
    // public string? Email { get; set; }  // optional  
    // public bool? EmailVerified { get; set; }  // optional  
    // public string? Attributes { get; set; }  // optional (Map of attributes)  
    // public string? FederatedIdentity { get; set; }  // optional  
    // public long? CreatedTimestamp { get; set; }  // optional  
    // public bool? Enabled { get; set; }  // optional  
    // public bool? Totp { get; set; }  // optional  
    // public string? FederatedLink { get; set; }  // optional  
    // public string? ServiceAccountClientId { get; set; }  // optional  
    // public List<CredentialRepresentationModel>? Credentials { get; set; }  // optional  
    // public HashSet<string>? DisabledCredentialTypes { get; set; }  // optional  
    // public List<string>? RequiredActions { get; set; }  // optional  
    // public string? RealmId { get; set; }  // optional  
    // public string? ClientRoles { get; set; }  // optional  
    // public List<SocialLinkRepresentationModel>? SocialLinks { get; set; }  // optional  
    // public int? NotBefore { get; set; }  // optional  
    // public string? ApplicationRoles { get; set; }  // optional (Map of application roles)  
    // public string? Access { get; set; }  // optional (Map of access)  
    //
    // internal static UserRepresentationModel FromUser(User user) =>
    //     new()
    //     {
    //         FirstName = user.FirstName.Value,
    //         LastName = user.LastName.Value,
    //         Email = user.Email.Value,
    //         Username = user.Email.Value,
    //         Enabled = true,
    //         EmailVerified = true,
    //         CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    //         Attributes = string.Empty,
    //         RequiredActions = new List<string>()
    //     };
    
    
    public string? Id { get; set; }  
    public string? Username { get; set; }  
    public string? FirstName { get; set; }  
    public string? LastName { get; set; }  
    public string? Email { get; set; }  
    public bool Enabled { get; set; }  
    //public bool? EmailVerified { get; set; }  // optional  
    public List<CredentialRepresentationModel>? Credentials { get; set; }  
    public static UserRepresentationModel FromUser(User user)  
    {  
        return new UserRepresentationModel  
        {  
            Username = user.Email.Value,  
            Email = user.Email.Value,  
            FirstName = user.FirstName.Value,  
            LastName = user.LastName.Value,  
            Enabled = true // or set based on your needs
            //EmailVerified = true
        };  
    }  
}