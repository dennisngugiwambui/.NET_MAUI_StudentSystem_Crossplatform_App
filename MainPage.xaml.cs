using StudentSystem.ViewModel;
using Microsoft.Maui.Controls;

namespace StudentSystem
{
    public partial class MainPage : ContentPage
    {
            public MainPage()
            {
                InitializeComponent();

               MainViewModel mainViewModel = new MainViewModel();

               BindingContext = mainViewModel;
            }

        // private async void OnTermFrameTapped(object sender, EventArgs e)
        //{
        // Navigate to Term.xaml
        // await Shell.Current.GoToAsync($"//Term");
        //}

        private async void OnTermFrameTapped(object sender, EventArgs e)
        {
            // Use Shell navigation to navigate to the TermPage
            await Shell.Current.GoToAsync($"//{nameof(Term)}");
        }

        private async void OnAssessmentFrameTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(Assesment)}");
        }

        private async void OnCourseFrameTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(Course)}");
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
          

        }
    }

}
