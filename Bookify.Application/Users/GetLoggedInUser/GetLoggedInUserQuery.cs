using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.GetLoggedInUser;

namespace Bookify.Application.Users.LoggedInUser;

public record GetLoggedInUserQuery :  IQuery<UserResponse>;