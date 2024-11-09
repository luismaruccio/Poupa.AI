namespace Poupa.AI.Common.Extensions.Messages
{
    public static class MessageExtension
    {
        public static string WithParameters(this string message, params object[] parameters)
        {
            return string.Format(message, parameters);
        }
    }
}
