﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //TO DO: make it so the messages written and read form the database so all people can read the same messages (make not instance spacific messages)
        //       make it so that only the administraor can add new messages
        public MainPage()
        {
            InitializeComponent();

            tempDatabase = new string[25];
        }
        //example database replace with real one once its constructed
        string[] tempDatabase;
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Message added", "Your message has been added to the main page", "exit");
            //example database stuff remove later
            for(int i = 0; i<tempDatabase.Length-1 ; i++)
            {
                //e.g. position 5 = pos 4 ...etc until pos 0 reached
                tempDatabase[(tempDatabase.Length - 1)-i] = tempDatabase[(tempDatabase.Length - 1) - (i+1)];
            }
            tempDatabase[0] = string.Format(inputBox.Text);
    
            inputBox.Text = string.Empty; //clear inputBox
            Messages.Text = string.Empty; //clears the text string

            //display the messages \/\/ make this better later
            for (int j = 0; j < tempDatabase.Length - 1; j++)
            {
                //adds all the values in the array to the text string if the value is not empty/""
                if(tempDatabase[j] != "") //not sure if this IF actully works yet.....
                {
                    Messages.Text += tempDatabase[j]+ "\n";
                } 
            }
        }
    }
}