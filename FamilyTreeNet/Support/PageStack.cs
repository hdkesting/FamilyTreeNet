using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace FamilyTreeNet.Support
{
    internal class PageStack
    {
        private const string SessionKey = "PageStack";

        // need to make it a public property, for (de)serialization
        // NB json serialization re-orders a stack
        public List<PageDetails> Pages { get; set; }  = new List<PageDetails>();

        public static void Clear(ISession session)
        {
            if (session is null)
            {
                throw new System.ArgumentNullException(nameof(session));
            }

            var ps = GetFromSession(session);
            ps.Pages.Clear();
            SetInSession(session, ps);
        }

        public static void PushPage(ISession session, string path, string parameters)
        {
            if (session is null)
            {
                throw new System.ArgumentNullException(nameof(session));
            }

            var ps = GetFromSession(session);
            ps.AddPage(new PageDetails(path, parameters));
            SetInSession(session, ps);
        }

        public static PageDetails GoBack(ISession session)
        {
            if (session is null)
            {
                throw new System.ArgumentNullException(nameof(session));
            }

            var ps = GetFromSession(session);
            PageDetails result = null;

            if (ps.Pages.Count > 0)
            {
                // discard current page
                ps.Pages.RemoveAt(ps.Pages.Count-1);
            }

            if (ps.Pages.Count > 0)
            {
                // return previous page, but keep on stack (will be re-added anyway)
                result = ps.Pages.Last();
            }

            SetInSession(session, ps);
            return result;
        }

        private static PageStack GetFromSession(ISession session)
        {
            var json = session.GetString(SessionKey);
            if (json is null)
            {
                return new PageStack();
            }

            var ps = JsonSerializer.Deserialize<PageStack>(json);
            return ps ?? new PageStack();
        }

        private static void SetInSession(ISession session, PageStack pageStack)
        {
            var json = JsonSerializer.Serialize(pageStack);
            session.SetString(SessionKey, json);
        }

        private bool AddPage(PageDetails newdetails)
        {
            if (Pages.Count > 0)
            {
                var det = Pages.Last();
                if (det.PagePath == newdetails.PagePath && det.PageParameters == newdetails.PageParameters)
                {
                    // it's already there - great
                    return false;
                }
            }

            Pages.Add(newdetails);
            return true;
        }

        public class PageDetails
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PageDetails"/> class.
            /// </summary>
            /// <remarks>
            /// Mainly to support deserialization.
            /// </remarks>
            public PageDetails()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PageDetails"/> class.
            /// </summary>
            /// <param name="path">The path.</param>
            /// <param name="parms">The parms.</param>
            public PageDetails(string path, string parms)
            {
                this.PagePath = path;
                this.PageParameters = parms;
            }

            public string PagePath { get; set; }

            public string PageParameters { get; set; }

            public string Url => string.IsNullOrEmpty(PageParameters) ? this.PagePath : this.PagePath + "?" + this.PageParameters;

            public override string ToString() => this.Url;
        }
    }
}
