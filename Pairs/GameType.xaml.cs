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
using System.Windows.Shapes;

namespace Pairs
{
    public partial class GameType : Window
    {
        public int width = 5; 
        public int height = 5;
        public GameType()
        {
            InitializeComponent();
            instructions1.Visibility = Visibility.Hidden;
            instructions2.Visibility = Visibility.Hidden;
            sizeBox.Visibility = Visibility.Hidden;
            readyButton.Visibility = Visibility.Hidden; 
        }

        private void standardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void customButton_Click(object sender, RoutedEventArgs e)
        {
            instructions1.Visibility = Visibility.Visible;
            instructions2.Visibility = Visibility.Visible;
            sizeBox.Visibility = Visibility.Visible;
            readyButton.Visibility = Visibility.Visible;
        }

        private void readyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sizeBox.Text.Length < 3 && sizeBox.Text.Length > 5)
            {
                MessageBox.Show("Your size is invalid\nPlease try again");
            }
            else
            {
                if (sizeBox.Text.Length == 3)
                {
                    if (sizeBox.Text[0] - '0' >= 2 && sizeBox.Text[0] - '0' < 10 && sizeBox.Text[2] - '0' >= 2 && sizeBox.Text[2] - '0' < 10)
                    {
                        width = sizeBox.Text[0] - '0';
                        height = sizeBox.Text[2] - '0';
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Your size is invalid\nPlease try again");
                    }
                }
                else if (sizeBox.Text[0] - '0' == 1 && sizeBox.Text[1] - '0' == 0 && sizeBox.Text[3] - '0' == 1 && sizeBox.Text[4] - '0' == 0)
                {
                    width = 10;
                    height = 10;
                    this.Close();
                }
                else if (sizeBox.Text[0] - '0' == 1 && sizeBox.Text[1] - '0' == 0 && sizeBox.Text[3] - '0' >= 2 && sizeBox.Text[3] - '0' < 10)
                {
                    width = 10;
                    height = sizeBox.Text[3] - '0';
                    this.Close();
                }
                else if(sizeBox.Text[0] - '0' >= 2 && sizeBox.Text[0] - '0' < 10 && sizeBox.Text[2] - '0' == 1 && sizeBox.Text[3] - '0' == 0)
                {
                    width = sizeBox.Text[0] - '0';
                    height = 10;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Your size is invalid\nPlease try again");
                }
            }
        }
    }
}
