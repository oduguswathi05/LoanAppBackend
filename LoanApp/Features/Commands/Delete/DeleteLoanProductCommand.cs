using MediatR;

namespace LoanApp.Features.Commands.Delete
{
    public record DeleteLoanProductCommand(int id):IRequest;
    
}
