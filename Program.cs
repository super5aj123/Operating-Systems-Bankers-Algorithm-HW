//Anthony Rowan
//10/28/24
//

using System.Numerics;

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

int[,] processTracker = new int [numProcesses,numResources];

for(int i = 0;i < numProcesses;i++)
{
    temp = read.ReadLine();
    numHolder = temp.Split(' ');
    for(int j = 0; j<numResources; j++)
    {
        processTracker[i,j] = int.Parse(numHolder[j]);
    }
}