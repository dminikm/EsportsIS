using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class PreviewList<T>
    {
        public PreviewList(Func<List<T>> load, Func<T, Command> addCmd, Func<T, Command> removeCmd, bool immediate = false)
        {
            this.lazyData = new Lazy<List<T>>(load);
            this.addCmd = addCmd;
            this.removeCmd = removeCmd;
            this.commands = new CommandQueue();
            this.immediate = immediate;
        }

        public void Add(T value)
        {
            this.commands.Add(this.addCmd(value));
            this.Data.Add(value);

            if (this.immediate)
            {
                this.commands.Do(1);
            }
        }

        public void Remove(T value)
        {
            this.commands.Add(this.removeCmd(value));
            this.Data.Remove(value);

            if (this.immediate)
            {
                this.commands.Do(1);
            }
        }

        public void Do()
        {
            this.commands.Do();
        }

        public void Undo()
        {
            this.commands.Undo();
        }

        public List<T> ToList()
        {
            return this.Data.Map((x) => x).ToList();
        }

        private CommandQueue commands;

        private Func<T, Command> addCmd;
        private Func<T, Command> removeCmd;

        private Lazy<List<T>> lazyData;
        private List<T> Data { get => lazyData.Value; }

        private bool immediate;
    }
}