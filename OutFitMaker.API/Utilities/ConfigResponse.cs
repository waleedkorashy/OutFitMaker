namespace OutFitMaker.API.Utilities
{
    public static class ConfigResponse
    {
        public static IServiceCollection ConfigureApiBehaviorOptions(this IServiceCollection services)
        {
            services.AddMvcCore().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (errorContext) =>
                {
                    var errors = errorContext.ModelState.Values.SelectMany(e => e.Errors.Select(m => new
                    {
                        ErrorMessage = m.ErrorMessage
                    })).ToList();
                    var result = new
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Data = (object)null,
                        Message =  string.Join(" , ", errors.Select(e => e.ErrorMessage).ToList())
                    };
                    return new Microsoft.AspNetCore.Mvc.OkObjectResult(result);
                };
            });
            return services;
        }
    }
}
