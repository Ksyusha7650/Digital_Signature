using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Digital_Signature
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _file;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonAddFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() != true) return;
            StackPanelFiles.Children.Add(new Label
            {
                Content = openFileDialog.FileName
            });
            _file = openFileDialog.FileName;
        }

        private void StackPanelFiles_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop) ?? Array.Empty<string>();
            StackPanelFiles.Children.Add(new Label
            {
                Content = files[0]
            });
            _file = files[0];
        }

        private void ButtonAddDS_Click(object sender, RoutedEventArgs e)
        {
            var streamReader = File.OpenText(_file);
            var content = "";
            while (!streamReader.EndOfStream)
            {
                content += streamReader.ReadLine();
                content += Environment.NewLine;
            }
            streamReader.Close();
            var sign = AlgorithmCrypting.Crypt(content);
            var streamWriter = File.CreateText(_file);
            streamWriter.WriteLine(sign);
            streamWriter.WriteLine(content);
            streamWriter.Close();
            MessageBox.Show("Add");
        }

        private void ButtonCheckDS_Click(object sender, RoutedEventArgs e)
        {
            var streamReader = File.OpenText(_file);
            var content = "";
            while (!streamReader.EndOfStream)
            {
                content += streamReader.ReadLine();
                content += Environment.NewLine;
            }
            streamReader.Close();
            var check = AlgorithmCrypting.Check(content);
            MessageBox.Show(check? "OK" : "NO");
        }
    }
}
