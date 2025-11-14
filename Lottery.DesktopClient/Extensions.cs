using System.Reflection;

namespace Lottery.DesktopClient
{
    public static class Extensions
    {
        public static T As<T>(this string str)
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }

        public static T DoubleBuffered<T>(this T c, bool setting) where T : Control
        {
            Type dgvType = c.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(c, setting, null);
            return c;
        }

        public static void AddEmptySpace(this FlowLayoutPanel flp)
        {
            var space = new Panel();
            space.Size = new Size(100, 50);
            flp.Controls.Add(space);
        }
    }
}
