namespace YukinoBot.Abstraction;

public interface IUser<TUserId> : IUser where TUserId : new()
{
    new TUserId GetUserId();
}

public interface IUser
{
    string GetUserId();
    string GetUserName();
}
