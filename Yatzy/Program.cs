using System;
using System.Collections.Generic;
/*
 * Under första lektionen så fixade jag tabellen och hur man lägger till spelare till yatzy spelet. 
 * Tabellen ändrar storlek beroende på hur många som är med och kolumnerna är lika stora vilket gör att det ser snyggt ut. 
 * Poängerna har jag också fixat medans jag fixade tabellen då det var bara att den skriver in ne siffra beroende på var 
 * den är i en väldigt stor array. Jag använde en array då listor så kan jag inte ha tomrum och jag fattade inte hur man 
 * fick en array var till varje spelare då jag inte kunde förstå hur man lägger till en array i koden.
 * 
 * Under andra lektionen så har jag börjat fixa turns i spelet och man kan kasta dice och välja vilka som man vill
 * kasta. Det är krångligt och det är svårt att fixa men jag fixade och nu kan man välja rätt och programmet krachar
 * inte direkt när man skriver något man inte borde. Det jag saknar nu är hur man räknar ut poäng och hur AI ska fungera.  
 * 
 * (HEMMA)Inget krashar längre och jag har fått så att man får poängen som man borde få. Jag fick den till en annan nummerarray som har mängden av 
 * olika nummer istället vilket gör att kalkylera mängden poäng 10x simplare enligt mig. Man kan fortfarande lägga poäng på samma ställe
 * två ggr vilket inte är bra så jag har en bool array om den har använts eller inte. Ändrar den samtidigt som poäng så super simpelt att lägga
 * in. Skriver AI nu, vilket är jättejobbigt. Många kalkulationr och buggar när jag la in array = annan array så ändrades annan array också??
 * fattar inget. Skrev om manuellt istället och funkar bra nu (array[1] = annan array[1]). AI kollar alla kombinationer och optimala kastningarna
 * för maximala poäng. 
 * 
 * använde binara tal som exempe när jag skulle välja vilka tärningar som skulle kastas. fixar alla kombinationer på det sättet då 1 = 10000 2 = 01000 3 = 11000 osv
 * kollar igenom alla 6 tal på tärningarna och fixar coola kalkulationer för att få fram avarage, jämför med andra och väljer bästa. rip chans då den bara lägger på
 * den den får mest poäng på men orkar inte fixa. koden blev mycket snabbare än jag trodde och allt funkar bra nu! Den sparar vilka den ska kasta fär att få maximala
 * poäng och o den klarar hela vägen som best så kastas dem. Detta händer 2 ggr då man kan kasta 2 ggr och sedan väljer den den med högst poäng och lägger där. De enda
 * jag skulle kunna ändra är ai så att den e smartare men jag kan inte 1000iq  yatzy strats. tycker att min kod 100% klar och jag har fixat allt så att man inte kan
 * krasha random av fel input eller något.
 * 
 * Skriver documentation <- här på (2022-05-04) fixade så att det står vem som vinner och att spelet är slut efter att alla rundor är över.
 * 
 * dokumentation av ai kod =
 * 
 * kollar alla olika kombinationer med for < 32 för det 2x2x2x2x2 (kasta eller inte) binar då den ändrar på 01000 då 1 är true
 * sen kollar den alla 6 kombinationer (1 till 6) med den tärningen och tar högsta poängen med det / 6
 * om det är 01100 (två tärningar kastade) tar den 6 kombinationer 6 ggr. Den gör så även om det är 11111. det blir runt 6x6x6x6x6 = 7776 kombinationer
 * tar / hur många ggr den kollade så att man får avarage och om den är större än förra största eller första så blir den man ska kasta = den kastade under kollen
 * när den kollat allt så kastar den enligt hur många som skulle kastas. sedan så väljer den combon med högst poäng och den e klar
 * */
namespace Yatzy
{
    class Program
    {
        static void Main(string[] args)
        {
            yatzy yatzy = new yatzy();
            yatzy.startgame(); //startar koden
        }
    }
    class yatzy
    {
        int[] yatzyPoints;
        bool[] yatzyUsed;
        List<string> players = new List<string>();
        int[] numbers = new int[6];
        private void namePlayers(int players)
        {
            for (int i = 0; i < players; i++)//gör namn för alla spelare
            {
                Console.Write($"{i}: ");
                string name = Console.ReadLine();
                if (name == "AI")
                {
                    addAI(i);
                }
                else if (name == "")
                {
                    Console.WriteLine("You have to write a name");
                    i--;
                    continue;
                }
                else
                {
                    addPlayer(name);
                }
                Console.WriteLine($"Added {name} at slot {i}");
            }
        }
        private int playerNumber()
        {
            int players = 0;
            while (true)//tar en input på antalet spelare
            {
                string playersString = Console.ReadLine();
                if (int.TryParse(playersString, out players))
                {
                    if (players < 2)
                    {
                        Console.WriteLine("Has to be more than 1");
                        continue;
                    }
                    break;
                }
                else Console.WriteLine("Only numbers allowed");
            }
            return players;
        }
        private void makeBoard(bool numbered)
        {
            int playerint = players.Count;
            string[] yatzyTable = {//resettar yatzytable
                           "Deltagare  ",
                           "Ettor      ",
                           "Tvår       ",
                           "Treor      ",
                           "Fyror      ",
                           "Femmor     ",
                           "Sexor      ",
                           "Summa      ",
                           "Bonus      ",
                           "Ett Par    ",
                           "Två par    ",
                           "Tretal     ",
                           "Fyrtal     ",
                           "Liten Stege",
                           "Stor Stege ",
                           "Kåk        ",
                           "Chans      ",
                           "Yatzy      ",
                           "Totalt     " };
            if (numbered)//ifall den ska bli numerrerad så lägger den till nummer
            {
                for(int i = 0; i < yatzyTable.Length; i++)//går igenomm hela
                {
                    if (i < 10) yatzyTable[i] = " " + yatzyTable[i];//lägger till en space så att de är lika stora om de är under 10
                    yatzyTable[i] = i + ": " + yatzyTable[i];//numererars
                }
            }
            Console.WriteLine();
            for (int i = 0; i < playerint; i++)//för antalet spelare
            {
                yatzyTable[0] += $"| {players[i]} ";//lägger till deras namn
            }
            for (int i = 1; i < 19; i++)//för hela arrayen
            {
                for(int j = 0; j < playerint; j++)//fr antalet spelare
                {
                    if(i == 18)//ifall den är på total
                    {
                        yatzyPoints[i + 19 * j] = 0;
                        for (int l = 1; l < 18; l++)//går igenom alla och lägger till dem till totalen
                        {
                            if(l == 11)
                            {
                                continue;
                            }
                            yatzyPoints[i + 19 * j] += yatzyPoints[i + 19 * j - l];
                        }
                        yatzyTable[i] += $"|{yatzyPoints[i + 19 * j]}";//lägger till den till arrayen
                        for (int k = -2; k < players[j].Length - yatzyPoints[i + 19 * j].ToString().Length; k++)//gör så att dfen är lika stor som namnet av spelaren
                        {
                            yatzyTable[i] += " ";
                        }
                        continue;
                    }
                    if(i == 8)//om den är bonus
                    {
                        if(yatzyPoints[i-1 + 19 * j] > 62)//om du borde ha bonus
                        {
                            yatzyPoints[i + 19 * j] = 50;
                        }
                        else yatzyPoints[i + 19 * j] = 0;
                    }
                    if(i == 7)//om den är summa
                    {
                        yatzyPoints[i + 19 * j] = 0;
                        for (int l = 1; l < 7; l++)//visar summa
                        {
                            yatzyPoints[i + 19 * j] += yatzyPoints[i + 19 * j - l];
                        }
                        yatzyTable[i] += $"|{yatzyPoints[i + 19 * j]}";
                        for (int k = -2; k < players[j].Length - yatzyPoints[i + 19 * j].ToString().Length; k++)//gör att den e lika lång
                        {
                            yatzyTable[i] += " ";
                        }
                        continue;
                    }
                    yatzyTable[i] += $"|{yatzyPoints[i+19*j]}";//visar antalet poäng på tabellen
                    for(int k = -2; k < players[j].Length - yatzyPoints[i + 19 * j].ToString().Length; k++)//gör den lika lång som namnet
                    {
                        yatzyTable[i] += " ";
                    }
                }
            }
            for (int i = 0; i < yatzyTable.Length; i++)//skriver ut den
            {
                Console.WriteLine(yatzyTable[i]);
            }
        Console.WriteLine();
        }
        private void addPlayer(string name)
        {
            players.Add(name);//läggertillspelareilista
        }
        private void addAI(int i)
        {
            players.Add("AI " + i);//ai blir numerereade
        }
        private void addPoints(int playerid, int points, int row)
        {
            yatzyPoints[row+19*playerid] = points;//lägger till poäng och sätter boolarratyen på true så att man inte kan lägga till där igen
            yatzyUsed[row+19*playerid] = true;

        }
        private void array()
        {
            yatzyPoints = new int[19 * players.Count];//skapar arrays beroende på hur många spelare spelar
            yatzyUsed = new bool[19 * players.Count];
        }
        public void startgame()
        {
            Console.WriteLine("Welcome to Yatzy!");
            Console.WriteLine("How many are playing? (AI included)"); //förklaring av vad man ska göra
            int playerInt = playerNumber(); //metoden tar in en int beroende på vad man skriver och annars så loopar den tills man får en int
            Console.WriteLine("Name your players, type AI if you want them to be bots");
            namePlayers(playerInt); //metoden gör att man behöver inputa lika många namn som antal spelare, om man skriver AI så blir det en ai
            array(); //skapar en array med 19xantal spelare så att man har en array stor nog för alla spelare och jag vet inget annat sätt att lösa det så att de är på en fixad position
            makeBoard(false); //den putputtar tabellen och false står för att den inte är numrerad
            Console.WriteLine("Press anything to start");
            Console.ReadKey();
            int turns = 0;
            while (true)//startar spelet på riktigt
            {
                if(turns > 14)//ifall man har spelat alla rundor redan
                {
                    break;
                }
                turns++;
                turn();//en runda i en metod
            }
            Console.WriteLine("Game is over!");//ifall alla rundor har hänt
            int winner = 0;
            int points1 = 0;
            for(int i = 0; i < players.Count; i++) //kollar igenom alla persoenrs total och sparar den med mest
            {
                if(points1 < yatzyPoints[18 + 19 * i])
                {
                    winner = i;
                    points1 = yatzyPoints[18 + 19 * i];
                }
            }
            Console.WriteLine();
            Console.WriteLine("Winner is: " + players[winner]);//skriver vem som vann
        }
        private void turn()
        {
            for (int i = 0; i < players.Count; i++)//kör lika många gånger som antal speare
            {
                Console.Clear();
                if (players[i] == $"AI {i}")//ifall det är ai gör ai turn istället
                {
                    aiTurn(i);//kollar igenom alla kombinationer för att få mest poäng och sedan kastar dem och väljer den combon med max poäng
                    continue;
                }
                Random rand = new Random();//lägger till random
                Console.WriteLine(players[i] + "'s Turn");
                int[] dice = { rand.Next(1, 7), rand.Next(1, 7), rand.Next(1, 7), rand.Next(1, 7), rand.Next(1, 7) };//kastar alla dice
                Console.WriteLine("Press anything to roll");
                Console.ReadKey();
                Console.WriteLine($"{players[i]} rolled...");
                rolling(dice, i, rand);//en metod där man kastar och väljer vilka man ska kasta igen och fortsätta
            }
        }
        private void rolling(int[] dice,int i, Random rand)
        {
            int count = 0;
            while (true)
            {
                bool[] diceRoll = { false, false, false, false, false };//en bool array om vilka man ska kasta, true = kasta den tärningen
                Console.WriteLine($"{dice[0]} {dice[1]} {dice[2]} {dice[3]} {dice[4]}");
                Console.WriteLine("Type a number(1-5) to select the die you want to reroll.");
                Console.WriteLine("If you do not want to reroll you can type continue");//förklaringar
                makeBoard(false);//visar tabellen igen utan numrering
                string input = Console.ReadLine();//tar inputen
                if (int.TryParse(input, out int diceId))//om det är en siffra
                {
                    if (0 < diceId && diceId < 6)
                    {
                        diceRoll[diceId - 1] = !diceRoll[diceId - 1];//reversar i bool arrayen så man kan ångra sig och välja vilka man ska kasta
                        Console.Clear();
                    }
                    else//om det inte är en siffra
                    {
                        Console.Clear();
                        Console.WriteLine("Has to be between 1-5");
                        continue;
                    }
                    while (true)//nu där man väljer fler tärnignar att kasta
                    {
                        if (count < 2)//ifall man int eredan kastat om två gånger
                        {
                            Console.WriteLine($"{dice[0]} {dice[1]} {dice[2]} {dice[3]} {dice[4]}");//skriver ut dina kast
                            for (int v = 0; v < 5; v++)//händer 5 ggn
                            {
                                if (diceRoll[v] == true)//ifall man ska kasta skriv en pil under den siffran så att man kan se vilka man ska kasta på ett bra sätt
                                {
                                    Console.Write("^ ");
                                }
                                else Console.Write("  ");
                            }
                            Console.WriteLine();
                            Console.WriteLine("Write another number(1-5) to select another die or type reroll to roll them");
                            makeBoard(false);//visar tabellen så att man kan satsa på något man itne redan tagit
                            input = Console.ReadLine();
                            if (int.TryParse(input, out diceId))//kollar om det är en siffra igen
                            {
                                if (0 < diceId && diceId < 6)
                                {
                                    diceRoll[diceId - 1] = !diceRoll[diceId - 1];//reversar boolarrayen igen
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Has to be between 1-5");//förklaring om man skrev något annat
                                }
                            }
                            else if (input == "reroll")//om man vill kasta dem man valt
                            {
                                count++;//går upp så man vet när man kastat två ggn
                                for (int d = 0; d < 5; d++)//kastar alla man har valt
                                {
                                    if (diceRoll[d] == true)
                                    {
                                        dice[d] = rand.Next(1, 7);
                                    }
                                }
                                Console.Clear();
                                Console.WriteLine($"{players[i]} rolled...");
                                break;
                            }
                            else//ifall det inte är något av dem
                            {
                                Console.Clear();
                                Console.WriteLine("Input unrecognizable");
                            }
                        }
                        else//ifall man redan kasat två gånger
                        {
                            Console.Clear();
                            Console.WriteLine("You can only reroll twice");
                            break;
                        }
                    }
                }
                else if (input == "continue")//man väljer att välja en combo med dessa tärningar
                {
                    choice(dice,i);//byter metod till där man ska välja combos och tar med dice arrayn
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Input unrecognicable");
                }
            }
        }
        private void choice(int[] dice,int i)
        {
            diceToNumbers(dice);//gör om arrayen av nummer till en array av antalet nummer av varje istället. tex 1 2 5 2 1 blir 2 2 0 0 1 0. enklare att räkna ut combos
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"{dice[0]} {dice[1]} {dice[2]} {dice[3]} {dice[4]}");//visar dina tärningar
                Console.WriteLine("What combo do you want?");
                Console.WriteLine("Type number corresponding to row");//förklaring
                makeBoard(true);//visar tabell
                string inputRow = Console.ReadLine();
                if (int.TryParse(inputRow, out int row) && rowUnused(row, i))//kollar om du skrivit nummer och att den är tom
                {
                    int points = howmanypoints(row);//metod vilket kollar antalet poäng beroende på combo och tärningar
                    Console.WriteLine("You will get " + points + " points if you place it in row " + row);//förklaring
                    Console.WriteLine("Do you want to place it there? Y/N");
                    string yesno = Console.ReadLine();
                    if (yesno == "Y")//ifall man vill lägga där skriver man Y annars så kan man välja en annan row att lägga på.
                    {
                        addPoints(i, points, row);//lägger till poängen
                        break;//slutar här och går tillbaka till nästa spelare i turn() metoden med for loopen
                    }
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Write an unused row with correct number");
                }
            }
        }
        private bool rowUnused(int row, int playerid)
        {
            if (row == 7 || row == 8 || row < 1 || row > 17)//kollar om det är en row man inte borde lägga på
            {
                return false;
            }
            return !yatzyUsed[playerid * 19 + row];//kollar igenom en bool array som visar om man redan har lagt något där
        }
        private void diceToNumbers(int[] dice)
        {
            for(int i = 0; i < numbers.Length; i++)//resettar arrayen
            {
                numbers[i] = 0;
            }
            for(int i = 0; i < dice.Length; i++)//går igenom 5 ggn
            {
                numbers[dice[i]-1]++;//lägger till siffran på en index mindre så att det passar
            }
        }
        private int howmanypoints(int row)
        {
            switch (row)
            {
                case 1:
                    return numbers[0]; //hur många ettor
                case 2:
                    return numbers[1] * 2; //antalet tvåor * 2
                case 3:
                    return numbers[2] * 3; //antalet treor * 3
                case 4:
                    return numbers[3] * 4; //samma med 4
                case 5:
                    return numbers[4] * 5; //samma med 5
                case 6:
                    return numbers[5] * 6; //samma med 6
                case 9://par
                    for(int i = 5; i > -1; i--)//går igenom omvänt så att den största siffran blir vald först. man vill inte få 2 påäng med en dice på 1 2 1 6 6
                    {
                        if(numbers[i] > 1)//om det finns mer än 1 av denna siffra
                        {
                            return 2 * (i+1);//ge summan av dem
                        } 
                    }
                    return 0;
                case 10://två par
                    for (int i = 5; i > -1; i--)//samma sak som förra
                    {
                        if (numbers[i] > 1)//om man har mer en 1
                        {
                            if(numbers[i] > 3)//om man har mer än 3 alltså som fyrtal men tvåpar istället
                            {
                                return 4 * (i + 1);//return summa
                            }
                            for(int j = i-1; j > -1; j--)//kollar igenom efter ett till par
                            {
                                if(numbers[j] > 1)
                                {
                                    return 2 * (j + 1) + 2 * (i + 1);//return summa
                                }
                            }
                        }
                    }
                    return 0;
                case 11://tretal
                    for (int i = 5; i > -1; i--)//samma som förra
                    {
                        if (numbers[i] > 2)//kollar om mer än 2
                        {
                            return 3 * (i + 1);//return summa
                        }
                    }
                    return 0;
                case 12://fyrtal
                    for (int i = 5; i > -1; i--)//samma som förra
                    {
                        if (numbers[i] > 3)//kollar om mer än 3
                        {
                            return 4 * (i + 1);//return summa
                        }
                    }
                    return 0;
                case 13:
                    if(numbers[4] == numbers[3] && numbers[2] == numbers[1] && numbers[3] == numbers[2] && numbers[0] == 1)//liten stege = 1 1 1 1 1 0
                    {
                        return 15;
                    }
                    return 0;
                case 14:
                    if (numbers[4] == numbers[3] && numbers[2] == numbers[1] && numbers[3] == numbers[2] && numbers[5] == 1)//stor stege = 0 1 1 1 1 1
                    {
                        return 20;
                    }
                    return 0;
                case 15://kåk
                    for(int i = 0; i < numbers.Length; i++)//kollar igenom alla nummer
                    {
                        if(numbers[i] == 2)//om en är 2
                        {
                            for (int j = 0; j < numbers.Length; j++)//kollar igenom alla igen
                            {
                                if(numbers[j] == 3)//om en är 3
                                {
                                    return (j + 1) * 3 + (i + 1) * 2;//summan
                                }
                            }
                        }
                    }
                    return 0;
                case 16://chans
                    return numbers[0] * 1 + numbers[1] * 2 + numbers[2] * 3 + numbers[3] * 4 + numbers[4] * 5 + numbers[5] * 6;//summan av alla
                case 17://yatzy
                    for(int i = 0; i < numbers.Length; i++)//kollar igenom alla
                    {
                        if(numbers[i] == 5)//om det vara finns en typ av nummer
                        {
                            return 50;
                        }
                    }
                    return 0;
                default:
                    return 0;
            }
        }
        private void aiTurn(int i)
        {
            Random rand = new Random();
            Console.WriteLine($"{players[i]} rolled...");
            int[] dice = { rand.Next(1, 7), rand.Next(1, 7), rand.Next(1, 7), rand.Next(1, 7), rand.Next(1, 7) };//dice åt ai
            Console.WriteLine($"{dice[0]} {dice[1]} {dice[2]} {dice[3]} {dice[4]}");
            double baseline = highestPoint(dice, i);//kollar vilket värde som är högst med denna dice
            bool[] diceRoll = { false, false, false, false, false };//vilka som ska kastas
            for (int j = 1; j < 32; j++)
            {
                bool[] tempDiceRoll = { false, false, false, false, false };//temp = temporär
                int[] tempDice = new int[5];
                tempDice[0] = dice[0];//transferar dice till tempdice
                tempDice[1] = dice[1];
                tempDice[2] = dice[2];
                tempDice[3] = dice[3];
                tempDice[4] = dice[4];
                int bin = j;
                double tempTotal = 0;
                int count = 0;
                if (bin > 15)//alla olika kombinationer med stil tagen från binära tal
                {
                    tempDiceRoll[4] = true;
                    bin -= 16;
                    count++;
                }
                if (bin > 7)
                {
                    tempDiceRoll[3] = true;
                    bin -= 8;
                    count++;
                }
                if (bin > 3)
                {
                    tempDiceRoll[2] = true;
                    bin -= 4;
                    count++;
                }
                if (bin > 1)
                {
                    tempDiceRoll[1] = true;
                    bin -= 2;
                    count++;
                }
                if (bin > 0)
                {
                    tempDiceRoll[0] = true;
                    count++;
                }
                if (tempDiceRoll[4] == true)//kollar igenom alla kombinationer av kombinationerna och sedan deras kombinationer och kombinationer av dem
                {
                    for (int l = 1; l < 7; l++)
                    {
                        tempDice[4] = l;
                        if (tempDiceRoll[3] == true)
                        {
                            for (int d = 1; d < 7; d++)
                            {
                                tempDice[3] = d;
                                if (tempDiceRoll[2] == true)
                                {
                                    for (int s = 1; s < 7; s++)
                                    {
                                        tempDice[2] = s;
                                        if (tempDiceRoll[1] == true)
                                        {
                                            for (int a = 1; a < 7; a++)
                                            {
                                                tempDice[1] = a;
                                                if (tempDiceRoll[0] == true)
                                                {
                                                    for (int q = 1; q < 7; q++)
                                                    {
                                                        tempDice[0] = q;
                                                        tempTotal += highestPoint(tempDice, i);
                                                    }
                                                }
                                                else tempTotal += highestPoint(tempDice, i);
                                            }
                                        }
                                        else tempTotal += highestPoint(tempDice, i);
                                    }
                                }
                                else tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[3] == true)
                {
                    for (int d = 1; d < 7; d++)
                    {
                        tempDice[3] = d;
                        if (tempDiceRoll[2] == true)
                        {
                            for (int s = 1; s < 7; s++)
                            {
                                tempDice[2] = s;
                                if (tempDiceRoll[1] == true)
                                {
                                    for (int a = 1; a < 7; a++)
                                    {
                                        tempDice[1] = a;
                                        if (tempDiceRoll[0] == true)
                                        {
                                            for (int q = 1; q < 7; q++)
                                            {
                                                tempDice[0] = q;
                                                tempTotal += highestPoint(tempDice, i);
                                            }
                                        }
                                        else tempTotal += highestPoint(tempDice, i);
                                    }
                                }
                                else tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[2] == true)
                {
                    for (int s = 1; s < 7; s++)
                    {
                        tempDice[2] = s;
                        if (tempDiceRoll[1] == true)
                        {
                            for (int a = 1; a < 7; a++)
                            {
                                tempDice[1] = a;
                                if (tempDiceRoll[0] == true)
                                {
                                    for (int q = 1; q < 7; q++)
                                    {
                                        tempDice[0] = q;
                                        tempTotal += highestPoint(tempDice, i);
                                    }
                                }
                                else tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[1] == true)
                {
                    for (int a = 1; a < 7; a++)
                    {
                        tempDice[1] = a;
                        if (tempDiceRoll[0] == true)
                        {
                            for (int q = 1; q < 7; q++)
                            {
                                tempDice[0] = q;
                                tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[0] == true)
                {
                    for (int q = 1; q < 7; q++)
                    {
                        tempDice[0] = q;
                        tempTotal += highestPoint(tempDice, i);
                    }
                }
                tempTotal = tempTotal / Math.Pow(6,count);//ifall den nya medelvärdet är större en största poängen fått av de dicesen man hade
                if(tempTotal > baseline)//ifall den är mer
                {
                    diceRoll[1] = tempDiceRoll[1];
                    diceRoll[2] = tempDiceRoll[2];
                    diceRoll[3] = tempDiceRoll[3];
                    diceRoll[4] = tempDiceRoll[4];
                    diceRoll[0] = tempDiceRoll[0];
                    baseline = tempTotal;
                }
            }
            for (int d = 0; d < 5; d++)//kasta dem enligt kombinationen som är bäst enligt poäng
            {
                if (diceRoll[d] == true)
                {
                    dice[d] = rand.Next(1, 7);
                }
            }
            for (int v = 0; v < 5; v++)//förklaring vilka som blev kastade för spelaren
            {
                if (diceRoll[v] == true)
                {
                    Console.Write("^ ");
                }
                else Console.Write("  ");
            }
            Console.WriteLine();
            Console.WriteLine($"{dice[0]} {dice[1]} {dice[2]} {dice[3]} {dice[4]}");//visar de nya dicen
            for (int j = 1; j < 32; j++)//allt igen då man kan kasta om 2 ggn
            {
                bool[] tempDiceRoll = { false, false, false, false, false };
                int[] tempDice = new int[5];
                tempDice[0] = dice[0];
                tempDice[1] = dice[1];
                tempDice[2] = dice[2];
                tempDice[3] = dice[3];
                tempDice[4] = dice[4];
                int bin = j;
                double tempTotal = 0;
                int count = 0;
                if (bin > 15)
                {
                    tempDiceRoll[4] = true;
                    bin -= 16;
                    count++;
                }
                if (bin > 7)
                {
                    tempDiceRoll[3] = true;
                    bin -= 8;
                    count++;
                }
                if (bin > 3)
                {
                    tempDiceRoll[2] = true;
                    bin -= 4;
                    count++;
                }
                if (bin > 1)
                {
                    tempDiceRoll[1] = true;
                    bin -= 2;
                    count++;
                }
                if (bin > 0)
                {
                    tempDiceRoll[0] = true;
                    count++;
                }
                if (tempDiceRoll[4] == true)
                {
                    for (int l = 1; l < 7; l++)
                    {
                        tempDice[4] = l;
                        if (tempDiceRoll[3] == true)
                        {
                            for (int d = 1; d < 7; d++)
                            {
                                tempDice[3] = d;
                                if (tempDiceRoll[2] == true)
                                {
                                    for (int s = 1; s < 7; s++)
                                    {
                                        tempDice[2] = s;
                                        if (tempDiceRoll[1] == true)
                                        {
                                            for (int a = 1; a < 7; a++)
                                            {
                                                tempDice[1] = a;
                                                if (tempDiceRoll[0] == true)
                                                {
                                                    for (int q = 1; q < 7; q++)
                                                    {
                                                        tempDice[0] = q;
                                                        tempTotal += highestPoint(tempDice, i);
                                                    }
                                                }
                                                else tempTotal += highestPoint(tempDice, i);
                                            }
                                        }
                                        else tempTotal += highestPoint(tempDice, i);
                                    }
                                }
                                else tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[3] == true)
                {
                    for (int d = 1; d < 7; d++)
                    {
                        tempDice[3] = d;
                        if (tempDiceRoll[2] == true)
                        {
                            for (int s = 1; s < 7; s++)
                            {
                                tempDice[2] = s;
                                if (tempDiceRoll[1] == true)
                                {
                                    for (int a = 1; a < 7; a++)
                                    {
                                        tempDice[1] = a;
                                        if (tempDiceRoll[0] == true)
                                        {
                                            for (int q = 1; q < 7; q++)
                                            {
                                                tempDice[0] = q;
                                                tempTotal += highestPoint(tempDice, i);
                                            }
                                        }
                                        else tempTotal += highestPoint(tempDice, i);
                                    }
                                }
                                else tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[2] == true)
                {
                    for (int s = 1; s < 7; s++)
                    {
                        tempDice[2] = s;
                        if (tempDiceRoll[1] == true)
                        {
                            for (int a = 1; a < 7; a++)
                            {
                                tempDice[1] = a;
                                if (tempDiceRoll[0] == true)
                                {
                                    for (int q = 1; q < 7; q++)
                                    {
                                        tempDice[0] = q;
                                        tempTotal += highestPoint(tempDice, i);
                                    }
                                }
                                else tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[1] == true)
                {
                    for (int a = 1; a < 7; a++)
                    {
                        tempDice[1] = a;
                        if (tempDiceRoll[0] == true)
                        {
                            for (int q = 1; q < 7; q++)
                            {
                                tempDice[0] = q;
                                tempTotal += highestPoint(tempDice, i);
                            }
                        }
                        else tempTotal += highestPoint(tempDice, i);
                    }
                }
                else if (tempDiceRoll[0] == true)
                {
                    for (int q = 1; q < 7; q++)
                    {
                        tempDice[0] = q;
                        tempTotal += highestPoint(tempDice, i);
                    }
                }
                tempTotal = tempTotal / Math.Pow(6, count);
                if (tempTotal > baseline)
                {
                    diceRoll[1] = tempDiceRoll[1];
                    diceRoll[2] = tempDiceRoll[2];
                    diceRoll[3] = tempDiceRoll[3];
                    diceRoll[4] = tempDiceRoll[4];
                    diceRoll[0] = tempDiceRoll[0];
                    baseline = tempTotal;
                }
            }
            for (int d = 0; d < 5; d++)
            {
                if (diceRoll[d] == true)
                {
                    dice[d] = rand.Next(1, 7);
                }
            }
            diceToNumbers(dice);
            for (int v = 0; v < 5; v++)
            {
                if (diceRoll[v] == true)
                {
                    Console.Write("^ ");
                }
                else Console.Write("  ");
            }
            Console.WriteLine();
            Console.WriteLine($"{dice[0]} {dice[1]} {dice[2]} {dice[3]} {dice[4]}");
            int row = 0;
            for (int o = 1; o < 18; o++)//´kollar alla rows
            {
                if (rowUnused(o, i))//ifall den e tom
                {
                    if (row == 0) row = o;//tar den som bas
                    int rowTemp = howmanypoints(o);//tar en temp på vilken row
                    if (rowTemp > howmanypoints(row)) row = o;//row byter om den nya ger mer poäng
                }
            }
            addPoints(i, howmanypoints(row),row);//lägger till poäng
            makeBoard(false);//visar tabell
            Console.ReadKey();//paus för spelaren att läsa
        }
        private int highestPoint(int[] tdice, int id)
        {
            int total = 0;
            diceToNumbers(tdice);//transformerar dice till nummerarrayen
            for(int i = 1; i < 18; i++)//går igenom alla rows
            {
                if (rowUnused(i, id))
                {
                    if (total < howmanypoints(i)) total = howmanypoints(i);//kollar om den större, om den är så blir den nya högsta
                }
            }
            return total;
        }
    }
}