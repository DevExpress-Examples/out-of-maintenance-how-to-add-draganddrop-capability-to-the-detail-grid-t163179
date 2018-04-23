using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragAndDropExample
{
    class TaskViewModel
    {

        private ObservableCollection<TaskList> _List;

        public ObservableCollection<TaskList> List
        {
            get
            {
                if (_List == null)
                {
                    _List = new ObservableCollection<TaskList>();
                    for (int i = 0; i < 10; i++)
                    {
                        TaskList taskList = new TaskList() { TaskGroup = "Group" + i, GroupNumber = i, List = new ObservableCollection<Task>() };
                        for (int j = 0; j < 6; j++)
                        {
                            taskList.List.Add(new Task() { Name = "Task" + j, Date = new DateTime(2014, 10, new Random().Next(1, 31)), IsCompleted = j % 2 != 0 });
                        }
                        _List.Add(taskList);
                    }
                }
                return _List;
            }
        }

        public class Task
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public bool IsCompleted { get; set; }
        }

        public class TaskList
        {
            public string TaskGroup { get; set; }
            public int GroupNumber { get; set; }
            public ObservableCollection<Task> List { get; set; }
        }
    }
}
