namespace AuthorizationAPI.Services
{
    public interface IJwtService
    {
        string GenerateSecurityToken(string username);
    }
}
