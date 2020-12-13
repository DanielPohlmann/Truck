using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Volvo.Core.Resources;

namespace Volvo.Core.DomainObject
{
    public class Vin
    {
        public const int VimLength = 17;
        public string Number { get; private set; }
        protected Vin() { }

        public Vin(string number)
        {
            if (!Validate(number)) 
                throw new DomainException(MessagesValidation.VinInvalid);
            Number = number.ToUpper();
        }

        //public static bool Validate(string number) => DaleNewman.Vin.IsValid(number);

        public static bool Validate(string number)
        {
            if (string.IsNullOrEmpty(number))
                return false; 

            //var regexVin = new Regex(@"^(?<wmi>[A-HJ-NPR-Z\d]{3})(?<vds>[A-HJ-NPR-Z\d]{5})(?<check>[\dX])(?<vis>(?<year>[A-HJ-NPR-Z\d])(?<plant>[A-HJ-NPR-Z\d])(?<seq>[A-HJ-NPR-Z\d]{6}))$");
            var regexVin = new Regex(@"^([A-Za-z0-9]{17})*$");
            return regexVin.IsMatch(number);
        }
    }
}
