using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class ManageSettingsPage : ContentPage
	{
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";

        public ManageSettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                    Library.Initialize(dto);
                }

                var library = Library.Instance;
                ReservedTimeEntry.Text = library.ToDTO().Settings.ReservedTime.ToString();
                ReservedReputationEntry.Text = library.ToDTO().Settings.ReservedReputation.ToString();
                MaxReservedEntry.Text = library.ToDTO().Settings.MaxReserved.ToString();
                ReturnTimeEntry.Text = library.ToDTO().Settings.ReturnTime.ToString();
                ReturnReputationEntry.Text = library.ToDTO().Settings.ReturnReputation.ToString();
                MaxLendedEntry.Text = library.ToDTO().Settings.MaxLended.ToString();
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Помилка завантаження налаштувань: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReservedReputationEntry.Text) ||
                string.IsNullOrWhiteSpace(ReservedTimeEntry.Text) ||
                string.IsNullOrWhiteSpace(MaxReservedEntry.Text) ||
                string.IsNullOrWhiteSpace(ReturnReputationEntry.Text) ||
                string.IsNullOrWhiteSpace(ReturnTimeEntry.Text) ||
                string.IsNullOrWhiteSpace(MaxLendedEntry.Text))
            {
                await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "OK");
                return;
            }

            try
            {
                LibraryDTO dto = File.Exists(libraryPath)
                ? Serializer.LoadFromXml<LibraryDTO>(libraryPath)
                : new LibraryDTO { Users = new List<UserDTO>(), Books = new List<BookDTO>(), Settings = new SettingsDTO() };
                Library library = Library.Initialize(dto);


                if (!(dto.Settings.ReservedReputation == int.Parse(ReservedReputationEntry.Text) &&
                    dto.Settings.ReservedTime == int.Parse(ReservedTimeEntry.Text) &&
                    dto.Settings.MaxReserved==int.Parse(MaxReservedEntry.Text)&&
                    dto.Settings.ReturnReputation == int.Parse(ReturnReputationEntry.Text) &&
                    dto.Settings.ReturnTime == int.Parse(ReturnTimeEntry.Text) &&
                    dto.Settings.MaxLended==int.Parse(MaxLendedEntry.Text))) 
                {
                    library.ChangeSettings(int.Parse(ReservedReputationEntry.Text), int.Parse(ReservedTimeEntry.Text), int.Parse(MaxReservedEntry.Text),
                    int.Parse(ReturnReputationEntry.Text), int.Parse(ReturnTimeEntry.Text), int.Parse(MaxLendedEntry.Text));
                    Serializer.SaveToXml(library.ToDTO(), libraryPath);
                    await DisplayAlert("Успіх", "Налаштування змінено", "OK");
                }
                LoadSettings();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Помилка зміни налаштувань: {ex.Message}", "OK");
            }
        }
    }
}