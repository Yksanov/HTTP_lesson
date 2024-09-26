using System.Net;

namespace Task_2;

class Program
{
    static async Task Main(string[] args)
    {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/mysite/");
            listener.Prefixes.Add("http://localhost:5000/white_rabbit/");
            listener.Start();
            Console.WriteLine("Waiting for a request...");
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerResponse response = context.Response;
            string status = context.Request.Url.AbsolutePath;
            switch (status)
            {
                case "/mysite/":
                    context.Response.StatusCode = (int)HttpStatusCode.OK; 
                    Console.WriteLine(context.Response.StatusCode);
                    string text1 = """
                                       <html>
                                          <head>
                                              <meta charset='utf-8'>
                                          </head>
                                          <body>
                                            <div>
                                                <h1> Follow the white rabbit </h1>
                                            </div>
                                          </body>
                                       </html>
                                       """;
                      byte[] buffers = System.Text.Encoding.UTF8.GetBytes(text1);
                      response.ContentLength64 = buffers.Length;
                      Stream stream1 = response.OutputStream;
                      await stream1.WriteAsync(buffers, 0, buffers.Length);
                    break;
                case "/white_rabbit/":
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    Console.WriteLine(context.Response.StatusCode);
                    string text2 = """
                                <html>
                                   <head>
                                       <meta charset='utf-8'>
                                   </head>
                                   <body>
                                       <h1> You are living in the matrix </h1>
                                   </body>
                                </html>
                                """;
                    byte[] buffer2 = System.Text.Encoding.UTF8.GetBytes(text2);
                    response.ContentLength64 = buffer2.Length;
                    Stream stream2 = response.OutputStream;
                    await stream2.WriteAsync(buffer2, 0, buffer2.Length);
                    break;
                default:
                    context.Response.StatusCode = 501;
                    context.Response.OutputStream.Write(new byte[0]);
                    Console.WriteLine(context.Response.StatusCode);
                      string text3 = $"""
                                     <html>
                                        <head>
                                            <meta charset='utf-8'>
                                        </head>
                                        <body>
                                            <h1> {HttpStatusCode.NotImplemented} </h1>
                                        </body>
                                     </html>
                                     """;
                      byte[] buffer3 = System.Text.Encoding.UTF8.GetBytes(text3);
                      response.ContentLength64 = buffer3.Length;
                      Stream stream3 = response.OutputStream;
                      await stream3.WriteAsync(buffer3, 0, buffer3.Length);
                    break;
            }
            context.Response.OutputStream.Close();
            listener.Close();
            listener.Stop();
    }
}