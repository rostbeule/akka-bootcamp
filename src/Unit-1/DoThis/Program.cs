using System;
using Akka.Actor;

namespace WinTail
{
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterActor = MyActorSystem
                .ActorOf(Props.Create(() => new ConsoleWriterActor()), "consoleWriterActor");
            
            // Props validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
            // IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");
            
            var tailCoordinatorActor = MyActorSystem
                .ActorOf(Props.Create(() => new TailCoordinatorActor()), "tailCoordinatorActor");
            
            var validationActor = MyActorSystem.
                ActorOf(Props.Create(() => new FileValidationActor(consoleWriterActor)), "validationActor");
            
            var consoleReaderActor = MyActorSystem
                .ActorOf(Props.Create(() => new ConsoleReaderActor()), "consoleReaderActor");
            
            

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
}
