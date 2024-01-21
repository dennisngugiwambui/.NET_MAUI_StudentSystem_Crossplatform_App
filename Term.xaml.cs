using StudentSystem.ViewModel;
using Microsoft.Data.Sqlite;
using StudentSystem.DataBase;
using SQLite;


namespace StudentSystem;


public partial class Term : ContentPage
{
    DatabaseHelper databaseHelper;

    private int _id = 0;
    public Term()
    {
        InitializeComponent();

        databaseHelper = new DatabaseHelper();

        LoadData();
    }

    async void LoadData()
    {
        var records = await databaseHelper.GetItemsAsync();

        recordsListView.ItemsSource = records;
    }

    public class TermRecord
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public String? StartDate { get; set; }
        public String? EndDate { get; set; }
        public int Year { get; set; }
        public string? Semester { get; set; }

        public string? Course { get; set; }
    }

   
    async private void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            // Get values from UI controls
            DateTime startDate = startDatePicker.Date;
            DateTime endDate = endDatePicker.Date;
            int year = 0;

            // Validate and parse year
            if (!int.TryParse(yearEntry.Text, out year))
            {
                await DisplayAlert("Error", "Please enter a valid numeric value for Year.", "OK");
                return;
            }

            // Validate required fields
            if (startDate == default || endDate == default || year == 0 || string.IsNullOrWhiteSpace(semesterEntry.Text))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            // Check if the start date is greater than the end date
            if (startDate > endDate)
            {
                await DisplayAlert("Error", "Start date cannot be greater than end date.", "OK");
                return;
            }

            string semester = semesterEntry.Text;

            TermRecord termRecord = new TermRecord();
            if (_id > 0)
            {
                termRecord.ID = _id;
            }
            termRecord.Semester = semester;
            termRecord.StartDate = startDate.ToShortDateString();
            termRecord.EndDate = endDate.ToShortDateString();
            termRecord.Year = year;
            termRecord.Course = courseNameEntry.SelectedItem?.ToString();

            await databaseHelper.SaveItemAsync(termRecord);

            if (Save.Text == "Update")
            {
                await DisplayAlert("Success", "Term Updated Successfully", "OK");
            }
            else
            {
                await DisplayAlert("Success", "Term saved Successfully", "OK");
            }

            LoadData();

            ClearFields();

            Save.Text = "Save";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private void ClearFields()
    {
        startDatePicker.Date = DateTime.Now;
        endDatePicker.Date = DateTime.Now;
        yearEntry.Text = string.Empty;
        semesterEntry.Text = string.Empty;
        courseNameEntry.SelectedItem = null;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        try
        {
            // Get the button that was clicked
            Button deleteButton = (Button)sender;

            // Get the corresponding item from the ListView's BindingContext
            TermRecord selectedItem = (TermRecord)deleteButton.BindingContext;

            // Check if selectedItem is not null
            if (selectedItem != null)
            {
                await databaseHelper.DeleteItemAsync(selectedItem);

                await DisplayAlert("Success", "Record Deleted successfully", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Selected item is null.", "OK");
            }

            LoadData();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }

    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        // Get the button that was clicked
        Button updateButton = (Button)sender;

        // Get the corresponding item from the ListView's BindingContext

        TermRecord selectedItem = (TermRecord)updateButton.BindingContext;

        //startDatePicker.Date = selectedItem.StartDate;
        //endDatePicker.Date = selectedItem.EndDate;

        // Convert the string to DateTime using DateTime.TryParse
        if (DateTime.TryParse(selectedItem.StartDate, out DateTime startDate))
        {
            startDatePicker.Date = startDate;
        }
        else
        {
            await DisplayAlert("Error!", "Error while loading start date", "OK");
        }

        // Repeat the process for EndDate
        if (DateTime.TryParse(selectedItem.EndDate, out DateTime endDate))
        {
            endDatePicker.Date = endDate;
        }
        else
        {
            await DisplayAlert("Error!", "Error while loading EndDate", "Ok");
        }
        yearEntry.Text = selectedItem.Year.ToString();
        semesterEntry.Text = selectedItem.Semester;
        courseNameEntry.SelectedItem = selectedItem.Course;



        Save.Text = "Update";
        _id = selectedItem.ID;

    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Use Navigation to go back
        await Navigation.PopAsync();
    }
}
