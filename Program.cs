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

    temp = read.ReadLine();
    string[] numHolder = temp.Split(' ');
    int[] resourceTracker = new int[numResources];
    for (int i = 0; i < numResources; i++)
    {
        resourceTracker[i] = int.Parse(numHolder[i]);
    }

    int[,] processTracker = new int[numProcesses, numResources];
    int[,] processMax = new int[numProcesses, numResources];

    for (int i = 0; i < numProcesses; i++)
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
        temp = read.ReadLine();
        numHolder = temp.Split(' ');
        for (int j = 0; j < numResources; j++)
        {
            processMax[i, j] = int.Parse(numHolder[j]);
        }
    }

    int[] availableResources = new int[numResources];
    int[,] needResources = new int[numProcesses, numResources];
    for (int j = 0; j < numResources; j++)
    {
        availableResources[j] = resourceTracker[j];
        for (int i = 0; i < numProcesses; i++)
        {
            availableResources[j] -= processTracker[i, j];
            needResources[i, j] = processMax[i, j] - processTracker[i, j];
        }
    }

    //avaliable resources, need resources, processtracker

    int[] originalAvailableResources = availableResources;
    int[,] originalNeedResources = needResources;
    int[,] originalProcessTracker = processTracker;

    while (true) //Loop until the user decides to exit
    {
        bool error = false;
        Console.Write("Which process is requesting more resources? (Enter -1 to exit): ");
        temp = Console.ReadLine();
        Console.WriteLine(temp);
        int[] safeSequence = new int[numProcesses];

        if (temp == "-1")
        {
            Environment.Exit(0);
        }
        else if (int.TryParse(temp, out int currentProcess) && currentProcess < numProcesses)
        {
            Console.Write("Please enter the number of each resource that process " + currentProcess + " is attempting to request, seperated by a space: ");
            temp = Console.ReadLine();
            numHolder = temp.Split(' ');
            int[] currentRequest = new int[numResources];
            bool doesParse = true;

            for (int i = 0; i < numResources; i++)
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
                    if (currentRequest[i] > needResources[currentProcess, i] || currentRequest[i] > availableResources[i])
                    {
                        error = true;
                        break;
                    }
                }

                if (error == false)
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
                        Console.WriteLine("System is unsafe.");
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

                // Reset arrays to initial state
                availableResources = originalAvailableResources;
                processTracker = originalProcessTracker;
                needResources = originalNeedResources;

            }
        }
        else
        {
            Console.WriteLine("Please ensure that you are entering a valid process number, with no extra characters such as spaces.");
        }
    }
}
catch
{
    Console.WriteLine("An error has occurred. Please ensure that your data.txt file exists in the folder with your executable and follows the required specifications.");
    Console.ReadLine();
}
