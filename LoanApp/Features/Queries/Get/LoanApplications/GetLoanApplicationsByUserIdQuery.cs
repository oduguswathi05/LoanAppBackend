using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.Get.LoanApplications
{
    public record GetLoanApplicationsByUserIdQuery(int userId):IRequest<List<LoanApplication>>;
   
}
