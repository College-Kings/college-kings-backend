using System.Net.Http.Json;
using CollegeKingsBackend.Application.V1.Lovense.Responses;
using CollegeKingsBackend.Application.V1.Lovense.Services;
using CollegeKingsBackend.Domain.Entities.Lovense;

namespace CollegeKingsBackend.Application.V1.Lovense.Queries;

public class LovenseQueries : ILovenseQueries
{
    private readonly ILovenseService _lovenseService;
    private HttpClient _httpClient;

    public LovenseQueries(HttpClient httpClient, ILovenseService lovenseService)
    {
        _httpClient = httpClient;
        _lovenseService = lovenseService;
    }
    
    public Dictionary<string, ILovenseUser>.ValueCollection GetUsers()
    {
        return _lovenseService.GetUsers().Values;
    }

    public ILovenseUser? GetUserById(string userId)
    {
        return _lovenseService.GetUsers().TryGetValue(userId, out ILovenseUser? value) ? value : null;
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

        HttpResponseMessage httpResponse = await _httpClient.PostAsync("https://api.lovense.com/api/lan/getQrCode", httpContent);
        LovenseQrCodeResponse? response = await httpResponse.Content.ReadFromJsonAsync<LovenseQrCodeResponse>();

        return response;
    }

    public LovenseUserResponse CreateUser(ILovenseUser user)
    {
        // if (GetUserById(user.Uid) is null)
        // {
        //     _logger.Log(LogLevel.Information, "New user connected.");
        // }
        
        _lovenseService.CreateUser(user.Uid, user);

        Dictionary<string, LovenseToyResponse> userToyResponse = user.Toys.Values.ToDictionary(toy => toy.Id,
            toy => new LovenseToyResponse
            {
                Id = toy.Id,
                Nickname = toy.Nickname,
                Name = toy.Name,
                Status = toy.Status
            })!;
        
        LovenseUserResponse response = new()
        {
            Uid = user.Uid,
            AppVersion = user.AppVersion,
            Toys = userToyResponse,
            WssPort = user.WssPort,
            HttpPort = user.HttpPort,
            WsPort = user.WsPort,
            AppType = user.AppType,
            Domain = user.Domain,
            UToken = user.UToken,
            HttpsPort = user.HttpsPort,
            Version = user.Version,
            Platform = user.Platform
        };

        return response;
    }
}