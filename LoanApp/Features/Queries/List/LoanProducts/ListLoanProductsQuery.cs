using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.List.LoanProducts
{
    public record ListLoanProductsQuery:IRequest<List<LoanProduct>>;
  
}
