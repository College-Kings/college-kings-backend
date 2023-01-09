namespace CollegeKingsBackend.Application.V1.Users.Services;

public interface IUserService
{
    Task ValidateUserCredentialsAsync();
    Task RefreshPatreonStatus();
    Task CheckIsPatreonUpToDate();
}