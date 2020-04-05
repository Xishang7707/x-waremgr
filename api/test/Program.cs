using common.Utils;
using System;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = @"Database=x_waremgr;Data Source=.;User Id=sa;Password=123456";
            var guid = "20b14440eec146789b1bbb8dc1d1195b";//Common.MakeGuid();
            string enc = Common.AESEncrypt(str, guid);
            string desc = Common.AESDecrypt(enc, guid);

            Console.WriteLine();
        }
    }
}
