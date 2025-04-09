using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Commands.Update.LoanApplications
{
    public record ReviewLoanApplicationCommand(int Id,ReviewDecision Decision,string? comment) :IRequest;
  
}
