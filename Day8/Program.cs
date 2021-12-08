HashSet<char>[] segmentCodes = new HashSet<char>[] {
    new HashSet<char> {'a','b','c','e','f','g'},
    new HashSet<char> {'c','f'},
    new HashSet<char> {'a','c','d','e','g'},
    new HashSet<char> {'a','c','d','f','g'},
    new HashSet<char> {'b','c','d','f'},
    new HashSet<char> {'a','b','d','f','g'},
    new HashSet<char> {'a','b','d','e','f','g'},
    new HashSet<char> {'a','c','f'},
    new HashSet<char> {'a','b','c','d','e','f','g'},
    new HashSet<char> {'a','b','c','d','f','g'}
};


string[] testInput = System.IO.File.ReadAllLines(@"testinput.txt");
string[] input = System.IO.File.ReadAllLines(@"input.txt");

Console.WriteLine("Part 1 test answer: "+Part1(testInput));
Console.WriteLine("Part 1 answer: "+Part1(input));
Console.WriteLine("Part 2 test answer: "+Part2(testInput));
Console.WriteLine("Part 2 answer: "+Part2(input));

int Part1(string[] lines) {
    int numUnique = 0;
    foreach (string s in lines) {
        string[] outputs = s.Substring(s.IndexOf('|')+2).Split(' ');
        foreach (string o in outputs) {
            //Console.Write(o+"; ");
            if (o.Length == 2 || o.Length == 3 || o.Length == 4 || o.Length == 7) numUnique++;
        }
        //Console.WriteLine();
    }
    return numUnique;
}

int Part2(string[] lines) {
    int sumOutputs = 0;
    foreach (string s in lines) {
        string[] sides = s.Split(" | ");
        string[] signals = sides[0].Split(' ');
        string[] outputs = sides[1].Split(' ');
        
        HashSet<char>[] signalSets = new HashSet<char>[10];
        //true value -> swapped value
        Dictionary<char,char> signalMap = new Dictionary<char,char>();
        for (int i=0;i<signalSets.Length;i++) {
            signalSets[i] = new HashSet<char>(signals[i]);
        }

        //voodoo to fill out signalMap
        //this is totally bullshit code, be warned
        //group the signalsets by length
        //I did it this horrible way because I couldn't figure out the toGroup method :(
        // IEnumerable<IGrouping<int, HashSet<char>>> query = signalSets.GroupBy(set => set.Count, set => set);
        // Dictionary<string,List<Product>> results = signalSets.GroupBy(set => set.Count).ToDictionary();
        Dictionary<int,List<HashSet<char>>> groupBySegments = new Dictionary<int, List<HashSet<char>>>();
        for (int i=2;i<8;i++) {
            groupBySegments[i] = new List<HashSet<char>>();
        }
        for (int i=0;i<signalSets.Length;i++) {
            int l = signalSets[i].Count;
            //Console.WriteLine(signalSets[i].Count);
            // if (!groupBySegments.ContainsKey('l')) {
            //     Console.WriteLine("creating new list");
            //     groupBySegments[l] = new List<HashSet<char>>();
            // }
            groupBySegments[l].Add(signalSets[i]);
        }
        // foreach (KeyValuePair<int,List<HashSet<char>>> kvp in groupBySegments) {
        //     Console.WriteLine("Num bars: "+kvp.Key+"; Count: "+kvp.Value.Count);
        // }
        //7-1 gives us A
        signalMap['a'] = groupBySegments[3][0].Except(groupBySegments[2][0]).First();
        HashSet<char> bAndD = groupBySegments[4][0].Except(groupBySegments[2][0]).ToHashSet();
        for (int i=0;i<3;i++) {
            //fun fact, this one is zero
            if (!(bAndD.IsSubsetOf(groupBySegments[6][i])) ) {
                signalMap['b'] = groupBySegments[6][i].Intersect(bAndD).First();
                signalMap['d'] = segmentCodes[8].Except(groupBySegments[6][i]).First();
                break;
            }
        }
        HashSet<char> cAndF = groupBySegments[2][0];
        for (int i=0;i<3;i++) {
            //fun fact, this one is 5
            if (!(cAndF.IsSubsetOf(groupBySegments[5][i])) ) {
                signalMap['f'] = groupBySegments[5][i].Intersect(cAndF).First();
                signalMap['c'] = cAndF.Except( new HashSet<char>() {signalMap['f']}).ToHashSet().First();
                break;
            }
        }
        HashSet<char> gAndE = segmentCodes[8].Except(new HashSet<char>() {signalMap['a'],signalMap['b'],signalMap['c'],signalMap['d'],signalMap['f']}).ToHashSet();
        for (int i=0;i<3;i++) {
            //fun fact, this one is 9
            if (!(gAndE.IsSubsetOf(groupBySegments[6][i])) ) {
                signalMap['g'] = groupBySegments[6][i].Intersect(gAndE).First();
                signalMap['e'] = gAndE.Except( new HashSet<char>() {signalMap['g']}).ToHashSet().First();
                break;
            }
        }
        //end bullshit

        // foreach (KeyValuePair<char,char> kvp in signalMap) {
        //     Console.WriteLine("Key: "+kvp.Key+"; Value: "+kvp.Value);
        // }

        //now that we have the signalMap, apply it to the segment codes to scramble them just like out actual outputs
        HashSet<char>[] answers = new HashSet<char>[10];
        for (int i=0;i<answers.Length;i++) {
            answers[i] = segmentCodes[i].Select(x =>signalMap[x]).ToHashSet();
            // foreach (char c in answers[i]) {
            //     Console.Write(c);
            // }
            // Console.WriteLine();
        }

        //go through outputs and decode them into numbers
        int outputNum = 0;
        for (int i=0;i<outputs.Length;i++) {
            HashSet<char> outputAsSet = new HashSet<char>(outputs[i]);
            bool foundNum = false;
            for (int j=0;j<answers.Length;j++) {
                // foreach (char c in outputAsSet) {
                //     Console.Write(c);
                // }
                // Console.Write("; "+i+"; "+outputAsSet.SetEquals(answers[i]));
                // Console.WriteLine();  
                if (outputAsSet.SetEquals(answers[j])) {
                    outputNum*=10;
                    outputNum+=j;
                    foundNum=true;
                    break;
                }
            }
            if (!foundNum) {
                Console.WriteLine("Trying klugey backup strat");
                char temp = signalMap['c'];
                signalMap['c']=signalMap['f'];
                signalMap['f']=temp;
                for (int f=0;f<answers.Length;f++) {
                    answers[f] = segmentCodes[f].Select(x =>signalMap[x]).ToHashSet();
                }
                Console.WriteLine("Invalid output: "+outputs[i]);
                for (int j=0;j<answers.Length;j++) {
                    // foreach (char c in outputAsSet) {
                    //     Console.Write(c);
                    // }
                    // Console.Write("; "+i+"; "+outputAsSet.SetEquals(answers[i]));
                    // Console.WriteLine();  
                    if (outputAsSet.SetEquals(answers[j])) {
                        outputNum*=10;
                        outputNum+=j;
                        foundNum=true;
                        break;
                    }
                }
                if (!foundNum) Console.WriteLine("You're really fucked now :(");
            }
        }
        //Console.WriteLine("; "+outputNum);
        sumOutputs+=outputNum;
    }
    return sumOutputs;
}
