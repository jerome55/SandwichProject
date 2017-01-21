using SnackProject.Data;
using SnackProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnackProject.Automatics
{
    public class TenHourExecutionManager
    {
        private static bool started = false;

        public static ApplicationDbContext context;
        
        private static Timer timer = new Timer(Save);

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

        private static void StartTimer()
        {
            now = DateTime.Now;
            tenHours = DateTime.Today.AddHours(10.0).AddMinutes(0);//10.0

            //Si on est déjà au delà de 10 heures, alors ce sera demain
            if (now > tenHours)
            {
                tenHours = tenHours.AddDays(1.0);
            }

            int msUntilTen = (int)((tenHours - now).TotalMilliseconds);
            //programmer le timer pour l'heure
            timer.Change(msUntilTen, Timeout.Infinite);
        }

        //sauvegarde le contexte dans la bd et redémarre le timer
        private async static void Save(object state)
        {
            await context.SaveChangesAsync();
            StartTimer();
        }
    }
}
