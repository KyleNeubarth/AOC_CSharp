string[] testInput = System.IO.File.ReadAllLines(@"testinput.txt");
string[] input = System.IO.File.ReadAllLines(@"input.txt");

Console.WriteLine("Part 1 test answer: "+Part1(testInput));
Console.WriteLine("Part 1 answer: "+Part1(input));
Console.WriteLine("Part 2 test answer: "+Part2(testInput));
Console.WriteLine("Part 2 answer: "+Part2(input));

int Part1(string[] lines) {
    int[,] inputArr = new int[lines.Length,lines[0].Length];
    for (int i=0;i<inputArr.GetLength(0);i++) {
        for (int j=0;j<inputArr.GetLength(1);j++) {
            //Console.WriteLine(i+"; "+j);
            inputArr[i,j]=int.Parse(lines[i][j].ToString());
        }
    }

    int sumDanger = 0;

    for (int i=0;i<inputArr.GetLength(0);i++) {
        for (int j=0;j<inputArr.GetLength(1);j++) {
            int me=inputArr[i,j];
            if (j!=0 && inputArr[i,j-1] <= me) continue;
            if (j!=inputArr.GetLength(1)-1 && inputArr[i,j+1] <= me) continue;
            if (i!=0 && inputArr[i-1,j] <= me) continue;
            if (i!=inputArr.GetLength(0)-1 && inputArr[i+1,j] <= me) continue;

            sumDanger+=me+1;
        }
    }
    return sumDanger;
}

int Part2(string[] lines) {
    int[,] inputArr = new int[lines.Length,lines[0].Length];
    for (int i=0;i<inputArr.GetLength(0);i++) {
        for (int j=0;j<inputArr.GetLength(1);j++) {
            inputArr[i,j]=int.Parse(lines[i][j].ToString());
        }
    }

    bool[,] inBasin = new bool[inputArr.GetLength(0),inputArr.GetLength(1)];

    List<HashSet<int[]>> basins = new List<HashSet<int[]>>();

    void ExtendBasin(HashSet<int[]> basin, int i,int j)
    {
        if (inBasin[i,j] || inputArr[i,j]==9) return;
        basin.Add(new int[] {i,j});
        inBasin[i,j] = true;
        int me=inputArr[i,j];
        if (j!=0 && inputArr[i,j-1] > me) ExtendBasin(basin,i,j-1);
        if (j!=inputArr.GetLength(1)-1 && inputArr[i,j+1] > me) ExtendBasin(basin,i,j+1);
        if (i!=0 && inputArr[i-1,j] > me) ExtendBasin(basin,i-1,j);
        if (i!=inputArr.GetLength(0)-1 && inputArr[i+1,j] > me) ExtendBasin(basin,i+1,j);
    };

    for (int i=0;i<inputArr.GetLength(0);i++) {
        for (int j=0;j<inputArr.GetLength(1);j++) {
            int me=inputArr[i,j];
            if (j!=0 && inputArr[i,j-1] <= me) continue;
            if (j!=inputArr.GetLength(1)-1 && inputArr[i,j+1] <= me) continue;
            if (i!=0 && inputArr[i-1,j] <= me) continue;
            if (i!=inputArr.GetLength(0)-1 && inputArr[i+1,j] <= me) continue;

            HashSet<int[]> newBasin = new HashSet<int[]>() {new int[] {i,j}};
            basins.Add(newBasin);
            inBasin[i,j]=true;

            if (j!=0 && inputArr[i,j-1] > me) ExtendBasin(newBasin,i,j-1);
            if (j!=inputArr.GetLength(1)-1 && inputArr[i,j+1] > me) ExtendBasin(newBasin,i,j+1);
            if (i!=0 && inputArr[i-1,j] > me) ExtendBasin(newBasin,i-1,j);
            if (i!=inputArr.GetLength(0)-1 && inputArr[i+1,j] > me) ExtendBasin(newBasin,i+1,j);
        }
    }
    var list = basins.OrderByDescending(x=>x.Count);
    int timer=0;
    int answer = 1;
    foreach (var v in list) {
        if (timer>2) break;
        timer++;
        answer*=v.Count;
    }
    return answer;
}

