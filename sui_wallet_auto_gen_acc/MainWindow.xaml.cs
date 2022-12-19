using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Shapes;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.LinkLabel;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

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
        private const string SUI_WALLET_EXTENSION_FILE_NAME = "opcgpfmipidbgpenhmajoajpbobppdil.crx";
        private const string DEFAULT_ACCOUNT_PASSWORD = "kiepdoden@123";
        private const string ACCOUNT_FILE_NAME = "\\accounts.txt";
        private const double WAIT_FOR_ELEMENT = 5;
        private const string TWITTER_LOGIN_PATH = "https://twitter.com";
        private const string TWITTER_INPUT_XPATH = "//input[@class='r-30o5oe r-1niwhzg r-17gur6a r-1yadl64 r-deolkf r-homxoj r-poiln3 r-7cikom r-1ny4l3l r-t60dpp r-1dz5y72 r-fdjqy7 r-13qz1uu']";
        private const string TWITTER_NEXT_BTN_XPATH = "//div[@class='css-18t94o4 css-1dbjc4n r-42olwf r-sdzlij r-1phboty r-rs99b7 r-ywje51 r-usiww2 r-2yi16 r-1qi8awa r-1ny4l3l r-ymttw5 r-o7ynqc r-6416eg r-lrvibr r-13qz1uu']";
        private const string TWITTER_LOGIN_BTN_XPATH = "//div[@data-testid='LoginForm_Login_Button']";
        private const string TWITTER_TWEET_BOX_XPATH = "//div[@class='public-DraftStyleDefault-block public-DraftStyleDefault-ltr']";
        private const string TWITTER_TWEET_BTN_XPATH = "//div[@class='css-18t94o4 css-1dbjc4n r-l5o3uw r-42olwf r-sdzlij r-1phboty r-rs99b7 r-19u6a5r r-2yi16 r-1qi8awa r-1ny4l3l r-ymttw5 r-o7ynqc r-6416eg r-lrvibr']";
        private const string TWITTER_SWITCH_ACC_BTN_XPATH = "//div[@data-testid='SideNav_AccountSwitcher_Button']";
        private const string TWITTER_LOGOUT_ACC_BTN_XPATH = "//a[@data-testid='AccountSwitcher_Logout_Button']";
        private const string TWITTER_LOGOUT_CONFIRM_BTN_XPATH = "//div[@data-testid='confirmationSheetConfirm']";
        private const string TWITTER_LOGIN_MAIN_BTN_XPATH = "//a[@data-testid='login']";
        private const string TWITTER_LOGIN_MAIN_BTN2_XPATH = "//a[@data-testid='loginButton']";
        #endregion

        private string chromeExtentionPath = string.Empty;
        private string fileAccountTwitterPath = string.Empty;
        private string fileAccountPath = string.Empty;
        private bool isChromePathSelected;
        private bool isAccountPathSelected;
        private bool isAccountTwitterPathSelected;
        private string accountPassword = string.Empty;
        private string twitterStatus = string.Empty;
        private StreamWriter sw;
        private StreamReader sr;
        private FolderBrowserDialog folderBrowserDialog;
        private OpenFileDialog openFileDialog;

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
            if (!isAccountPathSelected || !isChromePathSelected)
            {
                MessageBox.Show("Please select all the paths and try again.", "Start error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Add Sui Wallet extension for Chrome
            ChromeOptions options = new ChromeOptions();
            options.AddExtension(chromeExtentionPath);

            ChromeDriver chromeDriver = CreateChromeDriver(options, SUI_WALLET_CREATE_URL);
            var tabs = chromeDriver.WindowHandles;
            if (tabs.Count > 1)
            {
                chromeDriver.SwitchTo().Window(tabs[1]);
                chromeDriver.Close();
                chromeDriver.SwitchTo().Window(tabs[0]);
            }

            //Start creating account
            accountPassword = !string.IsNullOrWhiteSpace(textboxPassword.Text.Trim()) ? textboxPassword.Text.Trim() : DEFAULT_ACCOUNT_PASSWORD;
            WriteFile(accountPassword);
            while (true)
            {
                try
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
                        //Init StreamWriter
                        WriteFile(recoveryCode);
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
                catch (NoSuchWindowException)
                {
                    MessageBox.Show("Auto stopped.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private static ChromeDriver CreateChromeDriver(ChromeOptions? options = null, string url = "")
        {
            //Hide console prompt
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            ChromeDriver chromeDriver;
            if (options != null)
            {
                chromeDriver = new ChromeDriver(service, options);
            }
            else
            {
                chromeDriver = new ChromeDriver(service);
            }
            chromeDriver.Url = url;
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(WAIT_FOR_ELEMENT);

            return chromeDriver;
        }

        private void WriteFile(string content)
        {
            sw = new StreamWriter(fileAccountPath + ACCOUNT_FILE_NAME, append: true);
            sw.WriteLine(content);
            sw.Close();
        }

        private void buttonChromePath_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName) && openFileDialog.FileName.Contains(SUI_WALLET_EXTENSION_FILE_NAME))
            {
                chromeExtentionPath = openFileDialog.FileName;
                labelChromePath.Text = chromeExtentionPath;
                isChromePathSelected = true;
            }
            else
            {
                MessageBox.Show("Please select Sui Wallet extention .crx file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isChromePathSelected = false;
            }
        }

        private void buttonAccountPath_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                fileAccountPath = folderBrowserDialog.SelectedPath;
                labelAccountPath.Text = fileAccountPath;
                isAccountPathSelected = true;
            }
            else
            {
                MessageBox.Show("Invalid folder path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isAccountPathSelected = false;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void btnCommentSwitch_Click(object sender, RoutedEventArgs e)
        {
            StartAutoTwitter();
        }

        private void StartAutoTwitter()
        {
            if (!isAccountTwitterPathSelected)
            {
                MessageBox.Show("Please select the accounts file path and try again.", "Start error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sr = new StreamReader(fileAccountTwitterPath);
            ChromeDriver chromeDriver = CreateChromeDriver(null, TWITTER_LOGIN_PATH);
            string twitterLine;
            string[] twitterAccount;
            Random random = new Random();
            try
            {
                while ((twitterLine = sr.ReadLine()) != null)
                {
                    twitterAccount = twitterLine.Split(' ');
                    if (twitterAccount.Length == 0)
                    {
                        break;
                    }
                    try
                    {
                        var buttonLoginMain = chromeDriver.FindElement(By.XPath(TWITTER_LOGIN_MAIN_BTN_XPATH));
                        buttonLoginMain.Click();
                    }
                    catch (NoSuchElementException)
                    {
                        var buttonLoginMain = chromeDriver.FindElement(By.XPath(TWITTER_LOGIN_MAIN_BTN2_XPATH));
                        buttonLoginMain.Click();
                    }
                    var userNameBox = chromeDriver.FindElement(By.XPath(TWITTER_INPUT_XPATH));
                    if (userNameBox != null)
                    {
                        userNameBox.SendKeys(twitterAccount[0].Trim());
                    }
                    var buttonNext = chromeDriver.FindElement(By.XPath(TWITTER_NEXT_BTN_XPATH));
                    if (buttonNext != null)
                    {
                        buttonNext.Click();
                    }
                    var passwordBox = chromeDriver.FindElement(By.XPath(TWITTER_INPUT_XPATH));
                    if (passwordBox != null)
                    {
                        passwordBox.SendKeys(twitterAccount[1].Trim());
                    }
                    var buttonLogin = chromeDriver.FindElement(By.XPath(TWITTER_LOGIN_BTN_XPATH));
                    if (buttonLogin != null)
                    {
                        buttonLogin.Click();
                    }
                    var twitterBox = chromeDriver.FindElement(By.XPath(TWITTER_TWEET_BOX_XPATH));
                    if (twitterBox != null)
                    {
                        twitterStatus = !string.IsNullOrWhiteSpace(textboxComment.Text.Trim()) ? textboxComment.Text.Trim() : DEFAULT_ACCOUNT_PASSWORD;
                        twitterBox.SendKeys(twitterStatus + random.Next());
                    }
                    var buttonTweet = chromeDriver.FindElement(By.XPath(TWITTER_TWEET_BTN_XPATH));
                    if (buttonTweet != null)
                    {
                        buttonTweet.Click();
                    }
                    var buttonSwitchAcc = chromeDriver.FindElement(By.XPath(TWITTER_SWITCH_ACC_BTN_XPATH));
                    if (buttonSwitchAcc != null)
                    {
                        buttonSwitchAcc.Click();
                    }
                    var buttonLogoutAcc = chromeDriver.FindElement(By.XPath(TWITTER_LOGOUT_ACC_BTN_XPATH));
                    if (buttonLogoutAcc != null)
                    {
                        buttonLogoutAcc.Click();
                    }
                    var buttonLogoutConfirm = chromeDriver.FindElement(By.XPath(TWITTER_LOGOUT_CONFIRM_BTN_XPATH));
                    if (buttonLogoutConfirm != null)
                    {
                        buttonLogoutConfirm.Click();
                    }
                }
                MessageBox.Show("Auto stopped.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void buttonAccountTwPath_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                fileAccountTwitterPath = openFileDialog.FileName;
                labelAccountTwPath.Text = fileAccountTwitterPath;
                isAccountTwitterPathSelected = true;
            }
            else
            {
                MessageBox.Show("Please select file contants your twitter accounts.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isAccountTwitterPathSelected = false;
            }
        }
    }
}
