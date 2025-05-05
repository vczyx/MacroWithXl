using System;
using System.Collections.Generic;
using System.Linq;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public static class ExecutableExtensions
  {
    public const char StartSymbol = '$';
    public const char EndSymbol = ':';

    public static readonly Type[] FindExecutableTypes = { typeof(ECursorBtn), typeof(ECursorPos) };
    public static readonly Type DefaultExecutableTypes = typeof(ESendKey);

    public static Dictionary<string, Type> findExecutableDictionary { get; }

    public static bool TryParse(string identifier, string value, out string result)
    {
      var identifierPart = value.Split(StartSymbol)[1].Split(EndSymbol)[0];
      result = value.Remove(0, identifierPart.Length + 2);
      return identifierPart.Equals(identifier);
    }

    static ExecutableExtensions()
    {
      findExecutableDictionary = FindExecutableTypes.Select(type => {
        if (!(Activator.CreateInstance(type, "") is IExecutable instance))
          throw new Exception($"{type} is not an executable type");
        return (instance.identifier, type);
      }).ToDictionary(tuple => tuple.identifier, tuple => tuple.type);
    }

    public static Type GetExecutableType(string value, out string result)
    {
      if (!value.Contains(StartSymbol) || !value.Contains(EndSymbol))
      {
        result = value;
        return DefaultExecutableTypes;
      }
      
      var identifierPart = value.Split(StartSymbol)[1].Split(EndSymbol)[0];
      if (findExecutableDictionary.TryGetValue(identifierPart, out var type))
      {
        result = value.Remove(0, identifierPart.Length + 2);
        return type;
      }
      else
      {
        result = value;
        return DefaultExecutableTypes;
      }
    }
  }
}
