namespace Paradaim.Core.Infrastructures.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception ex)
        {
            return $"{ex.Message}:{ex.InnerException?.Message}";
        }
    }
}
