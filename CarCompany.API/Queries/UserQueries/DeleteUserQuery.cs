using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class DeleteUserQuery : IQuery<string>
    {
        public string? Email { get; set; }

        public DeleteUserQuery(string? email)
        {
            Email = email;
        }
    }
}
