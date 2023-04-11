using System.Text.Json.Serialization;
using System.Text.Json;
using YukinoBot.Abstraction;

namespace YukinoBot.OPQ.Net;

public abstract class OpqRequest<T> : IOutMessage where T : class
{
    public abstract string CgiCmd { get; }

    public T CgiRequest { get; set; } = null!;

    public virtual string Serialize() => JsonSerializer.Serialize(this, options: new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
}

