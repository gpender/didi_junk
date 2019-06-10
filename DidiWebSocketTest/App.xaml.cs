using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models;
using DidiWebSocketTest.ViewModels;
using Microsoft.Practices.Unity;

namespace DidiWebSocketTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IUnityContainer container
        {
            get { return UnityContainerProvider.Instance.GetContainer(); }
        }
        public App()
        {
            SciChart.Charting.Licensing.SciChartSurfaceLicenseProvider.SetRuntimeLicenseKey(@"<LicenseContract>
              <Customer>Parker Hannifin Manufacturing Ltd</Customer>
              <OrderId>ABT160314-8204-23119</OrderId>
              <LicenseCount>1</LicenseCount>
              <IsTrialLicense>false</IsTrialLicense>
              <SupportExpires>03/14/2017 00:00:00</SupportExpires>
              <ProductCode>SC-WPF-2D-PRO</ProductCode>
              <KeyCode>lwAAAAEAAABC9Mq2133RAYEAQ3VzdG9tZXI9UGFya2VyIEhhbm5pZmluIE1hbnVmYWN0dXJpbmcgTHRkO09yZGVySWQ9QUJUMTYwMzE0LTgyMDQtMjMxMTk7U3Vic2NyaXB0aW9uVmFsaWRUbz0xNC1NYXItMjAxNztQcm9kdWN0Q29kZT1TQy1XUEYtMkQtUFJPBRwBPJYeY3CWvAzPYInOiHJIzG0ZLLcHdhlOu5gCAFAytPkbxtKE5jtJxNYvl+AS</KeyCode>
            </LicenseContract>");
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainVM = container.Resolve<MainVM>();
            var window = new MainWindow { DataContext = mainVM };
            window.Show();
        }
    }
    public class UnityContainerProvider
    {
        private static UnityContainerProvider instance = null;
        private static readonly object padlock = new object();
        IUnityContainer container;
        string ipAddress = "169.254.3.17";
        UnityContainerProvider() { }
        public static UnityContainerProvider Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UnityContainerProvider();
                    }
                    return instance;
                }
            }
        }
        public IUnityContainer GetContainer()
        {
            if (container == null)
            {
                container = new UnityContainer();
                container.RegisterType<IProtocol, TestProtocol>(new ContainerControlledLifetimeManager(), new InjectionConstructor(new object[] { new WebSocketTransport(), ipAddress }));
                container.RegisterType<IProtocol, ScopeProtocol>(new ContainerControlledLifetimeManager(), new InjectionConstructor(new object[] { new WebSocketTransport(), ipAddress }));
                container.RegisterType<ITransport, WebSocketTransport>(new ContainerControlledLifetimeManager());
                container.RegisterType<IUtilityServices, UtilityServices>(new ContainerControlledLifetimeManager());
                container.RegisterType<WsScopeProtocolVM>();
                container.RegisterType<ScopeVM>();
                container.RegisterType<WsTestProtocolVM>();
            }
            return container;
        }
    }

}
