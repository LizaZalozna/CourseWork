using System;
namespace CourseWork
{
    public static class BookGenre
    {
        public enum LiteraryGenre
        {
            Crime,
            Fantasy,
            Horror,
            Romance,
            Historical,
            Adventure,
            ScienceFiction,
            DetectiveFiction,
            Thriller,
            Drama,
            Comedy,
            Biography,
            Autobiography,
            Dystopian,
            Utopian,
            Classic,
            Poetry,
            Satire,
            NonFiction,
            Children,
            FairyTale,
            Mythology,
            GraphicNovel
        }

        public static string ToString(this BookGenre.LiteraryGenre genre)
        {
            return genre switch
            {
                BookGenre.LiteraryGenre.Crime => "Кримінал",
                BookGenre.LiteraryGenre.Fantasy => "Фентезі",
                BookGenre.LiteraryGenre.Horror => "Жахи",
                BookGenre.LiteraryGenre.Romance => "Романтика",
                BookGenre.LiteraryGenre.Historical => "Історичний",
                BookGenre.LiteraryGenre.Adventure => "Пригоди",
                BookGenre.LiteraryGenre.ScienceFiction => "Наукова фантастика",
                BookGenre.LiteraryGenre.DetectiveFiction => "Детектив",
                BookGenre.LiteraryGenre.Thriller => "Трилер",
                BookGenre.LiteraryGenre.Drama => "Драма",
                BookGenre.LiteraryGenre.Comedy => "Комедія",
                BookGenre.LiteraryGenre.Biography => "Біографія",
                BookGenre.LiteraryGenre.Autobiography => "Автобіографія",
                BookGenre.LiteraryGenre.Dystopian => "Антиутопія",
                BookGenre.LiteraryGenre.Utopian => "Утопія",
                BookGenre.LiteraryGenre.Classic => "Класика",
                BookGenre.LiteraryGenre.Poetry => "Поезія",
                BookGenre.LiteraryGenre.Satire => "Сатира",
                BookGenre.LiteraryGenre.NonFiction => "Документалістика",
                BookGenre.LiteraryGenre.Children => "Дитяча література",
                BookGenre.LiteraryGenre.FairyTale => "Казка",
                BookGenre.LiteraryGenre.Mythology => "Міфологія",
                BookGenre.LiteraryGenre.GraphicNovel => "Графічний роман",
                _ => genre.ToString()
            };
        }
    }
}