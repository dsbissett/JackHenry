namespace JackHenry.Ui.Hubs.Clients
{
    public interface IPostClient
    {
        Task SendPostCount(int count);

        Task SendPostTitles(IEnumerable<string> titles);

        Task SendPostAuthors(IEnumerable<string> authors);
    }
}
