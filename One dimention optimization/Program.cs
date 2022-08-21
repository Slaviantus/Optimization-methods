using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisationOneDimention
{
    abstract class Optimisation
    {
        /*____________  Accuracy  ____________*/
        protected const double eps = 0.001;  //Точность эпсилон

        /*____________  Point A of segment  ____________*/
        protected double BeginX = -2.5; //Начало отрезка

        /*____________  Point B of segment  ____________*/
        protected double EndX = -1.0; //Конец отрезка

        /*____________  Sun of iteratiions  ____________*/
        protected int itr = 0;

        /*~~~~~~~~~~~~  Function  ~~~~~~~~~~~~*/
        protected double Function(double valueX)
        {
            return Math.Exp(valueX) - (1.0 / 3.0) * Math.Pow(valueX, 3.0) + 2.0 * valueX;

        }

        /*~~~~~~~~~~~~  Result output of methods  ~~~~~~~~~~~~*/
        protected void ShowResult(double minPoint, int iterations)
        {
            Console.WriteLine();
            Console.WriteLine("Minimum point of function = " + minPoint);
            Console.WriteLine("Function minimum = " + Function(minPoint));
            Console.WriteLine("Sum of iterations: " + iterations);
        }

    }


    class GoldenSection : Optimisation
    {
        public GoldenSection()
        {
            Console.WriteLine("_______________  Golden-section method  _______________");

            double a = BeginX;
            double b = EndX;
            double min = 0;
            itr = 0;

            double d = a + Math.Abs((a - b) * 0.618);
            double c = a + Math.Abs((a - b) * 0.382);

            while (Math.Abs(a - b) > eps)
            {
                if (Function(d) <= Function(c))
                {
                    a = c;
                    d = a + Math.Abs((a - b) * 0.618);
                    c = a + Math.Abs((a - b) * 0.382);
                    itr++;
                }
                else
                {
                    b = d;
                    d = a + Math.Abs((a - b) * 0.618);
                    c = a + Math.Abs((a - b) * 0.382);
                    itr++;
                }
            }

            min = a + Math.Abs((a - b) / 2.0);

            ShowResult(min, itr);

        }

    }



    class HalfDivisionIntervalMethod : Optimisation
    {
        double a;
        double b;
        double l;
        double xc;
        double y;
        double z;
        double intervalHalf;
        double intervalQuarter;

        /*__________________ Constructor __________________*/
        public HalfDivisionIntervalMethod()
        {
            Console.WriteLine("_______________  Interval bisection method  _______________");
            a = BeginX;
            b = EndX;
            l = eps;
            itr = 0;
            MethodHalfDivision();
        }


        /*__________________ Общий ход метода  __________________*/
        private void MethodHalfDivision()
        {
            double[] ab = new double[2];


            ab[0] = a;
            ab[1] = b;

            GetNewInterval(ab);

            while (Math.Abs(ab[1] - ab[0]) > l)
            {
                ab = IntervalCalculating(ab);
                itr++;
            }

            ShowResult(xc, itr);
        }


        /*__________________  Calculating a reduced interval  __________________*/
        private double[] IntervalCalculating(double[] interval)
        {

            if (Function(y) < Function(xc))
            {
                interval[1] = xc;
                GetNewInterval(interval);
                Console.WriteLine("A " + interval[0] + "  B " + interval[1]);
            }
            else if (Function(z) < Function(xc))
            {
                interval[0] = xc;
                GetNewInterval(interval);
                Console.WriteLine("A " + interval[0] + "  B " + interval[1]);
            }
            else
            {
                interval[0] = y;
                interval[1] = z;
                GetNewInterval(interval);
                Console.WriteLine("A " + interval[0] + "  B " + interval[1]);
            }
            return interval;
        }


        /*__________________  Calculation new interval  __________________*/
        private void GetNewInterval(double[] interval)
        {
            intervalHalf = Math.Abs(interval[1] - interval[0]) / 2;
            intervalQuarter = intervalHalf / 2;
            xc = interval[0] + intervalHalf;
            y = interval[0] + intervalQuarter;
            z = interval[1] - intervalQuarter;
        }
    }


    class QuadraticInterpolation : Optimisation
    {
        double x1;
        double x2;
        double x3;
        double deltaX;
        double eps1;
        double eps2;
        double f1;
        double f2;
        double f3;
        double Fmin;
        double Xmin;
        double Xcherta;
        double denominator;
        bool exit = false;
        bool allEntry = true;


        public QuadraticInterpolation()
        {
            Console.WriteLine("_______________  Quadratic interpolation method  _______________");

            x1 = -1.7; // Start point
            deltaX = 0.25; // Step
            eps1 = 0.02; // Accuracy 1
            eps2 = 0.05; // Accuracy 2
            itr = 0;


            while (!exit)
            {
                if (allEntry)
                {
                    GetNewPoints();
                }
                else
                {
                    f1 = Function(x1);
                    f2 = Function(x2);
                    f3 = Function(x3);
                }



                FindMIN();

                denominator = ((x2 - x3) * f1 + (x3 - x1) * f2 + (x1 - x2) * f3);
                if (denominator != 0)
                {
                    InterpolatingPolinom();

                    if (!Condition1() || !Condition2())
                    {
                        if ((Xcherta >= x1) && (Xcherta <= x3))
                        {
                            FindNewPoint();
                            allEntry = false;
                        }
                        else
                        {
                            x1 = Xcherta;
                            allEntry = true;
                        }
                    }

                    else
                    {
                        exit = true;
                    }

                }
                else
                {
                    Console.WriteLine("Interpolation polynomial denominator = 0");
                    allEntry = true;
                }


                Console.WriteLine(Xcherta);
                itr++;
            }

            ShowResult(Xcherta, itr);
        }

        private void FindMIN()
        {

            if ((f1 < f2) && (f1 < f3))
            {
                Fmin = f1;
                Xmin = x1;
            }
            else if ((f2 < f1) && (f2 < f3))
            {
                Fmin = f2;
                Xmin = x2;
            }
            else
            {
                Fmin = f3;
                Xmin = x3;
            }

        }

        private void GetNewPoints()
        {
            x2 = x1 + deltaX;

            f1 = Function(x1);
            f2 = Function(x2);

            if (f1 > f2)
            {
                x3 = x1 + 2 * deltaX;
            }
            else
            {
                x3 = x1 - deltaX;
            }

            f3 = Function(x3);

        }

        private void InterpolatingPolinom()
        {
            Xcherta = (0.5 * ((Math.Pow(x2, 2.0) - Math.Pow(x3, 2.0)) * f1 + (Math.Pow(x3, 2.0) - Math.Pow(x1, 2.0)) * f2 + (Math.Pow(x1, 2.0) - Math.Pow(x2, 2.0)) * f3)) / denominator;
        }

        private bool Condition1()
        {
            bool condition = false;

            if (Math.Abs((Fmin - Function(Xcherta)) / Function(Xcherta)) < eps1)
            {
                condition = true;
            }

            return condition;
        }


        private bool Condition2()
        {
            bool condition = false;

            if (Math.Abs((Xmin - Xcherta) / Xcherta) < eps2)
            {
                condition = true;
            }

            return condition;
        }

        private void FindNewPoint()
        {
            if (Function(Xcherta) <= Fmin)
            {
                if (Xcherta > x2)
                {
                    x1 = x2;
                    x2 = Xcherta;
                }
                else
                {
                    x3 = x2;
                    x2 = Xcherta;
                }
            }
            else
            {
                if (Xmin > x2)
                {
                    x1 = x2;
                    x2 = Xmin;
                }
                else
                {
                    x3 = x2;
                    x2 = Xmin;
                }
            }

        }


    }

    class Dichotomy : Optimisation
    {
        double y;
        double z;
        double smallValue;
        double l;
        double a;
        double b;
        double fy;
        double fz;
        double min;
        bool exit = false;
        public Dichotomy()
        {
            Console.WriteLine("_______________  Dichotomy method  _______________");

            l = 1.0;
            smallValue = 0.2;
            a = BeginX;
            b = EndX;
            itr = 0;

            while (!exit)
            {
                itr++;

                y = (a + b - smallValue) / 2.0;
                z = (a + b + smallValue) / 2.0;

                fy = Function(y);
                fz = Function(z);

                if (fy <= fz)
                {
                    b = z;
                }
                else
                {
                    a = y;
                }

                if (Math.Abs(b - a) <= l)
                {
                    min = (a + b) / 2.0;
                    exit = true;

                }
            }

            ShowResult(min, itr);

        }


    }


    class Fibonachi : Optimisation
    {
        double[] a;
        double[] b;
        double[] y;
        double[] z;

        double l0;
        double l;
        double epsConst;
        int k = 0;
        double min = 0;
        int N = 1;


        int[] numFibonachi;
        public Fibonachi()
        {
            Console.WriteLine("_______________  Fibonacci method  _______________");

            a = new double[1];
            b = new double[1];
            y = new double[1];
            z = new double[1];

            l = 0.25;
            a[0] = BeginX;
            b[0] = EndX;
            epsConst = eps;
            l0 = Math.Abs(b[0] - a[0]);

            GetN();

            Array.Resize<double>(ref a, N);
            Array.Resize<double>(ref b, N);
            Array.Resize<double>(ref y, N);
            Array.Resize<double>(ref z, N);

            Calculating();

            CalculatingEnding();

            ShowResult(min, N);

        }

        private void GetN()
        {
            numFibonachi = new int[2];
            numFibonachi[0] = N;

            if (numFibonachi[N - 1] < l0 / l)
            {

                N++;
                numFibonachi[1] = N;

                bool exit = false;
                while (!exit)
                {
                    if (numFibonachi[N - 1] >= l0 / l)
                    {
                        exit = true;
                    }
                    else
                    {
                        N++;
                        Array.Resize<int>(ref numFibonachi, N);
                        numFibonachi[N - 1] = numFibonachi[N - 2] + numFibonachi[N - 3];
                    }

                }
            }


        }

        private void Calculating()
        {
            y[0] = a[0] + ((double)numFibonachi[N - 3] / numFibonachi[N - 1] * (b[0] - a[0]));
            z[0] = a[0] + ((double)numFibonachi[N - 2] / numFibonachi[N - 1] * (b[0] - a[0]));


            while (k != (N - 2))
            {
                if (Function(y[k]) <= Function(z[k]))
                {
                    a[k + 1] = a[k];
                    b[k + 1] = z[k];
                    z[k + 1] = y[k];
                    y[k + 1] = a[k + 1] + (double)numFibonachi[N - k - 4] / numFibonachi[N - k - 2] * (b[k + 1] - a[k + 1]);

                }
                else
                {
                    a[k + 1] = y[k];
                    b[k + 1] = b[k];
                    y[k + 1] = z[k];
                    z[k + 1] = a[k + 1] + (double)numFibonachi[N - k - 3] / numFibonachi[N - k - 2] * (b[k + 1] - a[k + 1]);
                }

                ShowingSteps(k);
                k++;


            }

        }


        private void CalculatingEnding()
        {

            y[N - 1] = y[N - 2] = z[N - 2];
            z[N - 1] = y[N - 1] + epsConst;

            if (Function(y[N - 1]) <= Function(z[N - 1]))
            {
                a[N - 1] = a[N - 2];
                b[N - 1] = z[N - 1];
            }
            else
            {
                a[N - 1] = y[N - 1];
                b[N - 1] = b[N - 2];
            }

            min = (a[N - 1] + b[N - 1]) / 2.0;
            ShowingSteps(N - 2);
            ShowingSteps(N - 1);

        }

        private void ShowingSteps(int step)
        {
            Console.WriteLine("************************");
            Console.WriteLine("k = " + step);
            Console.WriteLine("a[k] = " + a[step]);
            Console.WriteLine("b[k] = " + b[step]);
            Console.WriteLine("y[k] = " + y[step]);
            Console.WriteLine("z[k] = " + z[step]);
        }


    }










    class Program
    {
        static void Main(string[] args)
        {
            GoldenSection goldenSectionMethod = new GoldenSection();
            Console.WriteLine();
            HalfDivisionIntervalMethod halfDivision = new HalfDivisionIntervalMethod();
            Console.WriteLine();
            QuadraticInterpolation quadraticInterpolation = new QuadraticInterpolation();
            Console.WriteLine();
            Dichotomy dichotomy = new Dichotomy();
            Console.WriteLine();
            Fibonachi fibonachiMethod = new Fibonachi();

            Console.ReadKey();
        }
    }
}