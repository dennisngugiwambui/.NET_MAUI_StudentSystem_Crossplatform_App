using System;
using Microsoft.Maui.Controls;
using SQLite;
using StudentSystem.DataBase;

namespace StudentSystem
{
    public partial class Assesment : ContentPage
    {
        DatabaseHelper databaseHelper;

        private int _id = 0;
        public Assesment()
        {
            InitializeComponent();

            databaseHelper = new DatabaseHelper();

            LoadData();

        }

        async void LoadData()
        {
            var asses = await databaseHelper.GetAssessmentsAsync();

            recordsListView.ItemsSource = asses;

            //recordsLabel.IsVisible = assessments.Count == 0;

        }

        public  class AssessmentRecord
        {
            [PrimaryKey, AutoIncrement]
            public int ID { get; set; }
            //public string? AssessmentType { get; set; }
            public string? AssessmentDate { get; set; }

            public string? AssessmentType { get; set; }

            public string? AssessmentInfo { get; set; }

            public string? AssessmentNotification { get; set; }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Get values from UI controls
                DateTime AssessmentDate = assessmentDatePicker.Date;
                string? AssessmentType = assessmentTypePicker.SelectedItem?.ToString();
                string? AssessmentInfo = assessmentInfo.ToString();
                string AssessmentNotification = assessmentNotification.SelectedItem.ToString();

                // Validate the values
                if (AssessmentDate == default || string.IsNullOrWhiteSpace(AssessmentType))
                {
                    await DisplayAlert("Error", "Please fill in all the required fields.", "OK");
                    return;
                }

                AssessmentRecord assessmentRecord = new AssessmentRecord();

                if (_id > 0)
                {
                    assessmentRecord.ID = _id;
                }

                // Assign validated values
                assessmentRecord.AssessmentDate = AssessmentDate.ToString();
                assessmentRecord.AssessmentType = AssessmentType;
                assessmentRecord.AssessmentInfo = AssessmentInfo;
                assessmentRecord.AssessmentNotification = AssessmentNotification;

                await databaseHelper.SaveAssessmentAsync(assessmentRecord);

                if (Save.Text == "Update")
                {

                    await DisplayAlert("Success", "Assessment Updated successfully", "OK");
                }
                else
                {
                    await DisplayAlert("Success", "Assessmnent saved Successfully", "OK");

                }

                LoadData();

                ClearFields();

                Save.Text = "Save";
                _id = -1;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void ClearFields()
        {
            assessmentDatePicker.Date = DateTime.Now;
            assessmentTypePicker.Title = "Select Assessment Type";
            assessmentInfo.Text = string.Empty;
            assessmentNotification.SelectedItem = null;
        }


        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Get the button that was clicked
            Button deleteButton = (Button)sender;

            // Get the corresponding item from the ListView's BindingContext

            AssessmentRecord selectedItem = (AssessmentRecord)deleteButton.BindingContext;

            try
            {
                await databaseHelper.DeleteAssessmentAsync(selectedItem);

                await DisplayAlert("Success", "Assessment Deleted successfully", "OK");
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

            AssessmentRecord selectedItem = (AssessmentRecord)updateButton.BindingContext;

            //assessmentDatePicker.Date = selectedItem.AssessmentDate.ToString();
            //assessmentTypePicker.Text = selectedItem.AssessmentType;

            // Convert the string to DateTime using DateTime.TryParse
            if (DateTime.TryParse(selectedItem.AssessmentDate, out DateTime assessmentDate))
            {
                assessmentDatePicker.Date = assessmentDate;
            }
            else
            {
                await DisplayAlert("Error", "Error in loading date", "OK");
            }
            
            assessmentTypePicker.SelectedItem = selectedItem.AssessmentType;



            Save.Text = "Update";
            _id = selectedItem.ID;
        }







        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Use Navigation to go back
            await Navigation.PopAsync();
        }
    }
}