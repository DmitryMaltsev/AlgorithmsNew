namespace MauiTest
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            var res = PickSingleFile();
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        public async Task PickSingleFile()
        {
            try
            {
                var pickOptions = new PickOptions
                {
                    PickerTitle = "Выберите bin файл прошивки"
                };
                FileResult? result = await FilePicker.Default.PickAsync(pickOptions);
                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    // ... further processing with the stream
                }
            }
            catch (TaskCanceledException)
            {
                // User canceled the operation
            }
            catch (Exception ex)
            {
                // Handle other exceptions 
            }
        }
    }

}
