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
        private string chromeExtentionPath = "D:/SideProjects/SuiWallet/sui_wallet_auto_gen_acc/opcgpfmipidbgpenhmajoajpbobppdil.crx";
        private string fileAccountPath = string.Empty;
        private bool isChromePathSelected;
        private bool isAccountPathSelected;
        private string accountPassword = "kiepdoden@123";
        private string SUI_WALLET_CREATE_URL = "chrome-extension://opcgpfmipidbgpenhmajoajpbobppdil/ui.html#/initialize/create";


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
            //if (!isAccountPathSelected && !isChromePathSelected)
            //{
            //    MessageBox.Show("Please select all the paths and try again.", "Start error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            ChromeOptions options = new ChromeOptions();
            options.AddExtension(chromeExtentionPath);

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            ChromeDriver chromeDriver = new ChromeDriver(service, options);
            chromeDriver.Url = SUI_WALLET_CREATE_URL;
            var tabs = chromeDriver.WindowHandles;
            if (tabs.Count > 1)
            {
                chromeDriver.SwitchTo().Window(tabs[1]);
                chromeDriver.Close();
                chromeDriver.SwitchTo().Window(tabs[0]);
            }
            var passwordBoxes = chromeDriver.FindElements(By.ClassName("TH2aGaKvkCfbW1zH9HHX"));
            var createPasswordBox = passwordBoxes[0];
            createPasswordBox.SendKeys(accountPassword);
            var confirmPasswordBox = passwordBoxes[1];
            confirmPasswordBox.SendKeys(accountPassword);
            var checkboxArgee = chromeDriver.FindElement(By.ClassName("MmDbA0GzFptVTURsV5YU"));
            checkboxArgee.Click();
            var buttonCreate = chromeDriver.FindElement(By.XPath("//button[@class='qvmPyb8NdzsMrFWoHAEN SH1FDQZ9VX53OguPvdQU Ha6uGJWJ9hrjJCBvvs5T']"));
            buttonCreate.Click();
        }

        private void buttonChromePath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAccountPath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
