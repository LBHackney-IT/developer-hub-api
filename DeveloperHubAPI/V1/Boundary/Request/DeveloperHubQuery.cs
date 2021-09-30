

namespace DeveloperHubAPI.V1.Domain 
{

    public class DeveloperHubQuery 
    {
        [FromRoute("id")]
        public string Id { get; set; }
    }
}