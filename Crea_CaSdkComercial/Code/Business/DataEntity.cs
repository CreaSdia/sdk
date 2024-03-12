using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crea_CaSdkComercial.Code.Business
{
    class DataEntity : Dictionary<string, object>
    {
        public DataEntity() : base(StringComparer.InvariantCultureIgnoreCase)
        {

        }

        public DataEntity(string key, object value) : base(StringComparer.InvariantCultureIgnoreCase)
        {
            this[key] = value;
        }

        public DataEntity GetEntity(string name)
        {
            try
            {
                var result = this[name] as DataEntity;
                if (result != null)
                {
                    return result;
                }

                return JsonConvert.DeserializeObject<DataEntity>(this[name].ToString());
            }
            catch
            {
                return new DataEntity();
            }
        }

        private object GetValue(string name)
        {
            var fieldNames = name.Split('.').Select(e => e.FisrtCharUpper()).ToList();

            return this.GetValue(fieldNames, this);
        }

        private object GetValue(List<string> fieldNames, DataEntity values)
        {
            var fieldName = fieldNames[0];

            if (!values.ContainsKey(fieldName))
            {
                return null;
            }

            if (fieldNames.Count > 1)
            {
                fieldNames.RemoveAt(0);
                return this.GetValue(fieldNames, values.GetEntity(fieldName));
            }

            return values[fieldName];
        }

        public string GetString(string name)
        {
            var result = this.GetValue(name);
            return result?.ToString() ?? "";
        }

        public int GetInt(string name)
        {
            object value;
            if (this.TryGetValue(name, out value))
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                {
                    return result;
                }
            }
            return 0;
        }

        public decimal GetDecimal(string name)
        {
            object value;
            if (this.TryGetValue(name, out value))
            {
                decimal result;
                if (decimal.TryParse(value.ToString(), out result))
                {
                    return result;
                }
            }
            return 0;
        }

        public bool GetBoolean(string name)
        {
            object value;
            if (this.TryGetValue(name, out value))
            {
                bool result;
                if (bool.TryParse(value.ToString(), out result))
                {
                    return result;
                }
            }
            return false;
        }

        public List<int> GetListInt(string name)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<int>>(this[name].ToString());
            }
            catch
            {
                return new List<int>();
            }
        }

        public List<T> GetList<T>(string name)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(this[name].ToString());
            }
            catch
            {
                return new List<T>();
            }
        }

        public List<string> GetListString(string name)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<string>>(this[name].ToString());
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
