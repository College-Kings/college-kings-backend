using CollegeKings.Domain.Entities;

namespace CollegeKingsBackend.Application.V1.Posts;

public static class PostsQueries
{
    public static List<Post> GetPosts()
    {
        List<Post> posts = new();

        for (int i = 0; i < 5; i++)
        {
            posts.Add(new Post{Id = Guid.NewGuid().ToString()});
        }
        
        return posts;
    }
}