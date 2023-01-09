using CollegeKingsWebServer.Contracts.Responses;
using CollegeKingsWebServer.Domain;

namespace CollegeKingsWebServer.Services;

public interface ILovenseService
{
    List<LovenseUser> GetUsers();

    LovenseUser? GetUserById(string userId);

    Task<LovenseQrCodeResponse?> CreateQrCode(string uid, string uname);
    
    void CreateUser(string userUid, LovenseUser user);
}