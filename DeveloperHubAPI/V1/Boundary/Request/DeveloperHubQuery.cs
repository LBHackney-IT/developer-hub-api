

namespace DeveloperHubAPI.V1.Domain 
{

    public class DeveloperHubQuery 
    {
        [FromRoute("id")]
        public int Id { get; set; }
    }
}