using System.Collections.Generic;
using System.Dynamic;

namespace decaf.common.Utilities;

public static class ParameterMapper
{
    public static object Map(IDictionary<string, object> dict)
    {
        var eo = new ExpandoObject();
        var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

        foreach (var kvp in dict)
        {
            eoColl.Add(kvp);
        }

        return eo;
    }

    public static Dictionary<string, object> Unmap(ExpandoObject obj)
    {
        var coll = (ICollection<KeyValuePair<string, object>>)obj;
        var dict = new Dictionary<string, object>();
        foreach(var kp in coll)
            dict.Add(kp.Key, kp.Value);

        return dict;
    }
}