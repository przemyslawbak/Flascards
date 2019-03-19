using Autofac;
using Flashcards.DataAccess;
using Flashcards.DataProvider;
using Flashcards.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DatabaseRepository>()
                .As<IDatabaseRepository>();
            builder.RegisterType<MainPage>()
                .AsSelf();
            builder.RegisterType<MainPageViewModel>()
                .AsSelf();
            builder.RegisterType<MainDataProvider>()
                .As<IMainDataProvider>();
            builder.RegisterType<PhraseDataProvider>()
                .As<IPhraseDataProvider>();
            return builder.Build();
        }
    }
}
