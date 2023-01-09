namespace CollegeKingsBackend.Domain.Entities.Lovense;

public interface ILovenseUser
{
    string Uid { get; set; }
    string AppVersion { get; set; }
    Dictionary<string, LovenseToy> Toys { get; set; }
    int WssPort { get; set; }
    int HttpPort { get; set; }
    int WsPort { get; set; }
    string AppType { get; set; }
    string Domain { get; set; }
    string UToken { get; set; }
    int HttpsPort { get; set; }
    int Version { get; set; }
    string Platform { get; set; }
}