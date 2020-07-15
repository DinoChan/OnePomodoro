using System;

namespace DataAccessLibrary
{
    public class Period
    {
        public int Id { get; set; }


        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public bool IsFocus { get; set; }

        public bool HasFinished { get; set; }
    }
}
