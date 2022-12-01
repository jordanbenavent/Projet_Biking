﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;

using Apache.NMS.ActiveMQ;

namespace ServeurSoapBiking
{
    public enum StatusRouting
    {
        WALKING=0, BIKING=1
    }
    public class MQ
    {

        public List<Step> steps;
        public List<Step> stepsWalkingDeparture;
        public List<Step> stepsBiking;
        public List<Step> stepsWalkingArrival;
        public StatusRouting status = StatusRouting.WALKING;
        public string nomQueue;
        public int lastPushWalkingDeparture = 0;
        public int lastPushBiking = 0;
        public int lastPushWalkingArrival = 0;

        public MQ(string name, List<Step> steps)
        {
            this.nomQueue = name;
            this.stepsWalkingDeparture = new List<Step>();
            this.stepsWalkingDeparture.Add(new Step("You Will only walk"));
            this.stepsWalkingDeparture.AddRange(steps);
            this.stepsBiking = new List<Step>();
            this.stepsWalkingArrival = new List<Step>();   
        }

        public MQ(string name, List<Step> steps1, List<Step> steps2, List<Step> steps3)
        {
            this.nomQueue = name;
            this.stepsWalkingDeparture = steps1;
            this.stepsWalkingDeparture.Add(new Step("Now you pick up a bike"));
            this.stepsBiking = steps2;
            this.stepsBiking.Add(new Step("Now you drop off a bike"));
            this.stepsWalkingArrival = steps3;
            this.stepsWalkingArrival.Add(new Step("Now you are arrived"));
        }

        public void PushOnQueue() {
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);

            // Create a single Connection from the Connection Factory.
            IConnection connection = connectionFactory.CreateConnection();
            connection.Start();

            // Create a session from the Connection.
            ISession session = connection.CreateSession();

            // Use the session to target a queue.
            IDestination destination = session.GetQueue(nomQueue);

            // Create a Producer targetting the selected queue.
            IMessageProducer producer = session.CreateProducer(destination);

            // You may configure everything to your needs, for instance:
            producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

            // Finally, to send messages:
            /*
            foreach(string instruction in instructions)
            {
                ITextMessage message = session.CreateTextMessage(steps[lastPush].id + " : " + steps[lastPush].instruction);
                producer.Send(message);
            }*/
            ITextMessage message = getMessage(session);
            //message = session.CreateTextMessage(steps[lastPush].id + " : " + steps[lastPush].instruction);
            producer.Send(message);


            Console.WriteLine("Message sent, check ActiveMQ web interface to confirm.");
            Console.ReadLine();


            // Don't forget to close your session and connection when finished.
            session.Close();
            connection.Close();
        }

        public bool IsDone()
        {
            if(stepsBiking.Count == 0 && stepsWalkingArrival.Count == 0)
            {
                return lastPushWalkingDeparture == stepsWalkingDeparture.Count;
            } else
            {
                return lastPushWalkingArrival == stepsWalkingArrival.Count;
            }
        }

        private ITextMessage getMessage(ISession session)
        {
            ITextMessage message;
            if(stepsBiking.Count == 0 && stepsWalkingArrival.Count == 0)
            {
                message = session.CreateTextMessage(stepsWalkingDeparture[lastPushWalkingDeparture].id + " : " + stepsWalkingDeparture[lastPushWalkingDeparture].instruction);
                lastPushWalkingDeparture++;
                return message;
            }
            if (lastPushWalkingDeparture < stepsWalkingDeparture.Count)
            {
                message = session.CreateTextMessage(stepsWalkingDeparture[lastPushWalkingDeparture].id + " : " + stepsWalkingDeparture[lastPushWalkingDeparture].instruction);
                lastPushWalkingDeparture++;
            }
            else if (lastPushBiking < stepsBiking.Count)
            {
                status = StatusRouting.BIKING;
                message = session.CreateTextMessage(stepsBiking[lastPushBiking].id + " : " + stepsBiking[lastPushBiking].instruction);
                lastPushBiking++;

            }
            else
            {
                status = StatusRouting.WALKING;
                message = session.CreateTextMessage(stepsWalkingArrival.Count+ "  " + stepsWalkingArrival[lastPushWalkingArrival].id + " : " + stepsWalkingArrival[lastPushWalkingArrival].instruction);
                lastPushWalkingArrival++;
            }
            return message;
        }
    }
}
