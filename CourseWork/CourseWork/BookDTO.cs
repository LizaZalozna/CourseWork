using System;
using System.Xml.Serialization;

namespace CourseWork
{
	[XmlType("Book")]
	public class BookDTO
	{
		[XmlElement("FullNameOfAuthor")]
		public string FullNameOfAuthor { get; set; }

		[XmlElement("NameOfBook")]
		public string NameOfBook { get; set; }

		[XmlElement("Genre")]
		public BookGenre.LiteraryGenre Genre { get; set; }

		[XmlElement("IsAvailable")]
		public bool IsAvailable { get; set; }

		[XmlElement("IsReserved")]
		public bool IsReserved { get; set; }

		[XmlElement("ReservedByLogin")]
		public string ReservedByLogin { get; set; }
	}
}