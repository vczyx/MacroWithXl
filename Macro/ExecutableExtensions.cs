using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public static class ExecutableExtensions
  {
    public const char StartSymbol = '$';
    public const char EndSymbol = ':';

    public static Type[] FindExecutableTypes;
    public static readonly Type DefaultExecutableTypes = typeof(ESendKey);

    public static Dictionary<string, Type> findExecutableDictionary { get; }
    public static Dictionary<string, IExecutable> emptyInstanceExecutableDictionary { get; }

    public static bool TryParse(string identifier, string value, out string result)
    {
      var identifierPart = value.Split(StartSymbol)[1].Split(EndSymbol)[0];
      result = value.Remove(0, identifierPart.Length + 2);
      return identifierPart.Equals(identifier);
    }

    static ExecutableExtensions()
    {
      FindExecutableTypes = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => typeof(IExecutable).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
        .ToArray();

      emptyInstanceExecutableDictionary = new Dictionary<string, IExecutable>();
      findExecutableDictionary = FindExecutableTypes.Select(type => {
        if (Activator.CreateInstance(type, "") is IExecutable instance)
        {
          emptyInstanceExecutableDictionary.Add(instance.identifier, instance);
          return (instance.identifier, type);
        }
        else throw new NotSupportedException($"Cannot create instance of type {type}");
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

    public static IExecutable CreateExecutable(string value)
    {
      return Activator.CreateInstance(GetExecutableType(value, out var resultValue), resultValue) as IExecutable;
    }

    public static async Task Execute(string value)
    {
      var executable = CreateExecutable(value);
      await executable.Execute();
    }
  }
}
