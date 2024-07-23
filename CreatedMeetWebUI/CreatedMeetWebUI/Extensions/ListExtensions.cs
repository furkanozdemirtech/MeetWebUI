namespace CreatedMeetWebUI.Extensions
{
    public static class ListExtensions
    {
        public static List<T> InitializeIfNull<T>(this List<T> list)
        {
            return list ?? new List<T>();
        }
    }
}
