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
        public List<List<string>> GetEventType()
        {
            //string connectionString = "Data Source=LAPTOP-20V122MK;Integrated Security=SSPI;Initial Catalog=marathondb";
            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select EventType.EventTypeName, Marathon.MarathonId From EventType, [Event], Marathon Where [Event].MarathonId = Marathon.MarathonId and EventType.EventTypeId = [Event].EventTypeId;";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<List<string>> list = new List<List<string>>();
            DataTableReader dtreader = dt.CreateDataReader();
            while (dtreader.Read())
            {
                List<string> rowlist = new List<string>();
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
        public List<string> GetGender()
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

            list.Add("Any");

            con.Close();
            return list;
        }

        public List<List<string>> GetAge(int fromAge, int toAge, int idMarathon, string idEventType, string gender)
        {
            var fromDate = getDateFormat(fromAge);
            var toDate = getDateFormat(toAge);

            SqlConnection con = new SqlConnection(getConString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "set language english Select RegistrationEvent.RaceTime, [User].FirstName, [User].LastName, Runner.DateOfBirth, Runner.CountryCode From [User], Runner, RegistrationEvent, Registration Where Runner.RunnerId = Registration.RunnerId and Registration.RegistrationId = RegistrationEvent.RegistrationId and [User].Email = Runner.Email and RaceTime is not null and RaceTime <> 0 and Runner.DateOfBirth between '" + toDate + "' and '" + fromDate + "' order by RaceTime;";
            if (gender != "Any") cmd.CommandText = "set language english Select RegistrationEvent.RaceTime, [User].FirstName, [User].LastName, Runner.DateOfBirth, Runner.CountryCode From [User], [Event], Runner, RegistrationEvent, Registration Where Runner.RunnerId = Registration.RunnerId and Registration.RegistrationId = RegistrationEvent.RegistrationId and [User].Email = Runner.Email and RaceTime is not null and RaceTime <> 0 and[Event].EventTypeId = '" + idEventType + "' and[Event].MarathonId = '" + idMarathon + "' and Runner.Gender = '" + gender + "' and Runner.DateOfBirth between '" + toDate + "' and '" + fromDate + "' order by RaceTime;";
            else cmd.CommandText = "set language english Select RegistrationEvent.RaceTime, [User].FirstName, [User].LastName, Runner.DateOfBirth, Runner.CountryCode From [User], [Event], Runner, RegistrationEvent, Registration Where Runner.RunnerId = Registration.RunnerId and Registration.RegistrationId = RegistrationEvent.RegistrationId and [User].Email = Runner.Email and RaceTime is not null and RaceTime <> 0 and[Event].EventTypeId = '" + idEventType + "' and[Event].MarathonId = '" + idMarathon + "' and Runner.DateOfBirth between '" + toDate + "' and '" + fromDate + "' order by RaceTime;";

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
                rowlist.Add(dtreader["FirstName"].ToString());
                rowlist.Add(dtreader["LastName"].ToString());
                rowlist.Add(dtreader["DateOfBirth"].ToString());
                rowlist.Add(dtreader["CountryCode"].ToString());
                list.Add(rowlist);
                index++;
            }

            con.Close();
            return list;
        }

       
        private string getDateFormat(int age)
        {
            var date = getDate(age);
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year.ToString();
            return (year + '-' + month + '-' + day).ToString();
        }

        private DateTime getDate(int age)
        {
            var timeNow = DateTime.Now;
            var ageDiff = timeNow.AddYears(-age);

            return ageDiff;
        }
    }
}
