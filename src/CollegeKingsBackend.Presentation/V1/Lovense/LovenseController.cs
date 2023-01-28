using CollegeKingsBackend.Application.V1.Lovense.Queries;
using CollegeKingsBackend.Application.V1.Lovense.Requests;
using CollegeKingsBackend.Application.V1.Lovense.Responses;
using CollegeKingsBackend.Domain.Entities.Lovense;
using Microsoft.AspNetCore.Mvc;

namespace CollegeKingsBackend.Presentation.V1.Lovense;

public class LovenseController : Controller
{
    private readonly LovenseQueries _lovenseQueries;

    public LovenseController(LovenseQueries lovenseQueries)
    {
        _lovenseQueries = lovenseQueries;
    }

    [HttpGet(ApiRoutes.LovenseUsers.Get)]
    public IActionResult Get([FromRoute] string userId)
    {
        ILovenseUser? user = _lovenseQueries.GetUserById(userId);
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
        
        LovenseQrCodeResponse? response = await _lovenseQueries.CreateQrCode(qrCodeRequest.Uid, qrCodeRequest.Uname);
        
        return Ok(response);
    }
    
    [HttpPost(ApiRoutes.LovenseUsers.Create)]
    public IActionResult Create([FromBody] LovenseCallbackRequest callbackRequest)
    {
        if (callbackRequest.Uid is null || callbackRequest.AppVersion is null || callbackRequest.AppType is null ||
            callbackRequest.Domain is null || callbackRequest.UToken is null || callbackRequest.Platform is null ||
            callbackRequest.Toys is null)
        {
            return BadRequest();
        }

        Dictionary<string, LovenseToy> userToys = new();
        foreach ((string toyId, LovenseToyRequest toy) in callbackRequest.Toys)
        {
            if (toy.Name is null || toy.Nickname is null) return BadRequest();

            userToys.Add(toyId, new LovenseToy
            {
                Id = toyId,
                Name = toy.Name,
                Status = toy.Status,
                Nickname = toy.Nickname
            });
        }
        
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

        LovenseUserResponse response = _lovenseQueries.CreateUser(user);

        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        string locationUrl = $"{baseUrl}/{ApiRoutes.LovenseUsers.Get.Replace("{userId}", user.Uid)}";
        
        return Created(locationUrl, response);
    }
}