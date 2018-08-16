using System.Xml.Serialization;

namespace Contracts
{
    [XmlRoot("TestObj", Namespace="http://schemas.datacontract.org/2004/07/Contracts")]
    public class TestObj
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}