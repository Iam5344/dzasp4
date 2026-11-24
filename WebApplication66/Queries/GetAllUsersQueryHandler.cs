using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManager.Data;
using UserManager.DTOs;

namespace UserManager.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserReadDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserReadDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<UserReadDto>>(users);
    }
}
