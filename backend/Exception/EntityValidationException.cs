namespace backend.Exceptions;

public class EntityValidationException : Exception
{
    public List<string> Errors { get; set; }
    public EntityValidationException(List<string> errors) : base("Validation failed.")
    {
        Errors = errors;
    }
}