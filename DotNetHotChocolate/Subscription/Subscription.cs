using DotNetHotChocolate.Model;

namespace DotNetHotChocolate.Subscription
{
  public class Subscription
  {
    /// <summary>
    /// subsription method or subscription resolver
    /// </summary>
    /// <param name="subjectInformation"></param>
    /// <returns></returns>
    [Subscribe]
    [Topic]
    public SubjectInformation OnPublished([EventMessage] SubjectInformation subjectInformation)
      => subjectInformation;

    /// <summary>
    /// subsriptionmethod/resolver for systemprocess
    /// </summary>
    /// <param name="processInformation"></param>
    /// <returns></returns>
    [Subscribe]
    [Topic]
    public ProcessInformation OnProcessAcitivity([EventMessage] ProcessInformation processInformation)
      => processInformation;

    /// <summary>
    /// subscription method/resolver for file
    /// </summary>
    /// <param name="fileInformation"></param>
    /// <returns></returns>
    [Subscribe]
    [Topic]
    public FileInformation OnFileActivity([EventMessage] FileInformation fileInformation)
      => fileInformation;
  }
}
