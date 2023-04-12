using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YukinoBot.Abstraction;

namespace YukinoBot.Entity;

public class User : IUser<long>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InnerId { get; set; }

    public long Id { get; set; }

    public string Nick { get; set; } = null!;

    public long? GroupId { get; set; }

    public long GetUserId() => Id;

    public string GetUserName() => Nick;

    string IUser.GetUserId() => Id.ToString();
}

