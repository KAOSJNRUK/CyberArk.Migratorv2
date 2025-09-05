using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace CyberArk.Migrator.App
{
    public partial class MainWindow : Window
    {
        private string? _selectedCsvPath;
        private const string AccountsEnc = "accounts.enc";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
                txtLog.ScrollToEnd();
            });
        }

        private int ParseInt(string input, int fallback)
        {
            return int.TryParse(input, out var val) ? val : fallback;
        }

        private async void CreateSampleCsv_Click(object sender, RoutedEventArgs e)
        {
            var samplePath = Path.Combine(Environment.CurrentDirectory, "sample.csv");
            await File.WriteAllTextAsync(samplePath, "Safe,Platform,Account\nDemoSafe,Windows,Admin");
            _selectedCsvPath = samplePath;
            txtSelectedCsv.Text = $"Sample CSV created: {samplePath}";
        }

        private void SelectExistingCsv_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                _selectedCsvPath = dlg.FileName;
                txtSelectedCsv.Text = $"Selected: {_selectedCsvPath}";
            }
        }

        private async void EncryptSelectedCsv_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedCsvPath) || !File.Exists(_selectedCsvPath))
            {
                Log("No CSV selected to encrypt.");
                return;
            }

            var outPath = _selectedCsvPath + ".enc";
            var bytes = await File.ReadAllBytesAsync(_selectedCsvPath);
            await File.WriteAllBytesAsync(outPath, bytes); // stub: replace with real encryption
            txtEncryptedPath.Text = $"Encrypted: {outPath}";
            Log($"Encrypted file written: {outPath}");
        }

        private async void RunImportPCloud_Click(object sender, RoutedEventArgs e)
        {
            await RunImportAsync("PCloud");
        }

        private async void RunImportSelfHosted_Click(object sender, RoutedEventArgs e)
        {
            await RunImportAsync("SelfHosted");
        }

        private async Task RunImportAsync(string targetType)
        {
            try
            {
                var encrypted = !string.IsNullOrWhiteSpace(txtEncryptedPath.Text) && txtEncryptedPath.Text.StartsWith("Encrypted:")
                    ? txtEncryptedPath.Text.Replace("Encrypted:", "").Trim()
                    : (_selectedCsvPath is null ? AccountsEnc : _selectedCsvPath + ".enc");

                Log($"Using encrypted file: {encrypted}");

                var doSafes     = (chkSafes?.IsChecked ?? false) == true;
                var doPlatforms = (chkPlatforms?.IsChecked ?? false) == true;
                var doAccounts  = (chkAccounts?.IsChecked ?? false) == true;

                if (!doSafes && !doPlatforms && !doAccounts)
                {
                    Log("Nothing selected to import. Enable at least one of: Safes, Platforms, Accounts.");
                    return;
                }

                // stub: simulate accounts load
                var accounts = new List<(string Safe, string Platform, string Account)>
                {
                    ("DemoSafe", "Windows", "Admin")
                };

                var batchSize = ParseInt(txtBatchSize.Text, 50);
                var dop = ParseInt(txtDop.Text, 4);

                // stub: just log operations
                if (doSafes)     Log("Ensuring safes...");
                if (doPlatforms) Log("Ensuring platforms...");
                if (doAccounts)  Log($"Importing {accounts.Count} accounts (batchSize={batchSize}, DoP={dop})");

                Log($"Done. Safes={doSafes}, Platforms={doPlatforms}, Accounts={doAccounts}.");
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
            }
        }
    }
}
