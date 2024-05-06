using DotNetHotChocolate.Model;
using HotChocolate.Data;
using MongoDB.Driver;

namespace DotNetHotChocolate.Query
{
  /// <summary>
  /// query for getting details
  /// </summary>
  public class InformationQuery
  {
    //previously the value were hardcoded but now we are getting the data from mongodb.

    //public SubjectInformation GetSubjectInformation() =>
    //	new SubjectInformation
    //	{
    //		Origin = "Machine Abstraction",
    //		Subject = "Collector",
    //		Content = new Data
    //		{
    //			Information = "This is the machine abstraction"
    //		},
    //		Type = " Publisher"
    //	};

    [UsePaging]
    [UseSorting]
    [UseFiltering]
    /// <summary>
    /// query whcih returns the data	
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public IExecutable<SubjectInformation> GetSubjectInformation([Service] IMongoCollection<SubjectInformation> collection)
    {
      return collection.AsExecutable();
    }

    /// <summary>
    /// query for process
    /// </summary>
    /// <returns></returns>
    public async Task<List<ProcessInformation>> GetProcessinformation()
    {
      var client = new MongoClient("mongodb://localhost:27017");
      var dataBase = client.GetDatabase("testdb");
      var collection = dataBase.GetCollection<ProcessInformation>("SystemProcesses");

     return collection.Find(x => true).ToList();
    }

    /// <summary>
    /// query for files
    /// </summary>
    /// <returns></returns>
    public async Task<List<FileInformation>> GetFileInformation()
    {
      var client = new MongoClient("mongodb://localhost:27017");
      var dataBase = client.GetDatabase("testdb");
      var collection = dataBase.GetCollection<FileInformation>("filecollection");

      return collection.Find(x => true).ToList();
    }

    public async Task<User> GetUserDetail(string username, string password)
    {
      var client = new MongoClient("mongodb://localhost:27017");
      var dataBase = client.GetDatabase("testdb");
      var collection = dataBase.GetCollection<User>("AllowedUser");

      var result = collection.Find(x => x.Username == username && x.Password == password).FirstOrDefault();

      if (result != null)
      {
        return result;
      }
      else
      {
        return new User { Username = "", Password = "" };
      }
    }
  }
}
