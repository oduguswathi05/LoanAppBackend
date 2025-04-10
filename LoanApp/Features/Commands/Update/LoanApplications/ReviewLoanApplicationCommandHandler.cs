using LoanApp.Data;
using MediatR;

namespace LoanApp.Features.Commands.Update.LoanApplications
{
    public class ReviewLoanApplicationCommandHandler : IRequestHandler<ReviewLoanApplicationCommand>
    {
        private readonly ApplicationDbContext _context;

        public ReviewLoanApplicationCommandHandler(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task Handle(ReviewLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            var existLoanApplication = await _context.LoanApplications.FindAsync(request.Id);
            if(existLoanApplication == null)
            {
                throw new Exception("Applicaton not found");
            }
            if(existLoanApplication.LoanStatus == "Rejected" || existLoanApplication.LoanStatus == "Approved")
            {
                throw new Exception("Application already reviewed.");
            }
            //if (request.Decision.ToString() == "Rejected" && request.comment == null && request.comment == "")
            //{
            //    throw new Exception("Comment is required when rejecting the application.");
            //}
            existLoanApplication.LoanStatus = request.Decision.ToString();
            existLoanApplication.ReviewComment = request.comment;
            existLoanApplication.ReviewedDate = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
