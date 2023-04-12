using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using YukinoBot.Abstraction;

namespace YukinoBot.Entity;

public class Media : IMedia
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonIgnore]
    public string Type { get; set; } = null!;

    public string? Url { get; set; }

    [JsonIgnore]
    public string? FilePath { get; set; }

    public Guid GetMediaId() => Id;
}
