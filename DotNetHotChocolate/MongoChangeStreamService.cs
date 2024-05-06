using DotNetHotChocolate.Model;
using HotChocolate.Subscriptions;
using MongoDB.Driver;
using DotNetHotChocolate.Subscription;

namespace DotNetHotChocolate
{
  public class MongoChangeStreamService : BackgroundService
  {
    private readonly ITopicEventSender _eventSender;
    public MongoChangeStreamService([Service] ITopicEventSender sender)
    {
      _eventSender = sender;
    }

    /// <summary>
    /// method which keeps a watch on collection for any change
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task WatchTestingDatabase(CancellationToken cancellationToken)
    {
      IMongoClient client = new MongoClient("mongodb://localhost:27017");
      IMongoDatabase database = client.GetDatabase("Testing");
      var collection = database.GetCollection<SubjectInformation>("Watch");

      var pipeLine = new EmptyPipelineDefinition<ChangeStreamDocument<SubjectInformation>>();

      var options = new ChangeStreamOptions
      {
        FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
      };

      using var cursor = await collection.WatchAsync(pipeLine, options, cancellationToken);

      while (await cursor.MoveNextAsync(cancellationToken))
      {
        foreach (var change in cursor.Current)
        {
          if (change.OperationType == ChangeStreamOperationType.Insert)
          {
            if (change.FullDocument != null)
            {
              await _eventSender.SendAsync(nameof(Subscription.Subscription.OnPublished), change.FullDocument, cancellationToken);
            }
          }
        }
      }
    }

    /// <summary>
    /// changestream method to watch on systemProcess collection
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task WatchSystemProcessesCollection(CancellationToken cancellationToken)
    {
      IMongoClient client = new MongoClient("mongodb://localhost:27017");
      IMongoDatabase database = client.GetDatabase("testdb");
      var collection = database.GetCollection<ProcessInformation>("SystemProcesses");

      var pipeLine = new EmptyPipelineDefinition<ChangeStreamDocument<ProcessInformation>>();

      var options = new ChangeStreamOptions
      {
        FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
      };

      using var cursor = await collection.WatchAsync(pipeLine, options, cancellationToken);

      while (await cursor.MoveNextAsync(cancellationToken))
      {
        foreach (var change in cursor.Current)
        {
          if (change.OperationType == ChangeStreamOperationType.Insert)
          {
            if (change.FullDocument != null)
            {
              await _eventSender.SendAsync(nameof(Subscription.Subscription.OnProcessAcitivity), change.FullDocument, cancellationToken);
            }
          }
        }
      }
    }

    /// <summary>
    /// mongochangestream method to watch file collection
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task WatchFileCollection(CancellationToken cancellationToken)
    {

      IMongoClient client = new MongoClient("mongodb://localhost:27017");
      IMongoDatabase database = client.GetDatabase("testdb");
      var collection = database.GetCollection<FileInformation>("filecollection");

      var pipeLine = new EmptyPipelineDefinition<ChangeStreamDocument<FileInformation>>();

      var options = new ChangeStreamOptions
      {
        FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
      };

      using var cursor = await collection.WatchAsync(pipeLine, options, cancellationToken);

      while (await cursor.MoveNextAsync(cancellationToken))
      {
        foreach (var change in cursor.Current)
        {
          if (change.OperationType == ChangeStreamOperationType.Insert)
          {
            if (change.FullDocument != null)
            {
              await _eventSender.SendAsync(nameof(Subscription.Subscription.OnFileActivity), change.FullDocument, cancellationToken);
            }
          }
        }
      }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      var taskForSystemProcess =  WatchSystemProcessesCollection(stoppingToken);
      var taskForSubjectInformation = WatchTestingDatabase(stoppingToken);
      var taskForFileInofrmation = WatchFileCollection(stoppingToken);
      await Task.WhenAll(taskForSystemProcess, taskForSubjectInformation, taskForFileInofrmation);
    }
  }
}