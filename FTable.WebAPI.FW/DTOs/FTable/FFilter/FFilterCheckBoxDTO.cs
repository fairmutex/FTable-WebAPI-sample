using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTable.WebAPI.FW.DTOs.FTable.FFilter
{
    public class FFilterCheckBoxDTO:IFFilterDataBaseDTO, IEnumerable
    {
        private ArrayList Values;

        public FFilterCheckBoxDTO() {
            this.Values = new ArrayList();
        }

        public void Add(string Value) { this.Values.Add(Value); }
        public void ClearPeople() { this.Values.Clear(); }
        public int Count { get { return this.Values.Count; } }

        IEnumerator IEnumerable.GetEnumerator() { return this.Values.GetEnumerator(); }
    }


}