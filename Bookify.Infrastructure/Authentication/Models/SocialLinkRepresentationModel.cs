namespace Bookify.Infrastructure.Authentication.Models;

public class SocialLinkRepresentationModel
{
    public string? SocialProvider { get; set; }  // optional  
    public string? SocialUserId { get; set; }    // optional  
    public string? SocialUsername { get; set; }   // optional  
}