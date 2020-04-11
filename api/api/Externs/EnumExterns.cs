using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace api.Externs
{
    public static class EnumExterns
    {
        public static string GetDesc(this Enum em)
        {
            return em.GetType()
            .GetMember(em.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DescriptionAttribute>()?
            .Description;
        }
    }
}
