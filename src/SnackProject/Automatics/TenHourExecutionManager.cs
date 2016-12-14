using SnackProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnackProject.Automatics
{
    public class TenHourExecutionManager
    {
        private static bool started = false;
        private static readonly object syncLock = new object();

        private static ApplicationDbContext context;

        private static Dictionary<string,ExecutionTask> taskList = new Dictionary<string,ExecutionTask>();
        private static Timer timer = new Timer(ExecuteAllTask);

        private static DateTime now;
        private static DateTime tenHours;

        public static void StartThread(ApplicationDbContext _context)
        {
            if (started == false)
            {
                context = _context;
                StartTimer();
            }
        }

        public static void AddNewTask(ExecutionTask newTask)
        {
            lock (syncLock)
            {
                string key = newTask.GetKey();
                if(taskList.ContainsKey(key))
                {
                    taskList.Remove(key);
                }
                taskList.Add(newTask.GetKey(), newTask);
            }
        }

        private static void StartTimer()
        {
            now = DateTime.Now;
            tenHours = DateTime.Today.AddHours(15.0).AddMinutes(15);//10.0

            //Si on est déjà au delà de 10 heures, alors ce sera demain
            if (now > tenHours)
            {
                tenHours = tenHours.AddDays(1.0);
            }

            int msUntilTen = (int)((tenHours - now).TotalMilliseconds);
            //programmer le timer pour l'heure
            timer.Change(msUntilTen, Timeout.Infinite);
        }
        
        private static void ExecuteAllTask(object state)
        {
            lock (syncLock)
            {
                foreach (KeyValuePair<string, ExecutionTask> keyValuePair in taskList)
                {
                    keyValuePair.Value.Execute(context);
                }
                taskList.Clear();

                //Ca redemarre le timer tous les jours
                StartTimer();
            }
        }
    }
}
