namespace Voyage.Models
{
    /// <summary>
    /// Indicates if the model can be soft deleted
    /// </summary>
    public interface ISoftDeleteable
    {
        bool Deleted { get; set; }
    }
}
