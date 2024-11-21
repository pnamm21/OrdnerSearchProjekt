using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace FolderChecker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            string baseDirectoryPath = PathTextBox.Text.Trim();

            if (string.IsNullOrEmpty(baseDirectoryPath))
            {
                MessageBox.Show("Pfad darf nicht leer sein.");
                return;
            }

            if (!Directory.Exists(baseDirectoryPath))
            {
                MessageBox.Show("Das angegebene Verzeichnis existiert nicht.");
                return;
            }

            List<string> expectedFolders = GenerateFolderNames(1, 999);
            ResultsListBox.Items.Clear();

            for (int i = 1; i <= 9; i++)
            {
                string subDirectoryPath = Path.Combine(baseDirectoryPath, i.ToString("D3"));

                if (Directory.Exists(subDirectoryPath))
                {
                    List<string> missingFolders = FindMissingFolders(subDirectoryPath, expectedFolders);
                    ResultsListBox.Items.Add($"Fehlende Ordner in {subDirectoryPath}:");

                    if (missingFolders.Count > 0)
                    {
                        foreach (var folder in missingFolders)
                        {
                            ResultsListBox.Items.Add(folder);
                        }
                    }
                    else
                    {
                        ResultsListBox.Items.Add("Alle erwarteten Ordner sind vorhanden.");
                    }
                }
                else
                {
                    ResultsListBox.Items.Add($"Das Verzeichnis {subDirectoryPath} existiert nicht.");
                }

                ResultsListBox.Items.Add("");
            }
        }

        private List<string> GenerateFolderNames(int start, int end)
        {
            List<string> folderNames = new List<string>();

            for (int i = start; i <= end; i++)
            {
                folderNames.Add(i.ToString("D3"));
            }

            return folderNames;
        }

        private List<string> FindMissingFolders(string directoryPath, List<string> expectedFolders)
        {
            List<string> missingFolders = new List<string>();

            string[] existingFolders = Directory.GetDirectories(directoryPath);

            foreach (var folder in expectedFolders)
            {
                bool folderExists = false;

                foreach (var existingFolder in existingFolders)
                {
                    if (existingFolder.EndsWith(folder))
                    {
                        folderExists = true;
                        break;
                    }
                }

                if (!folderExists)
                {
                    missingFolders.Add(folder);
                }
            }

            return missingFolders;
        }
    }
}
