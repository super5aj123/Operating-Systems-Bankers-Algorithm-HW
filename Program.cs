//Anthony Rowan
//10/28/24
//This program will find safe sequences to complete processes by using the banker's algorithm. The information used for this process will be taken from a text file named "data.txt".
//IMPORTANT: C# requires that relative file pathnames be in the folder of the executable, NOT the .sln file. Please ensure that if you are compiling from source, that you have a data.txt file in the folder with your new executable.
//I tried getting the repetition to work, but I can't figure out what variable or array I'm forgetting to reset. If you see weird code that looks like it was an attempt at repetition, that's why.
using System;
using System.IO;



try
{
    int numProcesses;
    int numResources;
    StreamReader read = new StreamReader("data.txt");
    string temp;

    numProcesses = int.Parse(read.ReadLine());
    numResources = int.Parse(read.ReadLine());

    temp = read.ReadLine(); //Temp is a variable used throughout the program to store a variety of strings for a short period.
    string[] numHolder = temp.Split(' '); //numHolder is used similarly to temp, but to store arrays of strings so they can be converted to ints.
    int[] resourceTracker = new int[numResources]; //Get the current avaliable number of resources
    for (int i = 0; i < numResources; i++)
    {
        resourceTracker[i] = int.Parse(numHolder[i]);
    }

    int[,] processTracker = new int[numProcesses, numResources];
    int[,] processMax = new int[numProcesses, numResources];

    for (int i = 0; i < numProcesses; i++) //Get the current resource status of each process
    {
        temp = read.ReadLine();
        numHolder = temp.Split(' ');
        for (int j = 0; j < numResources; j++)
        {
            processTracker[i, j] = int.Parse(numHolder[j]);
        }
    }

    for (int i = 0; i < numProcesses; i++)
    {
        temp = read.ReadLine(); //Get the maximum of each resource that each process can hold
        numHolder = temp.Split(' ');
        for (int j = 0; j < numResources; j++)
        {
            processMax[i, j] = int.Parse(numHolder[j]);
        }
    }

    int[] availableResources = new int[numResources];
    int[,] needResources = new int[numProcesses, numResources];
    for (int j = 0; j < numResources; j++) //Calculate the current number of avaliable resources, as well as how many resources each process needs
    {
        availableResources[j] = resourceTracker[j];
        for (int i = 0; i < numProcesses; i++)
        {
            availableResources[j] -= processTracker[i, j];
            needResources[i, j] = processMax[i, j] - processTracker[i, j];
        }
    }



        bool error = false;
        Console.Write("Which process is requesting more resources? (Enter -1 to exit): ");
        temp = Console.ReadLine();
        Console.WriteLine(temp);
        int[] safeSequence = new int[numProcesses];

        if (temp == "-1") //Exit if the user enters -1
        {
            Environment.Exit(0);
        }
        else if ((int.TryParse(temp, out int currentProcess)) == true && (currentProcess < numProcesses) == true) //If the user's input was understood and valid, then proceed.
        {
            Console.Write("Please enter the number of each resource that process " + currentProcess + " is attempting to request, seperated by a space: ");
            temp = Console.ReadLine();
            numHolder = temp.Split(' ');
            int[] currentRequest = new int[numResources];
            bool doesParse = true;

            for (int i = 0; i < numResources; i++) //Get the resources for the user's current request, and inform them if there was a problem.
            {
                doesParse = int.TryParse(numHolder[i], out currentRequest[i]);
                if (doesParse == false)
                {
                    Console.WriteLine("Your entry could not be successfully parsed. Please try again.");
                    break;
                }
            }

            if (doesParse) //Only proceed if the user's input could be successfully understood.
            {
                for (int i = 0; i < numResources; i++)
                {
                    if (currentRequest[i] > needResources[currentProcess, i] || currentRequest[i] > availableResources[i]) //If the user's input was more than the resources a process needs, or more than is avaliable, the request cannot be granted.
                    {
                        error = true;
                        break;
                    }
                }

                if (error == false) //If the process's resource request was within the allowable bounds, then proceed.
                {
                    for (int i = 0; i < numResources; i++)
                    {
                        availableResources[i] -= currentRequest[i];
                        processTracker[currentProcess, i] += currentRequest[i];
                        needResources[currentProcess, i] -= currentRequest[i];
                    }

                    bool[] finish = new bool[numProcesses];
                    for (int i = 0; i < numProcesses; i++)
                    {
                        finish[i] = false;
                    }
                    int count = 0;
                    while (count < numProcesses)
                    {
                        bool foundProcess = false;
                        for (int i = 0; i < numProcesses; i++)
                        {
                            if (finish[i] == false)
                            {
                                bool canProceed = true;
                                for (int j = 0; j < numResources; j++)
                                {
                                    if (needResources[i, j] > availableResources[j])
                                    {
                                        canProceed = false;
                                        break;
                                    }
                                }
                                if (canProceed == true)
                                {
                                    for (int j = 0; j < numResources; j++)
                                    {
                                        availableResources[j] += processTracker[i, j];
                                    }
                                    safeSequence[count++] = i;
                                    finish[i] = true;
                                    foundProcess = true;
                                }
                            }
                        }
                        if (foundProcess == false)
                        {
                            break;
                        }
                    }

                    if (count < numProcesses)
                    {
                        Console.WriteLine("Doing this would make the system unsafe. As such, this request cannot be granted.");
                    }
                    else
                    {
                        Console.Write("Request can be granted. Safe sequence: <");
                        for (int i = 0; i < count; i++)
                        {
                            Console.Write(safeSequence[i]);
                            if (i != count - 1)
                                Console.Write(", ");
                        }
                        Console.WriteLine(">");
                    }
                }
                else
                {
                    Console.WriteLine("Request cannot be granted. Resources not available or exceeds needs.");
                }



            }
        }
        else
        {
            Console.WriteLine("Please ensure that you are entering a valid process number, with no extra characters such as spaces.");
        }
}
catch
{
    Console.WriteLine("An error has occurred. Please ensure that your data.txt file exists in the folder with your executable and follows the required specifications.");
    Console.ReadLine();
}