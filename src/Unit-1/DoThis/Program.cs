﻿using System;
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

            // ConsoleWriterActor
            var consoleWriterActor = MyActorSystem.ActorOf(Props.Create(() =>
                new ConsoleWriterActor()));
            
            // ConsoleReaderActor
            var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() =>
                new ConsoleReaderActor(consoleWriterActor)));

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
}
