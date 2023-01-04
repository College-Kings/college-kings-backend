namespace CollegeKingsWebServer.Contracts.V1;

public static class ApiRoutes
{
    public const string Root = "api";
    public const string Version = "v1";
    public const string Base = $"{Root}/{Version}";

    public static class LovenseUsers
    {
        public const string GetAll = $"{Base}/lovense/users";
        public const string Get = $"{Base}/lovense/users/{{userId}}";
        public const string Create = $"{Base}/lovense/users";
        
    }

    public static class LovenseQrCode
    {
        public const string Create = $"{Base}/lovense/qrCode";
    }
}