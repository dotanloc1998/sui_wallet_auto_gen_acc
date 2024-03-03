using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
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
        private const string BUTTON_GET_START_XPATH = "/html/body/div/div/div[1]/div/div/div[2]";
        private const string BUTTON_CREATE_NEW_WALLET_XPATH = "/html/body/div/div/div[1]/div[1]/a";
        private const string BUTTON_DISPLAY_RECOVERY_PHRASE_XPATH = "/html/body/div/div/div[1]/div[3]/div/div[3]/div[2]/div[2]/button";
        private const string BUTTON_MENU_XPATH = "/html/body/div/div/div[1]/header/div[3]";
        private const string BUTTON_LOGOUT_XPATH = "/html/body/div/div/div[1]/div/div/div[2]/div[4]/button[2]";
        private const string BUTTON_CONFIRM_LOGOUT_XPATH = "/html/body/div[2]/div/div/div/div[2]/div/div[3]/div/button[2]";
        private const string PASSWORD_BOXES_TAGNAME = "input";
        private const string CHECKBOX_ARGEE_XPATH = "/html/body/div/div/div[1]/form/div/fieldset/label[3]/span";
        private const string CHECKBOX_SAVED_PHRASE_XPATH = "/html/body/div/div/div[1]/div[3]/div/div[7]/label/span";
        private const string BUTTON_CREATE_XPATH = "/html/body/div/div/div[1]/form/button";
        private const string RECOVERY_BOX_XPATH = "/html/body/div/div/div[1]/div[3]/div/div[3]/div[1]/div[1]/div";
        private const string BUTTON_OPEN_WALLET_XPATH = "/html/body/div/div/div[1]/div[3]/a";
        private const string COPY = "COPY";
        private const string SUI_WALLET_EXTENSION_FILE_NAME = "opcgpfmipidbgpenhmajoajpbobppdil.crx";
        private const string DEFAULT_ACCOUNT_PASSWORD = "kiepdoden@123";
        private const string ACCOUNT_FILE_NAME = "\\accounts.txt";
        private const string CHECKED_PASS = "\\checkedPassword.txt";
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
        private const string FACEBOOK_URL = "https://www.facebook.com/login";
        private const string FACEBOOK_INPUT_EMAIL_PHONE_ID = "email";
        private const string FACEBOOK_INPUT_PASS_ID = "pass";
        private const string FACEBOOK_BUTTON_LOGIN_XPATH = "/html/body/div[1]/div[1]/div[1]/div/div/div/div[2]/div/div[1]/form/div[2]/button";
        private const string FACEBOOK_BUTTON_LOGINWITHPASSWORD_XPATH = "/html/body/div[1]/div[1]/div[1]/div/div/form/div/div[3]/div/div[2]/a";
        private const string FACEBOOK_BUTTON_LOGINWITHPASSWORD_ID = "loginbutton";
        private const string FACEBOOK_LABEL_FORGOTPASS_XPATH = "/html/body/div[1]/div[1]/div[1]/div/div[2]/div[2]/form/div/div[2]/div[2]/div/div/div/span";
        private const string FACEBOOK_FORGOTPASS = "Forgotten password?";
        private const string FACEBOOK_ERROR_BOX_XPATH = "/html/body/div[1]/div[1]/div[1]/div/div[2]/div[2]/form/div[1]/div[1]";
        private const string FACEBOOK_CREDENTIALS = "Wrong credentials";
        #endregion

        private string chromeExtentionPath = string.Empty;
        private string fileAccountTwitterPath = string.Empty;
        private string fileAccountPath = string.Empty;
        private bool isChromePathSelected;
        private bool isAccountPathSelected;
        private bool isAccountTwitterPathSelected;
        private bool isAccountFBPathSelected;
        private string fileAccountFBPath = string.Empty;
        private string accountPassword = string.Empty;
        private string checkedPasswordsPath = string.Empty;
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
            WriteFile(fileAccountPath, ACCOUNT_FILE_NAME, accountPassword);
            bool shouldCreate = string.IsNullOrEmpty(textboxTimes.Text);
            int numberOfAccounts = 0;
            int count = 0;
            if (!shouldCreate)
            {
                numberOfAccounts = int.Parse(textboxTimes.Text.Trim());
            }
            while (shouldCreate || count < numberOfAccounts)
            {
                count++;
                try
                {
                    chromeDriver.Navigate().Refresh();
                    var buttonGetStart = chromeDriver.FindElement(By.XPath(BUTTON_GET_START_XPATH));
                    if (buttonGetStart != null)
                    {
                        buttonGetStart.Click();
                    }
                    var buttonWallet = chromeDriver.FindElement(By.XPath(BUTTON_CREATE_NEW_WALLET_XPATH));
                    if (buttonWallet != null)
                    {
                        buttonWallet.Click();
                    }
                    var passwordBoxes = chromeDriver.FindElements(By.TagName(PASSWORD_BOXES_TAGNAME));
                    if (passwordBoxes.Count > 1)
                    {
                        var createPasswordBox = passwordBoxes[0];
                        createPasswordBox.SendKeys(accountPassword);
                        var confirmPasswordBox = passwordBoxes[1];
                        confirmPasswordBox.SendKeys(accountPassword);
                    }
                    var checkboxArgee = chromeDriver.FindElement(By.XPath(CHECKBOX_ARGEE_XPATH));
                    if (checkboxArgee != null)
                    {
                        checkboxArgee.Click();
                    }
                    var buttonCreate = chromeDriver.FindElement(By.XPath(BUTTON_CREATE_XPATH));
                    if (buttonCreate != null)
                    {
                        buttonCreate.Click();
                    }
                    var buttonDisplayPhrase = chromeDriver.FindElement(By.XPath(BUTTON_DISPLAY_RECOVERY_PHRASE_XPATH));
                    if (buttonDisplayPhrase != null)
                    {
                        buttonDisplayPhrase.Click();
                    }
                    var recoveryBox = chromeDriver.FindElement(By.XPath(RECOVERY_BOX_XPATH));
                    if (recoveryBox != null)
                    {
                        string recoveryCode = recoveryBox.Text.Replace(COPY, string.Empty).Replace("  ", " ").Trim();
                        //Init StreamWriter
                        WriteFile(fileAccountPath, ACCOUNT_FILE_NAME, recoveryCode);
                    }
                    var checkboxSavedPhrase = chromeDriver.FindElement(By.XPath(CHECKBOX_SAVED_PHRASE_XPATH));
                    if (checkboxSavedPhrase != null)
                    {
                        checkboxSavedPhrase.Click();
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
                    var buttonLogout = chromeDriver.FindElement(By.XPath(BUTTON_LOGOUT_XPATH));
                    if (buttonLogout != null)
                    {
                        buttonLogout.Click();
                    }
                    var buttonConfirmLogout = chromeDriver.FindElement(By.XPath(BUTTON_CONFIRM_LOGOUT_XPATH));
                    if (buttonConfirmLogout != null)
                    {
                        buttonConfirmLogout.Click();
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

        private void WriteFile(string path, string fileName, string content)
        {
            sw = new StreamWriter(path + fileName, append: true);
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

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!isAccountFBPathSelected)
            {
                MessageBox.Show("Please select the accounts file path and try again.", "Start error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sr = new StreamReader(fileAccountFBPath);
            ChromeDriver chromeDriver = CreateChromeDriver(null, FACEBOOK_URL);
            string fbPass;
            var inputEmailPhone = chromeDriver.FindElement(By.Id(FACEBOOK_INPUT_EMAIL_PHONE_ID));
            if (inputEmailPhone != null)
            {
                inputEmailPhone.SendKeys(textboxEmailOrPhone.Text);
            }
            var inputPass = chromeDriver.FindElement(By.Id(FACEBOOK_INPUT_PASS_ID));
            if (inputPass != null)
            {
                inputPass.SendKeys("1234aaa5678*9");
            }
            var inputBtnLogin = chromeDriver.FindElement(By.Id(FACEBOOK_BUTTON_LOGINWITHPASSWORD_ID));
            if (inputBtnLogin != null)
            {
                inputBtnLogin.Click();
            }
            while ((fbPass = sr.ReadLine()) != null)
            {
                chromeDriver.Navigate().Refresh();
                inputPass = chromeDriver.FindElement(By.Id(FACEBOOK_INPUT_PASS_ID));
                if (inputPass != null)
                {
                    inputPass.SendKeys(fbPass);
                }

                inputBtnLogin = chromeDriver.FindElement(By.Id(FACEBOOK_BUTTON_LOGINWITHPASSWORD_ID));
                if (inputBtnLogin != null)
                {
                    inputBtnLogin.Click();
                }
                WriteFile(checkedPasswordsPath, CHECKED_PASS, fbPass);
                //var labelForgotPassword = chromeDriver.FindElement(By.XPath(FACEBOOK_LABEL_FORGOTPASS_XPATH));
                //if (labelForgotPassword != null)
                //{
                //    if (labelForgotPassword.Text != FACEBOOK_FORGOTPASS)
                //    {
                //        WriteFile(fileAccountFBPath, CHECKED_PASS, fbPass);
                //        break;
                //    }
                //}
                var errorBox = chromeDriver.FindElement(By.XPath(FACEBOOK_ERROR_BOX_XPATH));
                if (errorBox != null)
                {
                    if (errorBox.Text != FACEBOOK_CREDENTIALS)
                    {
                        WriteFile(checkedPasswordsPath, CHECKED_PASS, fbPass);
                        break;
                    }
                }
            }
            Process.Start("shutdown", "/s /t 0");
        }

        private void buttonAccountFBPath_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                fileAccountFBPath = openFileDialog.FileName;
                labelAccountFBPath.Text = fileAccountFBPath;
                isAccountFBPathSelected = true;
            }
            else
            {
                MessageBox.Show("Please select file contants your facebook passwords.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isAccountFBPathSelected = false;
            }
        }

        private void buttonCheckedAccountFBPath_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                checkedPasswordsPath = folderBrowserDialog.SelectedPath;
                labelCheckedAccountFBPath.Text = checkedPasswordsPath;
            }
            else
            {
                MessageBox.Show("Invalid folder path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
