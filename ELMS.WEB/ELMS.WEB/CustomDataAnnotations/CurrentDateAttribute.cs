using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.CustomDataAnnotations
{
    public class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            DateTime _Date = (DateTime)value;

            if (_Date >= DateTime.Today)
            {
                return true;
            }

            return false;
        }
    }
}