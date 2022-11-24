import com.soap.ws.client.generated.IServiceBiking;
import com.soap.ws.client.generated.ServiceBiking;
import org.apache.activemq.ActiveMQConnectionFactory;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.TextMessage;
import java.util.Scanner;

public class Client implements javax.jms.MessageListener{

    private javax.jms.Connection connect = null;
    private javax.jms.Session sendSession = null;
    private javax.jms.Session receiveSession = null;
    private javax.jms.MessageProducer sender = null;
    private javax.jms.Queue queue = null;

    private static final long DELAY = 100;

    public void lauch(){
        configurer();
        Scanner scanner = new Scanner(System.in);
        System.out.println("Saisissez adresse de départ");
        String departure = scanner.nextLine();
        System.out.println("Saisissez adresse de arrivée");
        String arrival = scanner.nextLine();
        ServiceBiking serviceBiking = new ServiceBiking();
        String route = serviceBiking.getBasicHttpBindingIServiceBiking().getRoute(departure,arrival);
        System.out.println(route);
    }


    private void configurer() {

        try
        {	// Create a connection.
            javax.jms.ConnectionFactory factory;
            factory = new ActiveMQConnectionFactory("user", "password", "tcp://localhost:61616");
            connect = factory.createConnection("user","password");
            // ce programme est donc en mesure d'accéder au broker ActiveMQ, avec connecteur tcp (openwire)
            // Si le producteur et le consommateur étaient codés séparément, ils auraient eu ce même bout de code

            this.configurerConsommateur();
            connect.start(); // on peut activer la connection.

        } catch (javax.jms.JMSException jmse){
            jmse.printStackTrace();
        }
    }

    private void configurerConsommateur() throws JMSException{
        // Pour consommer, il faudra simplement ouvrir une session
        receiveSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        queue = receiveSession.createQueue ("QueueServiceBiking");
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
