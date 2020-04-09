using System;
using System.Linq;

namespace common.SqlMaker.Exception
{
    /// <summary>
    /// 类型使用错误
    /// </summary>
    public class TypeErrorException : SystemException
    {
        private static string _ex_not_format = "类型错误：\n使用的类型不能为{0}";
        private static string _ex_use_format_ = "类型错误：\n使用的类型不能为{0}\n只能为{1}";
        public TypeErrorException(Type _er) : base(string.Format(_ex_not_format, _er.FullName)) { }
        public TypeErrorException(Type _er, params Type[] _ty) : base(string.Format(_ex_use_format_, _er.FullName, string.Join("或", _ty.Select(s => s.FullName)))) { }
        public TypeErrorException(Type _er, string _ty) : base(string.Format(_ex_use_format_, _er.FullName, _ty)) { }
    }
}
