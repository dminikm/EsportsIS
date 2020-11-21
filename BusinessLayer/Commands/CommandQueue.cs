using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class CommandQueue
    {
        public CommandQueue()
        {
            this.commands = new List<Command>();
            this.currentCommand = 0;
        }

        public CommandQueue Add(Command cmd)
        {
            this.commands.Add(cmd);
            return this;
        }

        public CommandQueue Remove(Command cmd)
        {
            this.commands = this.commands.Filter((x) => x != cmd).ToArr().ToList();
            return this;
        }

        public void Do(int numCmds = -1)
        {
            if (numCmds == -1) {
                numCmds = this.commands.Count;
            }

            int upto = Math.Min(this.currentCommand + numCmds, this.commands.Count);

            for (; this.currentCommand < upto; this.currentCommand++)
            {
                this.commands[this.currentCommand].Do();
            }
        }

        public void Undo(int numCmds = -1)
        {
            if (numCmds == -1) {
                numCmds = this.commands.Count;
            }

            int upto = Math.Max(this.currentCommand - numCmds, 0);

            for (; this.currentCommand >= upto; this.currentCommand--)
            {
                this.commands[this.currentCommand].Do();
            }
        }

        private List<Command> commands;
        int currentCommand;
    }
}