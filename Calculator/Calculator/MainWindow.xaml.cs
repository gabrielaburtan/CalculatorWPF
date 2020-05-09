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
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private Operations operations;
        private Memory memory;
        private bool memoryClicked;
        private string typeOfOperation;
        private bool operationChecked;
        private bool digitGroupingCheck;
        private System.Windows.Forms.NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            BtnPoint.Content = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            digitGroupingCheck = Properties.Settings.Default.CheckBox;
            CheckBox.IsChecked = digitGroupingCheck;
            operations = new Operations();
            memory = new Memory();
            memoryClicked = false;
            typeOfOperation = "";
            operationChecked = false;
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            BtnMemoryRecall.IsEnabled = false;
            BtnMemoryClear.IsEnabled = false;
        }

        private void NumericalButtons(bool value)
        {
            Btn0.IsEnabled = value;
            Btn1.IsEnabled = value;
            Btn2.IsEnabled = value;
            Btn3.IsEnabled = value;
            Btn4.IsEnabled = value;
            Btn5.IsEnabled = value;
            Btn6.IsEnabled = value;
            Btn7.IsEnabled = value;
            Btn8.IsEnabled = value;
            Btn9.IsEnabled = value;
        }

        private void EnableDisableButtons(bool value)
        {
            BtnMultiply.IsEnabled = value;
            BtnMinus.IsEnabled = value;
            BtnPlus.IsEnabled = value;
            DivisionBtn.IsEnabled = value;
            BtnPlusMinus.IsEnabled = value;
            BtnPercent.IsEnabled = value;
            InversionBtn.IsEnabled = value;
            SqrtBtn.IsEnabled = value;
            SquareBtn.IsEnabled = value;
        }

        private void FormatTextForOperations()
        {
            if (digitGroupingCheck == true)
            {
                string decimalPart = "";
                if (TxtDisplay.Text.Contains(".") == true)
                {
                    decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                }
                int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                {
                    changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                }
                if (TxtDisplay.Text.Contains("."))
                {
                    changedDisplay += decimalPart;
                }
                if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                {
                    changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                }
                TxtDisplay.Text = changedDisplay;
            }
            else
            {
                double number = double.Parse(TxtDisplay.Text);
                TxtDisplay.Text = number.ToString("G");
            }
        }
        private void FormatTextForClick(object sender)
        {
            if (digitGroupingCheck == true)
            {
                string decimalPart = "";
                if (TxtDisplay.Text.Contains(".") == true)
                {
                    decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                }
                int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                {
                    changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                }
                if (TxtDisplay.Text.Contains("."))
                {
                    changedDisplay += decimalPart;
                }
                if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                {
                    changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                }
                if (TxtDisplay.Text.Contains(".") == true && (sender as Button).Content.ToString() == "0")
                {
                    changedDisplay += "0";
                }
                TxtDisplay.Text = changedDisplay;
            }
            else
            {
                double number = double.Parse(TxtDisplay.Text);
                TxtDisplay.Text = number.ToString("G");
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if ((TxtDisplay.Text == "0" || (operationChecked)))
                TxtDisplay.Clear();
            operationChecked = false;
            Button button = sender as Button;
            if (memoryClicked == true)
            {
                TxtDisplay.Text = button.Content.ToString();
            }
            else
            {
                TxtDisplay.Text = TxtDisplay.Text + button.Content.ToString();
                FormatTextForClick(sender);
            }
            EnableDisableButtons(true);
            BtnEqual.IsEnabled = true;
            BtnC.IsEnabled = true;
            BtnCE.IsEnabled = true;
        }

        private void OperationClick(object sender, RoutedEventArgs e)
        {
            BtnC.IsEnabled = true;
            BtnCE.IsEnabled = false;
            Button B = (Button)sender;
            typeOfOperation = B.Content.ToString();
            EnableDisableButtons(false);
            BtnEqual.IsEnabled = false;
            BtnPoint.IsEnabled = true;

            TxtDisplay.Text = operations.MakeOperation(TxtDisplay.Text, typeOfOperation);

            if (TxtDisplay.Text == "Cannot divide by zero!")
            {
                EnableDisableButtons(false);
                NumericalButtons(false);
                BtnEqual.IsEnabled = false;
                BtnPoint.IsEnabled = false;
                BtnCE.IsEnabled = false;
                BtnMemoryAdd.IsEnabled = false;
                BtnMemorySubstract.IsEnabled = false;
                BtnMemoryStore.IsEnabled = false;
            }
            else
            {
                FormatTextForOperations();
            }
            operationChecked = true;
        }

        private void EqualClick(object sender, RoutedEventArgs e)
        {
            BtnEqual.IsEnabled = false;
            BtnPoint.IsEnabled = true;
            BtnC.IsEnabled = true;
            BtnCE.IsEnabled = false;
            TxtDisplay.Text = operations.EqualOperation(TxtDisplay.Text);
            if (TxtDisplay.Text == "Cannot divide by zero!")
            {
                EnableDisableButtons(false);
                NumericalButtons(false);
                BtnEqual.IsEnabled = false;
                BtnPoint.IsEnabled = false;
                BtnCE.IsEnabled = false;
                BtnMemoryAdd.IsEnabled = false;
                BtnMemorySubstract.IsEnabled = false;
                BtnMemoryStore.IsEnabled = false;
            }
            else
            {
                FormatTextForOperations();
            }
        }

        private void BtnCEClick(object sender, RoutedEventArgs e)
        {
            operations.numberStack.Push(Double.Parse(TxtDisplay.Text));
            TxtDisplay.Text = "0";
            if (operations.numberStack.Any())
            {
                operations.numberStack.Pop();
            }
            EnableDisableButtons(true);
            BtnC.IsEnabled = true;
            BtnCE.IsEnabled = false;
        }

        private void BtnCClick(object sender, RoutedEventArgs e)
        {
            NumericalButtons(true);
            BtnEqual.IsEnabled = true;
            BtnPoint.IsEnabled = true;
            BtnCE.IsEnabled = true;
            BtnMemoryAdd.IsEnabled = true;
            BtnMemorySubstract.IsEnabled = true;
            BtnMemoryStore.IsEnabled = true;
            TxtDisplay.Text = "0";
            operations.numberStack.Clear();
            operations.operationStack.Clear();
            operations.previousValue = 0;
            EnableDisableButtons(true);
        }

        private void SqrtBtnClick(object sender, RoutedEventArgs e)
        {
            BtnC.IsEnabled = true;
            if (Double.Parse(TxtDisplay.Text) < 0)
            {
                TxtDisplay.Text = "Invalid input!";
                EnableDisableButtons(false);
                NumericalButtons(false);
                BtnEqual.IsEnabled = false;
                BtnPoint.IsEnabled = false;
                BtnCE.IsEnabled = false;
                BtnMemoryAdd.IsEnabled = false;
                BtnMemorySubstract.IsEnabled = false;
                BtnMemoryStore.IsEnabled = false;
            }
            else
            {
                TxtDisplay.Text = Math.Sqrt(Convert.ToDouble(TxtDisplay.Text)).ToString();
                FormatTextForOperations();
            }
        }

        private void InversionBtnClick(object sender, RoutedEventArgs e)
        {
            BtnC.IsEnabled = true;
            if (TxtDisplay.Text == "0")
            {
                TxtDisplay.Text = "Cannot divide by zero!";
                EnableDisableButtons(false);
                NumericalButtons(false);
                BtnEqual.IsEnabled = false;
                BtnPoint.IsEnabled = false;
                BtnCE.IsEnabled = false;
                BtnMemoryAdd.IsEnabled = false;
                BtnMemorySubstract.IsEnabled = false;
                BtnMemoryStore.IsEnabled = false;
            }
            else
            {
                TxtDisplay.Text = (1.0 / Convert.ToDouble(TxtDisplay.Text)).ToString();
                FormatTextForOperations();
            }
        }

        private void BtnPointClick(object sender, RoutedEventArgs e)
        {
            TxtDisplay.Text = TxtDisplay.Text + ".";
            BtnPoint.IsEnabled = false;
        }

        private void SquareBtnClick(object sender, RoutedEventArgs e)
        {
            BtnC.IsEnabled = true;
            TxtDisplay.Text = (Convert.ToDouble(TxtDisplay.Text) * Convert.ToDouble(TxtDisplay.Text)).ToString();
            FormatTextForOperations();
        }

        private void BtnOpositeClick(object sender, RoutedEventArgs e)
        {
            TxtDisplay.Text = (-(Convert.ToDouble(TxtDisplay.Text))).ToString();
            FormatTextForOperations();
        }

        private void BtnBackspaceClick(object sender, RoutedEventArgs e)
        {
            NumericalButtons(true);
            BtnEqual.IsEnabled = true;
            BtnPoint.IsEnabled = true;
            BtnCE.IsEnabled = true;
            BtnMemoryAdd.IsEnabled = true;
            BtnMemorySubstract.IsEnabled = true;
            BtnMemoryStore.IsEnabled = true;
            if (TxtDisplay.Text == "Cannot divide by zero!" || TxtDisplay.Text == "Invalid input!")
            {
                TxtDisplay.Text = "0";
            }
            else if (TxtDisplay.Text.Length > 1 && TxtDisplay.Text != "0")
            {
                TxtDisplay.Text = TxtDisplay.Text.Remove(TxtDisplay.Text.Length - 1);
                FormatTextForOperations();
                if (!TxtDisplay.Text.Contains("."))
                {
                    BtnPoint.IsEnabled = true;
                }
                else
                {
                    BtnPoint.IsEnabled = false;
                }
            }
            else
            {
                TxtDisplay.Text = "0";
                BtnPoint.IsEnabled = true;
            }

            //operations.numberStack.Clear();
            //operations.operationStack.Clear();
            EnableDisableButtons(true);
        }

        private void BtnPercentClick(object sender, RoutedEventArgs e)
        {
            BtnC.IsEnabled = true;
            if (operations.numberStack.Count == 0)
            {
                TxtDisplay.Text = "0";
            }
            else
            {
                double value1 = Double.Parse(TxtDisplay.Text);
                TxtDisplay.Text = ((value1 / 100) * operations.numberStack.Peek()).ToString();
            }
            FormatTextForOperations();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CheckBox = digitGroupingCheck;
            Properties.Settings.Default.Save();
            SystemCommands.CloseWindow(this);
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
            notifyIcon.Icon = new System.Drawing.Icon("calculator.ico");
            notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(NotifyIconClick);
            this.ShowInTaskbar = false;
            notifyIcon.BalloonTipText = "The application has been minimized to system tray!";
            notifyIcon.ShowBalloonTip(500);
        }

        private void NotifyIconClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
            SystemCommands.RestoreWindow(this);
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.Show();
        }

        private void BtnMemoryClearClick(object sender, RoutedEventArgs e)
        {
            memoryClicked = true;
            BtnMemoryClear.IsEnabled = false;
            BtnMemoryRecall.IsEnabled = false;
            memory.MemoryClear();
        }

        private void BtnMemoryRecallClick(object sender, RoutedEventArgs e)
        {
            memoryClicked = true;
            TxtDisplay.Text = memory.MemoryRecall();
        }

        private void BtnMemoryAddClick(object sender, RoutedEventArgs e)
        {
            memoryClicked = true;
            memory.MemoryAdd(TxtDisplay.Text);
            BtnMemoryRecall.IsEnabled = true;
            BtnMemoryClear.IsEnabled = true;
        }

        private void BtnMemorySubstractClick(object sender, RoutedEventArgs e)
        {
            memoryClicked = true;
            memory.MemorySubstract(TxtDisplay.Text);
            BtnMemoryRecall.IsEnabled = true;
            BtnMemoryClear.IsEnabled = true;
        }

        private void BtnMemoryStoreClick(object sender, RoutedEventArgs e)
        {
            memoryClicked = true;
            memory.MemoryStore(TxtDisplay.Text);
            BtnMemoryRecall.IsEnabled = true;
            BtnMemoryClear.IsEnabled = true;
        }

        private void DigitGroupingClick(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true)
            {
                digitGroupingCheck = true;
            }
            else
            {
                digitGroupingCheck = false;
            }
            FormatTextForOperations();
        }

        private void CutClick(object sender, RoutedEventArgs e)
        {
            TxtDisplay.SelectAll();
            TxtDisplay.Cut();
            TxtDisplay.Text = "0";
            BtnPoint.IsEnabled = true;
            MessageBox.Show("Text was copied to clipboard successfully!");
        }

        private void PasteClick(object sender, RoutedEventArgs e)
        {
            string savedText = TxtDisplay.Text;
            TxtDisplay.Clear();
            Regex regex = new Regex("[+-]?[0-9]||[0-9][.0-9]||[0-9][,0-9]");
            if (regex.IsMatch(Clipboard.GetText()) == true)
            {
                TxtDisplay.Paste();
                if (TxtDisplay.Text.Contains(".") == true)
                {
                    BtnPoint.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Invalid input!");
                TxtDisplay.Text = savedText;
            }
        }

        private void CopyClick(object sender, RoutedEventArgs e)
        {
            TxtDisplay.SelectAll();
            TxtDisplay.Copy();
            MessageBox.Show("Text was copied to clipboard successfully!");
        }

        private string GetDigit(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                    return "0";
                case Key.D1:
                    return "1";
                case Key.D2:
                    return "2";
                case Key.D3:
                    return "3";
                case Key.D4:
                    return "4";
                case Key.D5:
                    return "5";
                case Key.D6:
                    return "6";
                case Key.D7:
                    return "7";
                case Key.D8:
                    return "8";
                case Key.D9:
                    return "9";
                case Key.NumPad0:
                    return "0";
                case Key.NumPad1:
                    return "1";
                case Key.NumPad2:
                    return "2";
                case Key.NumPad3:
                    return "3";
                case Key.NumPad4:
                    return "4";
                case Key.NumPad5:
                    return "5";
                case Key.NumPad6:
                    return "6";
                case Key.NumPad7:
                    return "7";
                case Key.NumPad8:
                    return "8";
                case Key.NumPad9:
                    return "9";
                default:
                    return null;
            }
        }

        private string GetOperation(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Add:
                    return "+";
                case Key.Subtract:
                    return "-";
                case Key.Multiply:
                    return "*";
                case Key.Divide:
                    return "/";
                default:
                    return null;
            }
        }

        private void KeyboardKeyDown(object sender, KeyEventArgs e)
        {
            if (TxtDisplay.Text != "0" && GetDigit(e) != null && operationChecked == false)
            {
                TxtDisplay.Text += GetDigit(e);
                if (digitGroupingCheck == true)
                {
                    string decimalPart = "";
                    if (TxtDisplay.Text.Contains(".") == true)
                    {
                        decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                    }
                    int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                    string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                    if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                    {
                        changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                    }
                    if (TxtDisplay.Text.Contains("."))
                    {
                        changedDisplay += decimalPart;
                    }
                    if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                    {
                        changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                    }
                    if (TxtDisplay.Text.Contains(".") == true && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                    {
                        changedDisplay += "0";
                    }
                    TxtDisplay.Text = changedDisplay;
                }
                else
                {
                    double number = double.Parse(TxtDisplay.Text);
                    TxtDisplay.Text = number.ToString("G");
                }
                EnableDisableButtons(true);
                BtnEqual.IsEnabled = true;
                BtnC.IsEnabled = true;
                BtnCE.IsEnabled = true;
            }
            else if (TxtDisplay.Text == "0" && GetDigit(e) != null)
            {
                TxtDisplay.Text = GetDigit(e);
                if (digitGroupingCheck == true)
                {
                    string decimalPart = "";
                    if (TxtDisplay.Text.Contains(".") == true)
                    {
                        decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                    }
                    int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                    string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                    if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                    {
                        changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                    }
                    if (TxtDisplay.Text.Contains("."))
                    {
                        changedDisplay += decimalPart;
                    }
                    if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                    {
                        changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                    }
                    if (TxtDisplay.Text.Contains(".") == true && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                    {
                        changedDisplay += "0";
                    }
                    TxtDisplay.Text = changedDisplay;
                }
                else
                {
                    double number = double.Parse(TxtDisplay.Text);
                    TxtDisplay.Text = number.ToString("G");
                }
                EnableDisableButtons(true);
                BtnEqual.IsEnabled = true;
                BtnC.IsEnabled = true;
                BtnCE.IsEnabled = true;
            }
            else if (TxtDisplay.Text != "0" && GetDigit(e) != null && operationChecked == true)
            {
                TxtDisplay.Text = GetDigit(e);
                operationChecked = false;
                if (digitGroupingCheck == true)
                {
                    string decimalPart = "";
                    if (TxtDisplay.Text.Contains(".") == true)
                    {
                        decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                    }
                    int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                    string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                    if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                    {
                        changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                    }
                    if (TxtDisplay.Text.Contains("."))
                    {
                        changedDisplay += decimalPart;
                    }
                    if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                    {
                        changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                    }
                    if (TxtDisplay.Text.Contains(".") == true && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                    {
                        changedDisplay += "0";
                    }
                    TxtDisplay.Text = changedDisplay;
                }
                else
                {
                    double number = double.Parse(TxtDisplay.Text);
                    TxtDisplay.Text = number.ToString("G");
                }
                EnableDisableButtons(true);
                BtnEqual.IsEnabled = true;
                BtnC.IsEnabled = true;
                BtnCE.IsEnabled = true;
            }
            if (e.Key == Key.Escape)
            {
                NumericalButtons(true);
                BtnEqual.IsEnabled = true;
                BtnPoint.IsEnabled = true;
                BtnCE.IsEnabled = true;
                BtnMemoryAdd.IsEnabled = true;
                BtnMemorySubstract.IsEnabled = true;
                BtnMemoryStore.IsEnabled = true;
                TxtDisplay.Text = "0";
                operations.numberStack.Clear();
                operations.operationStack.Clear();
                operations.previousValue = 0;
                EnableDisableButtons(true);
                if (digitGroupingCheck == true)
                {
                    string decimalPart = "";
                    if (TxtDisplay.Text.Contains(".") == true)
                    {
                        decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                    }
                    int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                    string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                    if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                    {
                        changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                    }
                    if (TxtDisplay.Text.Contains("."))
                    {
                        changedDisplay += decimalPart;
                    }
                    if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                    {
                        changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                    }
                    if (TxtDisplay.Text.Contains(".") == true && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                    {
                        changedDisplay += "0";
                    }
                    TxtDisplay.Text = changedDisplay;
                }
                else
                {
                    double number = double.Parse(TxtDisplay.Text);
                    TxtDisplay.Text = number.ToString("G");
                }
            }
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (operations.numberStack.Count != 0 && operations.operationStack.Count != 0)
                {
                    BtnCE.IsEnabled = false;
                    operationChecked = false;
                    BtnEqual.IsEnabled = false;
                    BtnPoint.IsEnabled = true;
                    BtnC.IsEnabled = true;
                    string text = operations.EqualOperation(TxtDisplay.Text);
                    TxtDisplay.Clear();
                    TxtDisplay.Text = text;
                    if (TxtDisplay.Text == "Cannot divide by zero!")
                    {
                        EnableDisableButtons(false);
                        NumericalButtons(false);
                        BtnEqual.IsEnabled = false;
                        BtnPoint.IsEnabled = false;
                        BtnCE.IsEnabled = false;
                        BtnMemoryAdd.IsEnabled = false;
                        BtnMemorySubstract.IsEnabled = false;
                        BtnMemoryStore.IsEnabled = false;
                    }
                    if(TxtDisplay.Text == "Invalid input!")
                    {
                        EnableDisableButtons(false);
                        NumericalButtons(false);
                        BtnEqual.IsEnabled = false;
                        BtnPoint.IsEnabled = false;
                        BtnCE.IsEnabled = false;
                        BtnMemoryAdd.IsEnabled = false;
                        BtnMemorySubstract.IsEnabled = false;
                        BtnMemoryStore.IsEnabled = false;
                    }
                    if(TxtDisplay.Text!="Invalid input!"&&TxtDisplay.Text!="Cannot divide by zero!")
                    {
                        if (digitGroupingCheck == true)
                        {
                            string decimalPart = "";
                            if (TxtDisplay.Text.Contains(".") == true)
                            {
                                decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                            }
                            int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                            string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                            if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                            {
                                changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                            }
                            if (TxtDisplay.Text.Contains("."))
                            {
                                changedDisplay += decimalPart;
                            }
                            if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                            {
                                changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                            }
                            if (TxtDisplay.Text.Contains(".") == true && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                            {
                                changedDisplay += "0";
                            }
                            TxtDisplay.Text = changedDisplay;
                        }
                        else
                        {
                            double number = double.Parse(TxtDisplay.Text);
                            TxtDisplay.Text = number.ToString("G");
                        }
                        EnableDisableButtons(true);
                        BtnEqual.IsEnabled = true;
                    }
                }
            }
            if(e.Key == Key.Decimal)
            {
                if(BtnPoint.IsEnabled == true)
                {
                    TxtDisplay.Text = TxtDisplay.Text + ".";
                    BtnPoint.IsEnabled = false;
                }
            }
            typeOfOperation = GetOperation(e);
            if (typeOfOperation != null)
            {
                BtnCE.IsEnabled = false;
                BtnC.IsEnabled = true;
                EnableDisableButtons(false);
                BtnEqual.IsEnabled = false;
                BtnPoint.IsEnabled = true;
                string text = operations.MakeOperation(TxtDisplay.Text, typeOfOperation);
                TxtDisplay.Clear();
                TxtDisplay.Text = text;
                if (TxtDisplay.Text == "Cannot divide by zero!")
                {
                    EnableDisableButtons(false);
                    NumericalButtons(false);
                    BtnEqual.IsEnabled = false;
                    BtnPoint.IsEnabled = false;
                    BtnCE.IsEnabled = false;
                    BtnMemoryAdd.IsEnabled = false;
                    BtnMemorySubstract.IsEnabled = false;
                    BtnMemoryStore.IsEnabled = false;
                }
                operationChecked = true;
                if (digitGroupingCheck == true)
                {
                    string decimalPart = "";
                    if (TxtDisplay.Text.Contains(".") == true)
                    {
                        decimalPart = TxtDisplay.Text.Substring(TxtDisplay.Text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                    }
                    int numberOnDisplay = (int)(double.Parse(TxtDisplay.Text));
                    string changedDisplay = numberOnDisplay.ToString("N", CultureInfo.CurrentCulture);
                    if (changedDisplay.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00"))
                    {
                        changedDisplay = changedDisplay.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "00", "");
                    }
                    if (TxtDisplay.Text.Contains("."))
                    {
                        changedDisplay += decimalPart;
                    }
                    if (TxtDisplay.Text.Contains(".") && changedDisplay[changedDisplay.Length - 1] == '0')
                    {
                        changedDisplay = changedDisplay.Substring(0, changedDisplay.Length - 1);
                    }
                    if (TxtDisplay.Text.Contains(".") == true && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                    {
                        changedDisplay += "0";
                    }
                    TxtDisplay.Text = changedDisplay;
                }
                else
                {
                    double number = double.Parse(TxtDisplay.Text);
                    TxtDisplay.Text = number.ToString("G");
                }
                
            }
            
        }
    }
}
