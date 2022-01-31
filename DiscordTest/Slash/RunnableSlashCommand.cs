using System.Reflection;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace DiscordTest.Slash;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class InfoAttribute : Attribute
{
    public readonly string Info;
    public InfoAttribute(string info) => Info = info;
}

public abstract class RunnableSlashCommand : BaseSlashCommand
{
    protected abstract Delegate RunDelegate { get; }
    private Dictionary<string, (Type, int)> parameterInfos = new();

    protected override IEnumerable<BaseSlashCommand> GetSubCommands() => Array.Empty<BaseSlashCommand>();

    protected override SlashCommandBuilder GetBuilder()
    {
        SlashCommandBuilder builder = new();
        builder.WithName(Name).WithDescription(Description);

        foreach (ParameterInfo parameterInfo in RunDelegate.Method.GetParameters())
        {
            if (parameterInfo.Name is null) continue;

            (ApplicationCommandOptionType optionType, bool found) = GetOptionType(parameterInfo);
            if (!found)
            {
                throw new ArgumentException($"Invalid parameter specified in Runnable Command {GetType().FullName}.{RunDelegate.Method.Name} -- " +
                                            $"{parameterInfo.ParameterType.Name} {parameterInfo.Name}");
            }

            string name = GetValidParameterName(parameterInfo.Name);
            string description =
                parameterInfo.GetCustomAttribute<InfoAttribute>()?.Info ?? $"The {name}.";
            builder.AddOption(name, optionType, description, !parameterInfo.IsOptional);

            parameterInfos.Add(name, (parameterInfo.ParameterType, parameterInfo.Position));
        }

        return builder;
    }

    protected override Task RunCommand(IEnumerable<SocketSlashCommandDataOption> options, string _)
    {
        object[] parameters = new object[parameterInfos.Count];
        foreach (SocketSlashCommandDataOption option in options)
        {
            (Type parameterType, int parameterPosition) = parameterInfos[option.Name];
            parameters[parameterPosition] = Convert.ChangeType(option.Value, parameterType);
        }

        object? ret = RunDelegate.DynamicInvoke(parameters);
        return ret as Task ?? Task.CompletedTask;
    }

    private static string GetValidParameterName(string name)
    {
        StringBuilder sb = new();
        foreach (char c in name)
        {
            if (char.IsLower(c))
            {
                sb.Append(c);
            }
            else if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLower(c));
            }
        }

        return sb.ToString();
    }

    private static (ApplicationCommandOptionType, bool) GetOptionType(ParameterInfo parameterInfo)
    {
        Type type = parameterInfo.ParameterType;
        if (type == typeof(int) || type == typeof(long) || type == typeof(uint) || type == typeof(ulong))
        {
            return (ApplicationCommandOptionType.Integer, true);
        }

        if (type == typeof(double) || type == typeof(float)) return (ApplicationCommandOptionType.Number, true);
        if (type == typeof(bool)) return (ApplicationCommandOptionType.Boolean, true);
        if (type == typeof(string)) return (ApplicationCommandOptionType.String, true);
        if (type == typeof(IGuildChannel)) return (ApplicationCommandOptionType.Channel, true);
        if (type == typeof(IMentionable)) return (ApplicationCommandOptionType.Mentionable, true);
        if (type == typeof(IUser)) return (ApplicationCommandOptionType.User, true);
        if (type == typeof(IRole)) return (ApplicationCommandOptionType.Role, true);

        return (ApplicationCommandOptionType.Boolean, false);
    }
}
