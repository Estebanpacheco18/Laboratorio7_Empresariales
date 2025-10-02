using Newtonsoft.Json;

namespace Lab07_EstebanPacheco.Middleware
{
    public class ParameterValidationMiddleware
    {
        private readonly RequestDelegate _next;
        public ParameterValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                var model = await DeserializeRequestBody(context);
                if (model == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Cuerpo de la solicitud inválido o vacío.");
                    return;
                }
                var validationErrors = ValidateModel(model);
                if (validationErrors.Any())
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(string.Join("\n", validationErrors));
                    return;
                }
            }
            await _next(context);
        }
        private async Task<object> DeserializeRequestBody(HttpContext context)
        {
            var contentType = context.Request.ContentType;
            if (contentType != null && contentType.Contains("application/json"))
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                // Validar solo para el endpoint de productos
                if (context.Request.Path.StartsWithSegments("/api/products") && context.Request.Method == HttpMethods.Post)
                {
                    var model = JsonConvert.DeserializeObject<Lab07_EstebanPacheco.DTO.CreateProductDto>(body);
                    return model;
                }
            }
            return null;
        }


        private List<string> ValidateModel(object model)
        {
            var errors = new List<string>();
            var properties = model.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(model);
                if (value == null)
                {
                    errors.Add($"El parámetro '{property.Name}' es obligatorio.");
                }
            }
            return errors;
        }
    }
}
