using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Helpers
{
    public class SqlHelper
    {

        public static void ValidarDuplicado(DbUpdateException dbex, ModelStateDictionary modelState, String field)
        {
            SqlException innerException = dbex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
            {
                modelState.AddModelError(field, field + " existente.");
            }
            else
            {
                modelState.AddModelError(String.Empty, "No se puedo guardar el registro en la BD: " + dbex.Message);
            }
        }
    }
}
