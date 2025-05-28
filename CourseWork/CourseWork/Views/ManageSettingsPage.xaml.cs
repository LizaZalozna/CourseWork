using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class ManageSettingsPage : ContentPage
	{
        private SettingsDTO currentSettings;
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";

        public ManageSettingsPage()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var library = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                    if (library.Settings != null)
                    {
                        currentSettings = library.Settings;
                        ReservedTimeEntry.Text = currentSettings.ReservedTime.ToString();
                        ReservedReputationEntry.Text = currentSettings.ReservedReputation.ToString();
                        ReturnTimeEntry.Text = currentSettings.ReturnTime.ToString();
                        ReturnReputationEntry.Text = currentSettings.ReturnReputation.ToString();
                    }
                }
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
                string.IsNullOrWhiteSpace(ReturnReputationEntry.Text) ||
                string.IsNullOrWhiteSpace(ReturnTimeEntry.Text))
            {
                await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "OK");
                return;
            }

            try
            {
                var library = File.Exists(libraryPath)
                    ? Serializer.LoadFromXml<LibraryDTO>(libraryPath)
                    : new LibraryDTO {};

                var newSettings = new SettingsDTO
                {
                    ReservedReputation = int.Parse(ReservedReputationEntry.Text),
                    ReservedTime = int.Parse(ReservedTimeEntry.Text),
                    ReturnReputation = int.Parse(ReturnReputationEntry.Text),
                    ReturnTime = int.Parse(ReturnTimeEntry.Text)
                };
                if (library.Settings != newSettings)
                {
                    library.Settings = newSettings;
                    Serializer.SaveToXml(library, libraryPath);
                    await DisplayAlert("Успіх", "Налаштування змінено", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Помилка зміни налаштувань: {ex.Message}", "OK");
            }
        }
    }
}