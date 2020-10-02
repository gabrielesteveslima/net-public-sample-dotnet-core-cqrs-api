namespace SampleProject.API.SeedWork
{
    using Application.Configuration.Validation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class InvalidCommandProblemDetails : ProblemDetails
    {
        public InvalidCommandProblemDetails(InvalidCommandException exception)
        {
            Title = exception.Message;
            Status = StatusCodes.Status400BadRequest;
            Detail = exception.Details;
            Type = "https://somedomain/validation-error";
        }
    }
}