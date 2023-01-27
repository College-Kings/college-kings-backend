using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CollegeKings.Local.Uploader;

public partial class MainWindow
{
    private string _project = string.Empty;
    private string _description = string.Empty;
    private string _pcFolderPath = "";
    private string _macFolderPath = "";
    private readonly List<string> _logList = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    public async Task AddLog(string text)
    {
        await Task.Run(() =>
        {
            _logList.Add(text);
            Dispatcher.Invoke(() =>
            {
                LogTextBox.Text = string.Join('\n', _logList);
            });
        });
    }
    
    private void EnableUploadButton()
    {
        UploadButton.IsEnabled = !string.IsNullOrWhiteSpace(_pcFolderPath) && !string.IsNullOrWhiteSpace(_macFolderPath);
    }
    
    private void ProjectSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem? selectedItem = ProjectSelector.SelectedItem as ComboBoxItem;
        _project = selectedItem?.Content as string ?? string.Empty;
    }
    
    private void PcFolderButton_OnClick(object sender, RoutedEventArgs e)
    {
        using (FolderBrowserDialog dialog = new())
        {
            DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            
            _pcFolderPath = dialog.SelectedPath;
            PcFolderTextBox.Text = $"PC: {_pcFolderPath}";
        }

        EnableUploadButton();
    }

    private void MacFolderButton_OnClick(object sender, RoutedEventArgs e)
    {
        using (FolderBrowserDialog dialog = new())
        {
            DialogResult result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            
            _macFolderPath = dialog.SelectedPath;
            MacFolderTextBox.Text = $"MAC: {_macFolderPath}";
        }

        EnableUploadButton();
    }

    private async void UploadButton_OnClick(object sender, RoutedEventArgs e)
    {
        Uploader pcUploader = new(_project, OsType.Pc)
        {
            Description = _description,
            FolderPath = _pcFolderPath,
            MainWindow = this
        };

        Uploader macUploader = new(_project, OsType.Mac)
        {
            Description = _description,
            FolderPath = _macFolderPath,
            MainWindow = this
        };

        await Task.Run(() => pcUploader.Run());
        await Task.Run(() => macUploader.Run());
    }

    private void DescriptionTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        _description = DescriptionTextBox.Text;
    }
}