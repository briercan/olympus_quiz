# olympus_quiz

Developer notes:
* Program.cs - I tried to keep this fairly light - if this was a production project, I'd change the following
  * Set up a DI container get the FileLockLineWriter and FileLineWriter (as well as set up interfaces for the classes)
  * Put the constants into config or command line args
* FileLineWriter.cs - encapsulates all of the IO operations
  * The methods here are specific to the requirements of the code quiz, but it allowed me to easily experiment with different methods of launching threads, etc
  * WriteLine() gets the last line using File.ReadLines which is ok for the number of lines for the project, but if the requirements needed a larger file, I'd probably use a reader and count backwards in the stream to find the last line.
* FileLockLineWriter.cs - Does the work of starting threads and maintaining locks on the file write operation.
  * My original idea was to start threads and perform the operations using a ConcurrentQueue, but it didn't seem to match the 'spirit' of what you were looking for (seemed a bit like cheating). Another thought was to use the TPL, but the requirements specifically were looking for Threads, so I went with them.
  * This class just starts the threads and uses a basic lock mechanism to prevent threads from operating on the file at the same time.
  * I also added a basic retry mechanism to handle unforseen problems
