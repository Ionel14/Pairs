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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pairs
{
    public partial class MainWindow : Window
    {
        public Hashtable users;
        private const string usersFile = @"..\..\Resurse\Users.txt";

        public MainWindow()
        {
            InitializeComponent();

            showLoggedUsers(usersFile);


        }

        private void showLoggedUsers(string filePath)
        {
            Uri fileUri = new Uri(filePath , UriKind.Relative);
            string[] lines = File.ReadAllLines(fileUri.ToString());

            ListBox usersList = (ListBox)FindName("usersList");
            users = new Hashtable();
            foreach (string line in lines)
            {
                string[] values = line.Split(' ');
                usersList.Items.Add(values[0]);
                User user = new User(values[0], values[1], Int32.Parse(values[2]), Int32.Parse(values[3]));
                users.Add(values[0], user);
            }
        }

        private void usersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usersList.SelectedItem != null)
            {
                DeleteUserButton.IsEnabled = true;
                PlayButton.IsEnabled = true;

                string username = usersList.SelectedItem.ToString();
                string avatarName = ((User)users[username]).avatarName;
                statisticsLabel.Content = "Games: " + ((User)users[username]).gamesPlayed + "\nWins: " + ((User)users[username]).wins;
                Uri imgUri = new Uri(@"Resurse\Avatars\" + avatarName, UriKind.Relative);
                BitmapImage bitmap = new BitmapImage(imgUri);
                userAvatar.Source = bitmap;
            }
        }

        private void updateFile()
        {
            string data = "";
            foreach (DictionaryEntry pair in users)
            {
                data += pair.Key.ToString() + ' ' + ((User)pair.Value).avatarName + ' ' + ((User)pair.Value).gamesPlayed + ' ' + ((User)pair.Value).wins + '\n';
            }
            
            Uri fileUri = new Uri(usersFile, UriKind.Relative);
            File.WriteAllText(fileUri.ToString(), data);
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vulcanean Ionel\n10lf213\nInformatica");
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            users.Remove(usersList.SelectedItem);
            if (File.Exists(@"..\..\Resurse\UsersSavedGamesData\" + usersList.SelectedItem + ".xml"))
            {
                File.Delete(@"..\..\Resurse\UsersSavedGamesData\" + usersList.SelectedItem + ".xml");
            }
            usersList.Items.RemoveAt(usersList.Items.IndexOf(usersList.SelectedItem));
            DeleteUserButton.IsEnabled = false;
            PlayButton.IsEnabled = false;
            updateFile();
        }

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            SignInPage signInPage = new SignInPage(users);
            this.Hide();
            signInPage.ShowDialog();
            this.Show();
            usersList.Items.Add(signInPage.usernameBox.Text);
            updateFile();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayScreen playScreen = new PlayScreen((User)users[usersList.SelectedItem.ToString()]);
            playScreen.Owner = this;
            this.Hide();
            playScreen.ShowDialog();
            updateFile();
        }
    }
}

