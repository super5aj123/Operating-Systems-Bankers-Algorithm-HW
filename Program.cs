//Anthony Rowan
//10/28/24
//

using System.Numerics;

int numProcesses; //Variable to hold the total number of processes
int numResources; //Variable to hold the total number of different resources
StreamReader read = new StreamReader("data.txt");
string temp;

numProcesses = int.Parse(read.ReadLine());
numResources = int.Parse(read.ReadLine());

temp = read.ReadLine();
string[] numHolder = temp.Split(' ');
int[] resourceTracker = new int[numResources]; //Vector to hold the current count of avaliable instances of the different resources
for (int i = 0; i < numResources; i++)
{
    resourceTracker[i] = int.Parse(numHolder[i]);
}

int[,] processTracker;