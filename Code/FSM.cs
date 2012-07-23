using System;
using System.Collections;
using System.Linq.Expressions;
using System.Text;

namespace ConsoleApplications.FSM
{
    // Alphabet class
    public class Alphabet
    {
        private string type;
        private ArrayList alphabet = new ArrayList();

        public Alphabet(string type)
        {
            this.type = type;
            switch (type)
            {
                // binary alphabet
                case "binary":
                    alphabet.Add("0");
                    alphabet.Add("1");
                    break;
                // english alphabet

                #region Alphabet

                case "alphabet":
                    alphabet.Add("A");
                    alphabet.Add("B");
                    alphabet.Add("C");
                    alphabet.Add("D");
                    alphabet.Add("E");
                    alphabet.Add("F");
                    alphabet.Add("G");
                    alphabet.Add("H");
                    alphabet.Add("I");
                    alphabet.Add("J");
                    alphabet.Add("K");
                    alphabet.Add("L");
                    alphabet.Add("M");
                    alphabet.Add("N");
                    alphabet.Add("O");
                    alphabet.Add("P");
                    alphabet.Add("Q");
                    alphabet.Add("R");
                    alphabet.Add("S");
                    alphabet.Add("T");
                    alphabet.Add("U");
                    alphabet.Add("V");
                    alphabet.Add("W");
                    alphabet.Add("X");
                    alphabet.Add("Y");
                    alphabet.Add("Z");
                    break;

                #endregion Alphabet

                // a custom alphabet separated by space or comma or a single word
                default:

                    if (type.Contains(",") || type.Contains(" "))
                    {
                        string[] elements;
                        elements = type.Split(new char[] { ' ', ',' });
                        foreach (string s in elements)
                        {
                            if (s != "")
                                alphabet.Add(s.ToUpper());
                        }
                        break;
                    }
                    else
                    {
                        foreach (char c in type)
                        {
                            alphabet.Add("" + c);
                        }
                        break;
                    }
            }
        }

        public ArrayList GetAlphabet { get { return alphabet; } }

        private int Count
        {
            get
            {
                return this.countHelper();
            }
        }

        private int countHelper()
        {
            int count = 0;
            switch (type)
            {
                case ("binary"):
                    return 2;
                case ("alphabet"):
                    return 26;
                default:
                    if (type.Contains(",") || type.Contains(" "))
                    {
                        string[] elements;
                        elements = type.Split(new char[] { ' ', ',' });
                        foreach (string s in elements)
                        {
                            if (s != "")
                                count += s.Length;
                        }
                        return count;
                    }

                    else
                        return type.Length;
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            foreach (String letter in alphabet)
            {
                if (letter != "")
                    s.Append(letter);
            }
            return s.ToString();
        }

        public String[] ToArray()
        {
            String[] alpha = new String[this.Count];

            for (int i = 0; i < this.Count; i++)
            {
                alpha[i] = "" + this.ToString()[i];
            }
            return alpha;
        }
    }

    public class FSM
    {
        private static string GetVariableName<T>(Expression<Func<T>> expression)
        {
            var body = ((MemberExpression)expression.Body);

            return body.Member.Name;
        }

        private String alphabetType { get; set; }

        private Alphabet a;
        private String[] alpha;
        private int partCount = 0;
        private PartitionContainer pc;

        public static ArrayList states = new ArrayList();

        // TODO: 1) Initialize the states
        private State s0 = new State();

        private State s1 = new State();
        private State s2 = new State();
        private State s3 = new State();
        //        State s4 = new State();
        //        State s5 = new State();
        //        State s6 = new State();
        //        State s7 = new State();
        //        State s8 = new State();
        //        State s9 = new State();

        public void build(String alphabet)
        {
            this.alphabetType = alphabet;
            a = new Alphabet(alphabet);
            alpha = a.ToArray();

            // TODO: 2) Add/Remove the appropriate initialized states
            foreach (State s in new State[] { s0, s1, s2, s3 })
            {
                //                State.n++;
                states.Add(s);
                s.input = alpha;
                //                s.output = alpha;
            }
            states.Sort(State.SortByName);

            /* TODO: 3) Set the output string & states for each state */
            s0.setOutput("0", new State[] { s0, s1 });
            s1.setOutput("0", new State[] { s2, s1 });
            s2.setOutput("0", new State[] { s0, s3 });
            s3.setOutput("1", new State[] { s2, s1 });
            //            s4.setOutput("0", new State[] { s3, s0 });
            //            s5.setOutputStates("", new State[] { s1, s4 });
            //            s6.setOutputStates("", new State[] { s3, s7 });
            //            s7.setOutputStates("", new State[] { s3, s6 });
            //            s8.setOutputStates("", new State[] { s2, s9 });
            //            s9.setOutputStates("", new State[] { s0, s1 });
        }

        // DONE: Create a way to loop through the states and print them as follows
        /*              I       O   ||  I       O
         * State: s0    0       1   ||  s1      s2
         * State: s1    0       1   ||  s0      s2
         * ...etc
         */

        public void print()
        {
            //            Console.Write("\t\tI\tO  ||   v\t w\n");
            foreach (State s in states)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }

        public void minimize()
        {
            // TODO: Save this for last, will use partitions class

            this.pc = new PartitionContainer(states);

            // renames the states in the FSM
            foreach (InnerPartition ip in pc)
            {
                foreach (State s in ip)
                {
                    // for each mapped state
                    for (int i = 0; i < s.io.Length; i++)
                    {
                        // get the mapped state that needs to be renamed
                        State curr = s.io[i].output[s.input[i]]; //[s.output[i]];

                        // if the mapped to state does not exist in the partitions
                        if (!states.Contains(curr))
                        {
                            foreach (State existing in pc.GetInnerPartitionOf(curr))
                            {
                                //                                Console.WriteLine(" " + curr.Name + " = " + existing.Name);

                                s.io[i].output[s.input[i]].output = existing.output;
                                s.io[i].output[s.input[i]] = existing;
                                //                                curr.Name = existing.Name;
                            }
                        }
                    }
                    //                    Console.WriteLine();
                }
            }
        }

        private void minimizeRecursiveHelper(PartitionContainer pc)
        {
            // TODO: Recursive? I don't think I did minimize right...
            Console.WriteLine("p{0}: {1}", partCount, pc);
        }

        public void input<T>(T s)
        {
            string z = Convert.ToString(s);
            foreach (State st in states)
            {
                if (st.output == null || st.output.Equals(String.Empty))
                {
                    string msg = String.Format("{0} does not have an output string.", st.Name);
                    Console.WriteLine(msg);
                    throw new Exception(msg);
                }
            }

            Console.Write("Given an input of {0} the output string is ", s);
            StringBuilder sb = new StringBuilder();

            State curr = s0;
            foreach (char i in z)
            {
                for (int j = 0; j < curr.io.Length; j++)
                {
                    string t = Convert.ToString(i);
                    if (curr.io[j].output.ContainsKey(t))
                        foreach (String input in curr.io[j].output[t].input)
                        {
                            if (input.Equals(t))
                            {
                                string output = curr.io[j].output[t].output;
                                sb.Append(output);
                                curr = curr.io[j].output[t];
                                break;
                            }
                        }
                }
            }
            Console.WriteLine(sb.ToString());
        }
    }
}