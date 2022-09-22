using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code
{
    internal class Changes
    {
        //Class variables
        public string tablename;
        public string column_name;
        public string name;
        public string mDate;
        public string old_data;
        public string new_data;
        public string row_data;

        //Parameterized Constructor
        public Changes(string tablename, string column_name, string name, string mDate, string old_data, string new_data, string row_data)
        {
            this.tablename = tablename;
            this.column_name = column_name;
            this.name = name;
            this.mDate = mDate;
            this.old_data = old_data;
            this.new_data = new_data;
            this.row_data = row_data;
        }
    }
}
