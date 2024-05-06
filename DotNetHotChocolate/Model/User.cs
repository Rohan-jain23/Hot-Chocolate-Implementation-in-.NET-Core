using MongoDB.Bson;

namespace DotNetHotChocolate.Model
{
  public class User
  {
    public ObjectId Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}
