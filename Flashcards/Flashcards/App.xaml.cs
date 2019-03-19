using Autofac;
using Flashcards.DataProvider;
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
            var bootStrapper = new BootStrapper(); //zmienna dla interfejsów Autofac
            var container = bootStrapper.BootStrap(); //zmienna dla odpalenia kontenera Autofac
            MainPage = new NavigationPage(container.Resolve<MainPage>());
        }

        static DatabaseRepository database;
        public static DatabaseRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseRepository();
                }
                return database;
            }
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
