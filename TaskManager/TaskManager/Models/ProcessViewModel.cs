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

        public string Name {
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
            get { return p.PriorityClass.ToString(); }
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
