using System;
using System.ComponentModel;

namespace OGESolver
{
    /// <summary>
    /// Единицы измерения информации. Число (n), соответствующее элементу перечисления, означает,
    /// что  в данной единице содержится 2^n бит.
    /// </summary>
    [Serializable]
    public enum InformationUnit
    {
        [Description("бит")]
        Bit = 0,
        [Description("байт")]
        Byte = 3,
        [Description("Кбит")]
        KBit = 10,
        [Description("Кбайт")]
        KByte = 13,
        [Description("Мбит")]
        MBit = 20,
        [Description("Мбайт")]
        MByte = 23,
        [Description("Гбит")]
        GBit = 30,
        [Description("Гбайт")]
        GByte = 33,
        [Description("Тбайт")]
        TByte = 43
    }
}
