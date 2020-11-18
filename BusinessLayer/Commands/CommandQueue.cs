using System;
using System.Collections.Generic;

namespace BusinessLayer {
    public class CommandQueue {
        public CommandQueue() {
            this.commands = new Queue<Command>();
        }

        public CommandQueue Add(Command cmd) {
            this.commands.Enqueue(cmd);
            return this;
        }

        public CommandQueue Remove(Command cmd) {
            this.commands.Filter((x) => x != cmd);
            return this;
        }

        public void Do() {
            foreach (var cmd in this.commands) {
                cmd.Do();
            }
        }

        public void Undo() {
            foreach (var cmd in this.commands.Rev()) {
                cmd.Undo();
            }
        }

        private Queue<Command> commands;
    }
}