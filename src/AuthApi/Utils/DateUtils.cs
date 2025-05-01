using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthApi.Utils
{
    public static class DateUtils
    {
        public static int AgeCalculate(DateTime DateOfBirth) 
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            var temp = new DateTime(DateTime.Today.Year,DateOfBirth.Month,DateOfBirth.Day);

            if (temp > DateTime.Today)
                age--;
            return age;
        }

        public static int AgeCalculateExtension(this DateTime DateOfBirth)
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            var temp = new DateTime(DateTime.Today.Year, DateOfBirth.Month, DateOfBirth.Day);

            if (temp > DateTime.Today)
                age--;
            return age;
        }

        /*public static int AgeCalculateV2(DateTime DateOfBirth) 
        {  
            int age = DateTime.Today.Year - DateOfBirth.Year; 
            if (DateOfBirth > DateTime.Today.AddYears(-age))
                return age--;   
            return age; 
        }*/
    }
}
