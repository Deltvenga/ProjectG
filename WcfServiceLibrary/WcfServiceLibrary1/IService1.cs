using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<string> GetMarathon();

        [OperationContract]
        List<List<string>> GetEventType(int idMarathon);

        [OperationContract]
        List<string> GetGender(bool hasAny);

        [OperationContract]
        List<List<string>> GetPreviousResult(int fromAge, int toAge, int idMarathon, string idEventType, string gender);

        [OperationContract]
        void AddRunner(string email, string password, string firstName, string lastName, string gender, string dateOfBirth, string countryCode, string role);

        [OperationContract]
        List<List<string>> GetCountry();

        [OperationContract]
        List<List<string>> GetRunnerPreviousResults(int idRunner);

        [OperationContract]
        string[] GetRunnerParam(int idRunner);

        [OperationContract]
        string[] GetTotalPreviousResults(int idMarathon, string idEventType);

        [OperationContract]
        List<List<string>> GetEventTypes();

        [OperationContract]
        List<List<string>> GetRaceKitOptions();

        [OperationContract]
        List<string> GetCharity();

        [OperationContract]
        List<List<string>> GetSponsorships(int idRunner);

        [OperationContract]
        List<string> GetRunnerCharity(int idRunner);

        [OperationContract]
        List<List<string>> GetRunners();

        [OperationContract]
        string GetMarathonStartDateTime();
        // TODO: Добавьте здесь операции служб
    }

    // Используйте контракт данных, как показано на следующем примере, чтобы добавить сложные типы к сервисным операциям.
    // В проект можно добавлять XSD-файлы. После построения проекта вы можете напрямую использовать в нем определенные типы данных с пространством имен "WcfServiceLibrary1.ContractType".
}
