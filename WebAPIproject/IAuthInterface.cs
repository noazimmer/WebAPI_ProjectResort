namespace WebAPIproject
{
    public interface IAuthInterface
    {
        public (string email, string password) DecodeJwtToken(string token);
    }
}
