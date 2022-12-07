using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProxySOAPExecutable
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IProxy
    {

        [OperationContract]
        string getStation(int number, string chosenContract);

        [OperationContract]
        string getContracts();

        [OperationContract]
        string getAllStationsOfAContract(string chosenContract);

    }

}
