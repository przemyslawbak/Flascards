using Autofac;
using Flashcards.DataAccess;
using Flashcards.Startup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Flashcards
{
    public partial class App : Application
    {
        public App()
        {
            var bootStrapper = new BootStrapper(); //Autofac class
            var container = bootStrapper.BootStrap(); //Autofac class
            MainPage = new NavigationPage(container.Resolve<MainPage>());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
