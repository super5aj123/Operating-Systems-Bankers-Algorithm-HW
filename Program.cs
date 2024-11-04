//Anthony Rowan
//10/28/24
//This program will find safe sequences to complete processes by using the banker's algorithm. The information used for this process will be taken from a text file named "data.txt".
//IMPORTANT: C# requires that relative file pathnames be in the folder of the executable, NOT the .sln file. Please ensure that if you are compiling from source, that you have a data.txt file in the folder with your new executable.
using System.Numerics;


try
{
    int numProcesses; //Variable to hold the total number of processes
    int numResources; //Variable to hold the total number of different resources
    StreamReader read = new StreamReader("data.txt"); //StreamReader object used to read the file
    string temp; //String to temporarially hold various values.

    numProcesses = int.Parse(read.ReadLine()); //Read the first line to get the number of processes
    numResources = int.Parse(read.ReadLine()); //Read the second line to get the number of resources

    temp = read.ReadLine();
    string[] numHolder = temp.Split(' ');
    int[] resourceTracker = new int[numResources]; //Array to hold the current count of avaliable instances of the different resources
    for (int i = 0; i < numResources; i++)
    {
        resourceTracker[i] = int.Parse(numHolder[i]);
    }


    int[,] processTracker = new int[numProcesses, numResources]; //This array stores the current number of each resource that each process is using.
    int[,] processMax = new int[numProcesses, numResources]; //This array stores the maximum number of each resource that each process can request.


    for (int i = 0; i < numProcesses; i++) //Get and store the resources already allocated to each process
    {
        temp = read.ReadLine();
        numHolder = temp.Split(' ');
        for (int j = 0; j < numResources; j++)
        {
            processTracker[i, j] = int.Parse(numHolder[j]);
        }
    }


    for (int i = 0; i < numProcesses; i++) //Get and store the number of each resource that each process needs to complete.
    {
        temp = read.ReadLine();
        numHolder = temp.Split(' ');
        for (int j = 0; j < numResources; j++)
        {
            processMax[i, j] = int.Parse(numHolder[j]);
        }
    }

    bool[] finish = new bool[numProcesses]; //Finish will store if a process has completed yet, or if it still needs to complete.
    for(int i = 0;i < numProcesses; i++)
    {
        finish[i] = false;
    }


    int[] originalResourceTracker = resourceTracker;
    int[,] originalProcessTracker = processTracker;
    int[,] originalProcessMax = processMax; //Creates arrays to store the original values from the data.txt file. I figured using a bit more memory would be better than reading the file again, on the off chance a user deletes or alters their file during runtime.



    while (true) //Loop until the user exits the program
    {


        Console.Write("Which process is requesting more resources? (Enter -1 to exit): ");
        temp = (Console.ReadLine());
        Console.WriteLine(temp);

        if (temp == "-1") //If the user inputs -1, exit the program.
        {
            Environment.Exit(0);
        }
        else if (int.TryParse(temp, out int currentProcess) == true && currentProcess<numProcesses) //If the user's input could successfully be translated to an integer, and is a valid process number, assign it to currentProcess, and then proceed.
        {



            resourceTracker = originalResourceTracker;
            processTracker = originalProcessTracker; //Reset the arrays to hold their original values.
        }

        else
        {
            Console.WriteLine("Please ensure that you are entering a valid process number, with no extra characters such as spaces.");
        }
    }

}

catch
{
    Console.WriteLine("An error has occured. Please ensure that your data.txt file exists in the folder with your executable, and follows the required specifications.");
    Console.ReadLine();
}