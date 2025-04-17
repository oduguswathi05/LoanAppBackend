using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.List.LoanApplications
{
    public record ListLoanApplicationsQuery:IRequest<List<LoanApplicationsDto>>;
   
}
