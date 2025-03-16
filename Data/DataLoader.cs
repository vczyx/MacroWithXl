using System.Windows;
using Newtonsoft.Json.Linq;

namespace InputMacro3.Data
{
  public static class DataLoader
  {
    public static T ToObjectFromJson<T>(this string json)
    {
      var jObject = JObject.Parse(json);
      return jObject.ToObject<T>();
    }

    public static void CopyObjectJson<T>(this T obj)
    {
      var jObject = JObject.FromObject(obj);
      Clipboard.SetText(jObject.ToString());
    }
  }
}
