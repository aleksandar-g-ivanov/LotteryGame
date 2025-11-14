namespace Lottery.DesktopClient.Card
{
    public class CardBuilder
    {
        Label _control;
        string _txt;
        object _data;
        Action<object> _clickHandler;
        
        public static CardBuilder New => new();

        public CardBuilder WithClickHandler(Action<object> handler)
        {
            _clickHandler = handler;
            return this;
        }

        public CardBuilder WithData(object data)
        {
            _data = data;
            return this;
        }

        public CardBuilder WithText(string txt)
        {
            _txt = txt;
            return this;
        }

        public CardBuilder Build()
        {
            _control = new Label();
            _control.Cursor = Cursors.Hand;
            _control.ForeColor = Color.Gray;
            _control.Margin = new Padding(5);
            _control.AutoSize = true;
            _control.MinimumSize = new Size(200, 50);
            _control.BorderStyle = BorderStyle.FixedSingle;
            _control.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _control.Padding = new Padding(10);
            _control.Text = _txt;
            _control.MouseDown += (s, e) => _control.ForeColor = Color.Orange;
            _control.MouseUp += (s, e) => _control.ForeColor = Color.Gray;
            _control.Click += (s, e) =>
            {
                if (_clickHandler != null)
                {
                    _clickHandler(_data);
                }
            };
            return this;
        }

        public Control Card => _control;

    }
}
