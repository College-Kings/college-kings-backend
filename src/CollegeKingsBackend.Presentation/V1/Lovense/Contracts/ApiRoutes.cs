namespace CollegeKingsBackend.Presentation.V1.Lovense.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = $"{Root}/{Version}";

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
    
    public static class Posts
    {
        public const string GetAll = $"{Base}/posts";
        public const string Create = $"{Base}/posts";
        public const string Get = $"{Base}posts/{{postId}}";
    }

}