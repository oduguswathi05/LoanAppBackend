using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LoanApp.Features.Commands.Create.Users
{
    public class CreateCutomerCommandHandler : IRequestHandler<CreateCustomerCommand, int?>
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public CreateCutomerCommandHandler(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }
        public async Task<int?> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = request.customer;
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == customer.Email);
            if (existingUser != null)
            {
                return null;
            }
            var hashedPassword = _passwordHasher.HashPassword(null, customer.PasswordHash);

            var newCustomer = new User
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PasswordHash = hashedPassword,
                PhoneNumber = customer.PhoneNumber,
            };
           


            await _context.Users.AddAsync(newCustomer, cancellationToken);
            await _context.SaveChangesAsync();

            return newCustomer.Id;
        }
    }
}
