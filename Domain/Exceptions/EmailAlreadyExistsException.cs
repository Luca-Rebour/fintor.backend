namespace Fintor.api.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
            : base($"A user is already registered with the email '{email}'.")
        {
        }
    }

}
