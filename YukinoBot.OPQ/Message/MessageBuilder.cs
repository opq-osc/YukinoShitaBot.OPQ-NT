using YukinoBot.Abstraction;
using YukinoBot.Entity;

namespace YukinoBot.OPQ.Message;

public class MessageBuilder : IMessageBuilder<Entity.Message>
{
    public List<Media>? Medias { get; internal set; }
    public List<User>? AtUsers { get; internal set; }
    public string Content { get; internal set; } = string.Empty;
    public User Sender { get; internal set; } = null!;
    public User? Receiver { get; set; }

    public Entity.Message Build()
    {
        if (Receiver is null || Sender is null)
        {
            throw new InvalidOperationException("Sender or Receiver is null");
        }
        return new Entity.Message
        {
            Content = Content,
            Medias = Medias ?? Enumerable.Empty<Media>(),
            AtUsers = AtUsers ?? Enumerable.Empty<User>(),
            From = Sender,
            To = Receiver,
            Time = DateTime.UtcNow,
        };
    }

    public IMessageBuilder AddMedia(IMedia media)
    {
        if (media is not Media m)
        {
            throw new ArgumentException("media must be type Media", nameof(media));
        }
        Medias ??= new();
        Medias.Add(m);
        return this;
    }

    public IMessageBuilder AtUser(IUser user)
    {
        if (user is not User u)
        {
            throw new ArgumentException("user must be type User", nameof(user));
        }
        AtUsers ??= new();
        AtUsers.Add(u);
        return this;
    }

    public IMessageBuilder SendTo(IUser user)
    {
        if (user is not User u)
        {
            throw new ArgumentException("user must be type User", nameof(user));
        }
        Receiver = u;
        return this;
    }

    public IMessageBuilder WithContent(string content)
    {
        Content = content;
        return this;
    }

    IMessage IMessageBuilder.Build()
    {
        return Build();
    }
}
