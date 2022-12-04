using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sui_wallet_auto_gen_acc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SUI_WALLET_EXTENSION_PATH = "D:/SideProjects/SuiWallet/sui_wallet_auto_gen_acc/opcgpfmipidbgpenhmajoajpbobppdil.crx";
        private string GET_START_BUTTON_CLASS = "//a[@class='uDUBihzsBafACO5RMGr7 btn primary']";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            GenerateAccount();
        }

        private void GenerateAccount()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddExtension(SUI_WALLET_EXTENSION_PATH);
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
            ChromeDriver chromeDriver = new ChromeDriver(service, options);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            var getStartBtn = chromeDriver.FindElements(By.XPath(GET_START_BUTTON_CLASS));
        }
    }
}
