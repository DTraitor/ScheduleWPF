using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace WPFFront.Views;

public partial class DayScheduleView
{
    public DayScheduleView()
    {
        InitializeComponent();

        this.WhenActivated(disposableRegistration =>
        {
            this.OneWayBind(ViewModel,
                    viewModel => viewModel.DayOfTheWeek,
                    view => view.DayOfTheWeek.Text)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                    viewModel => viewModel.Schedule,
                    view => view.LessonList.ItemsSource)
                .DisposeWith(disposableRegistration);
        });
    }
}