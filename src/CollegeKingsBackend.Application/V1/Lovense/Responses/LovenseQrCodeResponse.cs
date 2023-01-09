namespace CollegeKingsBackend.Application.V1.Lovense.Responses;

public class LovenseQrCodeResponse
{
    public required bool Result { get; set; }
    public required int Code { get; set; }
    public required string Message { get; set; }
    public required Dictionary<string, string> Data { get; set; }
}