using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul12HW

{
    public abstract class Car
    {
        public string Name { get; set; }
        public int Distance { get; set; }

        public int Speed { get; set; }

        public event EventHandler Finish;

        public Car(string name, int speed)
        {
            Name = name;
            Distance = 0;
            Speed = speed;
        }

        public void Move()
        {
            Distance += Speed;
            if (Distance >= 100)
                OnFinish();
        }

        protected virtual void OnFinish()
        {
            Finish?.Invoke(this, EventArgs.Empty);
        }
    }

    public class SportsCar : Car
    {
        public SportsCar(string name, int speed) : base(name, speed) { }
    }

    public class Bus : Car
    {
        public Bus(string name, int speed) : base(name, speed) { }
    }

    public class HyperCar : Car
    {
        public HyperCar(string name, int speed) : base(name, speed) { }
    }

    public class TrophyTruck : Car
    {
        public TrophyTruck(string name, int speed) : base(name, speed) { }
    }

    

    public class Racing
    {
        public delegate void RaceAction();

        public event EventHandler<string> WinnerAnnouncement;

        public void StartRace(Car[] cars)
        {
            RaceAction startDelegate = null;
            RaceAction raceDelegate = null;

            foreach (Car car in cars)
            {
                startDelegate += car.Move;
                car.Finish += OnCarFinish;
            }

            startDelegate?.Invoke();
            raceDelegate += startDelegate;

            while (!HasWinner(cars))
            {
                raceDelegate?.Invoke();
            }
        }

        private void OnCarFinish(object sender, EventArgs e)
        {
            if (sender is Car car)
            {
                WinnerAnnouncement?.Invoke(this, $"{car.Name} has finished the race!!!");
            }
        }

        private bool HasWinner(Car[] cars)
        {
            foreach (Car car in cars)
            {
                if (car.Distance >= 100)
                    return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sportsCar = new SportsCar("Spider", 12);
            var bus = new Bus("Toyota", 6);
            var hyperCar = new HyperCar("Aceca", 10);
            var trophyTruck = new TrophyTruck("Badja", 8);
            

            var game = new Racing();
            game.WinnerAnnouncement += OnWinnerAnnouncement;
            game.StartRace(new Car[] { sportsCar, bus, hyperCar, trophyTruck});
        }

        private static void OnWinnerAnnouncement(object sender, string e)
        {
            Console.WriteLine($"Winner!!!: {e}");
        }
    }
}
