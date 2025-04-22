using LoanApp.Data;
using MediatR;

namespace LoanApp.Features.Commands.Delete
{
    public class DeleteLoanProductCommandHandler : IRequestHandler<DeleteLoanProductCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteLoanProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteLoanProductCommand request, CancellationToken cancellationToken)
        {
            var loanProduct = await _context.LoanProducts.FindAsync(request.id, cancellationToken);
            if (loanProduct == null)
            {
                throw new Exception("Product not found");
            }
             
            loanProduct.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
