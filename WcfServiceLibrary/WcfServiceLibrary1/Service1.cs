using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace WcfServiceLibrary1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1
    {
        private string getConString()
        {
            return File.ReadAllLines("C:/conString.txt")[0];
        }

        /// <summary>
        /// Метод получения всех турниров
        /// </summary>
        /// <returns>Список всех турниров (год + город проведения)</returns>
        public List<string> GetMarathon()
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select Marathon.YearHeld, Country.CountryName From Marathon, Country Where Marathon.CountryCode = Country.CountryCode";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> list = new List<string>();
            DataTableReader dtreader = dt.CreateDataReader();
            while (dtreader.Read())
            {
                list.Add(dtreader["YearHeld"].ToString() + " - " + dtreader["CountryName"].ToString());
            }

            con.Close();
            return list;

        }

        /// <summary>
        /// Метод получения всех типов заездов в каждом из марафоне
        /// </summary>
        /// <returns>Список всех типов заездов в каждом марафоне</returns>
        public List<List<string>> GetEventType(int idMarathon)
        {
            //string connectionString = "Data Source=LAPTOP-20V122MK;Integrated Security=SSPI;Initial Catalog=marathondb";
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select EventType.EventTypeId, EventType.EventTypeName, Marathon.MarathonId From EventType, [Event], Marathon Where [Event].MarathonId = Marathon.MarathonId and Marathon.MarathonId = '" + idMarathon + "' and EventType.EventTypeId = [Event].EventTypeId;";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<List<string>> list = new List<List<string>>();
            DataTableReader dtreader = dt.CreateDataReader();
            while (dtreader.Read())
            {
                List<string> rowlist = new List<string>();
                rowlist.Add(dtreader["EventTypeId"].ToString());
                rowlist.Add(dtreader["EventTypeName"].ToString());
                rowlist.Add(dtreader["MarathonId"].ToString());
                list.Add(rowlist);
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения гендеров из БД
        /// </summary>
        /// <returns>список гендеров + any</returns>
        public List<string> GetGender(bool hasAny)
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * From Gender";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> list = new List<string>();
            DataTableReader dtreader = dt.CreateDataReader();
            while (dtreader.Read())
            {
                list.Add(dtreader["Gender"].ToString());
            }
            if (hasAny) list.Add("Any");

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения всех бегунов по определенным параметрам
        /// </summary>
        /// <param name="fromAge">нижний возрастной порог</param>
        /// <param name="toAge">верхний возрастной порог</param>
        /// <param name="idMarathon">номер марафона</param>
        /// <param name="idEventType">тип забега</param>
        /// <param name="gender">выбранный пол</param>
        /// <returns></returns>
        public List<List<string>> GetPreviousResult(int fromAge, int toAge, int idMarathon, string idEventType, string gender)
        {
            var fromDate = getDateFormat(fromAge);
            var toDate = getDateFormat(toAge);

            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "set language english Select RegistrationEvent.RaceTime, [User].FirstName, [User].LastName, Runner.DateOfBirth, Runner.CountryCode From [User], Runner, RegistrationEvent, Registration Where Runner.RunnerId = Registration.RunnerId and Registration.RegistrationId = RegistrationEvent.RegistrationId and [User].Email = Runner.Email and RaceTime is not null and RaceTime <> 0 and Runner.DateOfBirth between '" + toDate + "' and '" + fromDate + "' order by RaceTime;";
            if (gender != "Any") cmd.CommandText = "set language english Select RegistrationEvent.RaceTime, concat([User].FirstName, ' ', [User].LastName) as RunnerName, Runner.DateOfBirth, Runner.CountryCode From [User], [Event], Runner, RegistrationEvent, Registration Where Runner.RunnerId = Registration.RunnerId and Registration.RegistrationId = RegistrationEvent.RegistrationId and [User].Email = Runner.Email and RaceTime is not null and RaceTime <> 0 and [Event].EventTypeId = '" + idEventType + "' and [Event].MarathonId = '" + idMarathon + "' and Runner.Gender = '" + gender + "' and Runner.DateOfBirth between '" + toDate + "' and '" + fromDate + "' order by RaceTime;";
            else cmd.CommandText = "set language english Select RegistrationEvent.RaceTime, concat([User].FirstName, ' ', [User].LastName) as RunnerName, Runner.DateOfBirth, Runner.CountryCode From [User], [Event], Runner, RegistrationEvent, Registration Where Runner.RunnerId = Registration.RunnerId and Registration.RegistrationId = RegistrationEvent.RegistrationId and [User].Email = Runner.Email and RaceTime is not null and RaceTime <> 0 and [Event].EventTypeId = '" + idEventType + "' and [Event].MarathonId = '" + idMarathon + "' and Runner.DateOfBirth between '" + toDate + "' and '" + fromDate + "' order by RaceTime;";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<List<string>> list = new List<List<string>>();
            DataTableReader dtreader = dt.CreateDataReader();
            int index = 0;
            while (dtreader.Read())
            {
                List<string> rowlist = new List<string>();
                rowlist.Add(index.ToString());
                rowlist.Add(dtreader["RaceTime"].ToString());
                rowlist.Add(dtreader["RunnerName"].ToString());
                rowlist.Add(dtreader["DateOfBirth"].ToString());
                rowlist.Add(dtreader["CountryCode"].ToString());
                list.Add(rowlist);
                index++;
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод приведения дня рождения в формат SQL даты 
        /// </summary>
        /// <param name="age">Возраст бегунов</param>
        /// <returns>Строковое представление даты</returns>
        private string getDateFormat(int age)
        {
            var date = getDate(age);
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year.ToString();
            return (year + '-' + month + '-' + day).ToString();
        }

        /// <summary>
        /// Метод получения дня рождения бегунов для выборки забегов
        /// </summary>
        /// <param name="age">возраст бегунов</param>
        /// <returns>день рождения</returns>
        private DateTime getDate(int age)
        {
            var timeNow = DateTime.Now;
            var ageDiff = timeNow.AddYears(-age);

            return ageDiff;
        }

        /// <summary>
        /// Метод добавление нового пользователя и бегуна
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">пароль</param>
        /// <param name="firstName">имя</param>
        /// <param name="lastName">фамилия</param>
        /// <param name="gender">пол</param>
        /// <param name="dateOfBirth">дата рождения</param>
        /// <param name="country">страна</param>
        public void AddRunner(string email, string password, string firstName, string lastName, string gender, string dateOfBirth, string country)
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into [User] Values('" + email + "', '" + password + "', '" + firstName + "', '" + lastName + "', 'R');";
            cmd.ExecuteNonQuery();

            SqlCommand getCountry = con.CreateCommand();
            getCountry.CommandType = CommandType.Text;
            getCountry.CommandText = "Select CountryCode From Country Where CountryName = '" + country + "';";
            string countryCode = getCountry.ExecuteScalar().ToString();

            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "Insert into [Runner] Values('" + email + "', '" + gender + "', '" + dateOfBirth + "', '" + countryCode + "');";
            sqlCommand.ExecuteNonQuery();

            con.Close();
        }

        /// <summary>
        /// Метод получения всех стран
        /// </summary>
        /// <returns>список всех стран</returns>
        public List<string> GetCountry()
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select CountryName From Country;";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> list = new List<string>();
            DataTableReader dtreader = dt.CreateDataReader();

            while (dtreader.Read())
            {
                list.Add(dtreader["CountryName"].ToString());
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения всех предыдущих результатов авторизированного бегуна
        /// </summary>
        /// <param name="idRunner">идентификатор бегуна в системе</param>
        /// <returns>список всех предыдущих результатов</returns>
        public List<List<string>> GetRunnerPreviousResults(int idRunner)
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select concat(Marathon.YearHeld, ' ', Country.CountryName) as Marathon, EventType.EventTypeName as Event, RegistrationEvent.RaceTime as Time From Country, Marathon, EventType, RegistrationEvent, Runner, Registration Where Runner.RunnerId = Registration.RunnerId and Registration.RegistrationId = RegistrationEvent.RegistrationId and Runner.RunnerId = '" + idRunner + "' and Country.CountryCode = Marathon.CountryCode;";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<List<string>> list = new List<List<string>>();
            DataTableReader dtreader = dt.CreateDataReader();

            while (dtreader.Read())
            {
                var newList = new List<string>();
                newList.Add(dtreader["Marathon"].ToString());
                newList.Add(dtreader["Event"].ToString());
                newList.Add(dtreader["Time"].ToString());
                list.Add(newList);
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения пола и возрастной категории авторизированного бегуна
        /// </summary>
        /// <param name="idRunner">идентификатор бегуна</param>
        /// <returns>пол и возрастная категория бегуна</returns>
        public string[] GetRunnerParam(int idRunner)
        {
            string[] array = new string[2];

            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select Gender From Runner Where RunnerId = '" + idRunner + "';";
            array[0] = cmd.ExecuteScalar().ToString();

            SqlCommand cmdDate = con.CreateCommand();
            cmdDate.CommandType = CommandType.Text;
            cmdDate.CommandText = "Select DateOfBirth From Runner Where RunnerId = '" + idRunner + "';";

            var date = cmdDate.ExecuteScalar().ToString();
            var splitTime = date.Split(' ');
            var splitDate = splitTime[0].Split('.');
            
            var dateFormat = new DateTime(int.Parse(splitDate[2]), int.Parse(splitDate[1]), int.Parse(splitDate[0]));

            var ageInDays = (DateTime.Now - dateFormat).Days;
            var age = ageInDays / 365;

            if (age < 18) array[1] = "Under 18";
            if (age >= 18 && age <= 29) array[1] = "18 to 29";
            if (age >= 30 && age <= 39) array[1] = "30 to 39";
            if (age >= 40 && age <= 55) array[1] = "40 to 55";
            if (age >= 56 && age <= 70) array[1] = "56 to 70";
            if (age > 70) array[1] = "Over 70";

            return array;
        }

        /// <summary>
        /// Метод получения идентификатора забега (например, 11_1FM)
        /// </summary>
        /// <param name="idMarathon">айди марафона</param>
        /// <param name="idEventType">айди типа забега</param>
        /// <returns>идентификатор забега</returns>
        public string GetEventId(int idMarathon, string idEventType)
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select EventId From Event Where MarathonId = '" + idMarathon + "' and EventTypeId = '" + idEventType + "';";

            return cmd.ExecuteScalar().ToString();
        }

        /// <summary>
        /// Метод получения тотальных результатов по забегам (для страницы предыдущих результатов)
        /// </summary>
        /// <param name="idMarathon">айди марафона</param>
        /// <param name="idEventType">айди забега</param>
        /// <returns>массив, в котором [0] => кол-во бегунов, [1] => кол-во финишировавших, [2] => среднее время</returns>
        public string[] GetTotalPreviousResults(int idMarathon, string idEventType)
        {
            string[] array = new string[3];

            string idEvent = GetEventId(idMarathon, idEventType);

            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select count(RegistrationEventId) as TotalRunners From RegistrationEvent Where RegistrationEvent.EventId = '" + idEvent + "';";
            array[0] = cmd.ExecuteScalar().ToString();

            SqlCommand finished = con.CreateCommand();
            finished.CommandType = CommandType.Text;
            finished.CommandText = "Select count(RegistrationEventId) as TotalRunners From RegistrationEvent Where RegistrationEvent.EventId = '" + idEvent + "' and RaceTime is not null and RaceTime <> 0;";
            array[1] = finished.ExecuteScalar().ToString();

            SqlCommand avg = con.CreateCommand();
            avg.CommandType = CommandType.Text;
            avg.CommandText = "Select avg(RaceTime) From RegistrationEvent;";
            array[2] = avg.ExecuteScalar().ToString(); 
            return array;
        }

        /// <summary>
        /// Метод получения забегов марафона 2015 вместе со стоимостью участия
        /// </summary>
        /// <returns>список забегов и стоимость участия в них</returns>
        public List<List<string>> GetEventTypes()
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select EventType.EventTypeName, [Event].Cost From [Event], EventType Where [Event].EventTypeId = EventType.EventTypeId and MarathonId = 5 order by Cost desc";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<List<string>> list = new List<List<string>>();
            DataTableReader dtreader = dt.CreateDataReader();

            while (dtreader.Read())
            {
                var newList = new List<string>();
                newList.Add(dtreader["EventTypeName"].ToString());
                newList.Add(dtreader["Cost"].ToString());
                list.Add(newList);
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения опций на снаряжение
        /// </summary>
        /// <returns>список всех опций на снаряжение</returns>
        public List<List<string>> GetRaceKitOptions()
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * From RaceKitOption";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<List<string>> list = new List<List<string>>();
            DataTableReader dtreader = dt.CreateDataReader();

            while (dtreader.Read())
            {
                var newList = new List<string>();
                newList.Add(dtreader["RaceKitOptionId"].ToString());
                newList.Add(dtreader["RaceKitOption"].ToString());
                newList.Add(dtreader["Cost"].ToString());
                list.Add(newList);
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения всех видов благотворительности
        /// </summary>
        /// <returns>список благотворительностей</returns>
        public List<string> GetCharity()
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select CharityName, CharityDescription, CharityLogo From Charity";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> list = new List<string>();
            DataTableReader dtreader = dt.CreateDataReader();

            while (dtreader.Read())
            {
                list.Add(dtreader["CharityName"].ToString());
                list.Add(dtreader["CharityDescription"].ToString());
                list.Add(dtreader["CharityLogo"].ToString());
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения информации по благотворительности
        /// </summary>
        /// <param name="idRunner">идентификатор зарегистрированного в системе бегуна</param>
        /// <returns>информация по благотворительности (наименование, описание, лого)</returns>
        public List<string> GetRunnerCharity(int idRunner)
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select CharityName, CharityDescription, CharityLogo From Charity, Registration Where RunnerId = '" + idRunner + "' and Registration.CharityId = Charity.CharityId;";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<string> list = new List<string>();
            DataTableReader dtreader = dt.CreateDataReader();

            while (dtreader.Read())
            {
                list.Add(dtreader["CharityName"].ToString());
                list.Add(dtreader["CharityDescription"].ToString());
                list.Add(dtreader["CharityLogo"].ToString());
            }

            con.Close();
            return list;
        }

        /// <summary>
        /// Метод получения всех спонсоров и суммой пожертвования
        /// </summary>
        /// <param name="idRunner">идентификатор зарегистрированного в системе бегуна</param>
        /// <returns>список спонсоров конкретного участника и сумма пожертвования</returns>
        /// Для проверки работы idRunner = 193 (выводит двоих спонсоров, каждый с суммой по 500)
        public List<List<string>> GetSponsorships(int idRunner)
        {
            SqlConnection con = new SqlConnection(getConString());
            con.Open();

            SqlCommand getRegistrationId = con.CreateCommand();
            getRegistrationId.CommandType = CommandType.Text;
            getRegistrationId.CommandText = "Select RegistrationId From Registration Where RunnerId = '" + idRunner + "';";
            int idRegistration = (int)getRegistrationId.ExecuteScalar();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select SponsorName, Amount From Sponsorship Where Sponsorship.RegistrationId = '" + idRegistration + "';";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<List<string>> list = new List<List<string>>();
            DataTableReader dtreader = dt.CreateDataReader();

            while (dtreader.Read())
            {
                var newList = new List<string>();
                newList.Add(dtreader["SponsorName"].ToString());
                newList.Add(dtreader["Amount"].ToString());
                list.Add(newList);
            }

            con.Close();
            return list;
        }
    }
}
