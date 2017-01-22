using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SnackProject.Data.TaskListElement;

namespace SnackProject.Data
{
    public class TenHoursBottleneckManager
    {
        private static List<TaskListElement> currentExecutionsList = new List<TaskListElement>();

        public void AddMyselfToListAndWait(Task newTask, TaskListTypes taskListType)
        {
            Task[] tasksToWaitArray;
            
            lock (currentExecutionsList)
            {
                currentExecutionsList.Add(new TaskListElement { Task = newTask, TaskListType = taskListType });

                //Les orders opérations (confirmer un panier, annuler une commande, ...) doivent attendre 
                //la fin des 10h procédures (changements dans les prix, ...) déclanchées avant elle.
                //Les 10h procédures (changements dans les prix, ...) doivent attendre la fin des orders 
                //opérations (confirmer un panier, annuler une commande, ...) déclanchées avant elle.
                List<Task> tasksToWait = new List<Task>();
                for (int i=0; i<currentExecutionsList.Count-1; i++)
                {
                    if(currentExecutionsList.ElementAt(i).TaskListType == taskListType)
                    {
                        tasksToWait.Add(currentExecutionsList.ElementAt(i).Task);
                    }
                }
                tasksToWaitArray = tasksToWait.ToArray();
            }

            Task.WaitAll(tasksToWaitArray);
        }

        public void SignalImFinished(Task taskToRemove)
        {
            lock (currentExecutionsList)
            {
                bool found = false;
                for (int i=0; i<currentExecutionsList.Count && found == false; i++)
                {
                    if(currentExecutionsList.ElementAt(i).Task == taskToRemove)
                    {
                        currentExecutionsList.Remove(currentExecutionsList.ElementAt(i));
                        found = true;
                    }
                }
            }
        }
    }


    public class TaskListElement
    {
        public enum TaskListTypes
        {
            TenHoursProcedure,
            OrderOperation,
        };

        public Task Task { get; set; }
        public TaskListTypes TaskListType { get; set; }
    }
}
