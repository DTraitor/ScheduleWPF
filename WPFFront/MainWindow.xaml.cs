using System.Reactive.Disposables;
using System.Windows;
using ReactiveUI;
using WPFFront.ViewModels;

namespace WPFFront;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        datePicker.SelectedDate = DateTime.Now.Date;
        ViewModel = new AppViewModel();

        this.WhenActivated(disposableRegistration =>
        {
            this.OneWayBind(ViewModel,
                    viewModel => viewModel.SelectedSchedule,
                    view => view.searchResultsListBox.ItemsSource)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    viewModel => viewModel.SelectedDate,
                    view => view.datePicker.SelectedDate)
                .DisposeWith(disposableRegistration);
        });
    }

    private void StopHere(object sender, RoutedEventArgs e)
    {
        return;
    }
}