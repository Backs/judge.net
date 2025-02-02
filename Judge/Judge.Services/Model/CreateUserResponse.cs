using Judge.Web.Client.Users;

namespace Judge.Services.Model;

public record CreateUserResponse(CreateUserResult Result, User? User); 
