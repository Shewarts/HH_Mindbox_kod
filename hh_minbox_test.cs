using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mindbox_hh_test
{
    /// <Краткое_содержание>
    /// Абстрактный класс для вычисления площади фигур, фигуры можно описать набором отрезков. Примеры(образцы): радиус круга, стороны треугольника, полуоси эллипса и т.д.
    /// </Краткое_содержание>
    public  abstract class Shaping
    {
        /// <Краткое_содержание>
        /// Набор отрезков для описания фигуры
        /// </Краткое_содержание>
        protected List<double> _measurments;
        /// <Краткое_содержание>
        /// Установка и возврат нередактируемых отрезков, описывающих фигуру.
        /// </Краткое_содержание>
        /// <exception cref="ArgumentException">Заданный набор отрезков не описывает существующую фигуру</exception>
        public ReadOnlyCollection<double> Measurments
        {
            set
            {
                if (IsValid(value.ToList()))
                {
                    _measurments = value.ToList();
                }
                else
                {
                    throw new ArgumentException("Shaping measurments are invalid");
                }
            }
            get
            {
                return _measurments.AsReadOnly();
            }
        }
        /// <Краткое_содержание>
        /// Свойство для возврата площади фигуры
        /// </Краткое_содержание>
        public double Area { get { return CalculateArea(); } }
        /// <Краткое_содержание>
        /// Абстрактный метод для вычисление площади фигуры
        /// </Краткое_содержание>
        /// <returns>Площадь фигуры</returns>
        protected abstract double CalculateArea();
        /// <Краткое_содержание>
        /// Абстрактный метод, для проверки списка переданных отрезков(Описывают ли они действительную фигуру?)
        /// </Краткое_содержание>
        /// <param name="measurments">Список отрезков для проверки</param>
        /// <returns>True - список отрезков описывает действительную фигуру, False - список отрезков не описывает действительную фигуру</returns>
        protected abstract bool IsValid(List<double> measurments);
    }

    /// <Краткое_содержание>
    /// Класс для операции создания и вычиления площади для круга
    /// </Краткое_содержание>
    public class Circle_krug : Shaping
    {
        /// <Краткое_содержание>
        /// Конструктор нового экземпляра Circle с переданным радиусом
        /// </Краткое_содержание>
        /// <param name="radius">Радиус круга</param>
        public Circle_krug(double radius)
        {
            Measurments = new List<double>() { radius }.AsReadOnly();
        }

        /// <Краткое_содержание>
        /// Проверка переданного списка отрезков на описание действительного круга: длина списка - 1, double.MaxValue >= значение длины > 0
        /// </Краткое_содержание>
        /// <param name="measurments">Список отрезков, содержащий радиус</param>
        /// <returns>True - список отрезков описывает действительный круг, False - список отрезков не описывает действительный круг</returns>
        protected override bool IsValid(List<double> measurments)
        {
            if (measurments.Count == 1 && measurments[0] > 0 && measurments[0] <= double.MaxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <Краткое_содержание>
        /// Рассчитывается площадь круга по его радиусу
        /// </Краткое_содержание>
        /// <returns>Площадь круга</returns>
        protected override double CalculateArea()
        {
            return Math.PI * Measurments[0] * Measurments[0];
        }
    }

    /// <Краткое_содержание>
    /// Класс для операции создания и вычиления площади для треугольника
    /// </Краткое_содержание>
    public class Triangle : Shaping
    {
        /// <Краткое_содержание>
        /// Создаёт новый экземпляр Triangle с переданными сторонами
        /// </Краткое_содержание>
        /// <param name="side1">Сторона треугольника A</param>
        /// <param name="side2">Сторона треугольника B</param>
        /// <param name="side3">Сторона треугольника C</param>
        public Triangle(double side1, double side2, double side3)
        {
            Measurments = new List<double>() { side1, side2, side3 }.AsReadOnly();
        }

        /// <Краткое_содержание>
        /// Выполняет проверку переданного списка отрезков на описание действительного треугольника: длина списка - 3, double.MaxValue >= значение длины каждой из сторон > 0,
        /// Сумма длинн каждых 2 сторон больше длинны третьей стороны.
        /// </Краткое_содержание>
        /// <param name="measurments">Список отрезков, содержащий стороны треугольника</param>
        /// <returns>True - список отрезков описывает действительный треугольник, False - список отрезков не описывает действительный треугольник</returns>
        protected override bool IsValid(List<double> measurments)
        {
            if (measurments.Count == 3 &&
                measurments[0] > 0 &&
                measurments[1] > 0 &&
                measurments[2] > 0 &&
                measurments[0] <= double.MaxValue &&
                measurments[1] <= double.MaxValue &&
                measurments[2] <= double.MaxValue &&
                measurments[0] + measurments[1] > measurments[2] &&
                measurments[0] + measurments[2] > measurments[1] &&
                measurments[1] + measurments[2] > measurments[0]
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <Краткое_содержание>
        /// Проверяет является ли треугольник прямоугольным.
        /// </Краткое_содержание>
        /// <returns>True - треугольник является прямоугольным, False - треугольник не является прямоугольным, null - невозможно определить тип треугольника из-за переполнения double</returns>
        public bool? IsRightTriangle()
        {
            var orderedMeasurments = _measurments.OrderByDescending(m => m).ToList();

            double csqr = Math.Pow(orderedMeasurments[0], 2);
            double bsqr = Math.Pow(orderedMeasurments[1], 2);
            double asqr = Math.Pow(orderedMeasurments[2], 2);

            if (double.IsInfinity(csqr) || double.IsInfinity(bsqr + asqr))
            {
                return null;
            }

            if (csqr == bsqr + asqr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <Краткое_содержание>
        /// Делает рассчёт площади треугольника по 3 его сторонам
        /// </Краткое_содержание>
        /// <returns>Площадь треугольника. Возвращает double.PositiveInfinity, если в ходе вычисления произошло переполнение double</returns>
        protected override double CalculateArea()
        {
            double p = (Measurments[0] + Measurments[1] + Measurments[2]) / 2;
            return Math.Sqrt(p * (p - Measurments[0]) * (p - Measurments[1]) * (p - Measurments[2]));
        }
    }
}