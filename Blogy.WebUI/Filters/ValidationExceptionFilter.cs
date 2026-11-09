using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Blogy.WebUI.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{

    private readonly IModelMetadataProvider _modelMetadataProvider;

    public ValidationExceptionFilter(IModelMetadataProvider modelMetadataProvider)
    {
        _modelMetadataProvider = modelMetadataProvider;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ValidationException validationException)
        {
            return;
        }

        foreach (var error in validationException.Errors)
        {
            context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        var actionName = context.RouteData.Values["action"]?.ToString() ?? "Index";

        var submittedModel = context.ActionDescriptor.Parameters
            .Select(p => context.HttpContext.Request.Form.ContainsKey(p.Name) ? p : null)
            .FirstOrDefault();

        var viewResult = new ViewResult
        {
            ViewName = actionName,
            ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
        };

        context.Result = viewResult;
        context.ExceptionHandled = true;
    }
}
