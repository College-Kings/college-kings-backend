namespace CollegeKingsBackend.Domain.Entities.Lovense;

public class LovenseUser : ILovenseUser
{
    public required string Uid { get; set; }
    public required string AppVersion { get; set; }
    public required Dictionary<string, LovenseToy>? Toys { get; set; }
    public required int WssPort { get; set; }
    public required int HttpPort { get; set; }
    public required int WsPort { get; set; }
    public required string AppType { get; set; }
    public required string Domain { get; set; }
    public required string UToken { get; set; }
    public required int HttpsPort { get; set; }
    public required int Version { get; set; }
    public required string Platform { get; set; }
}