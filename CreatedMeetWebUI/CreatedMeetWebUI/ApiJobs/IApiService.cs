namespace CreatedMeetWebUI.ApiJobs
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string url);
        Task<T> GetByElment<T>(string url);

    }
}
