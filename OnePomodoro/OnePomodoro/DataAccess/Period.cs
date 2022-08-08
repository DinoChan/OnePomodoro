using System;

namespace DataAccessLibrary
{
    public class Period
    {
        public DateTime From { get; set; }
        public bool HasFinished { get; set; }
        public int Id { get; set; }
        public bool IsFocus { get; set; }
        public DateTime To { get; set; }
        public string ViewType { get; set; }
    }
}
