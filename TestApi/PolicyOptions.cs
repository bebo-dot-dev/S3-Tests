using System.ComponentModel.DataAnnotations;

namespace TestApi;

public class PolicyOptions
{
    public static string SectionName = "Policy";
    
    [Required]
    [Range(1, 1000)]
    public int JitterMsecs { get; set; }
    
    [Required]
    [Range(1, 5)]
    public int RetryCount { get; set; }
}