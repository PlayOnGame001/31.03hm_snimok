using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _31._03hm_snimok
{
    class Snimok
    {
        private string state;
        public Snimok(string state)
        {
            this.state = state;
        }
        public void Statistik()
        {
            Console.WriteLine("Snimok: Сделал.");
            this.state = Random_stat(30);
            Console.WriteLine($"Snimok: Как вам снимок: {state}");
        }
        private string Random_stat(int length = 10)
        {
            string allowedSymbols = "asdasdfDS[FLPGASDF12";
            string result = string.Empty;
            while (length > 0)
            {
                result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];
                Thread.Sleep(12);
                length--;
            }
            return result;
        }
        public Pamat Save()
        {
            return new ConcreteMemento(state);
        }
        public void Restore(Pamat memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Открыт класс " + memento.ToString());
            }
            this.state = memento.GetState();
            Console.Write($"Snimok: У вас есть выбор смены фото: {state}");
        }
    }
    public interface Pamat
    {
        string GetName();
        string GetState();
        DateTime GetDate();
    }
    class ConcreteMemento : Pamat
    {
        private string state;
        private DateTime date;
        public ConcreteMemento(string state)
        {
            this.state = state;
            date = DateTime.Now;
        }
        public string GetState()
        {
            return state;
        }
        public string GetName()
        {
            return $"{date} / ({state.Substring(0, 9)})...";
        }
        public DateTime GetDate()
        {
            return date;
        }
    }
    class Caretaker
    {
        private List<Pamat> mementos = new List<Pamat>();
        private Snimok originator = null;
        public Caretaker(Snimok originator)
        {
            this.originator = originator;
        }
        public void Backup()
        {
            Console.WriteLine("\nИнформация: сохранить оригинал...");
            mementos.Add(originator.Save());
        }
        public void Undo()
        {
            if (mementos.Count == 0)
            {
                return;
            }
            var memento = mementos.Last();
            mementos.Remove(memento);
            Console.WriteLine("Информация: Измена фото: " + memento.GetName());
            try
            {
                originator.Restore(memento);
            }
            catch (Exception)
            {
                Undo();
            }
        }
        public void ShowHistory()
        {
            Console.WriteLine("Информация : Лист выбора :");
            foreach (var memento in mementos)
            {
                Console.WriteLine(memento.GetName());
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Snimok originator = new Snimok("Зделано красиво.");
            Caretaker caretaker = new Caretaker(originator);
            caretaker.Backup();
            originator.Statistik();
            caretaker.Backup();
            originator.Statistik();
            caretaker.Backup();
            originator.Statistik();
            Console.WriteLine();
            caretaker.ShowHistory();
            Console.WriteLine("\nКлиент: Вернуться назад!\n");
            caretaker.Undo();
            Console.WriteLine("\n\nКлиент: Попытка еще одна!\n");
            caretaker.Undo();
            Console.WriteLine();
        }
    }
}
