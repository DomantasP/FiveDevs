using System.Collections.Generic;

namespace FiveDevsShop.Models
{
    public class Month
    {
        public string Name { get; }
        public int Index { get; }

        public Month(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public static readonly List<Month> AllMonths = new List<Month>()
        {
            new Month("Sausis", 1),
            new Month("Vasaris", 2),
            new Month("Kovas", 3),
            new Month("Balandis", 4),
            new Month("Gegužė", 5),
            new Month("Birželis", 6),
            new Month("Liepa", 7),
            new Month("Rugpjūtis", 8),
            new Month("Rugsėjis", 9),
            new Month("Spalis", 10),
            new Month("Lapkritis", 11),
            new Month("Gruodis", 12),
        };
    }
}
