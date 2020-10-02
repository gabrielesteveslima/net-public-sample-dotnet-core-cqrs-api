namespace SampleProject.API.SeedWork
{
    using Domain.SeedWork;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class BusinessRuleValidationExceptionProblemDetails : ProblemDetails
    {
        public BusinessRuleValidationExceptionProblemDetails(BusinessRuleValidationException exception)
        {
            Title = "Business rule validation error";
            Status = StatusCodes.Status409Conflict;
            Detail = exception.Details;
            Type = "https://somedomain/business-rule-validation-error";
        }
    }
}