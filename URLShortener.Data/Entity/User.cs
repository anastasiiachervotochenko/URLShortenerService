using System.ComponentModel.DataAnnotations;

namespace URLShortener.Data.Entity;

public class User
{
    [Key] public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}