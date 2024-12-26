using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;

namespace BLL
{
    public interface IAccountBLL
    {
        bool AuthenticateAccount(string email, string password, out int permissionID);
    }

    public class ACCOUNT_BLL : IAccountBLL
    {

        private readonly AccountAccess accountDAL;

        public ACCOUNT_BLL()
        {
            accountDAL = new AccountAccess();
        }


        public bool AuthenticateAccount(string email, string password, out int permissionID)
        {
            AccountAccess accountDAL = new AccountAccess();
            if (accountDAL.CheckAccountExists(email))
            {
                permissionID = accountDAL.GetPermissionID(email, password);
                return permissionID != 0; // Trả về true nếu permissionID khác 0
            }
            else
            {
                permissionID = 0;
                return false; // Trả về false nếu tài khoản không tồn tại
            }
        }

        public string UpdateImage(string id, byte[] imageBytes)
        {
            DAL.AccountAccess prc = new DAL.AccountAccess();
            prc.UpdateImageInDatabase(id, imageBytes);
            if (prc.GetState() != string.Empty)
            {
                return prc.GetState();
            }
            return string.Empty;
        }

        public byte[] GetImage(string id)
        {
            byte[] imageBytes = null;
            DAL.AccountAccess prc = new DAL.AccountAccess();
            imageBytes = prc.GetImageFromDatabase(id);
            return imageBytes;
        }

        public List<ACCOUNT> List_acc(ACCOUNT dto)
        {
            return new DAL.AccountAccess().GetMember(dto);
        }

        public int deleteAccount(string ID)
        {
            return new DAL.AccountAccess().DeleteAccount(ID);
        }

        public int UpdateAccountName(string id, string name)
        {
            return new DAL.AccountAccess().UpdateAccountName(id, name);
        }
        public int UpdateAccountPhone(string id, string phone)
        {
            return new DAL.AccountAccess().UpdateAccountPhone(id, phone);
        }
        public int UpdateAccountBirth(string id, DateTime birth)
        {
            return new DAL.AccountAccess().UpdateAccountBirth(id, birth);
        }

        public int UpdateAccountEmail(string id, string email)
        {
            return new DAL.AccountAccess().UpdateAccountEmail(id, email);
        }
        public int UpdateAccountPassword(string id, string password, string oldpassword)
        {
            return new DAL.AccountAccess().UpdateAccountPassword(id, password);
        }
        public bool IsPassExits(string id, string pass)
        {
            return new DAL.AccountAccess().IsPassExits(id, pass);
        }

    }
}
