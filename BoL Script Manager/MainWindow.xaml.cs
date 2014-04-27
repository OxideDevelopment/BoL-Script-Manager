using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult dlgResult = dlg.ShowDialog();

            if (dlgResult != System.Windows.Forms.DialogResult.OK)
                return;

            bolScriptLocation.Text = dlg.SelectedPath;
            Properties.Settings.Default.BoLScript = dlg.SelectedPath;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult dlgResult = dlg.ShowDialog();

            if (dlgResult != System.Windows.Forms.DialogResult.OK)
                return;

            scriptLocation.Text = dlg.SelectedPath;
            Properties.Settings.Default.Script = dlg.SelectedPath;

            string[] files = Directory.GetFiles(dlg.SelectedPath);
            foreach (string file in files)
            {
                _listBox.Items.Add(file);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            _listBox.Items.Clear();

            string[] files = Directory.GetFiles(scriptLocation.Text);
            foreach (string file in files)
            {
                _listBox.Items.Add(file);
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
                string[] filenameChop = script.Split('\\');
                string dest = scriptLocation.Text + "\\" + filenameChop[filenameChop.Length - 1];
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
                ShowMessage("Due to BoL restrictions, you may only choose 5 scripts.", "Error", MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(false);
                return;
            }

            if (_listBox.SelectedItems.Count < 1)
            {
                ShowMessage("Please select at least 1 script.", "Error", MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(false);
                return;
            }

           //Move checked scripts into BoL folder from Scripts folder.
            foreach (string script in _listBox.SelectedItems)
            {            
                string[] fileNameChopped = script.Split('\\');
                string dest = bolScriptLocation.Text + "\\" + fileNameChopped[fileNameChopped.Length - 1];
                File.Move(script, dest);
                Button_Click_1(null, null);
            }
        } 
    }
}
