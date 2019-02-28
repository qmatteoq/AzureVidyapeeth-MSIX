// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ExpenseItDemo.Data.Model;
using ExpenseItDemo.Data.Services;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ExpenseItDemo
{
    /// <summary>
    ///     Interaction logic for CreateExpenseReportDialogBox.xaml
    /// </summary>
    public partial class CreateExpenseReportDialogBox : Window
    {
        private Employee CurrentEmployee;
        private DatabaseService dbService;

        public ExpenseReport Report { get; set; }

        public CreateExpenseReportDialogBox()
        {
            InitializeComponent();
            dbService = new DatabaseService();
        }

        private void addExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
        }

        private void viewChartButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ViewChartWindow { Owner = this };
            dlg.Show();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Reports|*.rep";
            saveFileDialog.Title = "Save the report";
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                Stream fileStream = saveFileDialog.OpenFile();
                using (TextWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine($"Employee id: {CurrentEmployee.EmployeeId}");
                    writer.WriteLine($"Employee name: {CurrentEmployee.FirstName} {CurrentEmployee.LastName}");
                    writer.WriteLine($"Employee mail: {CurrentEmployee.Email}");
                    writer.WriteLine();
                    writer.WriteLine("Expenses:");
                    writer.WriteLine();
                    foreach (var expense in Report.Expenses)
                    {
                        writer.WriteLine($"{expense.Description} - Price: {expense.Cost} $");
                    }

                    writer.WriteLine($"Total: {Report.TotalExpenses} $");
                    writer.Flush();
                }

                string fullName = $"{CurrentEmployee.FirstName} {CurrentEmployee.LastName}";
                ShowToastNotification(fullName);
                UpdateTile(fullName, Report.TotalExpenses);

                DialogResult = true;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            CurrentEmployee = (Application.Current as App).SelectedEmployee;

            Report = new ExpenseReport(CurrentEmployee.EmployeeId);
            this.DataContext = Report;
        }

        public void ShowToastNotification(string employee)
        {
            string xml = $@"<toast>
            <visual>
                <binding template='ToastGeneric'>
                    <text>Expense Report created</text>
                    <text>The expense report for {employee} has been created</text>
                </binding>
            </visual>
        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            ToastNotification toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public void UpdateTile(string employee, double total)
        {
            string tileXml = $@"<tile>
                            <visual>
                                <binding template='TileMedium' branding='logo'>
                                    <group>
                                        <subgroup>
                                            <text hint-style='caption'>Expense report created</text>
                                            <text hint-style='captionSubtle' hint-wrap='true'>The expenses total for {employee} is {total} $</text>
                                        </subgroup>
                                    </group>
                                </binding>
                                <binding template='TileWide' branding='logo'>
                                    <group>
                                        <subgroup>
                                            <text hint-style='caption'>Expense report created</text>
                                            <text hint-style='captionSubtle' hint-wrap='true'>The expenses total for {employee} is {total} $</text>
                                        </subgroup>
                                    </group>
                                </binding>
                            </visual>
                        </tile>";

            XmlDocument tileDoc = new XmlDocument();
            tileDoc.LoadXml(tileXml);

            TileNotification notification = new TileNotification(tileDoc);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
    }
}