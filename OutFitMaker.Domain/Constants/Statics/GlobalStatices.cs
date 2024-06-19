using OutFitMaker.Domain.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Constants.Statics
{
    public static class GlobalStatices
    {
        #region Connection String
        public const string OutFitMakerConnectionString = "OutFitMakerConnection";
        #endregion

        #region JWT
        public const string CorsPolicy = "CorsPolicy";
        public static readonly string JWTKey = "safd3adflnasklsdvsdvsvnfen564fmg54ngg4k5lnbtbkmn5hk6lnhk5n6nhkn6jhn5sdfghloiyrbekqnhhdaqbvf2344";
        #endregion

        #region Shared
        public const string CannotBeFound = " CannotBeFound ";
        public const string OrderNotFound = " OrderNotFound ";
        public const string ProductUnAvailable = " Product UnAvailable ";
        public const string IsExist = " UserIsExist";
        public const string Success = " Success ";
        public const string Fail = " Fail "; 
        public const string UserNotFound = " UserNotFound ";
        public const string InvalidEmailOrPassword = " InvalidEmailOrPassword ";
        public const string InvalidVerificationCode = " InvalidVerificationCode ";
        public const string PleaseConfirmPasswordCodeFirst = " PleaseConfirmPasswordCodeFirst ";
        public const string VerificationCodeTimeOutPleaseResendCode = " VerificationCodeTimeOutPleaseResendCode ";
        #endregion

        #region Mail
        public static readonly string EmailMessageSubject = "Account Verification";
        public static readonly string DefaultOurEmail = "xyzmamdoh@gmail.com";
        public static readonly string EmailMessageBodyTemplate = "Welcome to OutFitMaker " +
                                  "We're excited to have you join our community.<br/><br/>" +
                                  "To complete your registration, please verify your email by entering the verification code below on the verification page:<br/><br/>" +
                                  "<a style='font-size:26px;'>{0}</a><br/><br/>" +
                                  "If you didn't request this verification code or believe it's a mistake, you can ignore this email. " +
                                  "Your account's security is important to us.<br/><br/>" +
                                  "Thank you for choosing OutFitMaker. We look forward to helping you get the most out of our app.<br/><br/>" +
                                  "Best regards,<br/>The OutFitMaker Team";
        public static readonly string PasswordMessageSubject = "Reset Your Password - Verification OTP Request";

        public static readonly string PasswordMessageBody = "We have received a request to reset the password for your account with OutFitMaker." +
                                     " To ensure the security of your account, we need to verify your identity using a one-time verification code (OTP) " +
                                    "Please use the following OTP to verify your identity and reset your password:.<br/><br/>" +
                                    "Verification OTP:<br/><br/>" +
                                    "<a style='font-size:26px;'>{0}</a><br/><br/>" +
                                    "Please make sure to enter this code within the next 15 minutes on the password reset page. If you did not request this password reset, please ignore this email. Your account's security is our top priority, and this email is to confirm that you initiated the password reset process. " +
                                    "If you have any questions or need further assistance, please do not hesitate to contact our support team ..<br/><br/>" +
                                    "Thank you for choosing OutFitMaker. We look forward to helping you get the most out of our app.<br/><br/>" +
                                    "Best regards,<br/>The OutFitMaker Team";
        #endregion

        #region Main
        public const string SizeNotFount = " This Size Not Found ";
        #endregion


        #region Roles
        public const string Admin = nameof(RolesEnum.Admin);
        public const string Customer = nameof(RolesEnum.Customer);
        #endregion
    }
}
