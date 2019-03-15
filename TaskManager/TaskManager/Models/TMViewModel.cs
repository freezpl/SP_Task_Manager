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
        { get
            {
                return currentProcess;
            }
            set
            {
                currentProcess = value;
                OnPropertyChanged(nameof(CurrentProcess));
                MessageBox.Show(currentProcess.Name);
            }
        }

        public string newTask;
        public string NewTask {
            get { return newTask; }
            set { newTask = value; }
        }
        
        public TMViewModel()
        {
            Processes = new ObservableCollection<ProcessViewModel>();
            foreach (var p in Process.GetProcesses())
                Processes.Add(new ProcessViewModel(p));

            DispatcherTimer timer2 = new DispatcherTimer(
                new TimeSpan(0, 0, 50),
                DispatcherPriority.Normal,
                ReloadProcesses,
                App.Current.Dispatcher);
        }

        void ReloadProcesses(object o, EventArgs e)
        {
            Processes.Clear();
            foreach (var p in Process.GetProcesses())
                Processes.Add(new ProcessViewModel(p));
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

        public AppCommand AddProc {
            get
            {
                return addProc ?? (addProc = new AppCommand((o) =>
                {
                    if(NewTask != null && NewTask.Length > 0)
                    Process.Start(NewTask);
                }));
            }
        }
    }
}