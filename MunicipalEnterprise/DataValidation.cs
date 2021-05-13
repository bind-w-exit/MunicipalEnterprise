using System.Linq;

namespace MunicipalEnterprise
{
    public class DataValidation
    {
        public static bool Name(string str)
        {
            if (str.All(x => x >= 'a' && x <= 'z' || x >= 'A' && x <= 'Z'))
                return true;
            return false;
        }

        public static bool Street(string str)
        {
            if (str.All(x => x >= 'a' && x <= 'z' || x >= 'A' && x <= 'Z' || x == ' '))
                return true;
            return false;
        }

        public static bool Numeric(string str)
        {
            if (str.All(x => x >= '0' && x <= '9'))
                if (str.FirstOrDefault() >= '1' && str.FirstOrDefault() <= '9')
                    return true;
            return false;
        }

        public static bool Empty(string str)
        {
            if (str.Length == 0 || str.All(x => x==' '))
                     return true;
            return false;
        }
    }
}
