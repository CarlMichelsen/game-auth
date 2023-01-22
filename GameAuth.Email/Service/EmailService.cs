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

    public async Task<bool> SendSupportEmail(string destinationEmail, string subject, string text)
    {
        var content = BaseEmailRequestContent(supportName, destinationEmail, subject, text);
        var res = await client.PostAsync($"{baseUri}/messages", content);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> SendVerificationEmail(string destinationEmail, string code)
    {
        var lines = new List<string>
        {
            "<h1>Verify your email</h1>",
            $"<p>{code}</p>"
        };

        var html = string.Join("", lines);
        var content = BaseEmailRequestContent(supportName, destinationEmail, "Verify your email", html);
        var res = await client.PostAsync($"{baseUri}/messages", content);
        return res.IsSuccessStatusCode;
    }

    private FormUrlEncodedContent BaseEmailRequestContent(string senderName, string destinationEmail, string subject, string html)
    {
        return new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("from", $"{senderName} <{senderName}@{domainName}>"),
            new KeyValuePair<string, string>("to", destinationEmail),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("html", html)
        });
    }
}