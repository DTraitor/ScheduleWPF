using System.Reactive.Disposables;
using System.Windows;
using Main;
using Main.Models;
using Main.ViewModels;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace WPFFront;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        datePicker.SelectedDate = DateTime.Now.Date;
        typeComboBox.ItemsSource = Enum.GetValues(typeof(LessonType)).Cast<LessonType>();
        weekNumberComboBox.ItemsSource = Enum.GetValues(typeof(WeekNumber)).Cast<WeekNumber>();
        dayNumberComboBox.ItemsSource = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();

        var mySqlOptions = new DbContextOptionsBuilder<ScheduleDbContext>()
            .UseMySQL(
            $"Server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
            $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
            $"Uid={Environment.GetEnvironmentVariable("DB_USER")};" +
            $"Pwd={Environment.GetEnvironmentVariable("DB_PASS")};"
            ).Options;
        ViewModel = new AppViewModel(new ScheduleDbContext(mySqlOptions));

        teacherComboBox.ItemsSource = ViewModel.Teachers;

        this.WhenActivated(disposableRegistration =>
        {
            //WeekList
            this.OneWayBind(ViewModel,
                    viewModel => viewModel.DayScheduleViewModels,
                    view => view.WeekList.ItemsSource)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                    viewModel => viewModel.SelectedDate,
                    view => view.datePicker.SelectedDate)
                .DisposeWith(disposableRegistration);

            this.Bind(ViewModel,
                viewModel => viewModel.Name,
                view => view.nameTextBox.Text)
                .DisposeWith(disposableRegistration);

            //type
            this.Bind(ViewModel,
                viewModel => viewModel.Type,
                view => view.typeComboBox.SelectedItem)
                .DisposeWith(disposableRegistration);

            //desc
            this.Bind(ViewModel,
                viewModel => viewModel.Description,
                view => view.descriptionTextBox.Text)
                .DisposeWith(disposableRegistration);
            //teacher
            this.Bind(ViewModel,
                viewModel => viewModel.Teacher,
                view => view.teacherComboBox.SelectedItem)
                .DisposeWith(disposableRegistration);
            //location
            this.Bind(ViewModel,
                viewModel => viewModel.Location,
                view => view.locationTextBox.Text)
                .DisposeWith(disposableRegistration);
            //begin date
            this.Bind(ViewModel,
                viewModel => viewModel.Begin,
                view => view.beginDatePicker.SelectedDate)
                .DisposeWith(disposableRegistration);
            //end date
            this.Bind(ViewModel,
                viewModel => viewModel.End,
                view => view.endDatePicker.SelectedDate)
                .DisposeWith(disposableRegistration);
            //begin time
            this.Bind(ViewModel,
                viewModel => viewModel.BeginTime,
                view => view.beginTimePicker.Value)
                .DisposeWith(disposableRegistration);
            //end time
            this.Bind(ViewModel,
                viewModel => viewModel.EndTime,
                view => view.endTimePicker.Value)
                .DisposeWith(disposableRegistration);
            //week number
            this.Bind(ViewModel,
                viewModel => viewModel.SelectedWeekNumber,
                view => view.weekNumberComboBox.SelectedItem)
                .DisposeWith(disposableRegistration);
            //day number
            this.Bind(ViewModel,
                viewModel => viewModel.DayOfWeek,
                view => view.dayNumberComboBox.SelectedItem)
                .DisposeWith(disposableRegistration);

            //bind command
            this.BindCommand(ViewModel,
                    viewModel => viewModel.AddNewLesson,
                    view => view.addButton)
                .DisposeWith(disposableRegistration);

            this.BindCommand(ViewModel,
                    viewModel => viewModel.NextWeek,
                    view => view.nextButton)
                .DisposeWith(disposableRegistration);

            this.BindCommand(ViewModel,
                    viewModel => viewModel.PreviousWeek,
                    view => view.previousButton)
                .DisposeWith(disposableRegistration);
        });
    }

    private void StopHere(object sender, RoutedEventArgs e)
    {
        return;
    }
}