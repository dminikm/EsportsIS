using System;
using System.Collections.Generic;

namespace BusinessLayer {
    public class PreviewList<T> {
        public PreviewList(Func<List<T>> load, Func<T, Command> addCmd, Func<T, Command> removeCmd) {
            this.lazyData = new Lazy<List<T>>(load);
            this.addCmd = addCmd;
            this.removeCmd = removeCmd;
            this.commands = new CommandQueue();
        }

        public void Add(T value) {
            this.commands.Add(this.addCmd(value));
            this.Data.Add(value);
        }

        public void Remove(T value) {
            this.commands.Add(this.removeCmd(value));
            this.Data.Remove(value);
        }

        public void Do() {
            this.commands.Do();
        }

        public void Undo() {
            this.commands.Undo();
        }

        private CommandQueue commands;

        Func<T, Command> addCmd;
        Func<T, Command> removeCmd;

        private Lazy<List<T>> lazyData;
        private List<T> Data { get => lazyData.Value; }
    }
}