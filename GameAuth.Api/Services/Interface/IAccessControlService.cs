namespace GameAuth.Api.Services.Interface;

public interface IAccessControlService
{
    public Task<bool> AllowAccess(string stringAccountId);
}