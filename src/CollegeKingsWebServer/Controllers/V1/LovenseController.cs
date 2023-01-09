using CollegeKingsWebServer.Contracts.Responses;
using CollegeKingsWebServer.Contracts.V1;
using CollegeKingsWebServer.Contracts.V1.Requests;
using CollegeKingsWebServer.Domain;
using CollegeKingsWebServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CollegeKingsWebServer.Controllers.V1;

public class LovenseController : Controller
{
    private readonly ILovenseService _lovenseService;

    public LovenseController(ILovenseService lovenseService)
    {
        _lovenseService = lovenseService;
    }

    [HttpGet(ApiRoutes.LovenseUsers.Get)]
    public async Task<IActionResult> Get([FromRoute] string userId)
    {
        LovenseUser? user = _lovenseService.GetUserById(userId);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    
    [HttpPost(ApiRoutes.LovenseQrCode.Create)]
    public async Task<IActionResult> Create([FromBody] LovenseQrCodeRequest qrCodeRequest)
    {
        if (qrCodeRequest.Uid is null || qrCodeRequest.Uname is null)
        {
            return BadRequest();
        }
        
        LovenseQrCodeResponse? response = await _lovenseService.CreateQrCode(qrCodeRequest.Uid, qrCodeRequest.Uname);
        
        return Ok(response);
    }
    
    [HttpPost(ApiRoutes.LovenseUsers.Create)]
    public async Task<IActionResult> Create([FromBody] LovenseCallbackRequest callbackRequest)
    {
        if (callbackRequest.Uid is null || callbackRequest.AppVersion is null || callbackRequest.AppType is null ||
            callbackRequest.Domain is null || callbackRequest.UToken is null || callbackRequest.Platform is null ||
            callbackRequest.Toys is null)
        {
            return BadRequest();
        }
        
        Dictionary<string, LovenseToy> userToys = callbackRequest.Toys.Values.ToDictionary(toy => toy.Id,
            toy => new LovenseToy
            {
                Id = toy.Id,
                Nickname = toy.Nickname,
                Name = toy.Name,
                Status = toy.Status
            });

        LovenseUser user = new()
        {
            Uid = callbackRequest.Uid,
            AppVersion = callbackRequest.AppVersion,
            Toys = userToys,
            WssPort = callbackRequest.WssPort,
            HttpPort = callbackRequest.HttpPort,
            WsPort = callbackRequest.WsPort,
            AppType = callbackRequest.AppType,
            Domain = callbackRequest.Domain,
            UToken = callbackRequest.UToken,
            HttpsPort = callbackRequest.HttpsPort,
            Version = callbackRequest.Version,
            Platform = callbackRequest.Platform
        };

        _lovenseService.CreateUser(user.Uid, user);

        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        string locationUrl = $"{baseUrl}/{ApiRoutes.LovenseUsers.Get.Replace("{userId}", user.Uid)}";
        
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
        
        return Created(locationUrl, response);
    }
}