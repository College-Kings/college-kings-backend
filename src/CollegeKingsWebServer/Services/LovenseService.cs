using CollegeKingsWebServer.Contracts.Responses;
using CollegeKingsWebServer.Domain;

namespace CollegeKingsWebServer.Services;

public class LovenseService : ILovenseService
{
    private readonly Dictionary<string, LovenseUser> _users;

    public LovenseService()
    {
        _users = new Dictionary<string, LovenseUser>();
    }
    
    public List<LovenseUser> GetUsers()
    {
        return _users.Values.ToList();
    }

    public LovenseUser? GetUserById(string userId)
    {
        return _users.TryGetValue(userId, out LovenseUser? value) ? value : null;
    }

    public async Task<LovenseQrCodeResponse?> CreateQrCode(string uid, string uname)
    {
        Dictionary<string, string> payload = new()
        {
            {"token", "eJFamEJC_aIzOFF7L-jANpY2XW2_RiwU8jboteQw-kWDYaYrON_vu7uMMPxEZ2gW"},
            {"uid", uid},
            {"uname", uname},
            {"utoken", uid},
            {"v", "2"}
        };
        
        JsonContent httpContent = JsonContent.Create(payload); 
        HttpClient client = new();

        HttpResponseMessage httpResponse = await client.PostAsync("https://api.lovense.com/api/lan/getQrCode", httpContent);
        LovenseQrCodeResponse? response = await httpResponse.Content.ReadFromJsonAsync<LovenseQrCodeResponse>();

        return response;
    }
    
    public void CreateUser(string userId, LovenseUser user)
    {
        if (!_users.ContainsKey(userId))
        {
            Console.WriteLine("New user connect");
        }
        _users[userId] = user;
    }
}