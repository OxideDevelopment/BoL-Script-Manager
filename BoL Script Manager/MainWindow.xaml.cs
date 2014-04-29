using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using AutoUpdaterDotNET;

namespace BoLScriptManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            AutoUpdater.OpenDownloadPage = true;
            AutoUpdater.LetUserSelectRemindLater = false;
            AutoUpdater.Start("https://raw.githubusercontent.com/OxideDevelopment/BoL-Script-Manager/master/BoL%20Script%20Manager/Updates/updater.xml");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult dlgResult = dlg.ShowDialog();

            if (dlgResult != System.Windows.Forms.DialogResult.OK)
                return;

            bolScriptLocation.Text = dlg.SelectedPath;
            Properties.Settings.Default.BoLScript = dlg.SelectedPath;
            Properties.Settings.Default.Save();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult dlgResult = dlg.ShowDialog();

            if (dlgResult != System.Windows.Forms.DialogResult.OK)
                return;

            scriptLocation.Text = dlg.SelectedPath;
            Properties.Settings.Default.Script = dlg.SelectedPath;
            Properties.Settings.Default.Save();

            string[] files = Directory.GetFiles(dlg.SelectedPath);
            foreach (string file in files)
            {
                //Get just the file name.
                string[] rawname = file.Split('\\');
                _listBox.Items.Add(rawname[rawname.Length - 1]);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            _listBox.Items.Clear();

            string[] files = Directory.GetFiles(scriptLocation.Text);
            foreach (string file in files)
            {
                //Get just the file name.
                string[] rawname = file.Split('\\');
                _listBox.Items.Add(rawname[rawname.Length - 1]);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bolScriptLocation.Text = Properties.Settings.Default.BoLScript;
            scriptLocation.Text = Properties.Settings.Default.Script;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Move the scripts in BoL to "Script" folder
            string[] bolScripts = Directory.GetFiles(bolScriptLocation.Text);
            foreach (string script in bolScripts)
            {
                //Get the filename.
                string dest = scriptLocation.Text + "\\" + script;
                File.Move(script, dest);
                Button_Click_1(null, null);
            }

        }

        public async Task<MessageDialogResult> ShowMessage(string message, string title, MessageDialogStyle dialogStyle)
        {
            //                We imported System.Windows.Forms..
            var metroWindow = (System.Windows.Application.Current.MainWindow as MetroWindow);
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            return await metroWindow.ShowMessageAsync(title, message, dialogStyle, metroWindow.MetroDialogOptions);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (_listBox.SelectedItems.Count > 5)
            {
                ShowMessage("As a free user, please remember you can only use 5 scripts.", "Warning!", MessageDialogStyle.Affirmative).ConfigureAwait(false);
            }

            if (_listBox.SelectedItems.Count < 1)
            {
                ShowMessage("Please select at least 1 script.", "Error", MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(false);
                return;
            }

           //Move checked scripts into BoL folder from Scripts folder.
            foreach (string script in _listBox.SelectedItems)
            { 
                string dest = bolScriptLocation.Text + "\\" + script;
                File.Move(script, dest);
                Button_Click_1(null, null);
            }
        } 
    }
}
