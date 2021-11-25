using SantaClausCrm.DataAccess;

namespace SantaClausCrm.Dtos
{
    public static class DtoExtension
    {
        public static bool IsValid(this Dto dto) {
            bool result = true;
            var pp = dto.GetType().GetProperties(System.Reflection.BindingFlags.Public);
            foreach(var p in pp) {
                if(p.PropertyType == typeof(int)) {
                    result &= (int)p.GetValue(dto) != 0;
                } else {
                    result &= p.GetValue(dto) is not null;
                } 
            }
            return result;
        }
    }
}