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

            Props consoleWriterProps = Props.Create(() => new ConsoleWriterActor());
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");
            
            // Props validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
            // IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");
            
            // make tailCoordinatorActor
            Props tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");
            
            Props fileValidationActorProps = Props.Create(() => 
                new FileValidationActor(consoleWriterActor, tailCoordinatorActor));
            IActorRef validationActor = MyActorSystem.ActorOf(fileValidationActorProps, "validationActor");
            
            Props consoleReaderProps = Props.Create(() => new ConsoleReaderActor(validationActor));
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");
            
            

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
}
