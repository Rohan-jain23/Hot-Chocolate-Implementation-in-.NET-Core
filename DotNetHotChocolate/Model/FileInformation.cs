using MongoDB.Bson;

namespace DotNetHotChocolate.Model
{
  public class FileInformation
  {
    public ObjectId Id { get; set; }
    public string? FileName { get; set; }
    public DateTime? CreationDate { get; set; }
    public double? FileSize { get; set; }
    public DateTime? LastModified { get; set; }
  }
}
