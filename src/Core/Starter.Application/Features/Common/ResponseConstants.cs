using System.Net;

namespace Starter.Application.Features.Common;
public static class ConstantMessages
{
    public static string Success => "Success";
    public static string AddedSuccesfully => " Added Successfully";
    public static string UpdatedSuccessfully => " Updated Successfully";
    public static string DeletedSuccessfully => " Deleted Successfully";
    public static string FailedToUpdate => "Failed to Updated";
    public static string FailedToCreate => "Failed to create ";
    public static string DataCreated => "Data Created";
    public static string DataFound => "Data found";
    public static string NoDataFound => "No Data found";
    public static string NotFound => "{0} Not Found";

    // Localized
    public static string Commission => "Commission";
    public static string Quarter => "Quarter";
    public static string Paid => "Paid";
    public static string Completed => "Completed";
    public static string TestReport => "TestReport";
}

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
