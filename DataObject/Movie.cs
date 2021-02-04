using System;

namespace DataObject
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public int? RatingID { get; set; }

        public override string ToString()
        {
            return $"Movie: {ID,-4} {Title,-25} {ReleaseDate, 25} {Hours, -3} {Minutes, -3} {RatingID, -4}";
        }
    }
}
