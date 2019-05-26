using SmartMarket.Services.HttpService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SmartMarket.Models
{
    public class ModelRestFul
    {
        public bool Error { get; set; }
        public string ErrMessageErroror { get; set; }
        public int ErrorCode { get; set; }
        public object Result { get; set; }

        public T Deserialize<T>(object data)
        {
            try
            {
                Serializer serializer = new Serializer();
                var a = data.ToString();
                var b = serializer.Deserialize<T>(a);
                return b;
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"Deserialize Cannot deserialize url res: with error: {exception.Message}");
#endif
                return default(T);
            }
        }

        public T Serialize<T>(string data)
        {
            try
            {
                Serializer serializer = new Serializer();
                var b = serializer.Deserialize<T>(data);
                return b;
            }
            catch (Exception exception)
            {
#if DEBUG
                Debug.WriteLine($"Deserialize Cannot deserialize url res: with error: {exception.Message}");
#endif
                return default(T);
            }
        }
    }
}
