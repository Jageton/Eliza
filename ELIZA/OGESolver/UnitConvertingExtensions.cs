using System;
using System.ComponentModel;
using System.Text;

namespace OGESolver
{
    public static class UnitConvertingExtensions
    {
        public static double Convert(this InformationUnit unit, double amt, InformationUnit to,
            out string convertionString)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} ({1})", amt, unit.GetFriendlyName());
            if (to > unit)
            {
                var current = unit;
                while (current < to)
                {
                    if (to - current >= 10)
                    {
                        sb.AppendFormat(" = {0} / 1024 = ", amt);
                        amt /= 1024;
                        current += 10;
                        sb.AppendFormat("{0} ({1})", amt, current.GetFriendlyName());
                    }
                    else
                    {
                        sb.AppendFormat(" = {0} / 8 = ", amt);
                        amt /= 8;
                        current += 3;
                        sb.AppendFormat("{0} ({1})", amt, current.GetFriendlyName());
                    }
                }
            }
            else
            {
                var current = unit;
                while (current > to)
                {
                    if (current - to >= 10)
                    {
                        sb.AppendFormat(" = {0} * 1024 = ", amt);
                        amt *= 1024;
                        current -= 10;
                        sb.AppendFormat("{0} ({1})", amt, current.GetFriendlyName());
                    }
                    else
                    {
                        sb.AppendFormat(" = {0} * 8 = ", amt);
                        amt *= 8;
                        current -= 3;
                        sb.AppendFormat("{0} ({1})", amt, current.GetFriendlyName());
                    }
                }
            }
            convertionString = sb.ToString();
            return amt;
        }
        public static double Convert(this TimeUnit unit, double amt, TimeUnit to,
            out string convertionString)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} ({1})", amt, unit.GetFriendlyName());
            if (to > unit)
            {
                var current = unit;
                while (current < to)
                {
                    sb.AppendFormat(" = {0} / 60 = ", amt);
                    amt /= 60;
                    current = (TimeUnit)(60 * (int)current);
                    sb.AppendFormat("{0} ({1})", amt, current.GetFriendlyName());
                }
            }
            else
            {
                var current = unit;
                while (current > to)
                {
                    sb.AppendFormat(" = {0} * 60 = ", amt);
                    amt *= 60;
                    current = (TimeUnit)((int)current / 60);
                    sb.AppendFormat("{0} ({1})", amt, current.GetFriendlyName());
                }

            }
            convertionString = sb.ToString();
            return amt;
        }
        public static string GetFriendlyName<T>(this T value) where T : struct
        {
            var type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("Значение должно быть перечислением.", "value");
            }
            var memberInfo = type.GetMember(value.ToString());
            if (memberInfo.Length > 0)
            {
                var attr = memberInfo[0].GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (attr.Length > 0)
                {
                    return ((DescriptionAttribute) attr[0]).Description;
                }
            }
            return value.ToString();
        }
    }
}
