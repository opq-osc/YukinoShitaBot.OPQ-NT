using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YukinoBot.Abstraction;

namespace YukinoBot.Entity;

public class User : IUser<long>, IEquatable<User?>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InnerId { get; set; }

    public long SenderId { get; set; }

    public string Nick { get; set; } = null!;

    public long? GroupId { get; set; }

    public long GetUserId() => SenderId;

    public string GetUserName() => Nick;

    string IUser.GetUserId() => SenderId.ToString();

    public override bool Equals(object? obj)
    {
        return obj is User user && Equals(user);
    }

    public bool Equals(User? other)
    {
        return other is not null &&
               SenderId == other.SenderId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SenderId);
    }
}
