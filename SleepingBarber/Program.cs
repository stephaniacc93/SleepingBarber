using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SleepingBarber
{
    class Program
    {
        static int seats = 5;
        static Queue<Client> products = new Queue<Client>(seats);
        static Random r = new Random();

        static void GoToBarberShop()
        {
            while (true)
            {
                for (int i = 0; i < r.Next(10); i++)
                {
                    if (products.Count <= seats)
                    {
                        Client product = new Client("Client", i);
                        lock (products)
                        {
                            products.Enqueue(product);
                            Monitor.Pulse(products);
                            Console.WriteLine("Cliente en la sala de espera\n");
                            Thread.Sleep(250);

                        }
                    }
                    else
                    {
                        Console.WriteLine("Cliente se fue porque no habia sillas disponibles :(\n");
                    }
                }
            }

        }

        static void CutHair()
        {
            while (true)
            {
                for (int i = 0; i < r.Next(10); i++)
                {
                    Client product = new Client();
                    lock (products)
                    {
                        while (products.Count == 0)
                        {
                            Console.WriteLine("NO HAY CLIENTES! Barber durmiendo\n");
                            Monitor.Wait(products);
                            Thread.Sleep(250);
                        }

                        product = products.Dequeue();
                        Console.WriteLine("UH OH! HAY CLIENTES ESPERANDO!\n");
                        Console.WriteLine("***Barber cortando el pelo***\n");
                    }
                }
            }

        }

        public class Client
        {
            string name;
            int ClientNumber;

            public Client(string name, int productNumber)
            {
                this.name = name;
                this.ClientNumber = productNumber;
            }

            public Client() { }
        }

        static void Main(string[] args)
        {

            Thread ClientThread = new Thread(GoToBarberShop);
            Thread BarberThread = new Thread(CutHair);
            ClientThread.Start();
            BarberThread.Start();
            ClientThread.Join();
            BarberThread.Join();
            Console.ReadLine();

        }
    }
}
