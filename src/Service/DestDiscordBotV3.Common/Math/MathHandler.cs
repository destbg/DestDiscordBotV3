namespace DestDiscordBotV3.Common.Math
{
    using org.mariuszgromada.math.mxparser;

    public class MathHandler : IMathHandler
    {
        public string Calculate(string expression) =>
            new Expression(expression).calculate().ToString();
    }
}