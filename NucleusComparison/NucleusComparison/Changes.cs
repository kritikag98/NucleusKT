using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NucleusComparison
{
    internal class Changes
    {
        public string column_name;
        public string old_data;
        public string new_data;
        public string row_data;
        public Changes(string column_name, string old_data, string new_data, string row_data)
        {
            this.column_name = column_name;
            this.old_data = old_data;
            this.new_data = new_data;
            this.row_data = row_data;
        }
    }
}
