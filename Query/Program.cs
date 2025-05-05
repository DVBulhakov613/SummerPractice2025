using Class_Lib;
using Class_Lib.Backend.Services;
using System.Reflection;

namespace Class_Lib
{
    class Query
    {
        static void Main()
        {

        }

        public static void PrintMethodNames(Type type)
        {
            foreach (var method in type.GetMethods(
                BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly))
            {
                Console.WriteLine(method.Name);
            }
        }
    }
}

