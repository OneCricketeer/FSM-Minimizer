using System;
using System.Collections;
using System.Text;

namespace ConsoleApplications.FSM
{
    // DONE: Change all methods to use state instead of name
    public class InnerPartition : System.Collections.IEnumerable
    {
        private ArrayList states = new ArrayList();

        #region Constructors

        public InnerPartition()
        {
            this.states = new ArrayList();
        }

        public InnerPartition(int size)
        {
            this.states = new ArrayList(size);
        }

        public InnerPartition(State s)
        {
            //            State.n++;
            //            if (states.Contains(s))
            //                throw new Exception(String.Format("Duplicate states ({0}) exist in a partition: {1}", s.Name, this.ToString()));
            states.Add(s);
        }

        public InnerPartition(State[] sArr)
        {
            foreach (State s in sArr)
            {
                //                State.n++;
                //                if (states.Contains(s))
                //                    throw new Exception(String.Format("Duplicate states ({0}) exist in a partition: {1}", s.Name, this.ToString()));
                states.Add(s);
            }
        }

        #endregion Constructors

        public void Add(State s)
        {
            this.states.Add(s);
        }

        public Array ToArray()
        {
            return states.ToArray();
        }

        public bool Contains(State s)
        {
            bool b = false;
            foreach (State st in this)
            {
                //                State.n++;
                //                Console.WriteLine("    " + st);
                if (st.Equals(s))
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        public override string ToString()
        {
            if (states.Count >= 1)
            {
                StringBuilder sb = new StringBuilder("{");
                foreach (State s in states)
                {
                    sb.Append(s.Name + ", ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("}");
                return sb.ToString();
            }
            else return String.Empty;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < states.ToArray().Length; i++)
            {
                yield return states.ToArray()[i];
            }
        }
    }

    internal class PartitionContainer : System.Collections.IEnumerator
    {
        public ArrayList partitions;

        #region Constructors

        public PartitionContainer()
        {
            this.partitions = new ArrayList();
        }

        public PartitionContainer(int size)
        {
            this.partitions = new ArrayList(size);
        }

        // DONE: File the states into partitons based on output states
        // DONE: Optimized to n-1
        // Where the magic happens
        public PartitionContainer(ArrayList partitions)
        {
            this.partitions = new ArrayList();

            // Set current state to test
            Object[] arr = partitions.ToArray();

            // base case to add the first state
            this.Add(new InnerPartition((State)arr[0]));

            // Test the remaining states
            for (int i = 1; i < arr.Length; i++)
            {
                //                State.n++;
                State curr = (State)arr[i];
                // if the states in the partition match the current state, don't add and continue
                if (this.Contains(curr))
                {
                    FSM.states.Remove(curr);
                    continue;
                    /* Create initial partition */
                    //                    GetInnerPartitionOf(curr).Add(curr);
                }

                // else, since they don't match, create a new partition
                else
                    this.Add(new InnerPartition((State)arr[i]));
            }
        }

        #endregion Constructors

        public void Add(InnerPartition p)
        {
            this.partitions.Add(p);
        }

        public InnerPartition GetInnerPartitionOf(State s)
        {
            foreach (InnerPartition p in this.partitions)
            {
                //                Console.WriteLine(p.ToString() + s.ToString());
                //                State.n++;
                if (p.Contains(s))
                    return p;
            }
            return null;
        }

        public bool Contains(State s)
        {
            if (GetInnerPartitionOf(s) == null)
            {
                return false;
            }
            return true;
        }

        // Creates {} if empty or {{s0}, {s1}} if not
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if (partitions.Count > 0)
            {
                foreach (InnerPartition p in partitions)
                {
                    String part = p.ToString();
                    if (part != String.Empty)
                    {
                        sb.Append(p.ToString() + ", ");
                    }
                }
            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 2, 2);
            sb.Append("}");
            return sb.ToString();
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < partitions.Count; i++)
            {
                yield return partitions.ToArray()[i];
            }
        }

        #region Unused methods for iteration

        public object Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion Unused methods for iteration
    }
}