using System.Reactive.Disposables;
using System.Windows;
using ReactiveUI;

namespace WPFFront.Views;

public partial class LessonView
{
    public LessonView()
    {
        InitializeComponent();
        this.WhenActivated(disposableRegistration =>
        {
            this.OneWayBind(ViewModel,
                    viewModel => viewModel.Name,
                    view => view.LessonName.Text)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                viewModel => viewModel.Description,
                view => view.Description.Text)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                    viewModel => viewModel.LessonType,
                    view => view.LessonType.Text)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                    viewModel => viewModel.LessonTime,
                    view => view.LessonTime.Text)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                    viewModel => viewModel.Location,
                    view => view.Location.Text)
                .DisposeWith(disposableRegistration);

            this.OneWayBind(ViewModel,
                    viewModel => viewModel.TeacherName,
                    view => view.Teacher.Text)
                .DisposeWith(disposableRegistration);

            this.BindCommand(ViewModel,
                    viewModel => viewModel.DeleteLesson,
                    view => view.DeleteLesson)
                .DisposeWith(disposableRegistration);
        });
    }
}