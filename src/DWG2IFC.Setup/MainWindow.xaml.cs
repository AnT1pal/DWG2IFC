using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Plant3D.IfcExporter.Setup
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            bool installed = InstallerService.IsInstalled();
            TxtPath.Text = "Путь: " + InstallerService.InstalledBundlePath;

            if (installed)
            {
                TxtStatus.Text = "✔ Установлен и готов к работе";
                TxtStatus.Foreground = new SolidColorBrush(Color.FromRgb(52, 211, 153));
                BtnInstall.Content = "Переустановить / Обновить";
                BtnUninstall.IsEnabled = true;
            }
            else
            {
                TxtStatus.Text = "✖ Не установлен";
                TxtStatus.Foreground = new SolidColorBrush(Color.FromRgb(248, 113, 113));
                BtnInstall.Content = "Установить плагин";
                BtnUninstall.IsEnabled = false;
            }
        }

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            var res = InstallerService.Install();
            if (res.Success)
            {
                RefreshStatus();
                MessageBox.Show(
                    "Плагин успешно установлен!\n\nПоддерживаются AutoCAD и Plant 3D (версии 2025 и 2026).\n\nВкладка на ленте: «IFC Экспорт»\nКоманды в командной строке:\nIFCEXPORT\nPLANTTOIFC\nIFCRIBBON",
                    "Установка завершена",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            else
            {
                MessageBox.Show(res.Message, "Ошибка установки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUninstall_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show(
                "Вы уверены, что хотите удалить плагин Plant 3D IFC Exporter из системы?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirm != MessageBoxResult.Yes) return;

            var res = InstallerService.Uninstall();
            if (res.Success)
            {
                RefreshStatus();
                MessageBox.Show(res.Message, "Удаление завершено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(res.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Directory.Exists(InstallerService.InstalledBundlePath)
                    ? InstallerService.InstalledBundlePath
                    : InstallerService.TargetPluginsDir;

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Process.Start(new ProcessStartInfo("explorer.exe", path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть каталог: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnWebsite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://baidurovlabs.ru") { UseShellExecute = true });
            }
            catch
            {
            }
        }
    }
}
