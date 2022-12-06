using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;

using Apache.NMS.ActiveMQ;
using static System.Collections.Specialized.BitVector32;


namespace ServeurBikingExecutable
{
    public enum StatusRouting
    {
        WALKING = 0, BIKING = 1, WALKING_END
    }
    public class MQ
    {

        public Routing routingDepart;
        public Routing routingBike;
        public Routing routingArrival;
        //public List<Step> steps;
        public List<Step> stepsWalkingDeparture;
        public List<Step> stepsBiking;
        public List<Step> stepsWalkingArrival;
        public StatusRouting status = StatusRouting.WALKING;
        public string nomQueue;
        public int lastPushWalkingDeparture = 0;
        public int lastPushBiking = 0;
        public int lastPushWalkingArrival = 0;
        public Station departStation;
        public Station arrivalStation;
        public int nbStep = 0;

        public MQ(string name, Routing routing)
        {
            this.nomQueue = name;
            this.routingDepart = routing;
            this.stepsWalkingDeparture = new List<Step>();
            this.stepsWalkingDeparture.Add(new Step("You will only walk"));
            this.stepsWalkingDeparture.AddRange(routing.features[0].properties.segments[0].steps);
            this.stepsBiking = new List<Step>();
            this.stepsWalkingArrival = new List<Step>();
        }

        public MQ(string name, Routing routing1, Routing routing2, Routing routing3, Station departStation, Station arrivalStation)
        {
            this.nomQueue = name;
            this.routingDepart = routing1;
            this.routingBike = routing2;
            this.routingArrival = routing3;
            this.stepsWalkingDeparture = routing1.features[0].properties.segments[0].steps;
            this.stepsWalkingDeparture.Add(new Step("Now you pick up a bike"));
            this.stepsBiking = routing2.features[0].properties.segments[0].steps;
            this.stepsBiking.Add(new Step("Now you drop off a bike"));
            this.stepsWalkingArrival = routing3.features[0].properties.segments[0].steps;
            this.stepsWalkingArrival.Add(new Step("Now you are arrived"));
            this.departStation = departStation;
            this.arrivalStation = arrivalStation;
        }

        public bool PushOnQueue()
        {

            try
            {
                Uri connecturi = new Uri("activemq:tcp://localhost:61616");
                ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);

                // Create a single Connection from the Connection Factory.
                IConnection connection = connectionFactory.CreateConnection();

                 // Create a session from the Connection.
                ISession session = connection.CreateSession();

                connection.Start();
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
                nbStep++;


                Console.WriteLine("Message sent, check ActiveMQ web interface to confirm.");
                //Console.ReadLine();


                // Don't forget to close your session and connection when finished.
                session.Close();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsDone()
        {
            if (stepsBiking.Count == 0 && stepsWalkingArrival.Count == 0)
            {
                return lastPushWalkingDeparture == stepsWalkingDeparture.Count;
            }
            else
            {
                return lastPushWalkingArrival == stepsWalkingArrival.Count;
            }
        }

        private ITextMessage getMessage(ISession session)
        {
            ITextMessage message;
            if (stepsBiking.Count == 0 && stepsWalkingArrival.Count == 0)
            {
                message = session.CreateTextMessage(stepsWalkingDeparture[lastPushWalkingDeparture].id + " : " + stepsWalkingDeparture[lastPushWalkingDeparture].instruction +" in " + stepsWalkingDeparture[lastPushWalkingDeparture].distance + " meters.");
                lastPushWalkingDeparture++;
                return message;
            }
            if (lastPushWalkingDeparture < stepsWalkingDeparture.Count)
            {
                message = session.CreateTextMessage(stepsWalkingDeparture[lastPushWalkingDeparture].id + " : " + stepsWalkingDeparture[lastPushWalkingDeparture].instruction +" in " + stepsWalkingDeparture[lastPushWalkingDeparture].distance + " meters.");
                lastPushWalkingDeparture++;
            }
            else if (lastPushBiking < stepsBiking.Count)
            {
                status = StatusRouting.BIKING;
                message = session.CreateTextMessage(stepsBiking[lastPushBiking].id + " : " + stepsBiking[lastPushBiking].instruction + " in " + stepsBiking[lastPushBiking].distance + " meters.");
                lastPushBiking++;

            }
            else
            {
                status = StatusRouting.WALKING_END;
                message = session.CreateTextMessage(stepsWalkingArrival.Count + "  " + stepsWalkingArrival[lastPushWalkingArrival].id + " : " + stepsWalkingArrival[lastPushWalkingArrival].instruction + " in " + stepsWalkingArrival[lastPushWalkingArrival].distance + " meters.");
                lastPushWalkingArrival++;
            }
            return message;
        }

        internal bool needRecalculateRouting()
        {
            if (nbStep == 3)
            {
                nbStep = 0;
                return true;
            }
            return false;
        }
    }


}
