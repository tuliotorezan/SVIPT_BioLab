/*
-----------------------------------------------------------
    FEDERAL UNIVERSITY OF UBERLÂNDIA
    Faculty of Electrical Engineering
    Biomedical Engineering Lab
-----------------------------------------------------------
    Author: Andrei Nakagawa, MSc
    email: andrei.ufu@gmail.com
-----------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Biolab_DataAcquisition
{
    public class ThreadHandler
    {
        //The background thread
        private Thread backgroundWorker;
        //Flag that indicates if the thread should be running
        private bool flagRun;
        //Flag that indicates if the thread should be paused
        private bool flagPause;
        //Flag that indicates if the thread has been killed
        private bool flagKilled;
        //The method to be called by the thread
        private Action threadFunc;
        //Mutex for handling thread access
        public Mutex mutex;
        
        //Default constructor
        //Needs to receive the function to be called by the thread
        public ThreadHandler(Action _threadFunc)
        {
            this.threadFunc = _threadFunc;
            this.flagRun = false;
            this.flagKilled = false;
            this.mutex = new Mutex();
            this.backgroundWorker = new Thread(this.Run);
            this.backgroundWorker.Priority = ThreadPriority.Normal;
        }

        //Sets the priority of the threads
        public void SetPriority(ThreadPriority _priority)
        {
            this.backgroundWorker.Priority = _priority;
        }

        //Starts the thread
        public void Start()
        {
            this.flagRun = true;
            this.backgroundWorker.Start();         
        }

        //Stops the thead
        public void Stop()
        {
            this.flagPause = true; //Necessary for breaking the first loop
            this.flagRun = false; //Breaks the final loop            
        }

        //Resumes the thread
        public void Resume()
        {
            this.flagPause = false;
        }
        
        //Pauses the thread
        public void Pause()
        {
            this.flagPause = true;
        }        

        //Returns whether the thread is still alive
        public bool isAlive()
        {
            return !this.flagKilled;
        }

        //Returns whether the thread is still running
        public bool isRunning()
        {
            return this.flagRun;
        }

        //Returns whether the thread is paused
        public bool IsPaused()
        {
            return this.flagPause;
        }

        //The method passed to the ThreadHandler will be called inside the Run method
        //This method has two flags to control its execution
        private void Run()
        {
            while (flagRun)
            {
                while(!flagPause)
                    this.threadFunc();
            }

            this.flagKilled = true;
        }
    }
}
