using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TaskManager.Commands;

namespace TaskManager.Models
{
    public class TMViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProcessViewModel> Processes { get; set; }

        ProcessViewModel currentProcess;
        public ProcessViewModel CurrentProcess
        {
            get
            {
                return currentProcess;
            }
            set
            {
                currentProcess = value;
                OnPropertyChanged(nameof(CurrentProcess));
                OnPropertyChanged(nameof(SelectedPrior));
            }
        }

        public string newTask;
        public string NewTask
        {
            get { return newTask; }
            set
            {
                newTask = value;
                OnPropertyChanged(nameof(NewTask));
            }
        }

        List<string> priorities = new List<string> { "Normal", "Idle", "High", "RealTime", "BelowNormal", "AboveNormal" };

        public List<string> Priorities { get { return priorities; } }

        public string SelectedPrior
        {
            get
            {
                if (currentProcess == null)
                    return null;
                return priorities.FirstOrDefault(s => s == CurrentProcess.PriorClass);
            }
            set
            {
                try
                {
                    if (currentProcess == null)
                    {
                        MessageBox.Show("Choose process!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    CurrentProcess.PriorClass = value;
                    OnPropertyChanged(nameof(SelectedPrior));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public TMViewModel()
        {
            Processes = new ObservableCollection<ProcessViewModel>();
            foreach (var p in Process.GetProcesses())
                Processes.Add(new ProcessViewModel(p));

            DispatcherTimer timer2 = new DispatcherTimer(
                new TimeSpan(0, 0, 5),
                DispatcherPriority.Normal,
                ReloadProcesses,
                App.Current.Dispatcher);
        }

        void ReloadProcesses(object o, EventArgs e)
        {
            int? id = null;
            if (CurrentProcess != null)
                 id = CurrentProcess.Id;
            Processes.Clear();
            foreach (var p in Process.GetProcesses())
                Processes.Add(new ProcessViewModel(p));
            if (id != null)
                CurrentProcess = Processes.FirstOrDefault(p => p.Id == id);
        }

        //InotifypropChange
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        //Commands
        AppCommand addProc;

        public AppCommand AddProc
        {
            get
            {
                return addProc ?? (addProc = new AppCommand((o) =>
                {
                    if (NewTask != null && NewTask.Length > 0)
                    {
                        try
                        {
                            Process.Start(NewTask);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        finally
                        {
                            NewTask = "";
                        }
                    }
                }));
            }
        }

        AppCommand removeProc;

        public AppCommand RemoveProc
        {
            get
            {
                return removeProc ?? (removeProc = new AppCommand((o) =>
                {
                    if (currentProcess == null)
                    {
                        MessageBox.Show("Choose process!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    MessageBoxResult res = MessageBox.Show("Are you sure? You want remowe task?", "Warning!",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (res == MessageBoxResult.Yes)
                    {
                        try
                        {
                            currentProcess.P.Kill();
                            ReloadProcesses(null, null);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }));
            }
        }
    }
}