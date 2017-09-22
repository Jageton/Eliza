using System;
using System.Globalization;

namespace ELIZA.Morphology
{
    public class Localisation: Attribute
    {
        public CultureInfo CultureInfo { get; set; }
        public string Name { get; set; }

        public Localisation(string info, string name)
        {
            CultureInfo = CultureInfo.CreateSpecificCulture(info);
            Name = name;
        }
    }
}
