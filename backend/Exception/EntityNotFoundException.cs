namespace backend.Exceptions;

public class EntityNotFoundException : Exception
{
    public List<string> Errors { get; set; }
    public EntityNotFoundException(List<string> errors) : base("Entity not found.")
    {
        Errors = errors;
    }
}