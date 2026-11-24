using MediatR;
using UserManager.DTOs;

namespace UserManager.Queries;

public record GetAllUsersQuery : IRequest<IEnumerable<UserReadDto>>;
