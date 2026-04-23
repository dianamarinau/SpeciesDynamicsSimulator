using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpeciesDynamicsSimulator
{
    public class Law
    {
        public List<Condition> conditions = new List<Condition>();
        public int start;
        public int end;
        public bool isActive = false;
        public Law(string buffer)
        {
            string[] elements = buffer.Split(new char[] { '{', '}', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            this.start = int.Parse(elements[0].Trim());
            this.end = int.Parse(elements[2].Trim());
            string blocked = elements[3].Trim();
            if(blocked == "block")
                this.isActive = true;
            string[] middle = elements[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string element in middle)
            {
                Condition condition = new Condition(element);
                conditions.Add(condition);
            }
        }
    }
}
