using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// The DLL file that implements a thread-safe data logger
// The class DataLog uses a generic type that is declared at the time an object is instantiated
namespace DataLibrary
{

    public class DataLog<T>
    {
      
        private readonly object logLock = new object();         // The mutex lock
        private FileStream log;

        // Constructor
        // Parameters: String location, the path to the folder that the log is to be written to
        public DataLog(String location)
        {
            //mutex = new Mutex();
            if (File.Exists(location))
                File.Delete(location);
            log = File.Create(location + "\\log.txt");
        }

        // Adds a new piece of data to the log
        // Calls the data's ToString() method to use as information
        // The call to write to the log is kept in a mutex lock to keep the library thread-safe
        public void writeToLog(T Data)
        {
            string dataString = Data.ToString() + "\n";
            byte[] info = new UTF8Encoding(true).GetBytes(dataString);

            // The data log is about to be modified so request the mutex
            lock (logLock)
            {
                log.Write(info, 0, info.Length);
            }
        }

        // Closes and disposes of the log stream
        public void closeLog()
        {
            log.Close();
            log.Dispose();
        }

        
    }
}
