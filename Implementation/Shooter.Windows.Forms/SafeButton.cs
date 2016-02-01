namespace Allberg.Shooter.Windows.Forms
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Summary description for SafeButtonBox.
    /// </summary>
    public class SafeButton : Button
    {
        public SafeButton()
        {
            _getTextMethod += GetText;
            _setTextMethod += SetText;

            _getEnabledMethod += GetEnabled;
            _setEnabledMethod += SetEnabled;
        }

        #region Text
        private delegate string GetTextMethodInvoker();
        private readonly GetTextMethodInvoker _getTextMethod;
        private delegate void SetTextMethodInvoker(string text);
        private readonly SetTextMethodInvoker _setTextMethod;

        override public string Text
        {
            get
            {
                if (InvokeRequired)
                {
                    return (string)Invoke(_getTextMethod);
                }
                return base.Text;
            }
            set
            {
                if (InvokeRequired)
                    throw new Exception("Invoke Required on " + Name);
                base.Text = value;
            }
        }

        private string GetText()
        {
            return Text;
        }
        private void SetText(string text)
        {
            Text = text;
        }
        #endregion

        #region Enabled
        private delegate bool GetEnabledMethodInvoker();
        private readonly GetEnabledMethodInvoker _getEnabledMethod;
        private delegate void SetEnabledMethodInvoker(bool enabled);
        private readonly SetEnabledMethodInvoker _setEnabledMethod;

        public new bool Enabled
        {
            get
            {
                if (InvokeRequired)
                {
                    return (bool)Invoke(_getEnabledMethod);
                }
                return base.Enabled;
            }
            set
            {
                if (InvokeRequired)
                {
                    Invoke(_setEnabledMethod, new object[] { value });
                }
                base.Enabled = value;
            }
        }

        private bool GetEnabled()
        {
            return Enabled;
        }
        private void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }
        #endregion
    }
}
