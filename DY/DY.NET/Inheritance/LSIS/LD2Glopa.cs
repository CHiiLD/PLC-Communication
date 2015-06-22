﻿
namespace DY.NET.LSIS
{
    public class LD2Glopa
    {
        /// <summary>
        /// 글로파 변수이름으로 변환한다
        /// </summary>
        /// <param name="ld">래더 변수 이름</param>
        /// <param name="type">변수의 타입</param>
        /// <returns>글로파 변수 이름</returns>
        public static string VarConvert(string ld, PLCVarType type)
        {
            string ret = ld;
            string type_s = null;
            switch (type)
            {
                case PLCVarType.BIT:
                    type_s = "X";
                    break;
                case PLCVarType.BYTE:
                    type_s = "B";
                    break;
                case PLCVarType.WORD:
                    type_s = "W";
                    break;
                case PLCVarType.DWORD:
                    type_s = "D";
                    break;
                case PLCVarType.LWORD:
                    type_s = "L";
                    break;
            }

            ret = ret.Insert(0, "%");
            ret = ret.Insert(2, type_s);
            return ret;
        }
    }
}
