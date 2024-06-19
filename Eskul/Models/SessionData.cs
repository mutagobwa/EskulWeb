using Eskul.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace SmartPaperEdms.Web.App_Code
{
    public static class SessionData
    {
        public static bool IsSignedIn
        {
            get
            {
                string json = SessionHelper.GetUser() ?? "{}";
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(json);
                return sd.IsSignedIn;
            }
        }
        public static string UserId
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.UserId;
            }
        }
        public static string LicenceCode
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.Licence;
            }
        }
        public static string ClientCode
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.ClientCode;
            }
        }
        public static string ProductCode
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.ProductCode;
            }
        }
        public static string Username
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.Username;
            }
        }
        public static string SchoolLogo
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.SchoolLogo;
            }
        }

        public static string FullNames
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.FullNames;
            }
        }

        public static string ProfileId
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.ProfileId;
            }
        }
        public static int Term
        {
            get
            {
                int term;
                switch (((JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail()).Term ?? "Term One").ToLower())
                {
                    case "term one":
                        term = 1;
                        break;
                    case "term two":
                        term = 2;
                        break;
                    case "term three":
                        term = 3;
                        break;
                    default:
                        term = 0;
                        break;
                }
                return term;
            }
        }
        public static string ProfileName
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.ProfileName;
            }
        }
        public static string UserProfileCode
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.UserProfileCode;
            }
        }
        public static int UserBranchId
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.BranchId;
            }
        }
        public static string UserBranchCode
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.UserBranchCode;
            }
        }
        public static string ActiveTerm
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.Term;
            }
        }

        public static string UserBranchName
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.UserBranchName;
            }
        }

        public static DateTime LcyDate
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.LcyDate;
            }
        }

        public static string UserIPAddress
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.UserIPAddress;
            }
        }

        public static string UserWorkStation
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.UserWorkStation;
            }
        }

        public static string WindowsUser
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.WindowsUser;
            }
        }
        
        public static bool CanAdd
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.CanAdd;
            }

        }
        public static bool IsAdmin
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.IsAdmin;
            }

        }
        public static bool CanUpdate
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.CanUpdate;
            }

        }
        public static bool CanDelete
        {
            get
            {
                SessionDetail sd = JsonConvert.DeserializeObject<SessionDetail>(SessionHelper.GetUser()) ?? new SessionDetail();
                return sd.CanDelete;
            }

        }

    }
}
