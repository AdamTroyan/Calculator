using Calculator.Commands;
using Calculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _KeyPressedString;

        public string KeyPressedString
        {
            get { return _KeyPressedString; }
            set { _KeyPressedString = value; OnPropertyChanged(nameof(KeyPressedString)); }
        }

        private string _Entered_Number;

        public string Entered_Number
        {
            get { return _Entered_Number; }
            set { _Entered_Number = value; OnPropertyChanged(nameof(Entered_Number)); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private ButtonPressedCommand _buttonPressedCommand;

        public ButtonPressedCommand buttonPressedCommand
        {
            get { return _buttonPressedCommand; }
            set { _buttonPressedCommand = value; }
        }

        List<string> EnteredKeys;
        double Number = 0;
        bool FirstNumberEntered = true;
        bool EqualToFlag = false;
        bool FunctionPressed = false;
        string SelectedFunction = "";
        public string PreviousEnteredKey = "";

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        void UpdateEnteredKeysOnGui()
        {
            string temp = "";

            for (int i = 0; i < EnteredKeys.Count; i++)
            {
                temp += EnteredKeys[i];
            }

            KeyPressedString = temp;
        }

        void Addition()
        {
            Number += Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
        }

        void Subtraction()
        {
            Number -= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
        }

        void Multiplication()
        {
            Number *= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
        }

        void Division()
        {
            Number /= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
        }

        void EqualTo()
        {
            EnteredKeys.Clear();
            EnteredKeys.Add(Entered_Number);

            EqualToFlag = false;
        }

        void Clear()
        {
            EnteredKeys.Clear();
            KeyPressedString = "";
            Entered_Number = "0";
            Number = 0;
            FirstNumberEntered = true;
            EqualToFlag = false;
        }

        bool PressedButtonIsOperator(string pressedButton)
        {
            if (pressedButton == "0" || pressedButton == "1" || pressedButton == "2" || pressedButton == "3" || pressedButton == "4" ||
                pressedButton == "5" || pressedButton == "6" || pressedButton == "7" || pressedButton == "8" || pressedButton == "9" || pressedButton == ".")
            {

                if (EqualToFlag)
                    Clear();

                EnteredKeys.Add(pressedButton);
                UpdateEnteredKeysOnGui();

                PreviousEnteredKey = pressedButton;

                if (FunctionPressed)
                {
                    Entered_Number = "0";
                    FunctionPressed = false;
                }

                if (Entered_Number == "0")
                    Entered_Number = pressedButton;
                else
                    Entered_Number += pressedButton;

                return false;
            }
            else
                return true;
        }

        public void GetPressedButton(string pressedButton)
        {
            if (PressedButtonIsOperator(pressedButton))
            {
                if (PreviousEnteredKey != pressedButton && PreviousEnteredKey != "+" && PreviousEnteredKey != "-" && PreviousEnteredKey != "/" &&
                    PreviousEnteredKey != "*")
                {
                    if (EnteredKeys.Count == 0)
                    {
                        EnteredKeys.Add(Entered_Number);
                        UpdateEnteredKeysOnGui();
                    }

                    if (FirstNumberEntered)
                    {
                        try
                        {
                            Number = Convert.ToDouble(Entered_Number);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        Entered_Number = Number.ToString();
                        FirstNumberEntered = false;
                    }

                    else
                    {
                        switch (SelectedFunction)
                        {
                            case "Addition": Addition(); break;
                            case "Subtraction": Subtraction(); break;
                            case "Multiplication": Multiplication(); break;
                            case "Division": Division(); break;
                            case "EqualTo": EqualTo(); break;
                        }
                    }

                    switch (pressedButton)
                    {
                        case "+":
                            SelectedFunction = "Addition";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;

                        case "-":
                            SelectedFunction = "Subtraction";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;

                        case "/":
                            SelectedFunction = "Division";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;

                        case "*":
                            SelectedFunction = "Multiplication";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;

                        case "=":
                            SelectedFunction = "EqualTo";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            EqualToFlag = true;
                            break;

                        case "Clr":
                            Clear();
                            FunctionPressed = true;
                            PreviousEnteredKey = pressedButton;
                            break;
                    }
                }

                else if (PreviousEnteredKey == "+" || PreviousEnteredKey == "-" || PreviousEnteredKey == "/" || PreviousEnteredKey == "*" | PreviousEnteredKey == "Clr")
                {
                    EnteredKeys.RemoveAt(EnteredKeys.Count() - 1);
                    EnteredKeys.Add(pressedButton);
                    UpdateEnteredKeysOnGui();

                    PreviousEnteredKey = pressedButton;
                    FunctionPressed = true;

                    switch (pressedButton)
                    {
                        case "+": SelectedFunction = "Addition"; break;
                        case "-": SelectedFunction = "Subtraction"; break;
                        case "*": SelectedFunction = "Multiplication"; break;
                        case "/": SelectedFunction = "Division"; break;
                        case "=": SelectedFunction = "EqualTo"; break;
                        case "Clr": Clear(); break;
                    }
                }
            }
        }

        public MainViewModel()
        {
            Entered_Number = "0";
            KeyPressedString = "";
            buttonPressedCommand = new ButtonPressedCommand(this);
            EnteredKeys = new List<string>();
        }
    }
}
