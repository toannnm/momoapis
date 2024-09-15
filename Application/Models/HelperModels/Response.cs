using System.Net;

namespace Application.Models.HelperModels
{
    public class Response<TEntity>
    {
        public HttpStatusCode Code { get; set; }
        public string? Errors { get; set; }
        public TEntity? Result { get; set; }

        public Response(TEntity? result = default) => (Code, Result) = (HttpStatusCode.OK, result);
        public Response(string? errors, int code) => (Errors, Code) = (errors, (HttpStatusCode)code);
    }
}
