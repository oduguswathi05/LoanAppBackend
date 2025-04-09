using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Commands.Create.Users
{
    public class CreateLoanOfficerCommandHandler : IRequestHandler<CreateLoanOfficerCommand, int?>
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public CreateLoanOfficerCommandHandler(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }
        public async Task<int?> Handle(CreateLoanOfficerCommand request, CancellationToken cancellationToken)
        {
            var loanOfficer = request.loanOfficer;
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loanOfficer.Email);
            if (existingUser != null)
            {
                return null;
            }
            var hashedPassword = _passwordHasher.HashPassword(null, loanOfficer.PasswordHash);

            var newLoanOfficer = new User
            {
                FirstName = loanOfficer.FirstName,
                LastName = loanOfficer.LastName,
                Email = loanOfficer.Email,
                PasswordHash = hashedPassword,
                PhoneNumber = loanOfficer.PhoneNumber,
                Role = "LoanOfficer"
            };



            await _context.Users.AddAsync(newLoanOfficer, cancellationToken);
            await _context.SaveChangesAsync();

            return newLoanOfficer.Id;
        }
    }
}
