using System;
using System.Threading.Tasks;

namespace Joshua.Model
{
    public interface IDataHandle
    {
        public event EventHandler RequestComplete;
        public Task SendRequest(string getroll, string v1, string v2);
        public string[] GetStrings();
    }
}