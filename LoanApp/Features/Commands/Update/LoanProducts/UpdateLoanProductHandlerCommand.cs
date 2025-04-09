using LoanApp.Data;
using MediatR;

namespace LoanApp.Features.Commands.Update.LoanProducts
{
    public class UpdateLoanProductHandlerCommand : IRequestHandler<UpdateLoanProductCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdateLoanProductHandlerCommand(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateLoanProductCommand request, CancellationToken cancellationToken)
        {
            var upLoanProduct = request.dto;
            var existLoanProduct = await _context.LoanProducts.FindAsync(request.id, cancellationToken);
            if (existLoanProduct != null)
            {
                throw new Exception("Loan Product not exist");
            }
            if(upLoanProduct.ProductName != null && upLoanProduct.ProductName != "")
            {
                existLoanProduct.ProductName = upLoanProduct.ProductName;
            }
            if (upLoanProduct.InterestRate != null)
            {
                existLoanProduct.InterestRate = upLoanProduct.InterestRate.Value;
            }
            if (upLoanProduct.MinLoanAmount != null)
            {
                existLoanProduct.MinLoanAmount = upLoanProduct.MinLoanAmount.Value;
            }
            if (upLoanProduct.MaxLoanAmount != null)
            {
                existLoanProduct.MaxLoanAmount = upLoanProduct.MaxLoanAmount.Value;
            }
            if (upLoanProduct.MinLoanTerm != null)
            {
                existLoanProduct.MinLoanTerm = upLoanProduct.MinLoanTerm.Value;
            }
            if (upLoanProduct.MaxLoanTerm != null)
            {
                existLoanProduct.MaxLoanTerm = upLoanProduct.MaxLoanTerm.Value;
            }

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
