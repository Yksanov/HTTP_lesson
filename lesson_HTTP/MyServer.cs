using System.Net;

namespace lesson_HTTP;

public class MyServer
{
    private string _siteDirectory;
    private HttpListener _listener;
    private int _port;

    public async Task RunServerAsync(string path, int port)
    {
        _siteDirectory = path;
        _port = port;
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{_port.ToString()}/");
        _listener.Start();
        Console.WriteLine($"Server started on {_port} \nFiles in {_siteDirectory}");
        await ListenAsync();
    }

    private async Task ListenAsync()
    {
        try
        {
            while (true)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                Process(context);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Process(HttpListenerContext context)
    {
        Console.WriteLine(context.Request.HttpMethod);
        string filename = context.Request.Url.AbsolutePath;
        Console.WriteLine(filename);
        filename = _siteDirectory + filename;
        if (File.Exists(filename))
        {
            try
            {
                Stream fileStream = File.Open(filename, FileMode.Open);
                context.Response.ContentType = GetContentType(filename);
                context.Response.ContentLength64 = fileStream.Length;
                byte[] buffer =  new byte[16 * 1024];
                int dataLength;
                do
                {
                    dataLength = fileStream.Read(buffer, 0, buffer.Length);
                    context.Response.OutputStream.Write(buffer, 0, dataLength);

                } while (dataLength > 0);
                fileStream.Close();
                context.Response.OutputStream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Write(new byte[0]);
            }
        }
        else
        {
            context.Response.StatusCode = 404;
            context.Response.OutputStream.Write(new byte[0]);
        }
        context.Response.OutputStream.Close();
    }

    private string? GetContentType(string filename)
    {
        var Dictionary = new Dictionary<string, string>()
        {
            { ".css", "text/css" },
            { ".js", "application/javascript" },
            { ".png", "image/png" },
            { ".jpg", "image/jpg" },
            { ".gif", "image/gif" },
            { ".html", "text/html" },
            { ".json", "application/json" },
        };
        string contentype = "";
        string extension = Path.GetExtension(filename);
        Dictionary.TryGetValue(extension, out contentype);
        return contentype;
    }

    public void Stop()
    {
        _listener.Abort();
        _listener.Stop();
    }
}