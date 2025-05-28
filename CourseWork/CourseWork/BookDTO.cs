using System;
namespace CourseWork
{
	public class BookDTO
	{
        public string FullNameOfAutor { get; set; }
        public string NameOfBook { get; set; }
        public BookGenre.LiteraryGenre Genre { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsReserved { get; set; }
        public string ReservedByLogin { get; set; }
    }
}