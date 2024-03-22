using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ATMSimulator
{
    public partial class Form1 : Form
    {
        TextBox accountNumberField;
        TextBox pinField;
        string currentPin = "";
        int minutes, seconds;
        int incorrectAttempts = 0;
        private static Account[] ac = new Account[3];
        private Account ActiveAccount = null;
        private ATM atm = new ATM(ac);
        private TextBox lastClickedTextBox;
        private object balanceLock = new object();
        public bool useRaceCondition = false;
        private Button toggleButton;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private DateTime lastUserClick;
        public Form1(Account ActiveAccount)
        {
            InitializeComponent(ActiveAccount);
            this.MouseClick += UserClick;
        }

        private void UserClick(object sender, MouseEventArgs e)
        {
            lastUserClick = DateTime.Now;
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.AliceBlue;
            ac[0] = new Account(300, 1111, 111111);
            ac[1] = new Account(750, 2222, 222222);
            ac[2] = new Account(3000, 3333, 333333);
            displayLogInWindow();
        }

        private void displayLogInWindow()
        {
            //store pin which user entered 
            // Panel for background image
            Panel backgroundPanel = new Panel();
            string imagePath1 = @"Resources\Login_bg.png"; 
            if (File.Exists(imagePath1)) 
            {
                backgroundPanel.BackgroundImage = Image.FromFile(imagePath1);
                backgroundPanel.BackgroundImageLayout = ImageLayout.Stretch;
                backgroundPanel.Size = new Size(400, 300); 
                backgroundPanel.Location = new Point(80, 10); 
                Controls.Add(backgroundPanel);
            }
            else
            {
                // Handle case where the image file is not found
                MessageBox.Show("Image file not found: " + imagePath1);
            }

            // Add labels and text boxes
            Label helloLabel = new Label();
            helloLabel.Location = new Point(60, 50);
            helloLabel.Size = new Size(180, 50);
            helloLabel.Text = "Note: if you enter incorrect account number or pin more than 5 times, the ATM will be locked";
            helloLabel.TextAlign = ContentAlignment.MiddleCenter;
            helloLabel.ForeColor = Color.White;
            helloLabel.BackColor = Color.Transparent;
            backgroundPanel.Controls.Add(helloLabel);

            Label accLabel = new Label();
            accLabel.Location = new Point(45, 120);
            accLabel.Size = new Size(90, 20);
            accLabel.Text = "Account number:";
            accLabel.TextAlign = ContentAlignment.MiddleRight;
            accLabel.ForeColor = Color.White;
            accLabel.BackColor = Color.Transparent;
            backgroundPanel.Controls.Add(accLabel);

            Label pinLabel = new Label();
            pinLabel.Location = new Point(45, 150);
            pinLabel.Size = new Size(90, 20);
            pinLabel.Text = "Pin number:";
            pinLabel.TextAlign = ContentAlignment.MiddleRight;
            pinLabel.ForeColor = Color.White;
            pinLabel.BackColor = Color.Transparent;
            backgroundPanel.Controls.Add(pinLabel);

            // Text boxes
            accountNumberField = new TextBox();
            accountNumberField.MouseClick += new MouseEventHandler(this.lastTextBox_Click);
            accountNumberField.Location = new Point(155, 120);
            accountNumberField.Text = "";
            accountNumberField.Size = new Size(80, 20); 
            accountNumberField.BackColor = Color.FromArgb(64, 64, 64); 
            accountNumberField.ForeColor = Color.White; 
            accountNumberField.BorderStyle = BorderStyle.FixedSingle; 
            backgroundPanel.Controls.Add(accountNumberField);

            pinField = new TextBox();
            pinField.MouseClick += new MouseEventHandler(this.lastTextBox_Click);
            pinField.Location = new Point(155, 150);
            pinField.Text = "";
            pinField.Size = new Size(80, 20); 
            pinField.BackColor = Color.FromArgb(64, 64, 64); 
            pinField.ForeColor = Color.White; 
            pinField.BorderStyle = BorderStyle.None; 
            backgroundPanel.Controls.Add(pinField);


            // Panel for buttons
            Panel buttonPanel = new Panel();
            string imagePath = @"Resources\ATM_Keypad.jpg"; // Replace this with the absolute path to your image file
            if (File.Exists(imagePath)) // Check if the file exists
            {
                buttonPanel.BackgroundImage = Image.FromFile(imagePath);
                buttonPanel.BackgroundImageLayout = ImageLayout.Stretch;
                buttonPanel.Size = new Size(400, 300);
                buttonPanel.Location = new Point(80, 320);
                Controls.Add(buttonPanel);
            }
            else
            {
                // Handle case where the image file is not found
                MessageBox.Show("Image file not found: " + imagePath);
            }

            //add log in button
            Button logInButton = new Button();
            logInButton.Location = new Point(300, 160);
            logInButton.Size = new Size(70, 50);
            logInButton.Text = "";
            logInButton.FlatStyle = FlatStyle.Flat;
            logInButton.FlatAppearance.BorderSize = 0;
            logInButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            logInButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            logInButton.BackColor = Color.Transparent;
            logInButton.ForeColor = Color.Black;
            logInButton.Click += new EventHandler(this.LogIn_Click);
            buttonPanel.Controls.Add(logInButton);
           
            //add number, enter and clear buttons
            Button button1 = new Button();
            button1.Location = new Point(40, 30);
            button1.Size = new Size(50, 50);
            button1.Text = "";
            button1.FlatStyle = FlatStyle.Flat; 
            button1.FlatAppearance.BorderSize = 0; 
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.BackColor = Color.Transparent; 
            button1.ForeColor = Color.Black;
            button1.Click += new EventHandler(this.button1_Click);
            buttonPanel.Controls.Add(button1);
            Button button2 = new Button();
            button2.Location = new Point(120, 30);
            button2.Size = new Size(50, 50);
            button2.Text = "";
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.BackColor = Color.Transparent;
            button2.ForeColor = Color.Black;
            button2.Click += new EventHandler(this.button2_Click);
            buttonPanel.Controls.Add(button2);
            Button button3 = new Button();
            button3.Location = new Point(200, 30);
            button3.Size = new Size(50, 50);
            button3.Text = "";
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            button3.ForeColor = Color.Black;
            button3.Click += new EventHandler(this.button3_Click);
            buttonPanel.Controls.Add(button3);
            Button button4 = new Button();
            button4.Location = new Point(40, 95);
            button4.Size = new Size(50, 50);
            button4.Text = "";
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button4.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button4.BackColor = Color.Transparent;
            button4.ForeColor = Color.Black;
            button4.Click += new EventHandler(this.button4_Click);
            buttonPanel.Controls.Add(button4);
            Button button5 = new Button();
            button5.Location = new Point(120, 95);
            button5.Size = new Size(50, 50);
            button5.Text = "";
            button5.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button5.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button5.BackColor = Color.Transparent;
            button5.ForeColor = Color.Black;
            button5.Click += new EventHandler(this.button5_Click);
            buttonPanel.Controls.Add(button5);
            Button button6 = new Button();
            button6.Location = new Point(200, 95);
            button6.Size = new Size(50, 50);
            button6.Text = "";
            button6.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button6.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button6.BackColor = Color.Transparent;
            button6.ForeColor = Color.Black;
            button6.Click += new EventHandler(this.button6_Click);
            buttonPanel.Controls.Add(button6);
            Button button7 = new Button();
            button7.Location = new Point(40, 160);
            button7.Size = new Size(50, 50);
            button7.Text = "";
            button7.FlatStyle = FlatStyle.Flat;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button7.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button7.BackColor = Color.Transparent;
            button7.ForeColor = Color.Black;
            button7.Click += new EventHandler(this.button7_Click);
            buttonPanel.Controls.Add(button7);
            Button button8 = new Button();
            button8.Location = new Point(120, 160);
            button8.Size = new Size(50, 50);
            button8.Text = "";
            button8.FlatStyle = FlatStyle.Flat;
            button8.FlatAppearance.BorderSize = 0;
            button8.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button8.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button8.BackColor = Color.Transparent;
            button8.ForeColor = Color.Black;
            button8.Click += new EventHandler(this.button8_Click);
            buttonPanel.Controls.Add(button8);
            Button button9 = new Button();
            button9.Location = new Point(200, 160);
            button9.Size = new Size(50, 50);
            button9.Text = "";
            button9.FlatStyle = FlatStyle.Flat;
            button9.FlatAppearance.BorderSize = 0;
            button9.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button9.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button9.BackColor = Color.Transparent;
            button9.ForeColor = Color.Black;
            button9.Click += new EventHandler(this.button9_Click);
            buttonPanel.Controls.Add(button9);
            Button button0 = new Button();
            button0.Text = "";
            button0.Location = new Point(120, 225);
            button0.Size = new Size(50, 50);
            button0.FlatStyle = FlatStyle.Flat;
            button0.FlatAppearance.BorderSize = 0;
            button0.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button0.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button0.BackColor = Color.Transparent;
            button0.ForeColor = Color.Black;
            button0.Click += new EventHandler(this.button0_Click);
            buttonPanel.Controls.Add(button0);
            Button buttonClear = new Button();
            buttonClear.Location = new Point(300, 95);
            buttonClear.Size = new Size(70, 50);
            buttonClear.Text = "";
            buttonClear.FlatStyle = FlatStyle.Flat;
            buttonClear.FlatAppearance.BorderSize = 0;
            buttonClear.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buttonClear.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buttonClear.BackColor = Color.Transparent;
            buttonClear.ForeColor = Color.Black;
            buttonClear.Click += new EventHandler(this.buttonClear_Click);
            buttonPanel.Controls.Add(buttonClear);

            // Create a panel to hold the buttons
           
        }

        private void lastTextBox_Click(object sender, MouseEventArgs e)
        {
            lastClickedTextBox = sender as TextBox;
        }


        void LogIn_Click(object sender, EventArgs e)
        {

            if (atm.findAccount(accountNumberField.Text, currentPin) != null)
            {
                Controls.Clear();
                ActiveAccount = atm.findAccount(accountNumberField.Text, currentPin);
                incorrectAttempts = 0;

                displayWindowAfterLogIn();
            }
            else
            {
                System.Windows.Forms.Label errorLabel = new System.Windows.Forms.Label();
                errorLabel.Location = new Point(200, 230);
                errorLabel.Size = new Size(350, 20);
                //display the error message to the user
                errorLabel.Text = "Error: incorrect account number or pin. Try again please";
                incorrectAttempts++;
                errorLabel.ForeColor = Color.DarkRed;
                Controls.Add(errorLabel);
                lastClickedTextBox = accountNumberField;
                accountNumberField.Text = "";
                pinField.Text = "";
                currentPin = "";

            }
            //if user failed to type in correct data more than 5 times
            if (incorrectAttempts > 5)
            {
                MessageBox.Show("You got more than 5 incorrect attempts. The ATM is locked");
                this.Close();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "1";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "1";
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "2";

            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "2";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "3";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "3";
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "4";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "4";
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "5";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "5";
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "6";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "6";
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "7";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "7";
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "8";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "8";
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "9";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "9";
            }
        }
        private void button0_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {

                lastClickedTextBox.Text += "0";
            }
            else
            {
                lastClickedTextBox.Text += "*";
                currentPin += "0";
            }
        }
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox == accountNumberField)
            {
                lastClickedTextBox = pinField;
                LogIn_Click(sender, e);
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (lastClickedTextBox.Text.Length >= 1)
            {
                lastClickedTextBox.Text = lastClickedTextBox.Text.Substring(0, lastClickedTextBox.Text.Length - 1);
            }

        }

        private void displayWindowAfterLogIn()
        {
            Panel backgroundPanel = new Panel();
            string imagePath = @"Resources\withdrawal.png"; 
            if (File.Exists(imagePath))
            {
                backgroundPanel.BackgroundImage = Image.FromFile(imagePath);
                backgroundPanel.BackgroundImageLayout = ImageLayout.Stretch;
                backgroundPanel.Size = new Size(500, 500); 
                backgroundPanel.Location = new Point(0, 0); 
                Controls.Add(backgroundPanel); 
            }
            else
            {
                MessageBox.Show("Image file not found: " + imagePath);
            }

            Button takeOutCashButton = new Button();
            takeOutCashButton.Location = new Point(100, 140);
            takeOutCashButton.Size = new Size(70, 40);
            takeOutCashButton.Text = "Take out cash";
            takeOutCashButton.BackColor = Color.Transparent;
            takeOutCashButton.FlatAppearance.BorderSize = 1;
            takeOutCashButton.Font = new Font(takeOutCashButton.Font, FontStyle.Bold); // Bold font
            takeOutCashButton.Click += new EventHandler(TakeOutCash_Click);
            backgroundPanel.Controls.Add(takeOutCashButton);

            Button viewBalanceButton = new Button();
            viewBalanceButton.Location = new Point(100, 200);
            viewBalanceButton.Size = new Size(70, 40);
            viewBalanceButton.Text = "View balance";
            viewBalanceButton.BackColor = Color.Transparent;
            viewBalanceButton.FlatAppearance.BorderSize = 1;
            viewBalanceButton.Font = new Font(viewBalanceButton.Font, FontStyle.Bold); // Bold font
            viewBalanceButton.Click += new EventHandler(this.ViewBalanceButton_Click);
            backgroundPanel.Controls.Add(viewBalanceButton);

            Button LogOutButton = new Button();
            LogOutButton.Location = new Point(100, 260);
            LogOutButton.Size = new Size(70, 40);
            LogOutButton.Text = "Log out";
            LogOutButton.BackColor = Color.Transparent;
            LogOutButton.FlatAppearance.BorderSize = 1;
            LogOutButton.Font = new Font(LogOutButton.Font, FontStyle.Bold); // Bold font
            LogOutButton.Click += new EventHandler(this.LogOutButton_Click);
            backgroundPanel.Controls.Add(LogOutButton);

            // Toggle button for Race and Non-Race condition
            Button toggleButton = new Button();
            toggleButton.Location = new Point(320, 170);
            toggleButton.Size = new Size(100, 140);
            toggleButton.Text = "Race Condition";
            toggleButton.BackColor = Color.Transparent;
            toggleButton.FlatAppearance.BorderSize = 0; // Remove border
            toggleButton.Font = new Font(toggleButton.Font, FontStyle.Bold); // Bold font
            toggleButton.Click += new EventHandler(this.ToggleCondition_Click);
            backgroundPanel.Controls.Add(toggleButton);

            // Button to proceed with ATM instances
            Button proceedButton = new Button();
            proceedButton.Location = new Point(320, 350); // Adjust position as needed
            proceedButton.Size = new Size(100, 50);
            proceedButton.Text = "Proceed Testing";
            proceedButton.BackColor = Color.Transparent;
            proceedButton.FlatAppearance.BorderSize = 1;
            proceedButton.Font = new Font(proceedButton.Font, FontStyle.Bold); // Bold font
            proceedButton.Click += new EventHandler(this.ProceedButton_Click);
            backgroundPanel.Controls.Add(proceedButton);
        }

        private void TrackingUserActivity_Tick(object sender, EventArgs e)
        {

            if ((DateTime.Now - lastUserClick).TotalSeconds > 30.0)
            {
                timer.Stop();
                if (MessageBox.Show("You have been inactive for more than 30 seconds. Do you want to continue?", " ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lastUserClick = DateTime.Now;
                    timer.Start();

                }
                else
                {
                    Controls.Clear();
                    displayLogInWindow();
                }

            }
        }

        private void ProceedButton_Click(object sender, EventArgs e)
        {
            InitializeATMInstances();
        }

        private void ToggleCondition_Click(object sender, EventArgs e)
        {
            // Toggle the useRaceCondition flag
            useRaceCondition = !useRaceCondition;

            // Change the button color and text based on the current condition
            Button toggleButton = (Button)sender;
            if (useRaceCondition)
            {
                // Set to Race Condition (Red)
                toggleButton.BackColor = Color.Red;
                toggleButton.Text = "Race Condition";
            }
            else
            {
                // Set to Non-Race Condition (Green)
                toggleButton.BackColor = Color.Green;
                toggleButton.Text = "Non-Race Condition";
            }
        }

        private void InitializeATMInstances()
        {
            if (ActiveAccount != null)
            {
                // Instantiate two instances of Form1
                Form1 formInstance1 = new Form1(ActiveAccount);
                Form1 formInstance2 = new Form1(ActiveAccount);

                // Set the ActiveAccount property for both instances
                formInstance1.ActiveAccount = ActiveAccount;
                formInstance2.ActiveAccount = ActiveAccount;

                // Set the useRaceCondition property for both instances
                formInstance1.useRaceCondition = useRaceCondition;
                formInstance2.useRaceCondition = useRaceCondition;

                // Set the positions for the forms
                int formWidth = 600; 
                int spacing = 5; 
                formInstance1.StartPosition = FormStartPosition.Manual;
                formInstance1.Size = new Size(formWidth, 750);
                formInstance1.Location = new Point(100, 100); 
                formInstance2.StartPosition = FormStartPosition.Manual;
                formInstance2.Size = new Size(formWidth, 750);
                formInstance2.Location = new Point(100 + formWidth + spacing, 100); 

                // Show both instances
                formInstance1.Show();
                formInstance2.Show();
            }
            else
            {
                MessageBox.Show("No active account. Please log in first.", "Error");
            }
        }

        private void createBackButton(int p)
        {
            Button backButton = new Button();
            backButton.Location = new Point(200, 400);
            backButton.Size = new Size(100, 50);
            backButton.Text = "Back";
            backButton.BackColor = Color.Aqua;
            if (p == 1)
            {
                backButton.Click += new EventHandler(this.LogIn_Click);
            }
            else
            {
                backButton.Click += new EventHandler(this.TakeOutCash_Click);
            }
            Controls.Add(backButton);
        }

        private void TakeOutCash_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            System.Windows.Forms.Label chooseAmountLabel = new System.Windows.Forms.Label();
            chooseAmountLabel.Location = new Point(200, 100);
            chooseAmountLabel.Size = new Size(350, 20);
            chooseAmountLabel.Text = "Choose the amount you want to take out";
            Controls.Add(chooseAmountLabel);
            Button button10 = new Button();
            button10.Location = new Point(200, 200);
            button10.Size = new Size(100, 50);
            button10.Text = "£10";
            button10.Click += new EventHandler(this.TakeCash_Click);
            Controls.Add(button10);
            Button button50 = new Button();
            button50.Location = new Point(200, 250);
            button50.Size = new Size(100, 50);
            button50.Text = "£100";
            button50.Click += new EventHandler(this.TakeCash_Click);
            Controls.Add(button50);
            Button button500 = new Button();
            button500.Location = new Point(200, 300);
            button500.Size = new Size(100, 50);
            button500.Text = "£500";
            button500.Click += new EventHandler(this.TakeCash_Click);
            Controls.Add(button500);
            createBackButton(1);
        }

        //print the transaction details
        private void printReceipt(int amount)
        {
            System.Windows.Forms.Label receipt = new System.Windows.Forms.Label();
            receipt.Location = new Point(200, 200);
            receipt.Size = new Size(300, 200);
            receipt.Text = "Transaction details: \n \n" +
                "Account number: " + +ActiveAccount.getAccountNum() + "\n\nAmount withdrawn: £" + amount + "\n\n" +
                "Account balance: £" + ActiveAccount.GetBalance() + "\n\n" +
                "Date: " + DateTime.Now.ToString();
            Controls.Add(receipt);

        }

        private void TakeCash_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            System.Windows.Forms.Label messageLabel = new System.Windows.Forms.Label();
            messageLabel.Location = new Point(200, 100);
            messageLabel.Size = new Size(350, 20);
            Controls.Add(messageLabel);

            // Parse the amount from the button text
            int amount = Convert.ToInt32(((Button)sender).Text.Substring(1));

            if (ActiveAccount != null) // Potential source of error
            {
                // Check if the account has sufficient balance to withdraw
                if (ActiveAccount.decrementBalance(amount, useRaceCondition))
                {
                    messageLabel.Text = "You have successfully taken out £" + amount;
                }
                else
                {
                    messageLabel.Text = "Error: you have less than £" + amount + " in your bank account";
                }
            }
            else
            {
                // Display error message if no active account is found
                messageLabel.Text = "Error: No active account found.";
            }

            createBackButton(2);
        }

        private void ViewBalanceButton_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            System.Windows.Forms.Label BalanceLabel = new System.Windows.Forms.Label();
            BalanceLabel.Location = new Point(200, 100);
            BalanceLabel.Size = new Size(350, 20);
            BalanceLabel.Text = "Your balance: £" + ActiveAccount.GetBalance();
            Controls.Add(BalanceLabel);
            createBackButton(1);

        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            displayLogInWindow();
        }

    }

    public class ATM
    {

        //local referance to the array of accounts
        readonly private Account[] ac;
        readonly private object balanceLock = new object();
        readonly private bool useRaceCondition = false;

        //this is a referance to the account that is being used
        readonly private Account ActiveAccount;

        // the atm constructor takes an array of account objects as a referance
        public ATM(Account[] ac)
        {
            this.ac = ac;
        }

        /*
         *    this method promts for the input of an account number
         *    the string input is then converted to an int
         *    a for loop is used to check the enterd account number
         *    against those held in the account array
         *    if a match is found a referance to the match is returned
         *    if the for loop completest with no match we return null
         * 
         */
        public Account findAccount(string accNumber, string pin)
        {

            // int input = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < this.ac.Length; i++)
            {
                if (ac[i].getAccountNum() == Convert.ToInt32(accNumber) && ac[i].checkPin(Convert.ToInt32(pin)))
                {
                    return ac[i];
                }
            }

            return null;
        }

        /*
         * 
         * offer withdrawable amounts
         * 
         * based on input attempt to withraw the corosponding amount of money
         * 
         */
        private void dispWithdraw()
        {
            Console.WriteLine("1> 10");
            Console.WriteLine("2> 50");
            Console.WriteLine("3> 500");

            int input = Convert.ToInt32(Console.ReadLine());

            if (input > 0 && input < 4)
            {

                //opiton one is entered by the user
                if (input == 1)
                {

                    //attempt to decrement account by 10 pounds
                    if (ActiveAccount.decrementBalance(10, useRaceCondition))
                    {
                        //if this is possible display new balance and await key press
                        Console.WriteLine("new balance " + ActiveAccount.GetBalance());
                        Console.WriteLine(" (prese enter to continue)");
                        Console.ReadLine();
                    }
                    else
                    {
                        //if this is not possible inform user and await key press
                        Console.WriteLine("insufficent funds");
                        Console.WriteLine(" (prese enter to continue)");
                        Console.ReadLine();
                    }
                }
                else if (input == 2)
                {
                    if (ActiveAccount.decrementBalance(50, useRaceCondition))
                    {
                        Console.WriteLine("new balance " + ActiveAccount.GetBalance());
                        Console.WriteLine(" (prese enter to continue)");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("insufficent funds");
                        Console.WriteLine(" (prese enter to continue)");
                        Console.ReadLine();
                    }
                }
                else if (input == 3)
                {
                    if (ActiveAccount.decrementBalance(500, useRaceCondition))
                    {
                        Console.WriteLine("new balance " + ActiveAccount.GetBalance());
                        Console.WriteLine(" (prese enter to continue)");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("insufficent funds");
                        Console.WriteLine(" (prese enter to continue)");
                        Console.ReadLine();
                    }
                }
            }
        }
        /*
         *  display balance of activeAccount and await keypress
         *  
         */
        private void dispBalance()
        {
            if (this.ActiveAccount != null)
            {
                Console.WriteLine(" your current balance is : " + ActiveAccount.GetBalance());
                Console.WriteLine(" (prese enter to continue)");
                Console.ReadLine();
            }
        }
    }


    /*
     *   The Account class encapusulates all features of a simple bank account
     */
    public class Account
    {
        //the attributes for the account
        private int balance;
        readonly private int pin;
        readonly private int accountNum;
        readonly private object balanceLock = new object();
        private static Semaphore balanceSemaphore;
        private Account ActiveAccount;

        // a constructor that takes initial values for each of the attributes (balance, pin, accountNumber)
        public Account(int balance, int pin, int accountNum)
        {
            this.balance = balance;
            this.pin = pin;
            this.accountNum = accountNum;
            balanceSemaphore = new Semaphore(1, 2);
        }

        //getter and setter functions for balance
        public int GetBalance()
        {
            return balance;
        }
        public void SetBalance(int newBalance)
        {
            this.balance = newBalance;
        }

        /*
         *   This funciton allows us to decrement the balance of an account
         *   it perfomes a simple check to ensure the balance is greater tha
         *   the amount being debeted
         *   
         *   returns:
         *   true if the transactions if possible
         *   false if there are insufficent funds in the account
         */
        public Boolean decrementBalance(int amount, bool useRaceCondition)
        {

            if (useRaceCondition == true)
            {
                Console.WriteLine("Race Condition");
                // Implement Race Condition scenario
                if (this.balance > amount)
                {
                    Thread t1 = new Thread(() =>
                    {
                        // Retrieve the initial balance
                        int newBalance = GetBalance();
                        Console.WriteLine("Thread 1 getbalance: " + newBalance);
                        // Simulate some processing time
                        Thread.Sleep(5000);
                        // Decrement the balance
                        SetBalance(newBalance -= amount);
                        Console.WriteLine("Thread 1 setbalance: " + (newBalance - amount));

                    });

                    Thread t2 = new Thread(() =>
                    {
                        // Retrieve the initial balance
                        int newBalance = GetBalance();
                        Console.WriteLine("Thread 2 getbalance: " + newBalance);
                        // Simulate some processing time
                        Thread.Sleep(3000);
                        // Decrement the balance
                        SetBalance(newBalance -= amount);
                        Console.WriteLine("Thread 2 setbalance: " + (newBalance - amount));
                    });

                    t1.Start();
                    t2.Start();

                    return true;
                }
                else
                {
                    Console.WriteLine("not working");
                    return false;
                }
            }
            else
            {
                // Implement Non-Race Condition scenario
                return decrementBalanceNonRace(amount);
            }
        }

        private Boolean decrementBalanceNonRace(int amount)
        {
            Console.WriteLine("Non-Race Condition");
            lock (balanceLock)
            {
                if (this.balance > amount)
                {
                    // Decrement the balance
                    balanceSemaphore.WaitOne();
                    this.balance -= amount;
                    balanceSemaphore.Release();
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        /*
         * This funciton check the account pin against the argument passed to it
         *
         * returns:
         * true if they match
         * false if they do not
         */
        public Boolean checkPin(int pinEntered)
        {
            if (pinEntered == pin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int getAccountNum()
        {
            return accountNum;
        }

    }
}