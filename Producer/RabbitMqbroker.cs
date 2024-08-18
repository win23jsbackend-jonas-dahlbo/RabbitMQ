using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;

namespace Producer;

public class RabbitMqbroker
{
    private readonly string _queueName;
    private readonly string _exchangeName;
    private readonly string _routingKey;

    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqbroker(string clientProvidedName, string connectionstring, string queueName, string exchangeName, string routingKey)
    {
        _queueName = queueName;
        _exchangeName = exchangeName;
        _routingKey = routingKey;

        _factory = new ConnectionFactory
        {
            Uri = new Uri(connectionstring),
            ClientProvidedName = clientProvidedName,
        };

        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        _channel.QueueDeclare(_queueName, false, false, false);
        _channel.QueueBind(_queueName, _exchangeName, _routingKey);
        _channel.BasicQos(0, 1, false);
    }

    public void Close()
    {
        _channel.Close();
        _connection.Close(); 
    }

    public bool Publish(string message)
    {
        try
        {
            var payload = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(_exchangeName, _routingKey, null, payload);

            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex);  return false; }
    }
}

