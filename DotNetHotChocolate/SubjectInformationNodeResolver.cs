using DotNetHotChocolate.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DotNetHotChocolate
{
  /// <summary>
  /// Resolver class for interaction with Db
  /// </summary>
  public class SubjectInformationNodeResolver
  {
    /// <summary>
    /// resolver method which gets the data from database
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<SubjectInformation> ResolveAsync([Service] IMongoCollection<SubjectInformation> collection, ObjectId id)
    {
      return (Task<SubjectInformation>)collection.Find(x => true);
    }
  }
}
