using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using YukinoBot.Abstraction;

namespace YukinoBot.Entity;

public class Message : IMessage<User, Media>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public User From { get; set; } = null!;
    public User To { get; set;} = null!;
    public IEnumerable<Media> Medias { get; set; } = null!;
    public IEnumerable<User> AtUsers { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Route { get; set; } = null!;
    public string Source { get; set; } = null!;
    public DateTime Time { get; set; }

    [JsonIgnore, NotMapped]
    IUser IMessage.From => From;

    [JsonIgnore, NotMapped]
    IUser IMessage.To => To;

    [JsonIgnore, NotMapped]
    IEnumerable<IMedia> IMessage.Medias => Medias;

    [JsonIgnore, NotMapped]
    IEnumerable<IUser> IMessage.AtUsers => AtUsers;
}
