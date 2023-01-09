using CollegeKingsBackend.Application.V1.Lovense.Responses;
using CollegeKingsBackend.Domain.Entities.Lovense;

namespace CollegeKingsBackend.Application.V1.Lovense.Services;

public interface ILovenseService
{
    Dictionary<string, ILovenseUser> GetUsers();

    void CreateUser(string userUid, ILovenseUser user);
}