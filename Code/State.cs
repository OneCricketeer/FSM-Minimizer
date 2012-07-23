using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplications.FSM
{
    public class State
    {
        // A comparer used to sort the states in the FSM by name

        #region Compare Methods

        private class StateComparerHelper : IComparer
        {
            public StateComparerHelper() { }

            /// <summary>
            /// Compares the two states. Used to sort them in the FSM
            /// </summary>
            /// <param name="obj1">State 1</param>
            /// <param name="obj2">State 2</param>
            /// <returns></returns>
            public int Compare(object obj1, object obj2)
            {
                //                n++;
                State s1 = (State)obj1;
                State s2 = (State)obj2;
                return String.Compare(s1.Name, s2.Name);
            }
        }

        /// <summary>
        /// Sorts the states by name
        /// </summary>
        public static IComparer SortByName
        {
            get { return (IComparer)new StateComparerHelper(); }
        }

        #endregion Compare Methods

        // A struct for the map containing a map
        public struct IO
        {
            public Dictionary<string, State> output;

            public IO(string input, State output)
            {
                this.output = new Dictionary<string, State>();
                this.output.Add(input, output);
            }
        }

        // Fields
        private static int stateCounter = 0;

        static public int n = 0;

        public string Name { get; set; }

        public string[] input { get; set; }

        public string output { get; set; }

        public IO[] io;

        public State(string s)
        {
            this.Name = s;
            //            foreach (var item in GetName(this))
            //            {
            //                Console.WriteLine(item);
            //            }
        }

        /// <summary>
        /// Tests for equivalence to another  state <paramref name="s"/>
        /// </summary>
        /// <param name="s">The state to test equivalence</param>
        /// <returns>True if the names of the output states match</returns>
        public bool Equals(State s)
        {
            //            Console.Write("Comparing (" + this + ") with (" + s + ") ");
            int size = this.io.Length;
            bool b = false;
            if (size != s.io.Length)
                return b;
            for (int i = 0; i < size; i++)
            {
                //                n++;
                b = this.io[i].output[input[i]].Name.Equals(s.io[i].output[input[i]].Name);
                if (!b)
                    break;
            }
            //            Console.WriteLine(b);
            return b;
        }

        /* Just an idea if I ever want a linked list implementaion
        public State(string s, T data)
        {
            this.Name = s;
            this.data = data;
        }
         * */

        /// <summary>
        /// Names the state with a static counter starting at 0
        /// </summary>
        public State()
        {
            this.Name = "s" + stateCounter++;
        }

        //        public State(string s, string input, string output, State outputState)
        //        {
        //            this.Name = s;
        //
        //            init(io, 0);
        //            this.io[0] = new IO();
        //
        //            Dictionary<String, State> outputFunction = new Dictionary<String, State>();
        //            outputFunction.Add(output, outputState);
        //            this.io[0].output.Add(input, outputFunction);
        //
        //            // Will this create a new reference of a state with the same name?
        //            // I'm passing an existing state, but making a new one...
        //            //            outputState = new State(outputState.Name);
        //        }
        //
        //        public State(String s, String[] inputs, String[] outputs, State[] outputStates)
        //        {
        //            this.Name = s;
        //            this.input = inputs;
        //            this.output = outputs;
        //
        //            Console.WriteLine(outputStates.Length);
        //            int size = inputs.Length - 1;
        //            IO[] map = new IO[size];
        //
        //            for (int i = 0; i <= size; i++)
        //            {
        //                Dictionary<String, State> d = new Dictionary<String, State>();
        //                d.Add(outputs[i], outputStates[i]);
        //                map[i] = new IO(inputs[i], d);
        //            }
        //        }

        /// <summary>
        /// Method to create a sink
        /// </summary>
        /// <param name="p">The output string of the state</param>
        /// <param name="outputState">The sink state</param>
        public void setOutput(string p, State outputState)
        {
            this.output = p;

            int size = input.Length;
            // Initialize the map of (input, output)
            IO[] map = new IO[size];
            for (int i = 0; i < size; i++)
            {
                //                n++;
                //                Dictionary<String, State> d = new Dictionary<String, State>();
                //                d.Add(output[i], outputState);
                map[i] = new IO(input[i], outputState);
            }
            this.io = map;
        }

        /// <summary>
        /// Allows multiple output states
        /// </summary>
        /// <param name="p">The output string of the state</param>
        /// <param name="outputStates">A list of states that the state outputs to. Must be the same length as the alphabet</param>
        public void setOutput(string p, State[] outputStates)
        {
            this.output = p;

            int size = outputStates.Length;

            // Check to see if each state has the correct number of inputs/outputs
            if (size != input.Length)
            {
                string msg = this.Name + " does not have the proper amount of output states.";
                Console.WriteLine(msg);
                throw new Exception(msg);
            }

            // Initialize the map of (input, output)
            IO[] map = new IO[size];
            for (int i = 0; i < size; i++)
            {
                //                n++;
                //                Dictionary<String, State> d = new Dictionary<String, State>();
                //                d.Add(input[i], outputStates[i]);
                map[i] = new IO(input[i], outputStates[i]);
            }

            this.io = map;
        }

        /// <summary>
        /// Returns the state represented as a string
        /// </summary>
        /// <returns>State: {name} {output 1} {output 2} ... || {output state 1} {output state 2} ...</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("State: {0," + -1 * (this.Name.Length + 4) + "}", this.Name);

            // Append the alphabetical inputs
            for (int i = 0; i < input.Length; i++)
            {
                if (i == input.Length - 1)
                    sb.AppendFormat("{0}  ||  ", input[i]);
                else
                    sb.AppendFormat("{0, -5}", input[i]);
            }

            // Append the state outputs
            int j = 0;
            foreach (IO inout in this.io)
            {
                if (this.io[j].output.ContainsKey(input[j]))
                    sb.AppendFormat("{0}\t", this.io[j].output[input[j]].Name);
                else
                    Console.WriteLine("this.io[{1}].output does not contain {0}", input[j], j);
                j++;
            }

            return sb.ToString().Trim();
        }
    }
}