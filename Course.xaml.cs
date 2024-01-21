using SQLite;
using StudentSystem.DataBase;
using System.Formats.Tar;
namespace StudentSystem;
using Microsoft.Maui.Storage;

public partial class Course : ContentPage
{
    DatabaseHelper databaseHelper;

    //ListView courseListView;


    private int _id=0;



    public Course()
    {
        InitializeComponent();

        databaseHelper = new DatabaseHelper();

        //materialsListView = FindByName<ListView>("materialsListView");

        LoadData();
    }

    async void LoadData()
    {
        var result = await databaseHelper.GetRecordAsync();

        //courseListView.ItemsSource = result;
        courseCollectionView.ItemsSource = result;
        //courseCardListView.ItemsSource = result;


    }

    public class CourseRecord
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string? Status { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? InstructorsName { get; set; }
        public string? InstructorsPhone { get; set; }
        public string? InstructorsEmail { get; set; }
        public string? NotificationEntry { get; set; }
        public string? CourseName { get; set; }



    }

    public class FileRecord
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string FilePath { get; set; }

        public string UploadDate { get; set; }
    }



    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            // Get selected course name from the Picker
            string courseName = (string)courseNameEntry.SelectedItem;
            string status = (string)statusEntry.SelectedItem;
            DateTime startDate = startDatePicker.Date;
            DateTime endDate = endDatePicker.Date;


            // Validate selected course name
            if (string.IsNullOrWhiteSpace(courseName))
            {
                await DisplayAlert("Error", "Please select a valid course.", "OK");
                return;
            }

            // Get values from other UI controls
            //string Status = statusEntry.SelectedItem.ToString();
            string InstructorsName = instructorsName.Text;
            string InstructorsPhone = instructorsPhone.Text;
            string InstructorsEmail = instructorsEmail.Text;
            string selectedNotification = notificationOnRadioButton.IsChecked ? "Yes" : "No";

            CourseRecord courseRecord = new CourseRecord();
            if (_id > 0)
            {
                courseRecord.ID = _id;
            }
            courseRecord.CourseName = courseName;
            courseRecord.Status = status;
            //courseRecord.Status = statusEntry.SelectedItem.ToString();
            courseRecord.StartDate = startDate.ToShortDateString();
            courseRecord.EndDate = endDate.ToShortDateString();
            courseRecord.InstructorsName = instructorsName.Text;
            courseRecord.InstructorsPhone = instructorsPhone.Text;
            courseRecord.InstructorsEmail = instructorsEmail.Text;
            courseRecord.NotificationEntry = notificationOnRadioButton.IsChecked ? "Yes" : "No";

            await databaseHelper.SaveRecordAsync(courseRecord);

            if (Save.Text == "Update")
            {
                await DisplayAlert("Success", "Course Updated Successfully", "OK");
            }
            else
            {
                await DisplayAlert("Success", "Course saved Successfully", "OK");
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
        courseNameEntry.SelectedItem = null;
        instructorsEmail.Text = string.Empty;
        instructorsName.Text = string.Empty;
        instructorsPhone.Text = string.Empty;
        //notificationEntry.SelectedItem = null;
        notificationOnRadioButton.IsChecked = false;
        notificationOffRadioButton.IsChecked = false;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        // Get the button that was clicked
        Button deleteButton = (Button)sender;

        // Get the corresponding item from the ListView's BindingContext
        CourseRecord selectedItem = (CourseRecord)deleteButton.BindingContext;

        try
        {
            // Use await when calling DeleteRecordAsync
            await databaseHelper.DeleteRecordAsync(selectedItem);

            await DisplayAlert("Success", "Course Deleted successfully", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }

        LoadData();

    }


    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        // Get the button that was clicked
        Button updateButton = (Button)sender;

        // Get the corresponding item from the ListView's BindingContext
        CourseRecord selectedItem = (CourseRecord)updateButton.BindingContext;

        // Get the selected date from the DatePicker
        string selectedDate = startDatePicker.Date.ToString(); // Adjust the format as needed

        // Set UI controls with values of the selected item
        
        instructorsName.Text = selectedItem.InstructorsName;
        instructorsPhone.Text = selectedItem.InstructorsPhone;
        instructorsEmail.Text = selectedItem.InstructorsEmail;
        string selectedNotification = notificationOnRadioButton.IsChecked ? "Yes" : "No";
        courseNameEntry.SelectedItem = selectedItem.CourseName;

        // Set the Save button text to "Update"
        Save.Text = "Update";

        // Set the _id for reference during update
        _id = selectedItem.ID;



    }
    private void OnStartDateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedStartDate = e.NewDate;
        // Perform any actions you need when the start date is selected
        // For example, show a notification
        DisplayAlert("Start Date Selected", $"Selected Start Date: {selectedStartDate.ToShortDateString()}", "OK");
    }

    private void OnEndDateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedEndDate = e.NewDate;
        // Perform any actions you need when the end date is selected
        // For example, show a notification
        DisplayAlert("End Date Selected", $"Selected End Date: {selectedEndDate.ToShortDateString()}", "OK");
    }

    private async void OnUploadClicked(object sender, EventArgs e)
    {
        try
        {
            var results = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick a file, please",
                FileTypes = FilePickerFileType.Pdf 
            });

            if (results != null)
            {
                // Save the picked file
                string fileName = Path.GetFileName(results.FullPath);
                string destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

                using (var sourceStream = await results.OpenReadAsync())
                {
                    using (var destinationStream = File.Create(destinationPath))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }
                }

                await DisplayAlert("Success", $"File saved successfully at {destinationPath}", "OK");

                // Display the PDF file using WebView
                await DisplayPdfFile(destinationPath);
            }
        }
        catch (Exception ex)
        { 
        
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }

    }

    private async Task DisplayPdfFile(string filePath)
    {
        try
        {
            // Create a WebView to display the PDF file
            WebView pdfWebView = new WebView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Load the PDF file into the WebView
            pdfWebView.Source = new UrlWebViewSource { Url = $"file://{filePath}" };

            // Create a new ContentPage to host the WebView
            ContentPage pdfPage = new ContentPage
            {
                Content = pdfWebView,
                Title = "PDF Viewer"
            };

            // Navigate to the new page
            await Navigation.PushAsync(pdfPage);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while displaying the PDF: {ex.Message}", "OK");
        }
    }

    public async void OnDeleteMaterialClicked(object sender, EventArgs e)
    {
        // Get the button that was clicked
        Button deleteButton = (Button)sender;

        // Get the corresponding item from the ListView's BindingContext
        FileRecord selectedMaterial = (FileRecord)deleteButton.BindingContext;

        

    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Use Navigation to go back
        await Navigation.PopAsync();
    }
}