using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.Text.Json;

public class Utilities
{
    public T ObjectToClass<T>(IDictionary<string, string> dict) where T : new()
    {
        var Class = new T();
        PropertyInfo[] properties = Class.GetType().GetProperties();
        
        foreach (PropertyInfo property in properties)
        {
            if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                continue;

            KeyValuePair<string, string> obj = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

            Type type = Class.GetType().GetProperty(property.Name).PropertyType;

            Class.GetType().GetProperty(property.Name).SetValue(Class, Convert.ChangeType(obj.Value, type), null);
        }
        return Class;
    }
    public List<T> ObjectToClass<T>(List<object> Obj) where T : new()
        
    {
        List<T> ListClass = new List<T>();

        foreach (IDictionary<string, string> dict in Obj)
        {
            var Class = new T();

            PropertyInfo[] properties = Class.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {

                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> obj = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                Type type = Class.GetType().GetProperty(property.Name).PropertyType;

                Class.GetType().GetProperty(property.Name).SetValue(Class, Convert.ChangeType(obj.Value, type), null);
            }
            ListClass.Add(Class);
        }
        return ListClass;
    }
}
