using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_lab1
{
    class HalfDivisionIntervalMethod
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
            Console.WriteLine("Interval bisection method");
            a = 0;
            b = 10;
            l = 1;
            MethodHalfDivision();
        }


        /*__________________ Function __________________*/

        private double Function(double x)
        {
            return 2 * Math.Pow(x, 2) - 12 * x;  
        }


        /*__________________ The main action of the method  __________________*/

        private void MethodHalfDivision()
        {
            double[] ab = new double[2];
            ab[0] = a;
            ab[1] = b;

            GetNewInterval(ab);

            while (Math.Abs(ab[1] - ab[0]) > l)
            {
                ab = IntervalCalculating(ab);
            }
            Console.WriteLine("**********************");
            Console.WriteLine("X = " + xc);
        }


        /*__________________  Calculation a reduced interval   __________________*/

        private double [] IntervalCalculating(double [] interval)
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


        /*__________________  Calculation of new interval  __________________*/

        private void GetNewInterval(double [] interval)
        {
            intervalHalf = Math.Abs(interval[1] - interval[0]) / 2;
            intervalQuarter = intervalHalf / 2;
            xc = interval[0] + intervalHalf;
            y = interval[0] + intervalQuarter;
            z = interval[1] - intervalQuarter;
        }


    }


    class HookJeavesMethod
    {
        double deltaX;
        double deltaY;
        double eps;
        double[] point;
        double[] oldpoint;
        double[] firstpoint;


        /*__________________ Constructor __________________*/

        public HookJeavesMethod()
        {
            Console.WriteLine("===================================================");
            Console.WriteLine("Hooke-Jeeves method");

 
            deltaX = 0.1;
            deltaY = 0.1;

            eps = 0.01;
            point = new double[2];
            oldpoint = new double[2];
            firstpoint = new double[2];


            point[0] = 0.9;
            point[1] = 0.5;

            oldpoint[0] = 0;
            oldpoint[1] = 0;
            firstpoint[0] = 0;
            firstpoint[1] = 0;
            MainControl();

        }


        /*__________________ Function __________________*/

        private double Function(double x, double y)
        {
            return Math.Pow(x, 2) + Math.Pow(y, 2) + 0.5 * Math.Pow((x - 1), 2) - Math.Log(-1 * (x - y - 2));
        }


        /*__________________ The main action of the method  __________________*/

        private void MainControl()
        {
            while ((deltaX > eps) || (deltaY > eps))
            {
                point = Slope(point);
                deltaX = deltaX / 2;
                deltaY = deltaY / 2;
                Console.WriteLine("Point VAHRAMEI x = " + point[0] + " y = " + point[1] + " Value Function " + Function(point[0], point[1]));
            }

        }


        /*__________________ Descent to the ravine  __________________*/

        private double[] Slope(double[] position)
        {
            firstpoint[0] = position[0];
            firstpoint[1] = position[1];

            position = Investigation(position);

            oldpoint[0] = position[0];
            oldpoint[1] = position[1];

            position[0] = position[0] + (position[0] - firstpoint[0]);
            position[1] = position[1] + (position[1] - firstpoint[1]);

            bool ravine = false;

            while (!ravine)
            {
                if (Function(position[0], position[1]) < Function(oldpoint[0], oldpoint[1]))
                {
                    position = Investigation(position);

                    firstpoint[0] = position[0];
                    firstpoint[1] = position[1];

                    position[0] = position[0] + (position[0] - oldpoint[0]);
                    position[1] = position[1] + (position[1] - oldpoint[1]);

                    oldpoint[0] = firstpoint[0];
                    oldpoint[1] = firstpoint[1];
                }
                else
                {
                    position = Investigation(position);


                    Console.WriteLine("Ravine is found!!");
                    position[0] = oldpoint[0];
                    position[1] = oldpoint[1];

                    oldpoint[0] = 0;
                    oldpoint[1] = 0;
                    firstpoint[0] = 0;
                    firstpoint[0] = 0;

                    ravine = true;

                }
            }

            return position;
        }




        /*__________________ Exploratory search  __________________*/

        private double[] Investigation(double[] position)
        {
            Console.WriteLine("Current point x = " + position[0] + " y = " + position[1]);
            Console.WriteLine("Possible point coordinates with x + d1     x = " + (position[0] + deltaX) + " y = " + position[1]);
            Console.WriteLine("Possible point coordinates with x - d1     x = " + (position[0] - deltaX) + " y = " + position[1]);
            Console.WriteLine("Possible point coordinates with y + d2     x = " + position[0] + " y = " + (position[1] + deltaY));
            Console.WriteLine("Possible point coordinates with y - d2     x = " + position[0] + " y = " + (position[1] - deltaY));
            Console.WriteLine("_______________________________________________________________________________");
            if (Function(position[0] + deltaX, position[1]) < Function(position[0], position[1]))
            {
                position[0] = position[0] + deltaX;
            }
            else if (Function(position[0] - deltaX, position[1]) < Function(position[0], position[1]))
            {
                position[0] = position[0] - deltaX;
            }

            if (Function(position[0], position[1] + deltaY) < Function(position[0], position[1]))
            {
                position[1] = position[1] + deltaY;
            }
            else if (Function(position[0], position[1] - deltaY) < Function(position[0], position[1]))
            {
                position[1] = position[1] - deltaY;
            }
            Console.WriteLine("Point " + position[0] + " - " + position[1] + " Value Function " + Function(position[0], position[1]));


            return position;
        }



    }



    class Program
    {
        static void Main(string[] args)
        {
            HalfDivisionIntervalMethod HalfDivision = new HalfDivisionIntervalMethod();
            HookJeavesMethod HookJeaves = new HookJeavesMethod();

            Console.ReadKey();

        }
    }
}


