using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;

namespace Demo.API.ModelBinders
{
    public class CustomModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var queryString = bindingContext.ActionContext.HttpContext.Request.Query;

            // Extract values from the query string
            bool success = queryString.TryGetValue("employeeIds", out var value);

            if (success) {
                // Convert query string value into desired parameter type
                var stringArray = value.ToString().Split(',');
                int[] intArray = stringArray.Select(int.Parse).ToArray();
                // Set the model on the ModelBindingContext
                bindingContext.Result = ModelBindingResult.Success(intArray);
            }

            return Task.CompletedTask;
        }
    }
}
