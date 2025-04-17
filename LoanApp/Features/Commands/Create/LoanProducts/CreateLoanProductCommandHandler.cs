using LoanApp.Data;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Commands.Create.LoanProducts
{
    public class CreateLoanProductCommandHandler : IRequestHandler<CreateLoanProductCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateLoanProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;  
        }
        public async Task<int> Handle(CreateLoanProductCommand request, CancellationToken cancellationToken)
        {
            var loanProduct = request.LoanProductDto;
            var LoanProduct = new LoanProduct
            {
                ProductName = loanProduct.ProductName,
                InterestRate = loanProduct.InterestRate,
                MinLoanAmount = loanProduct.MinLoanAmount,
                MaxLoanAmount = loanProduct.MaxLoanAmount,
                MinLoanTerm = loanProduct.MinLoanTerm,
                MaxLoanTerm = loanProduct.MaxLoanTerm,
                MinCreditScore = loanProduct.MinCreditScore,
                MinAnnualIncome = loanProduct.MinAnnualIncome

            };

            await _context.AddAsync(LoanProduct, cancellationToken);
            await _context.SaveChangesAsync();
            return LoanProduct.Id;
        } 
    }
}
