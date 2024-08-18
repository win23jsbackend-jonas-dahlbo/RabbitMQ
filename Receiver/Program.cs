using Receiver;

var broker = new RabbitMqbroker("Receiver App", "amqp://guest:guest@localhost:5672", "subscribe", "subscriber", "newsletter");

broker.Subscribe();
Console.ReadKey();
broker.Close();