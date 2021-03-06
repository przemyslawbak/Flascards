﻿using Autofac;
using Flashcards.DataAccess;
using Flashcards.DataProvider;
using Flashcards.ViewModels;
using Prism.Events;
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
            builder.RegisterType<EventAggregator>()
              .As<IEventAggregator>().SingleInstance();
            builder.RegisterType<DataRepository>()
                .As<IDataRepository>();
            builder.RegisterType<MainPage>()
                .AsSelf();
            builder.RegisterType<MainPageViewModel>()
                .AsSelf();
            builder.RegisterType<PhraseEditViewModel>()
                .As<IPhraseEditViewModel>();
            builder.RegisterType<GroupPageViewModel>()
                .As<IGroupPageViewModel>();
            builder.RegisterType<MainDataProvider>()
                .As<IMainDataProvider>();
            builder.RegisterType<PhraseDataProvider>()
                .As<IPhraseDataProvider>();
            return builder.Build();
        }
    }
}
