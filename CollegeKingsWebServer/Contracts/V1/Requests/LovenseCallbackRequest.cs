namespace CollegeKingsWebServer.Contracts.V1.Requests;

public class LovenseCallbackRequest
{
    public string? Uid { get; set; }
    public string? AppVersion { get; set; }
    public Dictionary<string, LovenseToyRequest>? Toys { get; set; }
    public int WssPort { get; set; }
    public int HttpPort { get; set; }
    public int WsPort { get; set; }
    public string? AppType { get; set; }
    public string? Domain { get; set; }
    public string? UToken { get; set; }
    public int HttpsPort { get; set; }
    public int Version { get; set; }
    public string? Platform { get; set; }
}
