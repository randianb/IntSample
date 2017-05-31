using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Options
{
    public class CheckAuth
    {
        public static string ValidCheck(int PackageIdx, int ProgramIdx, int WindowIdx)
        {
            string result = "000";

            // 프로그램번호만 있는 경우 
            if (ProgramIdx > 0 && WindowIdx <= 0)
            {
                // 권한 테이블을 탐색하면서 
                foreach (DataRow row in UserInfo.DtAuthority.Rows)
                {
                    // 권한 테이블에서 패키지 권한이 있는지 확인
                    if (Convert.ToInt32(row["PackageIdx"]) == PackageIdx &&
                        Convert.ToInt32(row["ProgramIdx"]) == 0 && Convert.ToInt32(row["WindowIdx"]) == 0)
                    {
                        result = Convert.ToInt32(row["AuthRead"]).ToString().Trim() +
                                 Convert.ToInt32(row["AuthEdit"]).ToString().Trim() +
                                 Convert.ToInt32(row["AuthDelete"]).ToString().Trim();
                    }
                    // 권한 테이블에서 프로그램 권한이 있는지 확인 
                    else if (Convert.ToInt32(row["PackageIdx"]) == PackageIdx &&
                             Convert.ToInt32(row["ProgramIdx"]) == ProgramIdx && Convert.ToInt32(row["WindowIdx"]) == 0)
                    {
                        result = Convert.ToInt32(row["AuthRead"]).ToString().Trim() +
                                 Convert.ToInt32(row["AuthEdit"]).ToString().Trim() +
                                 Convert.ToInt32(row["AuthDelete"]).ToString().Trim();
                    }
                }
            }
            return result;
        }

    }
}
