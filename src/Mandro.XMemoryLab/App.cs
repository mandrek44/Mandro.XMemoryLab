using System;
using Xamarin.Forms;

namespace Mandro.XMemoryLab
{
    public class App : Application
    {
        public static INavigation Navigation { get; set; }

        protected override void OnStart()
        {
            base.OnStart();

            // MEMORY LEAK: Switch following two lines to observe massive memory leak
            //var navigationPage = new NavigationPage(new OutOfMemoryExceptionPage(1));
            var navigationPage = new NavigationPage(new MemorySafePage(1));

            Navigation = navigationPage.Navigation;
            MainPage = navigationPage;
        }
    }

    public class MemorySafePage : ContentPage
    {
        private readonly int _count;

        public MemorySafePage(int count) 
        {
            _count = count;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Content = TestContent.Create(_count, () => new MemorySafePage(_count + 1));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Content = null;
        }
    }

    public class OutOfMemoryExceptionPage : ContentPage
    {
        public OutOfMemoryExceptionPage(int count)
        {
            Content = TestContent.Create(count, () => new OutOfMemoryExceptionPage(count + 1));
        }
    }

    public static class TestContent
    {
        private const int MaxCount = 5;

        public static View Create(int count, Func<Page> nextPageFactory)
        {
            var actionButton = new Button() { BackgroundColor = Color.Green };
            if (count >= MaxCount)
            {
                actionButton.Text = "Return to root";
                actionButton.Clicked += (sender, args) => App.Navigation.PopToRootAsync();
            }
            else
            {
                actionButton.Text = "Next " + (MaxCount - count);
                actionButton.Clicked += (sender, args) => App.Navigation.PushAsync(nextPageFactory());
            }

            return new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Image() { Source = ImageSource.FromFile("xamarin.png")},
                    new Label
                    {
                        XAlign = TextAlignment.Center,
                        Text = "Welcome to Xamarin Forms!"
                    },
                    actionButton
                }
            };
        }
    }
}
