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
        #region constants
        private const string SUI_WALLET_CREATE_URL = "chrome-extension://opcgpfmipidbgpenhmajoajpbobppdil/ui.html#/welcome";
        private const string BUTTON_GET_START_XPATH = "//a[@class='uDUBihzsBafACO5RMGr7 btn primary']";
        private const string BUTTON_WALLETS_XPATH = "//a[@class='btn MuTCPVF5Yv8Jw2J3JDL1']";
        private const string BUTTON_MENU_XPATH = "//a[@class='Pxyoz1jJ8wYGMCqDOEMG R0arsswJgfnkyaXeHjjT']";
        private const string BUTTON_ACCOUNT_CLASS = "M_3pz_0ZtIX322QlPAJr";
        private const string BUTTON_LOGOUT_CLASS = "Qka29CBKA19ik0cUSlIx";
        private const string PASSWORD_BOXES_CLASS = "TH2aGaKvkCfbW1zH9HHX";
        private const string CHECKBOX_ARGEE_CLASS = "MmDbA0GzFptVTURsV5YU";
        private const string BUTTON_CREATE_XPATH = "//button[@class='qvmPyb8NdzsMrFWoHAEN SH1FDQZ9VX53OguPvdQU Ha6uGJWJ9hrjJCBvvs5T']";
        private const string RECOVERY_BOX_CLASS = "FcQQKvA6jyUfX4nnitAd";
        private const string BUTTON_OPEN_WALLET_XPATH = "//button[@class='qvmPyb8NdzsMrFWoHAEN Rje_ybCV053wusA9gaJx SH1FDQZ9VX53OguPvdQU Ha6uGJWJ9hrjJCBvvs5T']";
        private const string COPY = "COPY";
        #endregion

        private string chromeExtentionPath = "D:/SideProjects/SuiWallet/sui_wallet_auto_gen_acc/opcgpfmipidbgpenhmajoajpbobppdil.crx";
        private string fileAccountPath = string.Empty;
        private bool isChromePathSelected;
        private bool isAccountPathSelected;
        private string accountPassword = "kiepdoden@123";

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
            //if (!(isAccountPathSelected || isChromePathSelected))
            //{
            //    MessageBox.Show("Please select all the paths and try again.", "Start error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //Add Sui Wallet extension for Chrome
            ChromeOptions options = new ChromeOptions();
            options.AddExtension(chromeExtentionPath);

            //Hide console prompt
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            //Create Chrome driver
            ChromeDriver chromeDriver = new ChromeDriver(service, options);
            chromeDriver.Url = SUI_WALLET_CREATE_URL;
            var tabs = chromeDriver.WindowHandles;
            if (tabs.Count > 1)
            {
                chromeDriver.SwitchTo().Window(tabs[1]);
                chromeDriver.Close();
                chromeDriver.SwitchTo().Window(tabs[0]);
            }
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            //Start creating account
            for (int i = 0; i < 10; i++)
            {
                var buttonGetStart = chromeDriver.FindElement(By.XPath(BUTTON_GET_START_XPATH));
                if (buttonGetStart != null)
                {
                    buttonGetStart.Click();
                }
                var buttonWallets = chromeDriver.FindElements(By.XPath(BUTTON_WALLETS_XPATH));
                if (buttonWallets.Count > 1)
                {
                    buttonWallets[0].Click();
                }
                var passwordBoxes = chromeDriver.FindElements(By.ClassName(PASSWORD_BOXES_CLASS));
                if (passwordBoxes.Count > 1)
                {
                    var createPasswordBox = passwordBoxes[0];
                    createPasswordBox.SendKeys(accountPassword);
                    var confirmPasswordBox = passwordBoxes[1];
                    confirmPasswordBox.SendKeys(accountPassword);
                }
                var checkboxArgee = chromeDriver.FindElement(By.ClassName(CHECKBOX_ARGEE_CLASS));
                if (checkboxArgee != null)
                {
                    checkboxArgee.Click();
                }
                var buttonCreate = chromeDriver.FindElement(By.XPath(BUTTON_CREATE_XPATH));
                if (buttonCreate != null)
                {
                    buttonCreate.Click();
                }
                var recoveryBox = chromeDriver.FindElement(By.ClassName(RECOVERY_BOX_CLASS));
                if (recoveryBox != null)
                {
                    string recoveryCode = recoveryBox.Text.Replace(COPY, string.Empty).Replace("  ", " ").Trim();
                    //TODO write down the 12 words code
                }
                var buttonOpenWallet = chromeDriver.FindElement(By.XPath(BUTTON_OPEN_WALLET_XPATH));
                if (buttonOpenWallet != null)
                {
                    buttonOpenWallet.Click();
                }
                var buttonMenu = chromeDriver.FindElement(By.XPath(BUTTON_MENU_XPATH));
                if (buttonMenu != null)
                {
                    buttonMenu.Click();
                }
                var buttonAccount = chromeDriver.FindElement(By.ClassName(BUTTON_ACCOUNT_CLASS));
                if (buttonAccount != null)
                {
                    buttonAccount.Click();
                }
                var buttonLogout = chromeDriver.FindElement(By.ClassName(BUTTON_LOGOUT_CLASS));
                if (buttonLogout != null)
                {
                    buttonLogout.Click();
                }
            }
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
