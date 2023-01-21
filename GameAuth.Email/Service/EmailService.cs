using GameAuth.Email.Service.Interface;

namespace GameAuth.Email.Service;

public class EmailService : IEmailService
{
    private readonly HttpClient client;
    private readonly string supportName;
    private readonly string domainName;
    private readonly string baseUri;
    public EmailService(HttpClient client)
    {
        this.client = client;
        supportName = "support";
        domainName = "currentcapitalism.com";
        baseUri = $"v3/{domainName}";
    }

    public async Task SendSupportEmail(string destinationEmail, string subject, string text)
    {
        var content = BaseEmailRequestContent(supportName, destinationEmail, subject, text);
        var res = await client.PostAsync($"{baseUri}/messages", content);
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine(res.ReasonPhrase);
        Console.WriteLine(res.Content.Headers.ToString());
        Console.WriteLine(res.StatusCode);
        Console.WriteLine(res.RequestMessage);
        Console.WriteLine(string.Join('\n', client.DefaultRequestHeaders.Select(kv => $"{kv.Key}: {string.Join(", ", kv.Value)}")));
    }

    private FormUrlEncodedContent BaseEmailRequestContent(string senderName, string destinationEmail, string subject, string text)
    {
        return new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", $"Sender {senderName} <{senderName}@{domainName}>"),
            new KeyValuePair<string, string>("to", destinationEmail),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("text", text)
        });
    }
}