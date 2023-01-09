namespace CollegeKingsWebServer.Contracts.Responses;

public class LovenseQrCodeResponse
{
    public bool Result { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string> Data { get; set; }
}