using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetTestSolution.Domain.Models
{
    public class ApiRequestResponseModel
    {
    }

    public class ApiResponseModel
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }

    public class APIResponseWithDataModel : ApiResponseModel
    {
        public dynamic Data { get; set; }
    }

    public class ApiResponseWithDataModel<T> : ApiResponseModel
    {
        public T Data { get; set; }
    }
    public class AuthenticateRequestModel
    {
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
    }

    public class LogoutRequestModel
    {
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string SessionID { get; set; }
    }

    public class AuthenticateResponseModel : ApiResponseModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }

    public class UserInfo
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string UserType { get; set; }
        public string SessionId { get; set; }
    }

    public class Tokens
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
    }

}
