namespace DiscordTest.Slash.Commands;

public class MathSlashCommand : BaseSlashCommand
{
    public override string Name => "math";
    protected override string Description => "Helpful math functions.";

    protected override IEnumerable<BaseSlashCommand> SubCommands { get; } = new BaseSlashCommand[]
    {
        new SquareSlashCommand(), new RoundSlashCommand()
    };

    private class SquareSlashCommand : RunnableSlashCommand
    {
        public override string Name => "square";
        protected override string Description => "Square a number.";
        protected override Delegate RunDelegate => Square;

        private void Square(double number)
        {
            command?.RespondAsync($"The square of '{number}' is '{number * number}'!");
        }
    }

    private class RoundSlashCommand : BaseSlashCommand
    {
        public override string Name => "round";
        protected override string Description => "Round a number.";

        protected override IEnumerable<BaseSlashCommand> SubCommands { get; } = new BaseSlashCommand[]
            { new RoundNumberSlashCommand { roundUp = true }, new RoundNumberSlashCommand { roundUp = false } };

        private class RoundNumberSlashCommand : RunnableSlashCommand
        {
            public bool roundUp;

            private string roundStr => roundUp ? "up" : "down";
            public override string Name => roundStr;
            protected override string Description => "Round a number up.";
            protected override Delegate RunDelegate => Round;

            private Task Round([Info("The number to round.")] double number)
            {
                double rounded = roundUp ? Math.Ceiling(number) : Math.Floor(number);
                return command?.RespondAsync($"'{number}' rounded {roundStr} is '{rounded}'!") ?? Task.CompletedTask;
            }
        }
    }
}
