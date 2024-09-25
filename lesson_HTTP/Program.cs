namespace lesson_HTTP;

class Program
{
    static async Task Main(string[] args)
    {
        MyServer server = new MyServer();
        await server.RunServerAsync("../../../site", 8888);
    }
}