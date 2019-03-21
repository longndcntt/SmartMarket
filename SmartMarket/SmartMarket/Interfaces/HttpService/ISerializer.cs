using System.IO;

namespace SmartMarket.Interfaces.HttpService
{
    public interface ISerializer
    {
        T Deserialize<T>(string data, string url = null);

        T DeserializeFromJsonStream<T>(Stream jsonStream, string url = null);

        string Serialize<T>(T obj);

        #region Parse XML

        T Deserialize<T>(string input) where T : class;

        string ObjectToSerialize<T>(T objectToSerialize);

        #endregion
    }
}
