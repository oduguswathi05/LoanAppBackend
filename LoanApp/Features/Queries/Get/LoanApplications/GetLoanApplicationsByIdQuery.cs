using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.Get.LoanApplications
{
    public record GetLoanApplicationsByIdQuery(int id):IRequest<LoanApplication>;
   
}
