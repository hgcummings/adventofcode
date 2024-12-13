using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace HGC.AOC.Common;

public static class RegexHelpers
{
    public static int GetInt(this Match match, string key)
    {
        return Int32.Parse(match.Groups[key].Value);
    }
    
    public static T Parse<T>(this Match match) where T : new()
    {
        var result = new T();
        foreach (var key in match.Groups.Keys)
        {
            var member = typeof(T).GetProperty(key);

            if (member != null)
            {
                if (member.PropertyType == typeof(String))
                {
                    member.SetValue(result, match.Groups[key].Value);                    
                }
                else if (member.PropertyType == typeof(Int32) || 
                         (member.PropertyType == typeof(Int32?) && 
                          match.Groups[key].Value != String.Empty))
                {
                    member.SetValue(result, Int32.Parse(match.Groups[key].Value));
                }
                else if (member.PropertyType == typeof(Int64) || 
                         (member.PropertyType == typeof(Int64?) && 
                          match.Groups[key].Value != String.Empty))
                {
                    member.SetValue(result, Int64.Parse(match.Groups[key].Value));
                }
            }
        }

        return result;
    }
}