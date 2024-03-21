using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMSimulator
{


    public partial class Form1 : Form
    {
        TextBox accountNumberField;
        TextBox pinField;
        string currentPin = "";
        private static Account[] ac = new Account[3];
        private Account ActiveAccount = null;
        private ATM atm = new ATM(ac);
        private TextBox lastClickedTextBox;
        public Form1()
        {
            InitializeComponent();
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
            
            currentPin = "";
            System.Windows.Forms.Label helloLabel = new System.Windows.Forms.Label();
            helloLabel.Location = new Point(250, 100);
            helloLabel.Size = new Size(150, 50);
            helloLabel.Text = "Hello from ATM!";
            Controls.Add(helloLabel);
            System.Windows.Forms.Label accLabel = new System.Windows.Forms.Label();
            accLabel.Location = new Point(50, 150);
            accLabel.Size = new Size(150, 20);
            accLabel.Text = "Enter your account number:";
            accLabel.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(accLabel);
            System.Windows.Forms.Label pinLabel = new System.Windows.Forms.Label();
            pinLabel.Location = new Point(50, 200);
            pinLabel.Size = new Size(150, 20);
            pinLabel.Text = "Enter pin:";

            pinLabel.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(pinLabel);

            accountNumberField = new TextBox();
            accountNumberField.MouseClick += new MouseEventHandler(this.lastTextBox_Click);
            accountNumberField.Location = new Point(200, 150);
            accountNumberField.Text = "";
            accountNumberField.Size = new Size(200, 100);
            accountNumberField.ReadOnly = true;
            Controls.Add(accountNumberField);
            lastClickedTextBox = accountNumberField;
            pinField = new TextBox();
            pinField.MouseClick += new MouseEventHandler(this.lastTextBox_Click);
            pinField.Location = new Point(200, 200);
            pinField.Text = "";
            pinField.Size = new Size(200, 100);
            pinField.ReadOnly = true;
            Controls.Add(pinField);

            Button logInButton = new Button();
            logInButton.Location = new Point(200, 450);
            logInButton.Size = new Size(150, 50);
            logInButton.Text = "Log in";
            logInButton.BackColor = Color.Aqua;
            logInButton.Click += new EventHandler(this.LogIn_Click);
            Controls.Add(logInButton);

            Button button1 = new Button();
            button1.Location = new Point(200, 250);
            button1.Size = new Size(50, 50);
            button1.Text = "1";
            button1.BackColor = Color.Aqua;
            button1.Click += new EventHandler(this.button1_Click);
            Controls.Add(button1);
            Button button2 = new Button();
            button2.Location = new Point(250, 250);
            button2.Size = new Size(50, 50);
            button2.Text = "2";
            button2.BackColor = Color.Aqua;
            button2.Click += new EventHandler(this.button2_Click);
            Controls.Add(button2);
            Button button3 = new Button();
            button3.Location = new Point(300, 250);
            button3.Size = new Size(50, 50);
            button3.Text = "3";
            button3.BackColor = Color.Aqua;
            button3.Click += new EventHandler(this.button3_Click);
            Controls.Add(button3);
            Button button4 = new Button();
            button4.Location = new Point(200, 300);
            button4.Size = new Size(50, 50);
            button4.Text = "4";
            button4.BackColor = Color.Aqua;
            button4.Click += new EventHandler(this.button4_Click);
            Controls.Add(button4);
            Button button5 = new Button();
            button5.Location = new Point(250, 300);
            button5.Size = new Size(50, 50);
            button5.Text = "5";
            button5.BackColor = Color.Aqua;
            button5.Click += new EventHandler(this.button5_Click);
            Controls.Add(button5);
            Button button6 = new Button();
            button6.Location = new Point(300, 300);
            button6.Size = new Size(50, 50);
            button6.Text = "6";
            button6.BackColor = Color.Aqua;
            button6.Click += new EventHandler(this.button6_Click);
            Controls.Add(button6);
            Button button7 = new Button();
            button7.Location = new Point(200, 350);
            button7.Size = new Size(50, 50);
            button7.Text = "7";
            button7.BackColor = Color.Aqua;
            button7.Click += new EventHandler(this.button7_Click);
            Controls.Add(button7);
            Button button8 = new Button();
            button8.Location = new Point(250, 350);
            button8.Size = new Size(50, 50);
            button8.Text = "8";
            button8.BackColor = Color.Aqua;
            button8.Click += new EventHandler(this.button8_Click);
            Controls.Add(button8);
            Button button9 = new Button();
            button9.Location = new Point(300, 350);
            button9.Size = new Size(50, 50);
            button9.Text = "9";
            button9.BackColor = Color.Aqua;
            button9.Click += new EventHandler(this.button9_Click);
            Controls.Add(button9);
            Button buttonEnter = new Button();
            buttonEnter.Location = new Point(200, 400);
            buttonEnter.Size = new Size(50, 50);
            buttonEnter.BackColor = Color.LimeGreen;
            buttonEnter.Click += new EventHandler(this.buttonEnter_Click);
            Controls.Add(buttonEnter);
            Button button0 = new Button();
            button0.Location = new Point(250, 400);
            button0.Size = new Size(50, 50);
            button0.Text = "0";
            button0.BackColor = Color.Aqua;
            button0.Click += new EventHandler(this.button0_Click);
            Controls.Add(button0);
            Button buttonClear = new Button();
            buttonClear.Location = new Point(300, 400);
            buttonClear.Size = new Size(50, 50);
            buttonClear.BackColor = Color.Yellow;
            buttonClear.Click += new EventHandler(this.buttonClear_Click);
            Controls.Add(buttonClear);
        }

        private void lastTextBox_Click(object sender, MouseEventArgs e)
        {
            lastClickedTextBox = sender as TextBox;
        }

        void LogIn_Click(object sender, EventArgs e)
       {

            if (atm.findAccount(accountNumberField.Text, currentPin) !=null)
            {
                Controls.Clear();
                ActiveAccount = atm.findAccount(accountNumberField.Text, currentPin);
                displayWindowAfterLogIn();
            }
            else
            {
                System.Windows.Forms.Label errorLabel = new System.Windows.Forms.Label();
                errorLabel.Location = new Point(200, 230);
                errorLabel.Size = new Size(350, 20);
                errorLabel.Text = "Error: incorrect account number or pin. Try again please";
                errorLabel.ForeColor = Color.DarkRed;    
                Controls.Add(errorLabel);
                lastClickedTextBox = accountNumberField;
                accountNumberField.Text = "";
                pinField.Text = "";
                currentPin = "";

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
            System.Windows.Forms.Label WelcomeLabel = new System.Windows.Forms.Label();
            WelcomeLabel.Location = new Point(200, 100);
            WelcomeLabel.Size = new Size(350, 20); ;
            WelcomeLabel.Text = "Welcome! Choose one of the following options:";
           
            Controls.Add(WelcomeLabel);

            Button takeOutCashButton = new Button();
            takeOutCashButton.Location = new Point(200, 200);
            takeOutCashButton.Size = new Size(100, 50);
            takeOutCashButton.Text = "Take out cash";
            takeOutCashButton.BackColor = Color.Aqua;
            takeOutCashButton.Click += new EventHandler(takeOutCash_Click);
            Controls.Add(takeOutCashButton);

            Button viewBalanceButton = new Button();
            viewBalanceButton.Location = new Point(200, 250);
            viewBalanceButton.Size = new Size(100, 50);
            viewBalanceButton.Text = "View balance";
            viewBalanceButton.BackColor = Color.Aqua;
            viewBalanceButton.Click += new EventHandler(this.ViewBalanceButton_Click);
            Controls.Add(viewBalanceButton);

            Button LogOutButton = new Button();
            LogOutButton.Location = new Point(200, 300);
            LogOutButton.Size = new Size(100, 50);
            LogOutButton.Text = "Log out";
            LogOutButton.BackColor = Color.Aqua;
            LogOutButton.Click += new EventHandler(this.LogOutButton_Click);
            Controls.Add(LogOutButton);
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
                backButton.Click += new EventHandler(this.takeOutCash_Click);
            } 
            Controls.Add(backButton);
        }

        private void takeOutCash_Click(object sender, EventArgs e)
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
            button10.Click += new EventHandler(this.takeCash_Click);
            Controls.Add(button10);
            Button button50 = new Button();
            button50.Location = new Point(200, 250);
            button50.Size = new Size(100, 50);
            button50.Text = "£100";
            button50.Click += new EventHandler(this.takeCash_Click);
            Controls.Add(button50);
            Button button500 = new Button();
            button500.Location = new Point(200, 300);
            button500.Size = new Size(100, 50);
            button500.Text = "£500";
            button500.Click += new EventHandler(this.takeCash_Click);
            Controls.Add(button500);
            createBackButton(1); 

        }


        private void takeCash_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            System.Windows.Forms.Label messageLabel = new System.Windows.Forms.Label();
            messageLabel.Location = new Point(200, 100);
            messageLabel.Size = new Size(350, 20);
            Controls.Add(messageLabel);
            if (ActiveAccount.decrementBalance(Convert.ToInt32(((Button)sender).Text.Substring(1, ((Button)sender).Text.Length - 1))))
            { 
                
                messageLabel.Text = "You have successfuly taken out " + ((Button)sender).Text;
                
                
            }
            else
            {
                messageLabel.Text = "Error: you have less than " + ((Button)sender).Text +" on your bank account";
            }
            
            createBackButton(2);
        }

        private void ViewBalanceButton_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            System.Windows.Forms.Label BalanceLabel = new System.Windows.Forms.Label();
            BalanceLabel.Location = new Point(200, 100);
            BalanceLabel.Size = new Size(350, 20);
            BalanceLabel.Text = "Your balance: £"+ActiveAccount.getBalance();
            Controls.Add(BalanceLabel);
            createBackButton(1);

        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            displayLogInWindow();
        }




    }


    class ATM
    {

        private Thread thread;
        //local referance to the array of accounts
        private Account[] ac;

        //this is a referance to the account that is being used
        private Account activeAccount = null;

        // the atm constructor takes an array of account objects as a referance
        public ATM(Account[] ac)
        {
            this.ac = ac;
            //Console.WriteLine("hello from ATM");

            // an infanite loop to keep the flow of controll going on and on
           // while (true)
          //  {

                //ask for account number and store result in acctiveAccount (null if no match found)
              //  activeAccount = this.findAccount();

             //   if (activeAccount != null)
             //   {
                    //if the account is found check the pin 
               //     if (activeAccount.checkPin(this.promptForPin()))
               //     {
                        //if the pin is a match give the options to do stuff to the account (take money out, view balance, exit)
               //         dispOptions();
               ///     }
               // }
              //  else
              //  {   //if the account number entered is not found let the user know!
              //      Console.WriteLine("no matching account found.");
              //  }

                //wipes all text from the console
             //   Console.Clear();
            //}


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
         *  this jsut promt the use to enter a pin number
         *  
         * returns the string entered converted to an int
         * 
         */
      //  private int promptForPin(string accNumber)
       //{
           
      //      int pinNumEntered = Convert.ToInt32(accNumber);
      //      return pinNumEntered;
      //  }

        /*
         * 
         *  give the use the options to do with the accoutn
         *  
         *  promt for input
         *  and defer to appropriate method based on input
         *  
         */
        private void dispOptions()
        {
            Console.WriteLine("1> take out cash");
            Console.WriteLine("2> balance");
            Console.WriteLine("3> exit");

            int input = Convert.ToInt32(Console.ReadLine());

            if (input == 1)
            {
                dispWithdraw();
            }
            else if (input == 2)
            {
                dispBalance();
            }
            else if (input == 3)
            {


            }
            else
            {

            }

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

                    //attempt to decrement account by 10 punds
                    if (activeAccount.decrementBalance(10))
                    {
                        //if this is possible display new balance and await key press
                        Console.WriteLine("new balance " + activeAccount.getBalance());
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
                    if (activeAccount.decrementBalance(50))
                    {
                        Console.WriteLine("new balance " + activeAccount.getBalance());
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
                    if (activeAccount.decrementBalance(500))
                    {
                        Console.WriteLine("new balance " + activeAccount.getBalance());
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
            if (this.activeAccount != null)
            {
                Console.WriteLine(" your current balance is : " + activeAccount.getBalance());
                Console.WriteLine(" (prese enter to continue)");
                Console.ReadLine();
            }
        }

    }

    /*
     *   The Account class encapusulates all features of a simple bank account
     */
    class Account
    {
        //the attributes for the account
        private int balance;
        private int pin;
        private int accountNum;

        // a constructor that takes initial values for each of the attributes (balance, pin, accountNumber)
        public Account(int balance, int pin, int accountNum)
        {
            this.balance = balance;
            this.pin = pin;
            this.accountNum = accountNum;
        }

        //getter and setter functions for balance
        public int getBalance()
        {
            return balance;
        }
        public void setBalance(int newBalance)
        {
            this.balance = newBalance;
        }

        /*
         *   This funciton allows us to decrement the balance of an account
         *   it perfomes a simple check to ensure the balance is greater tha
         *   the amount being debeted
         *   
         *   reurns:
         *   true if the transactions if possible
         *   false if there are insufficent funds in the account
         */
        public Boolean decrementBalance(int amount)
        {
            if (this.balance > amount)
            {
                balance -= amount;
                return true;
            }
            else
            {
                return false;
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
