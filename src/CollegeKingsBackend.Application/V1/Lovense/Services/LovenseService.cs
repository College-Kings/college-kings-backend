using CollegeKingsBackend.Domain.Entities.Lovense;

namespace CollegeKingsBackend.Application.V1.Lovense.Services;

public class LovenseService : ILovenseService
{
    private readonly Dictionary<string, ILovenseUser> _users;

    public LovenseService()
    {
        _users = new Dictionary<string, ILovenseUser>();
    }
    
    public Dictionary<string, ILovenseUser> GetUsers()
    {
        return _users;
    }

    public void CreateUser(string userUid, ILovenseUser user)
    {
        _users[userUid] = user;
    }
}