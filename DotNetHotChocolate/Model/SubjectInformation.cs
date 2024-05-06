using MongoDB.Bson;

namespace DotNetHotChocolate.Model
{
  [Node(
    IdField = nameof(Id),
    NodeResolverType = typeof(SubjectInformationNodeResolver),
    NodeResolver = nameof(SubjectInformationNodeResolver.ResolveAsync))]
  /// <summary>
  /// model for the query
  /// </summary>
  public class SubjectInformation
  {
    public ObjectId Id { get; set; }
    public string origin { get; set; }
    public string subject { get; set; }
    public string content { get; set; }
    public string type { get; set; }
  }
}
