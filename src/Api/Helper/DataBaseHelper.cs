using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Api.Helper
{
    public static class DataBaseHelper
    {
        public static string GetDefaultKeySql(List<string> tabela)
        {
      return $@"SELECT KU.table_name as TABLENAME,column_name as PRIMARYKEYCOLUMN
              FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
              INNER JOIN
                  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
                        ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
                          TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME AND 
                          KU.table_name in({string.Join(',', tabela)})
              ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION";

    }
    }
}
