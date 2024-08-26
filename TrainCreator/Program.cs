using System;
using System.Collections.Generic;

namespace TrainCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Play();
        }
    }

    class Game
    {
        private Dispatcher _dispatcher;

        public Game()
        {
            _dispatcher = new Dispatcher();
        }

        public void Play()
        {
            const string CommandAddTrain = "1";
            const string CommandExit = "2";

            string userInput;

            bool isWork = true;

            while (isWork)
            {
                Console.Clear();

                _dispatcher.ShowTrains();

                Console.WriteLine($"{CommandAddTrain}) Добавить поезд" +
                                  $"\n{CommandExit}) Выход");

                Console.Write("Введите команду: ");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandAddTrain:
                        _dispatcher.CreateTrain();
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Неверная команда");
                        break;
                }

                Console.ReadKey();
            }
        }
    }

    class Dispatcher
    {
        private List<Train> _trains = new List<Train>();

        private int _maxPassengersInWagon = 50;
        private int _maxPassengersInTrain = 501;

        public void ShowTrains()
        {
            Console.WriteLine("Созданы следующие составы:");

            for (int i = 0; i < _trains.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {_trains[i]}");
            }

            Console.WriteLine();
        }

        public void CreateTrain()
        {
            Direction direction = CreateDirection();

            int numberOfPassengers = GetRandomPassengers();

            List<Wagon> wagons = CreateWagons(numberOfPassengers);

            Train train = new Train(direction, wagons);

            Console.WriteLine("Поезд создан: ");
            Console.WriteLine(train);

            _trains.Add(train);
        }

        private Direction CreateDirection()
        {
            string startDirection;
            string finishDirection;

            bool isEqualDirection;
            bool isNull;

            do
            {
                Console.Write("Введите точку отправления: ");
                startDirection = Console.ReadLine();

                Console.Write("Введите точку прибытия: ");
                finishDirection = Console.ReadLine();

                isEqualDirection = startDirection.ToLower() == finishDirection.ToLower();
                isNull = startDirection == "" || finishDirection == "";
            }
            while (isEqualDirection || isNull);

            return new Direction(startDirection, finishDirection);
        }

        private int GetRandomPassengers()
        {
            Random random = new Random();

            return random.Next(_maxPassengersInTrain);
        }

        private List<Wagon> CreateWagons(int numberOfPassengers)
        {
            List<Wagon> wagons = new List<Wagon>();

            int maxWagons = (numberOfPassengers + _maxPassengersInWagon - 1) / _maxPassengersInWagon;

            for (int i = 0; i < maxWagons - 1; i++)
            {
                numberOfPassengers -= _maxPassengersInWagon;
                wagons.Add(new Wagon(_maxPassengersInWagon, _maxPassengersInWagon));
            }

            wagons.Add(new Wagon(_maxPassengersInWagon, numberOfPassengers));

            return wagons;
        }
    }

    class Train
    {
        private List<Wagon> _wagons = new List<Wagon>();
        private Direction _direction;

        public Train(Direction direction, List<Wagon> wagons)
        {
            _direction = direction;
            _wagons = wagons;
        }

        public override string ToString()
        {
            return $"Сформирован состав по маршруту {_direction} с количеством вагонов: {_wagons.Count}. " +
                $"Количество занятых мест: {TakeCountOfPassengers()}. Количество свободных мест: {TakeRemainingSeats()}";
        }

        private int TakeCountOfPassengers()
        {
            int passengers = 0;

            foreach (var wagon in _wagons)
            {
                passengers += wagon.NumberOfPassengers;
            }

            return passengers;
        }

        private int TakeRemainingSeats()
        {
            int remainingSeats = 0;

            foreach (var wagon in _wagons)
            {
                remainingSeats += wagon.TakeRemainingSeats();
            }

            return remainingSeats;
        }
    }

    class Wagon
    {      
        private int _maxPassengers;

        public Wagon(int maxPassengers, int numberOfPassengers)
        {
            _maxPassengers = maxPassengers;
            NumberOfPassengers = numberOfPassengers;
        }

        public int NumberOfPassengers { get; private set; }

        public int TakeRemainingSeats()
        {
            return _maxPassengers - NumberOfPassengers;
        }
    }

    class Direction
    {
        private string _start;
        private string _finish;

        public Direction(string start, string finish)
        {
            _start = start;
            _finish = finish;
        }

        public override string ToString()
        {
            return $"{_start} - {_finish}";
        }
    }
}
