using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Interface
{
    class CheckRegisterForm
    {
        public string email;
        public string pass;
        public string confpass;
        public string firstname;
        public string lastname;
        public DateTime dateOfBirth;


        private bool[] check = new bool[5];
        public bool[] Check { get => check; private set => check = value; }

        //проверка формы
        public bool[] CheckForm()
        {
            CheckEmail();
            CheckPassword();
            CheckFirstName();
            CheckLastName();
            CheckBirthday();

            return Check;
        }

        //чек маила
        private void CheckEmail()
        {
            Regex regex = new Regex(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$");
            MatchCollection matches = regex.Matches(email);
            Check[0] = matches.Count > 0 ? true : false;
        }

        //чек совпадения паролей и их длины
        private void CheckPassword()
        {
            Check[1] = (pass.Length > 5 && pass == confpass) ? true : false;
        }

        private void CheckFirstName()
        {
            Regex regex = new Regex(@"^\p{Lu}\p{Ll}*(?:-\p{Lu}\p{Ll}*)?$");
            MatchCollection matches = regex.Matches(firstname);
            Check[2] = matches.Count > 0 ? true : false;
        }

        private void CheckLastName()
        {
            Regex regex = new Regex(@"^\p{Lu}\p{Ll}*(?:-\p{Lu}\p{Ll}*)?$");
            MatchCollection matches = regex.Matches(lastname);
            Check[3] = matches.Count > 0 ? true : false;
        }

        //проверка даты рождения
        private void CheckBirthday()
        {
           Check[4] = dateOfBirth < DateTime.Now ? true : false;
        }

    }
}
