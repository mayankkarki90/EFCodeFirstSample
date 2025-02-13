namespace EFCodeFirstSample.Infrastructure
{
    public static class Extensions
    {
        public static IApplicationBuilder UseLogContextMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogContextMiddleware>();
            return app;
        }
    }
}
