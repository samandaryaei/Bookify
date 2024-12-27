namespace Bookify.Infrastructure.Authentication.Models;

public class CredentialRepresentationModel
{
    public string? Id { get; set; }  // optional  
    public string? Type { get; set; }  // optional  
    public string? UserLabel { get; set; }  // optional  
    public long? CreatedDate { get; set; }  // optional  
    public string? SecretData { get; set; }  // optional  
    public string? CredentialData { get; set; }  // optional  
    public int? Priority { get; set; }  // optional  
    public string? Value { get; set; }  // optional  
    public bool? Temporary { get; set; }  // optional  
    public string? Device { get; set; }  // optional  
    public string? HashedSaltedValue { get; set; }  // optional  
    public string? Salt { get; set; }  // optional  
    public int? HashIterations { get; set; }  // optional  
    public int? Counter { get; set; }  // optional  
    public string? Algorithm { get; set; }  // optional  
    public int? Digits { get; set; }  // optional  
    public int? Period { get; set; }  // optional  
    public Dictionary<string, object>? Config { get; set; }  // optional
}