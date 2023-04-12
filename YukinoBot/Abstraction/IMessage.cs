namespace YukinoBot.Abstraction;

public interface IMessage<out TUser, out TMedia> : IMessage where TUser : IUser where TMedia : IMedia
{
    new TUser From { get; }
    new TUser To { get; }
    new IEnumerable<TMedia> Medias { get; }
    new IEnumerable<TUser> AtUsers { get; }
}

public interface IMessage
{
    long Id { get; }
    string Content { get; }
    string Route { get; }
    DateTime Time { get; }

    IUser From { get; }
    IUser To { get; }

    IEnumerable<IMedia> Medias { get; }
    IEnumerable<IUser> AtUsers { get; }
}