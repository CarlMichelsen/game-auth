namespace GameAuth.Api.Models.Dto;

public record AuthResponse<T>
{
    public AuthResponse() { }
    public AuthResponse(T? input) { data = input; }
    public AuthResponse(T? input, string err) { data = input; errors.Add(err); }
    public AuthResponse(T? input, IEnumerable<string> errors) { data = input; this.errors = errors.Concat(errors).ToList(); }

    public bool Ok { get => !errors.Any() && !Equals(data, default(T)); }
    public List<string> Errors { get => errors; }
    public T? Data { get => Ok ? data : default; set => data = value; }

    private readonly List<string> errors = new();
    private T? data;
}