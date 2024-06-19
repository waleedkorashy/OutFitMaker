using Microsoft.AspNetCore.Http;
using OutFitMaker.Domain.Constants.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Helper
{
    public static class Responder
    {
        /// <summary>
        /// This will return success response with 200 status code
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Data">Object Data</param>
        /// <returns></returns>
        public static APIResponse<object> Success(string msg = null, object Data = null)
        {
            return new APIResponse<object>
            {
                Status = StatusCodes.Status200OK,
                Message = GlobalStatices.Success + msg,
                Data = Data
            };
        }

        /// <summary>
        /// This will return fail response with 400 status code
        /// </summary>
        /// <param name="msg">Failed : {msg}</param>
        /// <returns>400 , Data =NUll</returns>
        public static APIResponse<object> Fail(string msg)
        {
            return new APIResponse<object>
            {
                Status = StatusCodes.Status400BadRequest,
                Message = GlobalStatices.Fail + msg,
                Data = null
            };
        }
        /// <summary>
        /// This will return fail response with 500 status code
        /// </summary>
        /// <param name="msg">Failed : {msg}</param>
        /// <returns>500 , Data =NUll</returns>
        public static APIResponse<object> Exception(Exception msg, object Data)
        {
            return new APIResponse<object>
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = GlobalStatices.Fail + msg.Message,
                Data = Data
            };
        }
    }
}
