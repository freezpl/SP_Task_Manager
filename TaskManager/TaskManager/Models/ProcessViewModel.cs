using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class ProcessViewModel : INotifyPropertyChanged
    {
        Process p;

        public Process P { get { return p; } }

        public string Name
        {
            get { return p.ProcessName; }
        }

        public int Id
        {
            get { return p.Id; }
        }

        public int Priority
        {
            get { return p.BasePriority; }
        }

        public string PriorClass
        {
            get
            {
                try
                {
                    return p.PriorityClass.ToString();
                }
                catch
                {

                }
                return null;
            }
            set
            {
                if (value == null && value.Length == 0)
                    return;
                try
                {
                    switch (value)
                    {
                        case "Normal":
                            p.PriorityClass = ProcessPriorityClass.Normal;
                            break;
                        case "Idle":
                            p.PriorityClass = ProcessPriorityClass.Idle;
                            break;
                        case "High":
                            p.PriorityClass = ProcessPriorityClass.High;
                            break;
                        case "RealTime":
                            p.PriorityClass = ProcessPriorityClass.RealTime;
                            break;
                        case "BelowNormal":
                            p.PriorityClass = ProcessPriorityClass.BelowNormal;
                            break;
                        case "AboveNormal":
                            p.PriorityClass = ProcessPriorityClass.AboveNormal;
                            break;
                        default:
                            break;
                    }
                    OnPropertyChanged(nameof(PriorClass));
                }
                catch
                {

                }
            }
        }

        public string Memory
        {
            get { return p.WorkingSet64.ToString(); }
        }

        public ProcessViewModel(Process p)
        {
            this.p = p;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
