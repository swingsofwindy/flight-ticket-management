using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace BLL
{
    public class SessionManager
    {
        private static readonly Dictionary<string, UserSession> sessions = new Dictionary<string, UserSession>();

        public static void StartSession(string sessionId, string mail, List<string> permissions)
        {
            sessions[sessionId] = new UserSession(mail, permissions);
        }

        public static void EndSession(string sessionId)
        {
            sessions.Remove(sessionId);
        }

        public static UserSession GetSession(string sessionId)
        {
            if (sessions.ContainsKey(sessionId))
            {
                return sessions[sessionId];
            }
            else
            {
                return null;
            }
        }
    }

    public class UserSession
    {
        public string mail { get; private set; }
        public List<string> permissions { get; private set; }

        public UserSession(string mail, List<string> permissions)
        {
            this.mail = mail;
            this.permissions = permissions;
        }

        public void EndSession()
        {
            this.mail = null;
            this.permissions = null;
        }
    }


}
