using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.List.LoanProducts
{
    public record SuggestLoanProductQuery(LoanApplication Application):IRequest<List<LoanProduct>>;
    
}
