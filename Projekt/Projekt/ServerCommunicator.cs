using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class ServerCommunicator
{
    private readonly string serverUrl;

    public ServerCommunicator(string url)
    {
        serverUrl = url;
    }

    public async Task SendFileAsync(string filePath)
    {
        using (HttpClient client = new HttpClient())
        {
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                content.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "file", Path.GetFileName(filePath));
                HttpResponseMessage response = await client.PostAsync(serverUrl, content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
