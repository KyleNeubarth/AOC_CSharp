string[] lines = System.IO.File.ReadAllLines(@"testinput.txt")[0].Split(',');
int[] testInput = new int[lines.Length];
for (int i=0;i<lines.Length;i++) {
    testInput[i] = Int32.Parse(lines[i]);
}

lines = System.IO.File.ReadAllLines(@"input.txt")[0].Split(',');
int[] input = new int[lines.Length];
for (int i=0;i<lines.Length;i++) {
    input[i] = Int32.Parse(lines[i]);
}
Console.WriteLine("Part 1 test answer: "+Part1(testInput));
Console.WriteLine("Part 1 answer: "+Part1(input));
Console.WriteLine("Part 2 test answer: "+Part2(testInput));
Console.WriteLine("Part 2 answer: "+Part2(input));

//naive solution
double Part1(int[] crabs) {

    double min=double.MaxValue;
    int minPos = 0;

    for (int j=0;j<crabs.Max()+1;j++) {
        double fuel=0;
        foreach (float i in crabs) {
            fuel+=(double)MathF.Abs(i-j);
        }
        if (fuel < min) {
            min=fuel;
            minPos = j;
        }
        //Console.WriteLine("Position "+j+": "+fuel);
    }
    Console.WriteLine("Min fuel used:"+min+"; Min position: "+minPos);
    return min;
}

//also naive solution  lol
double Part2(int[] crabs) {

    double min=double.MaxValue;
    int minPos = 0;

    for (int j=0;j<crabs.Max()+1;j++) {
        double fuel=0;
        //Console.WriteLine("pos "+j);
        foreach (float i in crabs) {
            double sum = 0;
            for (int d=1;d<(double)MathF.Abs(i-j)+1;d++) {
                sum+=d;
            }
            //Console.WriteLine("crab "+i+": "+sum+" fuel.");
            fuel+=sum;
        }
        if (fuel < min) {
            min=fuel;
            minPos = j;
        }
        //Console.WriteLine("Position "+j+": "+fuel);
    }
    Console.WriteLine("Min fuel used:"+min+"; Min position: "+minPos);
    return min;
}
