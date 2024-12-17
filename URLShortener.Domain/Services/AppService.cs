using AutoMapper;
using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Data.Entity;
using URLShortener.Domain.Contracts.Interfaces;
using URLShortener.Domain.Contracts.Models.DomainModels;
using URLShortener.Domain.Contracts.Models.RequestModels;
using URLShortener.Domain.Exceptions;

namespace URLShortener.Domain.Services;

public class AppService : IAppService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private const string UniqueEmailError = "Email should be unique";
    private const string UserCouldNotBeFoundError = "User could not be found";

    public AppService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserModel> GetUserByIdAsync(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            throw new NotFoundException(UserCouldNotBeFoundError);
        }

        return _mapper.Map<User, UserModel>(user);
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        var mappedUsers = _mapper.Map<List<User>, List<UserModel>>(users);

        return mappedUsers;
    }

    public async Task CreateUserAsync(CreateUserRequestModel userModel)
    {
        var user = await _context.Users.Where(x => x.Email == userModel.Email).ToListAsync();
        if (user.Count != 0)
        {
            throw new DuplicateEmailException(UniqueEmailError);
        }

        _context.Users.Add(_mapper.Map<CreateUserRequestModel, User>(userModel));
        await _context.SaveChangesAsync();
    }
}