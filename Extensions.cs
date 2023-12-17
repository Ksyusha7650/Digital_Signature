using System;
using System.IO;
using Microsoft.Win32;

namespace Digital_Signature;

public class Extensions
{
    public static OpenFileDialog OpenFileDialog(string nameFilter, string filter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = $"{nameFilter} (*.{filter})|*.{filter}|Все файлы (*.{filter})|*.{filter}",
            FilterIndex = 2,
            RestoreDirectory = true
        };
        return openFileDialog;
    }

    public static OpenFileDialog OpenTextFileDialog()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter =
                "Текстовые файлы (*.txt)|*.txt|Документы Word (*.docx)|*.docx|PDF файлы (*.pdf)|*.pdf|Все файлы (*.*)|*.*",
            FilterIndex = 2,
            RestoreDirectory = true
        };
        return openFileDialog;
    }

    public static SaveFileDialog SaveFileDialog() // выбрать папку
    {
        var saveFileDialog = new SaveFileDialog
        {
            FileName = "Сохранить здесь"
        };
        return saveFileDialog;
    }

    public static string ReadFromFile(string path)
    {
        var streamReader = File.OpenText(path);
        var content = "";
        while (!streamReader.EndOfStream)
        {
            content += streamReader.ReadLine();
            content += Environment.NewLine;
        }

        streamReader.Close();
        return content;
    }
}