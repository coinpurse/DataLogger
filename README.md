# DataLogger

The DataLibrary folder contains the project of the thread-safe data logger DLL file.
This library uses a mutex lock to provide a thread-safe data logger for users.
It also uses a DataLog class with a generic type for expandability, and when the writeToLog() method
is called, the method uses the objects toString() to record the data into the text file.

The WinformMemLogger folder contains the winform project that references the DataLibrary DLL file.
The application asks the user to input an interval amount of time in milliseconds and a folder path.
Once those parameters have been given by the user and the Start Log button is pressed, the application
will begin recording the percent of committed memory by the system at every interval for 10 seconds.
For example, if the interval time is set to 1 second, the application will log 11 times.
Once the application is finished recording, a message will appear to tell the user that the logging is complete.