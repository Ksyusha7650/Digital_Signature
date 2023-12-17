using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Digital_Signature;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string _file, _key;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonAddFile_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = Extensions.OpenTextFileDialog();
        if (openFileDialog.ShowDialog() != true) return;
        StackPanelFile.Children.Clear();
        StackPanelFile.Children.Add(new Label
        {
            Content = openFileDialog.FileName
        });
        _file = openFileDialog.FileName;
    }

    private void ButtonAddKey_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = Extensions.OpenFileDialog("Ключи", "pem");
        if (openFileDialog.ShowDialog() != true) return;
        StackPanelKey.Children.Clear();
        StackPanelKey.Children.Add(new Label
        {
            Content = openFileDialog.FileName
        });
        _key = openFileDialog.FileName;
    }

    private void StackPanelFiles_Drop(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        StackPanelFile.Children.Clear();
        StackPanelFile.Children.Add(new Label
        {
            Content = files[0]
        });
        _file = files[0];
    }

    private void StackPanelKey_Drop(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (!files[0].EndsWith(".pem"))
            return;
        StackPanelKey.Children.Clear();
        StackPanelKey.Children.Add(new Label
        {
            Content = files[0]
        });
        _key = files[0];
    }

    private void ButtonAddDS_Click(object sender, RoutedEventArgs e)
    {
        if (_file is null)
        {
            MessageBox.Show("Добавьте файл!");
            return;
        }

        if (_key is null)
        {
            MessageBox.Show("Добавьте ключ!");
            return;
        }

        var content = Extensions.ReadFromFile(_file);
        var key = Extensions.ReadFromFile(_key);
        try
        {
            var sign = AlgorithmCrypto.Crypt(content, key);
            var streamWriter = File.CreateText(_file.Split(".")[0] + ".sig");
            streamWriter.WriteLine(sign);
            streamWriter.Close();
            MessageBox.Show("Подпись создана!");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void ButtonCheckDS_Click(object sender, RoutedEventArgs e)
    {
        if (_file is null)
        {
            MessageBox.Show("Добавьте файл!");
            return;
        }

        if (_key is null)
        {
            MessageBox.Show("Добавьте ключ!");
            return;
        }

        var openFileDialog = Extensions.OpenFileDialog("Подписи", "sig");
        if (openFileDialog.ShowDialog() != true) return;
        var content = Extensions.ReadFromFile(_file);
        var key = Extensions.ReadFromFile(_key);
        var signature = Extensions.ReadFromFile(openFileDialog.FileName);
        try
        {
            var check = AlgorithmCrypto.Check(content, key, signature);
            MessageBox.Show(check ? "Подпись действительна" : "Подпись недействительна");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        AlgorithmCrypto.MakeKeys();
    }
}