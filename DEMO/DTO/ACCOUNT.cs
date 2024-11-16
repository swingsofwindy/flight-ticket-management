using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ACCOUNT
    {
        private string userID;
        private string userName;
        private string phone;
        private string email;
        private DateTime birthday;
        private string passWordUser;
        private int perrmissionID;
        private int isDeleted;

        public string UserID
        {
            get => this.userID;
            set => this.userID = value;
        }
        public string UserName
        {
            get => this.userName;
            set => this.userName = value;
        }
        public string Phone
        {
            get => this.phone;
            set => this.phone = value;
        }
        public string Email
        {
            get => this.email;
            set => this.email = value;
        }
        public DateTime Birth
        {
            get => this.birthday;
            set => this.birthday = value;
        }
        public string PasswordUser
        {
            get => this.passWordUser;
            set => this.passWordUser = value;
        }
        public int PermissonID
        {
            get => this.perrmissionID;
            set => this.perrmissionID = value;
        }
        
        public int IsDeleted
        {
            get => this.isDeleted;
            set => this.isDeleted = value;
        }
    }
}
