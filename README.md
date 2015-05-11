# Mandro.XMemoryLab
Project for testing memory leaks in Xamarin.Forms

Check the src\Mandro.XMemoryLab\App.cs file and switch between two types of pages for testing: OutOfMemoryExceptionPage and MemorySafePage.

When testing monitor the heap size (i.e. using monitor from Android SDK). When clicking the button in OutOfMemoryExceptionPage you'll see that heap grows until OutOfMemoryException. When using MemorySafePage, the heap stays withing (almost) constant limits of size.

## MemorySafePage

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

The idea is simple - just null the Content of the page of Disapearring.
