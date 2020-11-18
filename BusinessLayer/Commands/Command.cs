namespace BusinessLayer {
    public class Command {
        public Command(CommandDelegate run, CommandDelegate undo) {
            this.run = run;
            this.undo = undo;
        }

        public void Do() {
            run();
        }

        public void Undo() {
            undo();
        }

        public delegate void CommandDelegate();

        private CommandDelegate run;
        private CommandDelegate undo;


        // Blank delegate
        public static CommandDelegate Blank = () => {};
    }
}