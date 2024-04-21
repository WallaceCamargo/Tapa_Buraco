using Tapa_Buraco.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Tapa_Buraco.WebSite.Controllers
{
    public class BaseController : Controller
    {
        public DTO.UsuarioAutenticacao UsuarioAutenticacao
        {
            get
            {
                return Helper.GetObjectFromSession<DTO.UsuarioAutenticacao>("UsuarioAutenticacao");
            }
            set
            {
                Helper.SetObjectToSession("UsuarioAutenticacao", value);
            }
        }

        public WebApiAutenticacao webApi
        {
            get
            {
                return Helper.GetObjectFromSession<WebApiAutenticacao>("webApi");
            }
            set
            {
                Helper.SetObjectToSession("webApi", value);
            }
        }

        public bool siteIframeWithoutMenu
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                {
                    return System.Web.HttpContext.Current.Session["siteIframeWithoutMenu"] == null ? false : Convert.ToBoolean(System.Web.HttpContext.Current.Session["siteIframeWithoutMenu"]);
                }

                return false;
            }
            set
            {
                if (System.Web.HttpContext.Current != null)
                {
                    System.Web.HttpContext.Current.Session["siteIframeWithoutMenu"] = value;
                }
            }
        }

        #region Constants
        private string _COOKIE_LASTHOST = string.Format("mf_lasthost_{0}", "RAP");
        private string _COOKIE_LASTCHECK = string.Format("mf_lastcheckdate_{0}", "RAP");
        #endregion

        #region Overrides
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            SetLastHost(requestContext.HttpContext);
            base.Initialize(requestContext);
        }
        #endregion

        #region User
        public bool isLogged
        {
            get { return UsuarioAutenticacao != null; }
        }
        #endregion

        #region User Session Cookie
        public void CreateUserSessionNumber()
        {
            if (isLogged)
            {
                UsuarioAutenticacao.Session = string.Format("{0}{1}{2}{3}{4}", Guid.NewGuid().ToString().Replace("-", ""),
                       Guid.NewGuid().ToString().Replace("-", ""), Guid.NewGuid().ToString().Replace("-", ""),
                       Guid.NewGuid().ToString().Replace("-", ""), Guid.NewGuid().ToString().Replace("-", ""));
                UsuarioAutenticacao = UsuarioAutenticacao;
            }
        }
        public void SetUserSessionCookie()
        {
            string cookiekey = string.Format("01UKBF7XLBB3BHAZ87YFI4W1SSOJ3MN5");
            string idtoken = UsuarioAutenticacao != null ? UsuarioAutenticacao.Session : string.Empty;
            if (!string.IsNullOrWhiteSpace(idtoken))
            {
                Helper.AddCookie(cookiekey, idtoken);
            }
        }
        public string GetUserSessionCookie()
        {
            string ret = string.Empty;
            string cookiekey = string.Format("01UKBF7XLBB3BHAZ87YFI4W1SSOJ3MN5");
            ret = Helper.RequestCookie<string>(cookiekey);
            return ret;
        }
        public void RemoveUserSessionCookie()
        {
            string cookiekey = string.Format("01UKBF7XLBB3BHAZ87YFI4W1SSOJ3MN5");
            Helper.RemoveCookie(cookiekey);
        }
        #endregion

        #region Last Host
        private void SetLastHost(System.Web.HttpContextBase httpContext)
        {
            string host = string.Empty;
            if (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null)
            {
                host = System.Web.HttpContext.Current.Request.Url.Authority;
            }
            string save = string.Format("{0}", host);
            Helper.AddCookie(_COOKIE_LASTHOST, value: save, httpContext: httpContext);
        }
        #endregion

        #region Check If Must Check Session
        public bool MustCheckSession()
        {
            bool ret = true;
            try
            {
                string lastTimeString = Helper.RequestCookie<string>(_COOKIE_LASTCHECK);
                if (lastTimeString != null)
                {
                    string host = string.Empty;
                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null && System.Web.HttpContext.Current.Request.Url != null)
                    {
                        host = System.Web.HttpContext.Current.Request.Url.Authority;
                    }
                    var hostCookie = Helper.RequestCookie<string>(_COOKIE_LASTHOST);
                    if (hostCookie == null) { hostCookie = string.Empty; }
                    if (host.ToLower() == hostCookie.ToLower())
                    {
                        long lastTime = 0;
                        if (long.TryParse(lastTimeString, out lastTime))
                        {
                            DateTime newDate = new DateTime(lastTime);
                            ret = newDate <= DateTime.Now;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return ret;
        }
        public void RemoveCheckSessionCookie()
        {
            Helper.RemoveCookie(_COOKIE_LASTCHECK);
        }
        public void SetCheckSessionCookie()
        {
            string lastTimeString = Helper.RequestCookie<string>(_COOKIE_LASTCHECK);
            if (lastTimeString != null)
            {
                RemoveCheckSessionCookie();
            }
            var dateAux = DateTime.Now.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["MINUTES_SESSION_TIMEOUT"]?.ToString() ?? "600")); //600 = 10horas
            string save = string.Format("{0}", dateAux.Ticks);
            Helper.AddCookie(_COOKIE_LASTCHECK, value: save);
        }
        #endregion

        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            if (Request.QueryString["donothing"] == "true")
            {
                base.OnAuthentication(filterContext);
                return;
            }

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                #region Validate Session
                var keyAux = isLogged ? UsuarioAutenticacao.Session : GetUserSessionCookie();
                if (!string.IsNullOrWhiteSpace(keyAux))
                {
                    if (MustCheckSession())
                    {
                        UsuarioAutenticacao = null;

                        //UserAuthenticate = new Services.ServiceClient().GetUserSession(keyAux);
                        if (!isLogged)
                        {
                            RemoveCheckSessionCookie();
                            RemoveUserSessionCookie();
                        }
                    }
                }
                #endregion

                #region Login Process init
                if (string.Format("/{0}/{1}", ((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName, filterContext.ActionDescriptor.ActionName) != ConfigurationManager.AppSettings["MF::Auth:URLHandleUnauthorized"])
                {
                    if (!isLogged && string.Format("/{0}/{1}", ((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName, filterContext.ActionDescriptor.ActionName) != ConfigurationManager.AppSettings["Auth::LoginURL"])
                    {
                        filterContext.Result = Redirect(ConfigurationManager.AppSettings["LOGIN_URL"]);                        
                    }

                    if (isLogged && string.Format("/{0}/{1}", ((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName, filterContext.ActionDescriptor.ActionName) == ConfigurationManager.AppSettings["Auth::LoginURL"])
                    {
                        UsuarioAutenticacao = null;
                        filterContext.Result = Redirect(ConfigurationManager.AppSettings["LOGIN_URL"]);                        
                    }
                    return;

                    //if (isLogged)
                    //{
                    //    if (UsuarioAutenticacao.Permission != null && UserAuthenticate.Permission.Count() > 0)
                    //    {
                    //        var listDomain = UserAuthenticate.Domain.Split(',').ToList();

                    //        var permission = UserAuthenticate.Permission.Where(f => f.Action == filterContext.ActionDescriptor.ActionName && f.Controller == ((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName && f.System == Shared.Core.AppConfig.Constants.WebApi.Authenticate.SystemId && listDomain.Contains(f.Domain)).FirstOrDefault();

                    //        if (permission == null)
                    //        {
                    //            filterContext.Result = Redirect(Shared.Core.AppConfig.Constants.Auth.UnauthorizedURL);
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            if (permission.Validity.HasValue)
                    //            {
                    //                if (permission.Validity.Value < DateTime.Now)
                    //                {
                    //                    filterContext.Result = Redirect(Shared.Core.AppConfig.Constants.Auth.UnauthorizedURL);
                    //                    return;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        filterContext.Result = Redirect(Shared.Core.AppConfig.Constants.Auth.UnauthorizedURL);
                    //        return;
                    //    }
                    //}
                }
                #endregion

                #region Cookies
                if (isLogged)
                {
                    SetUserSessionCookie();
                    SetCheckSessionCookie();
                }
                #endregion
            }
            base.OnAuthentication(filterContext);
        }


    }
}