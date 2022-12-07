import com.soap.ws.client.generated.IServiceBiking;
import com.soap.ws.client.generated.ServiceBiking;
import org.apache.activemq.ActiveMQConnectionFactory;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.TextMessage;
import java.util.Scanner;

public class Client implements javax.jms.MessageListener{

    private javax.jms.Connection connect = null;
    private javax.jms.Session receiveSession = null;
    private javax.jms.Queue queue = null;

    private static final long DELAY = 100;

    public synchronized void lauch(){
        while(true) {
            Scanner scanner = new Scanner(System.in);
            System.out.println("Saisissez adresse de départ");
            String departure = scanner.nextLine();
            System.out.println("Saisissez adresse de arrivée");
            String arrival = scanner.nextLine();
            ServiceBiking serviceBiking = new ServiceBiking();
            String queue = serviceBiking.getBasicHttpBindingIServiceBiking().getRoute(departure, arrival);

            if (!queue.equals("Trajet Impossible, vous devez rester dans la même ville")) {
                configurer(queue);

                while (serviceBiking.getBasicHttpBindingIServiceBiking().nextStep(queue)) {
                    //recuperation des infos
                    try {
                        wait(1500);
                    } catch (Exception e) {
                        System.out.println("Error wait");
                    }
                }
            }
            System.out.println(queue);
        }

    }


    private void configurer(String queue) {

        try
        {	// Create a connection.
            javax.jms.ConnectionFactory factory;
            factory = new ActiveMQConnectionFactory("user", "password", "tcp://localhost:61616");
            connect = factory.createConnection("user","password");
            // ce programme est donc en mesure d'accéder au broker ActiveMQ, avec connecteur tcp (openwire)
            // Si le producteur et le consommateur étaient codés séparément, ils auraient eu ce même bout de code

            this.configurerConsommateur(queue);
            connect.start(); // on peut activer la connection.

        } catch (javax.jms.JMSException jmse){
            System.out.println(queue);
        }
    }

    private void configurerConsommateur(String nom) throws JMSException{
        // Pour consommer, il faudra simplement ouvrir une session
        receiveSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        queue = receiveSession.createQueue (nom);
        javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
        qReceiver.setMessageListener(this);
        // Now that 'receive' setup is complete, start the Connection

    }

    @Override
    public void onMessage(Message message) {
        // Methode permettant au consommateur de consommer effectivement chaque msg recu
        // via la queue
        try {
            System.out.println(((TextMessage) message).getText());
        } catch (Exception e){
            System.out.println("Une erreur");
        }
    }
}
