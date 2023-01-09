using CollegeKings.Domain.Entities;

namespace CollegeKings.Application.Posts;

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