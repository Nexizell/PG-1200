using System;

namespace Easing
{
    public class EaseIn
    {
        public static float Sine(float x)
        {
            return 1 - (float)Math.Cos((x * (float)Math.PI) / 2);
        }

        public static float Cubic(float x)
        {
            return x * x * x;
        }

        public static float Quint(float x)
        {
            return x * x * x * x * x;
        }

        public static float Circ(float x)
        {
            return 1 - (float)Math.Sqrt(1 - (float)Math.Pow(x, 2));
        }

        public static float Elastic(float x)
        {
            return x == 0 ? 0 : x == 1 ? 1 : -(float)Math.Pow(2, 10 * x - 10) * (float)Math.Sin((x * 10 - 10.75f) * ((2 * (float)Math.PI) / 3));
        }

        public static float Quad(float x)
        {
            return x * x;
        }

        public static float Quart(float x)
        {
            return x * x * x * x;
        }

        public static float Expo(float x)
        {
            return x == 0 ? 0 : (float)Math.Pow(2, 10 * x - 10);
        }

        public static float Back(float x)
        {
            return (1.70158f + 1f) * x * x * x - 1.70158f * x * x;
        }

        public static float Bounce(float x)
        {
            return 1 - EaseOut.Bounce(1 - x);
        }
    }

    public class EaseOut
    {
        public static float Sine(float x)
        {
            return (float)Math.Sin((x * (float)Math.PI) / 2);
        }

        public static float Cubic(float x)
        {
            return 1 - (float)Math.Pow(1 - x, 3);
        }

        public static float Quint(float x)
        {
            return 1 - (float)Math.Pow(1 - x, 5);
        }

        public static float Circ(float x)
        {
            return (float)Math.Sqrt(1 - (float)Math.Pow(x - 1, 2));
        }

        public static float Elastic(float x)
        {
            return x == 0 ? 0 : x == 1 ? 1 : (float)Math.Pow(2, -10 * x) * (float)Math.Sin((x * 10 - 0.75f) * ((2 * (float)Math.PI) / 3)) + 1;
        }

        public static float Quad(float x)
        {
            return 1 - (1 - x) * (1 - x); 
        }

        public static float Quart(float x)
        {
            return 1 - (float)Math.Pow(1 - x, 4);
        }

        public static float Expo(float x)
        {
            return x == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * x);
        }

        public static float Back(float x)
        {
            return 1 + (1.70158f + 1) * (float)Math.Pow(x - 1, 3) + 1.70158f * (float)Math.Pow(x - 1, 2);
        }

        public static float Bounce(float x)
        {
            return (x < 1 / 2.75f) ? 7.5625f * x * x : (x < 2 / 2.75f) ? 7.5625f * (x -= 1.5f / 2.75f) * x + 0.75f : (x < 2.5 / 2.75f) ? 7.5625f * (x -= 2.25f / 2.75f) * x + 0.9375f : 7.5625f * (x -= 2.625f / 2.75f) * x + 0.984375f;
        }
    }

    public class EaseInOut
    {
        public static float Sine(float x)
        {
            return -((float)Math.Cos((float)Math.PI * x) - 1) / 2;
        }

        public static float Cubic(float x)
        {
            return x < 0.5f ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;
        }

        public static float Quint(float x)
        {
            return x < 0.5f ? 16 * x * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 5) / 2;
        }

        public static float Circ(float x)
        {
            return x < 0.5f ? (1 - (float)Math.Sqrt(1 - (float)Math.Pow(2 * x, 2))) / 2 : ((float)Math.Sqrt(1 - (float)Math.Pow(-2 * x + 2, 2)) + 1) / 2;
        }

        public static float Elastic(float x)
        {
            return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f ? -((float)Math.Pow(2, 20 * x - 10) * (float)Math.Sin((20 * x - 11.125f) * ((2 * (float)Math.PI) / 4.5f))) / 2 : ((float)Math.Pow(2, -20 * x + 10) * (float)Math.Sin((20 * x - 11.125f) * ((2 * (float)Math.PI) / 4.5f))) / 2 + 1;
        }

        public static float Quad(float x)
        {
            return x < 0.5f ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }

        public static float Quart(float x)
        {
            return x < 0.5f ? 8 * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 4) / 2;
        }

        public static float Expo(float x)
        {
            return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f ? (float)Math.Pow(2, 20 * x - 10) / 2 : (2 - (float)Math.Pow(2, -20 * x + 10)) / 2;
        }

        public static float Back(float x)
        {
            return x < 0.5f ? ((float)Math.Pow(2 * x, 2) * ((1.70158f + 1) * 2 * x - 1.70158f)) / 2 : ((float)Math.Pow(2 * x - 2, 2) * ((1.70158f + 1) * (x * 2 - 2) + 1.70158f) + 2) / 2;
        }

        public static float Bounce(float x)
        {
            return x < 0.5f ? (1 - EaseOut.Bounce(1 - 2 * x)) / 2 : (1 + EaseOut.Bounce(2 * x - 1)) / 2;
        }
    }

    public enum EaseType
    {
        In,

        Out,

        InOut
    };

    public enum EasingMethod
    {
        Sine, Cubic, Quint, Circ, Elastic, Quad, Quart, Expo, Back, Bounce
    }

    public class Ease
    {
        public static float MethodByEnum(EaseType type, EasingMethod method, float x)
        {
            if (type == EaseType.In)
            {
                switch (method)
                {
                    case EasingMethod.Cubic:
                        return EaseIn.Cubic(x);

                    case EasingMethod.Quint:
                        return EaseIn.Quint(x);

                    case EasingMethod.Circ:
                        return EaseIn.Circ(x);

                    case EasingMethod.Elastic:
                        return EaseIn.Elastic(x);

                    case EasingMethod.Quad:
                        return EaseIn.Quad(x);

                    case EasingMethod.Quart:
                        return EaseIn.Quart(x);

                    case EasingMethod.Expo:
                        return EaseIn.Expo(x);

                    case EasingMethod.Back:
                        return EaseIn.Back(x);

                    case EasingMethod.Bounce:
                        return EaseIn.Bounce(x);

                    default:
                        return EaseIn.Sine(x);
                }
            }

            if (type == EaseType.Out)
            {
                switch (method)
                {
                    case EasingMethod.Cubic:
                        return EaseOut.Cubic(x);

                    case EasingMethod.Quint:
                        return EaseOut.Quint(x);

                    case EasingMethod.Circ:
                        return EaseOut.Circ(x);

                    case EasingMethod.Elastic:
                        return EaseOut.Elastic(x);

                    case EasingMethod.Quad:
                        return EaseOut.Quad(x);

                    case EasingMethod.Quart:
                        return EaseOut.Quart(x);

                    case EasingMethod.Expo:
                        return EaseOut.Expo(x);

                    case EasingMethod.Back:
                        return EaseOut.Back(x);

                    case EasingMethod.Bounce:
                        return EaseOut.Bounce(x);

                    default:
                        return EaseOut.Sine(x);
                } 
            }

            if (type == EaseType.InOut)
            {
                switch (method)
                {
                    case EasingMethod.Cubic:
                        return EaseInOut.Cubic(x);

                    case EasingMethod.Quint:
                        return EaseInOut.Quint(x);

                    case EasingMethod.Circ:
                        return EaseInOut.Circ(x);

                    case EasingMethod.Elastic:
                        return EaseInOut.Elastic(x);

                    case EasingMethod.Quad:
                        return EaseInOut.Quad(x);

                    case EasingMethod.Quart:
                        return EaseInOut.Quart(x);

                    case EasingMethod.Expo:
                        return EaseInOut.Expo(x);

                    case EasingMethod.Back:
                        return EaseInOut.Back(x);

                    case EasingMethod.Bounce:
                        return EaseInOut.Bounce(x);

                    default:
                        return EaseInOut.Sine(x);
                } 
            }

            throw new NullReferenceException("what did you do ? ? ? ? ?");
        }
    }
}
