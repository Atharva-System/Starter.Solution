using System.Net;

namespace Starter.Blazor.Core.Constants;

public static class HttpStatusCodes
{
    //200
    public static int OK => (int)HttpStatusCode.OK;

    //201
    public static int Created => (int)HttpStatusCode.Created;

    //202
    public static int Accepted => (int)HttpStatusCode.Accepted;

    //226
    public static int IMUsed => (int)HttpStatusCode.IMUsed;

    //400
    public static int BadRequest => (int)HttpStatusCode.BadRequest;

    //401
    public static int Unauthorized => (int)HttpStatusCode.Unauthorized;

    //403
    public static int Forbidden => (int)HttpStatusCode.Forbidden;

    //404
    public static int NotFound => (int)HttpStatusCode.NotFound;

    //409
    public static int Conflict => (int)HttpStatusCode.Conflict;

    //500
    public static int InternalServerError => (int)HttpStatusCode.InternalServerError;
}
