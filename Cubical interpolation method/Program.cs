using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_lab2
{
    class CubicalInterpolationMethod
    {
        double x0;
        double x1;
        double x2;
        double eps1;
        double eps2;
        double delta;


        /*______________ Constructor ______________*/

        public CubicalInterpolationMethod()
        {
            x0 = 1;
            delta = 1;
            eps1 = 0.01;
            eps2 = 0.03;
            MainControl();

        }


        /*______________ Function ______________*/

        private double Function(double x)
        {
            return 2 * Math.Pow(x, 2) + 16 / x;

        }


        /*______________ Derivative function ______________*/

        private double DiffFunction(double x)
        {
             return 4 * x - 16 / Math.Pow(x, 2);

        }


        /*______________ The main action of method ______________*/

        private void MainControl()
        {
            Console.WriteLine("_____ Calculations _____");
            double M = MCalculation();
            Console.WriteLine("M = " + M);
            Console.WriteLine("X1 = " + x1);
            Console.WriteLine("X2 = " + x2);
            double f1 = Function(x1);
            Console.WriteLine("f1 = " + f1);
            double f1diff = DiffFunction(x1);
            Console.WriteLine("f1' = " + f1diff);
            double f2 = Function(x2);
            Console.WriteLine("f2 = " + f2);
            double f2diff = DiffFunction(x2);
            Console.WriteLine("f2' = " + f2diff);

            double xCherta = CubicMinPoint(f1, f1diff, f2, f2diff);
            Console.WriteLine("xCherta = " + xCherta);

            xCherta = DecreaseControl(xCherta);

            bool fail = false;

            while (!EndingControl(xCherta))
            {
                if (DiffFunction(xCherta) * DiffFunction(x1) < 0)
                {
                    x2 = x1;
                    x1 = xCherta;
                }
                else if (DiffFunction(xCherta) * DiffFunction(x2) < 0)
                {
                    x1 = xCherta;
                }
                else
                {
                    Console.WriteLine("Something goes wrong...");
                    fail = true;
                    break;
                }

               xCherta = CubicMinPoint(Function(x1), DiffFunction(x1), Function(x2), DiffFunction(x2));
               xCherta = DecreaseControl(xCherta);

            }

            if (!fail)
            {
                Console.WriteLine("***************************************");
                Console.WriteLine("Total result x = " + xCherta);
            }

        }


        /*______________ Calculation М number ______________*/

        private double MCalculation()
        {
            double k = 0;
            x1 = x0;

            if (DiffFunction(x0) < 0)
            {
                x2 = x1 + Math.Pow(2, k) * delta;
                while (DiffFunction(x1) * DiffFunction(x2) > 0)
                {
                    x1 = x2;
                    k++;
                    x2 = x1 + Math.Pow(2, k) * delta;
                }

                return k + 1;
            }
            else
            {
                x2 = x1 - Math.Pow(2, k) * delta;
                while (DiffFunction(x1) * DiffFunction(x2) > 0)
                {
                    x1 = x2;
                    k++;
                    x2 = x1 - Math.Pow(2, k) * delta;
                }
                return k + 1;
            }
        }


        /*______________ Calculation the minimum point of a cubic interpolation polynomial ______________*/

        private double CubicMinPoint(double f1, double f1diff, double f2, double f2diff)
        {
            double z = (3 * (f1 - f2)) / (x2 - x1) + f1diff + f2diff;
            double w;

            if (x1 < x2)
            {
                w = Math.Pow((Math.Pow(z, 2) - f1diff * f2diff), 0.5);
            }
            else
            {
                w = -1 * Math.Pow((Math.Pow(z, 2) - f1diff * f2diff), 0.5);
            }

            double mju = (f2diff + w - z) / (f2diff - f1diff + 2 * w);

            Console.WriteLine("z = " + z);
            Console.WriteLine("w = " + w);
            Console.WriteLine("mju = " + mju);

            double xCherta;

            if (mju < 0)
            {
                xCherta = x2;
            }
            else if (mju > 1)
            {
                xCherta = x1;
            }
            else
            {
                xCherta = x2 - mju * (x2 - x1);
            }

            return xCherta;

        }


        /*______________ Descending conditions ______________*/

        private double DecreaseControl(double xCherta)
        {
            Console.WriteLine("_____ Descending conditions _____");

            if (Function(xCherta) < Function(x1))
            {
                Console.WriteLine("Value xCherta hasn't been changed");
                return xCherta;
            }
            else
            {
                while (Function(xCherta) > Function(x1))
                {
                    xCherta = xCherta - 0.5 * (xCherta - x1);
                }

                Console.WriteLine("New value xCherta = " + xCherta);
                return xCherta;
            }

        }


        /*______________ End Condition ______________*/

        private bool EndingControl(double xCherta)
        {
            Console.WriteLine("_____ End Condition _____");

            if ((Math.Abs(DiffFunction(xCherta)) <= eps1) && (Math.Abs((xCherta - x1) / xCherta) <= eps2))
            {
                Console.WriteLine("1 condition " + Math.Abs(DiffFunction(xCherta)));
                Console.WriteLine("Eps 1 " + eps1);
                Console.WriteLine("2 condition " + Math.Abs((xCherta - x1) / xCherta));
                Console.WriteLine("Eps 2 " + eps2);

                Console.WriteLine("Satisfies end condition");
                return true;
            }
            else
            {
                Console.WriteLine("1 condition " + Math.Abs(DiffFunction(xCherta)));
                Console.WriteLine("Eps 1 " + eps1);
                Console.WriteLine("2 condition " + Math.Abs((xCherta - x1) / xCherta));
                Console.WriteLine("Eps 2 " + eps2);

                Console.WriteLine("Doesnt satisfy end condition");
                return false;
            }

        }











    }



    class Program
    {
        static void Main(string[] args)
        {
            CubicalInterpolationMethod cubicInterpolation = new CubicalInterpolationMethod();
            Console.ReadKey();
        }
    }
}
