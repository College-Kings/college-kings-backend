using CollegeKingsBackend.Application.V1.Lovense.Responses;
using CollegeKingsBackend.Domain.Entities.Lovense;

namespace CollegeKingsBackend.Application.V1.Lovense.Queries;

public interface ILovenseQueries
{
    Dictionary<string, ILovenseUser>.ValueCollection GetUsers();

    ILovenseUser? GetUserById(string userId);

    Task<LovenseQrCodeResponse?> CreateQrCode(string uid, string uname);

    LovenseUserResponse CreateUser(ILovenseUser user);

}