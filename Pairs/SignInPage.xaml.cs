using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public partial class SignInPage : Window
    {
        public string username;
        public string imageName;
        Hashtable users;

        private string[] imagePaths = Directory.GetFiles(@"..\..\Resurse\Avatars", "*.png");
        private int indexAvatar = 0;

        public SignInPage(Hashtable users)
        {
            InitializeComponent();
            this.users = users;
            Uri imgUri = new Uri(imagePaths[indexAvatar], UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(imgUri);
            avatar.Source = bitmap;
        }

        public SignInPage()
        {
            InitializeComponent();

            Uri imgUri = new Uri(imagePaths[indexAvatar], UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(imgUri);
            avatar.Source = bitmap;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            username = usernameBox.Text;
            string[] imageName = imagePaths[indexAvatar].Split('\\');
            User user = new User(usernameBox.Text, imageName[imageName.Length - 1], 0, 0);
            users.Add(usernameBox.Text, user);
            this.Close();
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            indexAvatar++;
            if (indexAvatar >= imagePaths.Length)
            {
                indexAvatar = 0;
            }
            Uri imgUri = new Uri(imagePaths[indexAvatar], UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(imgUri);
            avatar.Source = bitmap;
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            indexAvatar--;
            if (indexAvatar < 0)
            {
                indexAvatar = imagePaths.Length - 1;
            }
            Uri imgUri = new Uri(imagePaths[indexAvatar], UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(imgUri);
            MessageBox.Show(imgUri.ToString());
            avatar.Source = bitmap;
        }
    }
}
